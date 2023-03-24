using Domain.Interface;

namespace Domain.Musics.Queue
{
    public class OneSongLoopMusicQueueState : IQueueState
    {
        public Music? Dequeue(MusicQueue queue)
        {
            return queue.Now;
        }

        public IQueueState Next => new NormalMusicQueueState();
    }
}
