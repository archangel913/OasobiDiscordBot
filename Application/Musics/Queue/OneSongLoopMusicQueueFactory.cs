using Application.Languages;
using Domain.Interface;

namespace Application.Musics.Queue
{
    public class OneSongLoopMusicQueueFactory : IQueueStateFactory
    {
        public OneSongLoopMusicQueueFactory(LanguageDictionary language)
        {
            this.Language = language;
        }

        private LanguageDictionary Language { get; }


        public IQueueState Create()
        {
            return new OneSongLoopMusicQueue(this.Language);
        }
    }
}