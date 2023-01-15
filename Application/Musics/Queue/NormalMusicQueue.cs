using Domain.Musics.Queue;

namespace Application.Musics.Queue
{
    public class NormalMusicQueue : NormalMusicQueueBase
    {
        public override string ToString()
        {
            return Musics.Language["Application.Musics.Queue.NormalMusicQueue.NormalMusicQueueName"];
        }
    }
}