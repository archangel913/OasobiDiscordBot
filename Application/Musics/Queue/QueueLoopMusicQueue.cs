using Domain.Musics.Queue;

namespace Application.Musics.Queue
{
    public class QueueLoopMusicQueue : QueueLoopMusicQueueBase
    {
        public override string ToString()
        {
            return Musics.Language["Application.Musics.Queue.QueueLoopMusicQueue.QueueLoopMusicQueueName"];
        }
    }
}