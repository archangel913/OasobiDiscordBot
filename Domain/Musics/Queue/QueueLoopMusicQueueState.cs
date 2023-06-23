using Domain.Interface;

namespace Domain.Musics.Queue
{
    public class QueueLoopMusicQueueState : IQueueState
    {
        public Music? Dequeue(MusicQueue queue)
        {
            if (queue.Now is not null)
            {
                queue.Enqueue(queue.Now);
                queue.Now = queue.Queue[0];
                queue.TryRemoveAt(0, out _);
            }
            return queue.Now;
        }

        public IQueueState Next => new OneSongLoopMusicQueueState();
    }
}
