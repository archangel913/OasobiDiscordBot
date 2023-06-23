using System.Text;
using Discord;
using Domain.Musics;
using Application.Interface;
using Application.Languages;
using Domain.Musics.Queue;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interface;

namespace Application.Musics
{
    public class Musics
    {
        public Musics(IServiceProvider services)
        {
            this.Language = services.GetRequiredService<ILanguageRepository>().Find();
            this.Service = services;
        }

        public LanguageDictionary Language { get; }

        private IServiceProvider Service { get; }

        public string SetController(IVoiceChannel voiceChannel, IUserMessage message)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                musicPlayer.Controller = message;
                return "complete";
            }
            catch (Exception e)
            {
                if (e.Message == "IVoiceChannel is null" || e.Message == "voiceChannel is invalid")
                {
                    return Language["Application.Musics.Musics.InvalidVoiceChannelExecption"];
                }
                else
                {
                    throw;
                }
            }
        }

        public IUserMessage? GetController(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                var controller = musicPlayer.Controller;
                if (controller is null) return null;
                return controller;
            }
            catch
            {
                throw;
            }
        }

        public void AddCurrentQueuePage(IVoiceChannel voiceChannel, int page)
        {
            var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
            musicPlayer.AddCurrentQueuePage(page);
        }

        public void SetCurrentQueuePage(IVoiceChannel voiceChannel, int page)
        {
            var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
            musicPlayer.CurrentQueuePage = page;
        }

        public int GetCurrentQueuePage(IVoiceChannel voiceChannel)
        {
            var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
            return musicPlayer.CurrentQueuePage;
        }


        public async Task<bool> Play(IVoiceChannel voiceChannel, string url, Func<IVoiceChannel, Task> func)
        {
            try
            {
                if (voiceChannel is null) throw new ArgumentException("IVoiceChannel is null");
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                await musicPlayer.ConnecetAsync();
                var addMusicList = await musicPlayer.Add(url);
                musicPlayer.Play(func, MusicPlayerProvider.DeleteMusicPlayer);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Exit(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                musicPlayer.Disconnect();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public EmbedBuilder Queue(IVoiceChannel voiceChannel)
        {
            try
            {
                var page = GetCurrentQueuePage(voiceChannel);
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                var builder = new EmbedBuilder();
                builder.WithColor(0x02B4C0);
                builder.WithTitle(Language["Application.Musics.Musics.Queue.QueueTitle"]);

                var nowMusicFieldBuilder = GetNowMusicFieldBuilder(page, musicPlayer);
                builder.AddField(nowMusicFieldBuilder);

                if (musicPlayer.GetNow() is not null && page >= 1)
                {
                    var waitingMusicsBuilder = GetWaitingMusicFieldBuilder(page, musicPlayer);
                    builder.AddField(waitingMusicsBuilder);
                }

                return builder;
            }
            catch (Exception e)
            {
                if (e.Message == "IVoiceChannel is null" || e.Message == "voiceChannel is invalid")
                {
                    var builder = new EmbedBuilder();
                    builder.WithColor(0x02B4C0);
                    builder.WithTitle(Language["Application.Musics.Musics.GetNowMusicFieldBuilder.ErrorTitle"]);
                    builder.WithDescription(Language["Application.Musics.Musics.InvalidVoiceChannelExecption"]);
                    return builder;
                }
                else
                {
                    throw;
                }
            }
        }

        private EmbedFieldBuilder GetNowMusicFieldBuilder(int page, MusicPlayer musicPlayer)
        {
            var nowNullable = musicPlayer.GetNow();
            var nowMusicFieldBuilder = new EmbedFieldBuilder();
            if (page < 1)
            {
                nowMusicFieldBuilder.WithName(Language["Application.Musics.Musics.GetNowMusicFieldBuilder.ErrorTitle"]);
                nowMusicFieldBuilder.WithValue(Language["Application.Musics.Musics.GetWaitingMusicFieldBuilder.Doesn'tHave"]);
            }
            else if (nowNullable is null)
            {
                nowMusicFieldBuilder.WithName(Language["Application.Musics.Musics.GetNowMusicFieldBuilder.NotPlayingTitle"]);
                nowMusicFieldBuilder.WithValue(Language["Application.Musics.Musics.GetNowMusicFieldBuilder.NotPlayingMessage"]);
            }
            else
            {
                nowMusicFieldBuilder.WithName(Language["Application.Musics.Musics.GetNowMusicFieldBuilder.NowPlayingTitle"]);
                nowMusicFieldBuilder.WithValue($"_{nowNullable?.Title}_\n*{nowNullable?.Url}*\n\n");
            }
            return nowMusicFieldBuilder;
        }

        private EmbedFieldBuilder GetWaitingMusicFieldBuilder(int page, MusicPlayer musicPlayer)
        {
            var waitingMusicsBuilder = new EmbedFieldBuilder();
            var queue = musicPlayer.GetQueue();
            int allPage = GetPagesCount(queue);

            // キューが空かどうかでフィールドの名前を変更する。
            string waitingMusicsFieldName = queue.Count == 0 ? Language["Application.Musics.Musics.GetWaitingMusicFieldBuilder.TitleOfWaiting"] : Language["Application.Musics.Musics.GetWaitingMusicFieldBuilder.TitleOfIsNoWaiting"];

            waitingMusicsBuilder.WithName(waitingMusicsFieldName);
            var waitingMusics = new StringBuilder();
            // 指定されたページにあるキュー内の音楽を追加する。
            for (int i = (page - 1) * 10; i < (page - 1) * 10 + 10; i++)
            {
                if (i >= queue.Count)
                    break;
                waitingMusics.Append($"\n{(i + 1).ToString()} : {queue[i].Title}");
            }


            // 一回も繰り返してない場合、指定されたページに曲がない。
            string pageInfo = (page - 1) * 10 >= queue.Count ?
                string.Format(Language["Application.Musics.Musics.GetWaitingMusicFieldBuilder.Doesn'tHave"], page)
                : string.Format(Language["Application.Musics.Musics.GetWaitingMusicFieldBuilder.CurrentPageInfo"], page, allPage);
            waitingMusics.Append(pageInfo);

            waitingMusicsBuilder.WithValue(waitingMusics.ToString());
            return waitingMusicsBuilder;
        }

        private int GetPagesCount(IReadOnlyList<Music> queue)
        {
            int count = queue.Count / 10;
            // 10で割り切れなかった用にページを一枚増やす。
            if (queue.Count % 10 > 0) count++;
            return count;
        }

        public bool Pause(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                bool pause = musicPlayer.SwitchPauseState();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Skip(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                if (musicPlayer.CanSkip)
                {
                    musicPlayer.Skip();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public PlayingOption Shuffle(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                var shuffleState = musicPlayer.SwitchShuffleState();
                return new PlayingOption(shuffleState, musicPlayer.MusicQueue.State, musicPlayer.IntegerVolume);
            }
            catch
            {
                throw;
            }
        }

        public PlayingOption Loop(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                var loopState = musicPlayer.ChangeLoopState();
                return new PlayingOption(musicPlayer.MusicQueue.IsShuffle, loopState, musicPlayer.IntegerVolume);
            }
            catch
            {
                throw;
            }
        }

        public string Remove(IVoiceChannel voiceChannel, int index)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                string deleteMusicName;
                if (musicPlayer.TryRemoveAt(index - 1, out deleteMusicName))
                    return string.Format(Language["Application.Musics.Musics.Remove.Removed"], deleteMusicName);
                else
                    return Language["Application.Musics.Musics.Remove.CouldNotRemove"];
            }
            catch (Exception e)
            {
                if (e.Message == "IVoiceChannel is null" || e.Message == "voiceChannel is invalid")
                {
                    return Language["Application.Musics.Musics.InvalidVoiceChannelExecption"];
                }
                else
                {
                    throw;
                }
            }
        }

        public PlayingOption Volume(IVoiceChannel voiceChannel, int volume)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(this.Service, voiceChannel);
                musicPlayer.AdjustVolume(volume);
                return new PlayingOption(musicPlayer.MusicQueue.IsShuffle, musicPlayer.MusicQueue.State, musicPlayer.IntegerVolume);
            }
            catch
            {
                throw;
            }
        }
    }
}
