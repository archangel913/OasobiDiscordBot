using Domain.Interface;

namespace Domain.Musics.Queue
{
    public abstract class OneSongLoopMusicQueueBase : IQueueState
    {
        public Music? Dequeue(MusicQueue queue)
        {
            return queue.Now;
        }

        public IQueueState ChangeLoopState(MusicQueue queue)
        {
            return queue.StateFactories.NormalMusicQueue.Create();
        }

        public abstract string ToString();
    }
}
