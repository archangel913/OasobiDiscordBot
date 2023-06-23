using Discord;
using Domain.Musics;

namespace Application.Musics
{
    public static class MusicPlayerProvider
    {
        private static readonly Dictionary<ulong, MusicPlayer> VCIdMusicPlayerPairs = new();

        public static MusicPlayer GetMusicPlayer(IServiceProvider services, IVoiceChannel voiceChannel)
        {
            if (voiceChannel is null) throw new ArgumentException("IVoiceChannel is null");
            if(VCIdMusicPlayerPairs.TryGetValue(voiceChannel.Id, out var musicPlayer))
            {
                return musicPlayer;
            }
            var newMusicPlayer = new MusicPlayer(services, voiceChannel);
            VCIdMusicPlayerPairs.Add(voiceChannel.Id, newMusicPlayer);
            //logger.WriteBotSystemLog("Generate MusicPlayer  guild : " + newMusicPlayer.GuildName + "  channel : " + newMusicPlayer.ChannelName);
            return newMusicPlayer;
        }

        public static void DeleteMusicPlayer(MusicPlayer musicPlayer)
        {
            VCIdMusicPlayerPairs.Remove(musicPlayer.VoiceChannelId);
            //logger.WriteBotSystemLog("Delete MusicPlayer  guild : " + musicPlayer.GuildName + "  channel : " + musicPlayer.ChannelName);
        }

        public static void Clear()
        {
            VCIdMusicPlayerPairs.Clear();
        }

        public static List<MusicPlayer> GetAllMusicPlayers()
        {
            return new List<MusicPlayer>(VCIdMusicPlayerPairs.Select(pair => pair.Value).ToList());
        }
    }
}
