namespace Domain.Musics.YouTubeMusics
{
    public class YoutubeApiStructure
    {
        public string kind { get; set; }

        public string etag { get; set; }

        public string nextPageToken { get; set; }

        public string prevPageToken { get; set; }

        public PageInfo pageInfo { get; set; }
    }
}
