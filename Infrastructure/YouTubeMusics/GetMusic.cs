using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Musics;
using Domain.Interface;

namespace Infrastructure.YouTubeMusics
{
    internal class GetMusic : IGetMusic
    {
        public async Task<List<Music>> GetMusicsAsync(string url)
        {
            var client = new Http.Http();

            string request = YouTubeURLFactory.CreateRequestURL(url);

            var response = await client.GetAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            var videos = Regex.Match(json, "\"PLAYER_VARS\":{.*\"video_id\"");
            String strVideos = videos.Value
                .Replace("\",\"video_id\"", "")
                .Replace("\"PLAYER_VARS\":{\"embedded_player_response\":\"", "")
                .Replace("\\", "")
                .Replace("u0027", "'");

            var musicList = new List<Music>();
            JObject jsonObj = JObject.Parse(strVideos);

            if (jsonObj["embedPreview"]["thumbnailPreviewRenderer"]["playlist"] != null)
            {
                foreach (JObject v in jsonObj["embedPreview"]["thumbnailPreviewRenderer"]["playlist"]["playlistPanelRenderer"]["contents"])
                {
                    string title = "";
                    foreach (JObject t in v["playlistPanelVideoRenderer"]["title"]["runs"])
                    {
                        title = t["text"].ToString();
                    }
                    string id = v["playlistPanelVideoRenderer"]["videoId"].ToString();
                    musicList.Add(new Music(title, @$"https://www.youtube.com/watch?v={id}"));
                }
            }
            else
            {
                foreach (JObject title in jsonObj["embedPreview"]["thumbnailPreviewRenderer"]["title"]["runs"])
                {
                    musicList.Add(new Music(title["text"].ToString(), YouTubeURLFactory.CreateWatchableURL(url)));
                }
            }
            return musicList;
        }

        /*
        引数 https://www.youtube.com/*
        普通の動画（短縮）https://youtu.be/_wBtgy9k6v4
        普通の動画（普通の）https://www.youtube.com/watch?v=_wBtgy9k6v4
        プレイリスト（普通の）：https://www.youtube.com/playlist?list=PL6gpkvSpVRsdWdqYYCPXghmSF1DToiTUv
        プレイリスト（短縮）：https://youtube.com/playlist?list=PL6gpkvSpVRsdWdqYYCPXghmSF1DToiTUv
        ショート（普通の）：https://www.youtube.com/shorts/M6qG56gv0uY
        ショート（短縮）：https://youtube.com/shorts/M6qG56gv0uY?feature=share
        ：
        モドリッチ
        https://www.youtube.com/embed/<videoId>
        https://www.youtube.com/embed/playlist?list=<playListId>
        */


        private class YouTubeURLFactory
        {
            private static IReadOnlyCollection<string> Patterns { get; } = new List<string>()
            {
                "https://youtu.be/",
                "https://www.youtube.com/watch?v=",
                "https://youtube.com/",
                "https://www.youtube.com/",
            };

            private static string EmbedURL { get; } = "https://www.youtube.com/embed/";

            private static string WatchableURL { get; } = "https://www.youtube.com/watch?v=";

            public static string CreateRequestURL(string baseURL)
            {
                string url = baseURL;
                foreach (var pattern in Patterns)
                {
                    string tmp = new string(url);
                    url = baseURL.Replace(pattern, EmbedURL);
                    if (url != tmp) { break; }
                }
                url = url.Replace("shorts/", "");
                url = url.Replace("?feature=share", "");
                return url;
            }

            public static string CreateWatchableURL(string url)
            {
                string id = CreateRequestURL(url).Replace(EmbedURL, "");
                return $"{WatchableURL}{id}";
            } 
        }
    }
}
