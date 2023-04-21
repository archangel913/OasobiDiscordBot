namespace Domain.Musics.YouTubeMusics.YoutubePlaylists
{
    public class PlaylistSnippet
    {
        public DateTimeOffset publishedAt { get; set; }

        public string channelId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Thumbnails thumbnails { get; set; }

        public string channelTitle { get; set; }

        public string playlistId { get; set; }

        public uint position { get; set; }

        public ResourceId resourceId { get; set; }
    }
}
