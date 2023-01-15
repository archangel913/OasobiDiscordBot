using Domain.Musics;

namespace Domain.Interface
{
    public interface IVideoLib
    {
        public string GetYoutubeVideoUri(string url);

        public Task<Stream> GetYoutubeVideoStreamAsync(string url);
    }
}
