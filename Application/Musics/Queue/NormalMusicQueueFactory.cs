using Application.Languages;
using Domain.Interface;

namespace Application.Musics.Queue
{
    public class NormalMusicQueueFactory : IQueueStateFactory
    {
        public NormalMusicQueueFactory(LanguageDictionary language)
        {
            this.Language = language;
        }

        private LanguageDictionary Language { get; }

        public IQueueState Create()
        {
            return new NormalMusicQueue(this.Language);
        }
    }
}