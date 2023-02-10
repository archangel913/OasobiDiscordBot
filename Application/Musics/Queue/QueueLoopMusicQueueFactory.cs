using Application.Languages;
using Domain.Interface;

namespace Application.Musics.Queue
{
    public class QueueLoopMusicQueueFactory : IQueueStateFactory
    {
        public QueueLoopMusicQueueFactory(LanguageDictionary language) 
        { 
            this.Language = language;
        }

        private LanguageDictionary Language { get; }

        public IQueueState Create()
        {
            return new QueueLoopMusicQueue(this.Language);
        }
    }
}