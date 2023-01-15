using Domain.Interface;

namespace Application.Musics.Queue
{
    public class OneSongLoopMusicQueueFactory : IQueueStateFactory
    {
        public IQueueState Create()
        {
            return new OneSongLoopMusicQueue();
        }
    }
}