using Domain.Interface;

namespace Application.Musics.Queue
{
    public class NormalMusicQueueFactory : IQueueStateFactory
    {
        public IQueueState Create()
        {
            return new NormalMusicQueue();
        }
    }
}