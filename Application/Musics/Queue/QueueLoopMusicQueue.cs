using Application.Languages;
using Domain.Musics.Queue;

namespace Application.Musics.Queue
{
    public class QueueLoopMusicQueue : QueueLoopMusicQueueBase
    {
        public QueueLoopMusicQueue(LanguageDictionary language)
        {
            this.Language = language;
        }

        private LanguageDictionary Language { get; }


        public override string ToString()
        {
            return "queue";
        }
    }
}