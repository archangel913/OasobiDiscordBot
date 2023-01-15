using Domain.Musics.Queue;

namespace Application.Musics.Queue
{
    public class OneSongLoopMusicQueue : OneSongLoopMusicQueueBase
    {
        public override string ToString()
        {
            return Musics.Language["Application.Musics.Queue.OneSongLoopMusicQueue.OneSongLoopMusicQueueName"];
        }
    }
}