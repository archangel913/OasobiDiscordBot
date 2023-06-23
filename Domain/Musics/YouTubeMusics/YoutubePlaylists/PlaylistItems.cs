namespace Domain.Musics.YouTubeMusics.YoutubePlaylists
{
    public class PlaylistItems
    {
        public string kind { get; set; }

        public string etag { get; set; }

        public string id { get; set; }

        public PlaylistSnippet snippet { get; set; }

        public ContentDetails ContentDetails { get; set; }

        public Status status { get; set; }
    }
}
