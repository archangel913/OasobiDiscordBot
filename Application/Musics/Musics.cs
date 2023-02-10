using System.Text;
using Discord;
using Domain.Musics;
using Application.Interface;
using Application.Languages;
using Domain.Musics.Queue;
using Application.Musics.Queue;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Musics
{
    public class Musics
    {
        public Musics(IServiceProvider services)
        {
            this.Language = services.GetRequiredService<ILanguageRepository>().Find();
            this.QueueStateFactories = new(
                new OneSongLoopMusicQueueFactory(this.Language),
                new QueueLoopMusicQueueFactory(this.Language),
                new NormalMusicQueueFactory(this.Language));
        }

        public LanguageDictionary Language { get; }

        private QueueStateFactories QueueStateFactories { get; }

        public async Task<string> Play(IVoiceChannel voiceChannel, string url)
        {
            try
            {
                if (voiceChannel is null) throw new ArgumentException("IVoiceChannel is null");
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                await musicPlayer.ConnecetAsync();
                var addMusicList = await musicPlayer.Add(url);
                musicPlayer.Play();
                return string.Format(Language["Application.Musics.Musics.Play.AddedMusics"], addMusicList.Count, addMusicList[0].Title);
            }
            catch (Exception e)
            {
                if (e.Message == "IVoiceChannel is null" || e.Message == "voiceChannel is invalid")
                {
                    return Language["Application.Musics.Musics.InvalidVoiceChannelExecption"];
                }
                else if (e.Message == "Invalid argument in GetVideoAsync" || e.Message == "Invalid argument in GetPlaylistItemsAsync")
                {
                    return Language["Application.Musics.Musics.InvalidYouTubeUrlExecption"];
                }
                else
                {
                    throw;
                }
            }
        }

        public string Exit(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                musicPlayer.Disconnect();
                return Language["Application.Musics.Musics.Exit.Exited"];
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

        public EmbedBuilder Queue(int page, IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
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
                nowMusicFieldBuilder.WithValue(Language["Application.Musics.Musics.GetNowMusicFieldBuilder.ErrorMessage"]);
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

        public string Pause(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                bool pause = musicPlayer.SwitchPauseState();
                return pause ? Language["Application.Musics.Musics.Pause.Paused"] : Language["Application.Musics.Musics.Pause.Unpaused"];
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

        public string Skip(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                if (musicPlayer.CanSkip)
                {
                    musicPlayer.Skip();
                    return Language["Application.Musics.Musics.Skipped"];
                }
                else return Language["Application.Musics.Musics.CouldNotSkip"];
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

        public string Shuffle(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                bool isShuffled = musicPlayer.SwitchShuffleState();
                return isShuffled ? Language["Application.Musics.Musics.ShuffleOn"] : Language["Application.Musics.Musics.ShuffleOff"];
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

        public string Loop(IVoiceChannel voiceChannel)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                musicPlayer.ChangeLoopState();
                return musicPlayer.MusicQueue.State.ToString();
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

        public string Remove(IVoiceChannel voiceChannel, int index)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
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

        public string Volume(IVoiceChannel voiceChannel, double volume)
        {
            try
            {
                var musicPlayer = MusicPlayerProvider.GetMusicPlayer(voiceChannel, QueueStateFactories);
                musicPlayer.SetVolume(volume);
                if (volume != 0)
                    return string.Format(Language["Application.Musics.Musics.SetVolume"], volume);
                else
                    return Language["Application.Musics.Musics.SetDefaltVolume"];
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
    }
}
