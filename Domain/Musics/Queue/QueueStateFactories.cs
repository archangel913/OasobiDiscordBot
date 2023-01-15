using Domain.Interface;

namespace Domain.Musics.Queue
{
    public class QueueStateFactories
    {
        public QueueStateFactories(IQueueStateFactory oneSoungLoopQueue, IQueueStateFactory queueLoopQueue, IQueueStateFactory normalMusicQueue)
        {
            this.OneSongLoopQueue= oneSoungLoopQueue;
            this.QueueLoopMusicQueue= queueLoopQueue;
            this.NormalMusicQueue= normalMusicQueue;
        }

        public IQueueStateFactory OneSongLoopQueue { get; }

        public IQueueStateFactory QueueLoopMusicQueue { get; }

        public IQueueStateFactory NormalMusicQueue { get; }
    }
}