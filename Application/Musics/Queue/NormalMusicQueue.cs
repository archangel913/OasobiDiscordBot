using Application.Languages;
using Domain.Musics.Queue;

namespace Application.Musics.Queue
{
    public class NormalMusicQueue : NormalMusicQueueBase
    {
        public NormalMusicQueue(LanguageDictionary language)
        {
            this.Language = language;
        }

        private LanguageDictionary Language { get; }


        public override string ToString()
        {
            return this.Language["Application.Musics.Queue.NormalMusicQueue.NormalMusicQueueName"];
        }
    }
}