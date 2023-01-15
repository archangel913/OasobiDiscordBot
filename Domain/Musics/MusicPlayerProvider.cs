using Discord;
using Domain.Interface;
using Domain.Musics.Queue;

namespace Domain.Musics
{
    public static class MusicPlayerProvider
    {
        private static readonly List<MusicPlayer> MusicPlayerList = new();

        private static readonly IDiscordLogger Logger = Factory.Factory.GetService<IDiscordLogger>();

        public static MusicPlayer GetMusicPlayer(IVoiceChannel voiceChannel, QueueStateFactories factories)
        {
            bool isExist = false;
            if (voiceChannel is null) throw new ArgumentException("IVoiceChannel is null");
            foreach (var musicPlayer in MusicPlayerList)
            {
                if (musicPlayer.IsMatchVoiceChunnel(voiceChannel))
                {
                    isExist = true;
                    if (musicPlayer.IsMatchVoiceChunnel((IEntity<ulong>)voiceChannel))
                        return musicPlayer;
                }
            }
            if (isExist) throw new ArgumentException("voiceChannel is invalid");
            var newMusicPlayer = new MusicPlayer(voiceChannel, factories);
            MusicPlayerList.Add(newMusicPlayer);
            Logger.WriteBotSystemLog("Generate MusicPlayer  guild : " + newMusicPlayer.GuildName + "  channel : " + newMusicPlayer.ChannelName);
            return newMusicPlayer;
        }

        public static void DeleteMusicPlayer(MusicPlayer musicPlayer)
        {
            MusicPlayerList.Remove(musicPlayer);
            Logger.WriteBotSystemLog("Delete MusicPlayer  guild : " + musicPlayer.GuildName + "  channel : " + musicPlayer.ChannelName);
        }

        public static List<MusicPlayer> GetAllMusicPlayers()
        {
            return new List<MusicPlayer>(MusicPlayerList);
        }
    }
}
