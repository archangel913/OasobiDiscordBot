using Discord;
using Discord.Audio;
using Domain.Interface;

namespace Infrastructure.Discord
{
    internal class AudioSender : IAudioSender
    {

        private IAudioClient? Client { get; set; }

        private IVoiceChannel? VoiceChannel { get; set; }

        private AudioOutStream? AudioOutStream { get; set; }

        public bool IsConnect
            => Client?.ConnectionState == ConnectionState.Connected || Client?.ConnectionState == ConnectionState.Connecting;

        public async Task ConnectAsync(IVoiceChannel channel)
        {
            try
            {
                this.VoiceChannel = channel;
                this.Client = await channel.ConnectAsync();
                AudioOutStream = Client.CreatePCMStream(AudioApplication.Music);
            }
            catch(Exception e)
            {
                var r = new LocalFile.FileRepository();
                r.WriteLogFile(@"Log/currentLog.txt",e.Message);
                if (e.StackTrace is not null)
                {
                    r.WriteLogFile(@"Log/currentLog.txt", e.StackTrace);
                }
            }
        }

        public async Task UpdateAsync()
        {
            if (this.VoiceChannel is null) throw new NullReferenceException("channel is null.");
            this.Client = await this.VoiceChannel.ConnectAsync();
            this.AudioOutStream = this.Client.CreatePCMStream(AudioApplication.Music);
        }

        public async Task DisconnectAsync()
        {
            if (this.Client is null) throw new NullReferenceException("Client is null.");
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.Client.StopAsync();
            this.AudioOutStream.Close();
            this.Client.Dispose();
        }

        public async Task SendMusic(byte[] buffer, int offset, int count)
        {
            try
            {
                if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
                await this.AudioOutStream.WriteAsync(buffer.AsMemory(offset, count));
            }
            catch(Exception e)
            {
                var r = new LocalFile.FileRepository();
                r.WriteLogFile(@"Log/currentLog.txt", e.Message);
                if (e.StackTrace is not null)
                {
                    r.WriteLogFile(@"Log/currentLog.txt", e.StackTrace);
                }
                await this.UpdateAsync();
            }
        }

        public async Task FlushAsync()
        {
            if (this.AudioOutStream is null) throw new NullReferenceException("AudioOutStream is null.");
            await this.AudioOutStream.FlushAsync().ConfigureAwait(false);
        }
    }
}
