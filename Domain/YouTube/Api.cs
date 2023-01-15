using Newtonsoft.Json.Linq;
using Domain.Interface;

namespace Domain.YouTube
{
    public class Api
    {
        private static Api INSTANCE = new();

        private Api() { }

        private string? ApiKey;

        public static Api GetInstance()
        {
            return INSTANCE;
        }
        public void Setkey(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        public async Task<JObject?> GetPlaylistItemsAsync(string url, string pageToken)
        {
            if (!url.Contains("list=")) throw new ArgumentException("Invalid argument in GetPlaylistItemsAsync");
            var part = "part=snippet";
            var baseUrl = "https://www.googleapis.com/youtube/v3/";
            var type = "playlistItems?";
            var maxreslt = "maxResults=50";
            var id = "playlistId=" + url.Split("list=")[1];
            var key = "key=" + this.ApiKey;
            string request = baseUrl + type + maxreslt + '&' + part + '&' + id + '&' + key;
            if (pageToken is not "") request += ('&' + "pageToken=" + pageToken);

            return await ExecuteHttpRequestAsync(request);
        }

        public async Task<JObject?> GetVideoAsync(string url)
        {
            if (!(url.Contains("v=") || url.Contains("shorts"))) throw new ArgumentException("Invalid argument in GetVideoAsync");
            var part = "part=snippet";
            var baseUrl = "https://www.googleapis.com/youtube/v3/";
            var type = "videos?";
            string id;
            if (url.Contains("v="))
            {
                id = "id=" + url.Split("v=")[1];
            }
            else
            {
                id = "id=" + url.Split("shorts/")[1];
            }

            var key = "key=" + this.ApiKey;
            string request = baseUrl + type + '&' + part + '&' + id + '&' + key;

            return await ExecuteHttpRequestAsync(request);
        }

        private async Task<JObject?> ExecuteHttpRequestAsync(string request)
        {
            var http = Factory.Factory.GetService<IHttp>();
            var response = await http.GetAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json);
        }
    }
}
