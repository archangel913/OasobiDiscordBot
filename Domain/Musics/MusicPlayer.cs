using Discord;
using Domain.Interface;
using Domain.Musics.Queue;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace Domain.Musics
{
    public class MusicPlayer
    {
        public MusicPlayer(IServiceProvider services, IVoiceChannel voiceChannel)
        {
            this.AudioSender = services.GetRequiredService<IAudioSender>();
            this.VideoLib = services.GetRequiredService<IVideoLib>();
            this.Logger = services.GetRequiredService<IDiscordLogger>();
            this.MusicGetter = services.GetRequiredService<IGetMusic>();
            this.Voicechannel = voiceChannel;
            this.GuildName = voiceChannel.Guild.Name;
            this.ChannelName = voiceChannel.Name;
            this.VoiceChannelId = voiceChannel.Id;
            this.MusicQueue = new MusicQueue(new NormalMusicQueueState());
        }

        private IVoiceChannel Voicechannel { get; set; }

        private IAudioSender AudioSender { get; }

        public MusicQueue MusicQueue { get; private set; }

        private IVideoLib VideoLib { get; }

        private IDiscordLogger Logger { get; }

        private IGetMusic MusicGetter { get; }

        private Task? PlayTask { get; set; }

        private bool CanLoad { get; set; } = false;

        private bool IsPause { get; set; } = false;

        private bool IsSkip { get; set; } = false;

        private bool IsExit { get; set; } = false;

        private double CalculatedVolume { get; set; } = 0.1;

        public int IntegerVolume { get; set; } = 20;

        public IUserMessage? Controller { get; set; }

        public int CurrentQueuePage { get; set; } = 1;

        public bool CanSkip { get; private set; } = false;

        public string GuildName { get; }

        public string ChannelName { get; }

        public ulong VoiceChannelId { get; }

        public void AddCurrentQueuePage(int page)
        {
            this.CurrentQueuePage += page;
        }

        public async Task ConnecetAsync()
        {
            if (!AudioSender.IsConnect)
                await AudioSender.ConnectAsync(this.Voicechannel);
        }

        public async Task UpdateAsync(IVoiceChannel voiceChannel)
        {
            this.Voicechannel = voiceChannel;
            await AudioSender.UpdateAsync();
        }

        public void Disconnect()
        {
            this.CanLoad = false;
            this.IsExit = true;
            if (this.PlayTask is null) return;
            this.PlayTask.Wait();
        }

        public void Play(Func<IVoiceChannel, Task> func, Action<MusicPlayer> action)
        {
            this.PlayTask ??= Task.Run(async () => await PlayAsync(func, action));
        }

        /**
         * 実際に曲を再生する＝play
         * ではなく、
         * キューに曲を追加する＝Playなのでは
         * 
         * 具体的な手段についてはドメイン層ではなくインフラ層送りでいいかもしれない
         */

        private async Task PlayAsync(Func<IVoiceChannel, Task> updateQueue, Action<MusicPlayer> deleteMusicPlayer)
        {
            var now = MusicQueue.Dequeue();
            while (now is not null && IsExit is false)
            {
                var encoder = Encoder.VideoToWave();
                _ = Task.Run(() => ReadInputStreamToEncoderAsync(now.Url, encoder.StandardInput.BaseStream));
                var encodedStream = encoder.StandardOutput.BaseStream;
                int blockSize = 3840;
                byte[] buffer = new byte[blockSize];
                int byteCount;
                this.CanSkip = true;
                this.Logger.WriteBotSystemLog(now.Title + " is playing in " + this.GuildName + "'s " + this.ChannelName);
                while (((byteCount = encodedStream.Read(buffer, 0, blockSize)) > 0) && IsExit is false)
                {
                    try
                    {
                        CalcVolume(buffer, blockSize);
                        if (byteCount < blockSize)
                        {
                            for (int i = byteCount; i < blockSize; i++)
                                buffer[i] = 0;
                        }
                        if (this.IsSkip)
                        {
                            this.IsSkip = false;
                            break;
                        }
                        while (this.IsPause)
                        {
                            await Task.Delay(100);
                        }
                        await AudioSender.SendMusic(buffer, 0, blockSize);
                    }
                    catch (Exception e)
                    {
                        await this.AudioSender.UpdateAsync();
                        Console.WriteLine("at MusicPlayer line 140");
                        Console.WriteLine(e);
                    }
                }
                this.CanSkip = false;
                await AudioSender.FlushAsync();
                encoder.Dispose();
                encodedStream.Dispose();
                now = MusicQueue.Dequeue();
                await updateQueue(Voicechannel);
            }
            await this.AudioSender.DisconnectAsync();
            if (this.Controller is not null) await this.Controller.DeleteAsync();
            deleteMusicPlayer(this);
        }

        private async Task ReadInputStreamToEncoderAsync(string url, Stream encoderInputStream)
        {
            try
            {
                this.CanLoad = true;
                this.Logger.WriteBotSystemLog(this.GuildName + "'s " + this.ChannelName + " Start download music");
                using var input = await VideoLib.GetYoutubeVideoStreamAsync(url);
                byte[] readBuffer = new byte[16 * 1024];
                this.CanLoad = true;
                int read;
                while (((read = input.Read(readBuffer, 0, readBuffer.Length)) > 0) && this.CanLoad)
                {
                    encoderInputStream.Write(readBuffer, 0, read);
                }
                this.Logger.WriteBotSystemLog(this.GuildName + "'s " + this.ChannelName + " Finish download music");
            }
            catch (Exception e)
            {
                this.Logger.WriteErrorLog(e.Message + "\n==========StackTrace==========\n" + e.StackTrace);
                if (e.InnerException is not null)
                    this.Logger.WriteErrorLog(e.InnerException.Message + "\n==========InnerException StackTrace==========\n" + e.InnerException.StackTrace);
            }
            finally
            {
                encoderInputStream.Close();
            }
        }

        public async Task<List<Music>> Add(string url)
        {
            var musicList = await this.MusicGetter.GetMusicsAsync(url);
            for (int i = 0; i < musicList.Count; i++)
            {
                this.MusicQueue.Enqueue(musicList[i]);
            }
            return musicList;
        }

        public bool SwitchShuffleState() => this.MusicQueue.SwitchShuffleState();

        public bool TryRemoveAt(int index, out string deleteMusicName) => MusicQueue.TryRemoveAt(index, out deleteMusicName);

        public IQueueState ChangeLoopState()
        {
            this.MusicQueue.ChangeLoopState();
            return this.MusicQueue.State;
        }

        public Type GetLoopType() => MusicQueue.GetType();

        public List<Music> GetQueue() => MusicQueue.GetReadOnlyQueue();

        public Music? GetNow() => MusicQueue.Now;

        public bool SwitchPauseState()
        {
            this.IsPause = !this.IsPause;
            return this.IsPause;
        }

        public void Skip()
        {
            this.CanLoad = false;
            this.IsSkip = true;
        }

        public void AdjustVolume(int volume)
        {
            if (volume == 0)
            {
                this.CalculatedVolume = 0.1;
                this.IntegerVolume = 20;
            }
            else
            {
                this.CalculatedVolume *= Math.Pow(10, (double)volume / 5);
                this.IntegerVolume += volume;
            }
        }

        private void CalcVolume(byte[] buffer, int blockSize)
        {
            short tmp;
            for (int i = 0; i < blockSize; i += 2)
            {
                tmp = buffer[i + 1];
                tmp = (short)((tmp << 8) | buffer[i]);
                if (((this.CalculatedVolume * tmp) < 0) && (tmp > 0)) tmp = short.MaxValue;
                else if (((this.CalculatedVolume * tmp) > 0) && (tmp < 0)) tmp = short.MinValue;
                else tmp = (short)(this.CalculatedVolume * tmp);
                buffer[i] = (byte)tmp;
                buffer[i + 1] = (byte)(tmp >> 8);
            }
        }
    }
}
