using Domain.Musics;
using VideoLibrary;
using Domain.Interface;
using System;
using System.IO;

namespace Infrastructure.Videos
{
    internal class Video : IVideoLib
    {
        public string GetYoutubeVideoUri(string url)
        {
            string uri = "";
            try
            {
                var youtube = YouTube.Default;
                var video = youtube.GetVideo(url);
                uri = video.Uri;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return uri;
        }
        public async Task<Stream> GetYoutubeVideoStreamAsync(string url)
        {
            var youtube = YouTube.Default;
            var video = youtube.GetVideo(url);
            var stream = await video.StreamAsync();
            return stream;
        }
    }
}
