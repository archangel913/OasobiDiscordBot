using Domain.Musics;
using Domain.Musics.Queue;

namespace Domain.Interface
{
    public interface IQueueState
    {
        IQueueState ChangeLoopState(MusicQueue queue);

        Music? Dequeue(MusicQueue queue);

        string ToString();
    }
}