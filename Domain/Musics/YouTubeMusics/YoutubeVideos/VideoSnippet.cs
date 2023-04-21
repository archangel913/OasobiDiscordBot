namespace Domain.Musics.YouTubeMusics.YoutubeVideos
{
    public class VideoSnippet
    {
        public DateTimeOffset publishedAt { get; set; }

        public string channelId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Thumbnails thumbnails { get; set; }

        public string channelTitle { get; set; }

        public IList<string> tags { get; set; }

        public string categoryId { get; set; }

        public string liveBroadcastContent { get; set; }

        public Localized localized { get; set; }

        public string defaultAudioLanguage { get; set; }
    }
}
