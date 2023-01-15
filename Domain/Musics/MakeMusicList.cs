using Domain.YouTube;

namespace Domain.Musics
{
    public class MakeMusicList
    {
        private string Url;

        private List<Music> MusicList = new List<Music>();
        public MakeMusicList(string url)
        {
            this.Url = url;
        }

#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
        public async Task<List<Music>> GetMusicsAsync()
        {
            this.Url = this.Url.Replace("https://youtu.be/", "https://www.youtube.com/watch?v=");
            if (this.Url.Contains("v=") || this.Url.Contains("shorts"))
            {
                var jObject = await Api.GetInstance().GetVideoAsync(this.Url);
                var items = jObject["items"];
                var title = items[0]["snippet"]["title"].ToString();
                var videoUrl = $"https://www.youtube.com/watch?v={items[0]["id"].ToString()}";
                this.MusicList.Add(new Music(title, videoUrl));
            }
            else
            {
                var jObject = await Api.GetInstance().GetPlaylistItemsAsync(this.Url, "");
                var items = jObject["items"];
                for (int i = 0; i < items.Count(); i++)
                {
                    var title = items[i]["snippet"]["title"].ToString();
                    var videoUrl = $"https://www.youtube.com/watch?v={items[i]["snippet"]["resourceId"]["videoId"].ToString()}";
                    if (title != "Private video")
                        this.MusicList.Add(new Music(title, videoUrl));
                }
                if (jObject["nextPageToken"] is not null) await GetNextPage(jObject["nextPageToken"].ToString());
            }
            return this.MusicList;
        }

        private async Task GetNextPage(string pageToken = "")
        {
            var jObject = await Api.GetInstance().GetPlaylistItemsAsync(this.Url,pageToken);
            var items = jObject["items"];
            for (int i = 0; i < items.Count(); i++)
            {
                var title = items[i]["snippet"]["title"].ToString();
                var videoUrl = "https://www.youtube.com/watch?v=" + items[i]["snippet"]["resourceId"]["videoId"].ToString();
                this.MusicList.Add(new Music(title, videoUrl));
            }
            if (jObject["nextPageToken"] is not null) await GetNextPage(jObject["nextPageToken"].ToString());
        }
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
    }
}
