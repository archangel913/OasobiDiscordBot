using Domain.Interface;

namespace Domain.Musics.Queue
{
    public abstract class NormalMusicQueueBase : IQueueState
    {
        public Music? Dequeue(MusicQueue queue)
        {
            queue.Now = null;
            if (queue.Queue.Count != 0)
            {
                queue.Now = queue.Queue[0];
                queue.TryRemoveAt(0, out _);
            }
            return queue.Now;
        }

        public IQueueState ChangeLoopState(MusicQueue queue)
        {
            return queue.StateFactories.QueueLoopMusicQueue.Create();
        }

        public abstract string ToString();
    }
}
