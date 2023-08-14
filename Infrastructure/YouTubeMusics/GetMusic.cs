using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Musics;
using Domain.Interface;
using Domain.Musics.YouTubeMusics;
using Domain.Musics.YouTubeMusics.YoutubePlaylists;
using Domain.Musics.YouTubeMusics.YoutubeVideos;
using Application.Interface;

namespace Infrastructure.YouTubeMusics
{
    internal class GetMusic : IGetMusic
    {
        private readonly string YoutubeBaseUrl = @"https://www.youtube.com/watch?v=";

        private readonly string YoutubeApiKey;

        public GetMusic(string key)
        {
            this.YoutubeApiKey = key;
        }

        public async Task<List<Music>> GetMusicsAsync(string url)
        {
            var musicList = new List<Music>();
            using (var client = new HttpClient())
            {
                var factory = new YouTubeApiURLFactory(this.YoutubeApiKey);
                string nextPageToken = "";

                while (true)
                {
                    var request = factory.CreateRequestVideoUrl(url, nextPageToken);
                    var response = await client.GetAsync(request);
                    var json = await response.Content.ReadAsStringAsync();
                    YoutubeApiStructure youtubeVideos = new();
                    try
                    {
                        if (request.Contains("playlist"))
                        {
                            youtubeVideos = JsonSerializer.Deserialize<YoutubePlaylists>(json)!;
                        }
                        else
                        {
                            youtubeVideos = JsonSerializer.Deserialize<YouTubeVideos>(json)!;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    if (youtubeVideos is YouTubeVideos)
                    {
                        foreach (var item in ((YouTubeVideos)youtubeVideos).items)
                        {
                            var id = item.id;
                            var title = item.snippet.title;
                            musicList.Add(new Music(title, this.YoutubeBaseUrl + id));
                        }
                    }
                    else
                    {
                        foreach (var item in ((YoutubePlaylists)youtubeVideos).items)
                        {
                            var snippet = item.snippet;
                            var id = snippet.resourceId.videoId;
                            var title = snippet.title;
                            musicList.Add(new Music(title, this.YoutubeBaseUrl + id));
                        }
                    }
                    if(youtubeVideos.nextPageToken == "" || youtubeVideos.nextPageToken is null)
                    {
                        break;
                    }
                    nextPageToken = youtubeVideos.nextPageToken;
                }
            }
            return musicList;
        }

        /*
        普通の動画（短縮）https://youtu.be/_wBtgy9k6v4
        普通の動画（普通の）https://www.youtube.com/watch?v=_wBtgy9k6v4
        プレイリスト（普通の）：https://www.youtube.com/playlist?list=PL6gpkvSpVRsdWdqYYCPXghmSF1DToiTUv
        プレイリスト（短縮）：https://youtube.com/playlist?list=PL6gpkvSpVRsdWdqYYCPXghmSF1DToiTUv
        ショート（普通の）：https://www.youtube.com/shorts/M6qG56gv0uY
        ショート（短縮）：https://youtube.com/shorts/M6qG56gv0uY?feature=share
        */


        private class YouTubeApiURLFactory
        {
            public YouTubeApiURLFactory(string key)
            {
                this.Key += key;
            }

            private static IReadOnlyCollection<string> Patterns { get; } = new List<string>()
            {
                "https://youtu.be/",
                "https://www.youtube.com/watch?v=",
                "https://www.youtube.com/shorts/",
                "https://youtube.com/shorts/",
                "https://youtube.com/playlist?list=",
                "https://www.youtube.com/playlist?list="
            };

            private readonly int VideoIdLength = 11;

            private readonly string BaseUrl = @"https://www.googleapis.com/youtube/v3/";

            private readonly string Part = @"part=snippet";

            private readonly string Id = @"id=";

            private readonly string PlaylistId = @"playlistId=";

            private readonly string Key = @"key=";

            public string CreateRequestVideoUrl(string url, string nextPageToken = "")
            {
                // youtube musicの場合、music.を削除する
                url = url.Replace("music.", "");
                foreach (var pattern in Patterns)
                {
                    string originalUrl = url;
                    url = url.Replace(pattern, "");
                    if (url != originalUrl) { break; }
                }
                var id = url.Split("&")[0];
                string request;
                if (id.Length == this.VideoIdLength)
                {
                    request = $"{this.BaseUrl}videos?{this.Part}&{this.Id}{id}&{this.Key}";
                }
                else
                {
                    request = $"{this.BaseUrl}playlistItems?{this.Part}&{this.PlaylistId}{id}&{this.Key}&maxResults=50";
                    if (nextPageToken != "")
                    {
                        request += "&pageToken=" + nextPageToken;
                    }
                }
                return request;
            }
        }
    }
}
