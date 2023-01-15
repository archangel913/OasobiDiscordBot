using Domain.Interface;

namespace Application.Musics.Queue
{
    public class QueueLoopMusicQueueFactory : IQueueStateFactory
    {
        public IQueueState Create()
        {
            return new QueueLoopMusicQueue();
        }
    }
}