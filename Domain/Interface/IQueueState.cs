using Domain.Musics;
using Domain.Musics.Queue;

namespace Domain.Interface
{
    public interface IQueueState
    {
        Music? Dequeue(MusicQueue queue);

        IQueueState Next { get; }
    }
}