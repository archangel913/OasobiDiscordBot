using Domain.Interface;

namespace Domain.Musics.Queue
{
    public class MusicQueue
    {
        public MusicQueue(IQueueState queueState)
        {
            this.Queue = new List<Music>();
            this.State = queueState;
        }

        public Music? Now { get; internal set; }

        public bool IsShuffle { get; private set; } = false;

        internal List<Music> Queue { get; set; } = new();

        protected static object LockObject { get; } = new object();

        public IQueueState State { get; private set; }

        public void ChangeLoopState()
        {
            this.State = this.State.Next;
        }

        public Music? Dequeue() => this.State.Dequeue(this);

        public List<Music> GetReadOnlyQueue() => new(this.Queue);

        public void Enqueue(Music music)
        {
            lock (LockObject)
            {
                Queue.Add(music);
            }
        }

        public bool SwitchShuffleState()
        {
            lock (LockObject)
            {
                this.IsShuffle = !this.IsShuffle;
                if (this.IsShuffle)
                {
                    Random r = new();
                    for (int i = 0; i < this.Queue.Count; i++)
                    {
                        var idx = r.Next(0, this.Queue.Count - i);
                        this.Queue.Add(this.Queue[idx]);
                        this.Queue.RemoveAt(idx);
                    }
                }
                else
                {
                    this.Queue.Sort();
                }
                return this.IsShuffle;
            }
        }

        public bool TryRemoveAt(int index, out string deleteMusicName)
        {
            lock (LockObject)
            {
                deleteMusicName = "";
                if (index > this.Queue.Count) { return false; }
                deleteMusicName = this.Queue[index].Title;
                this.Queue.RemoveAt(index);
                return true;
            }
        }
    }
}