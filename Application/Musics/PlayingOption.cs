using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Musics
{
    public class PlayingOption
    {
        public PlayingOption(bool isShuffle, IQueueState queueState, double volume)
        {
            this.IsShuffle = isShuffle;
            this.QueueState = queueState;
            this.Volume = volume;
        }

        public bool IsShuffle { get; }

        public IQueueState QueueState { get; }

        public double Volume { get; }
    }
}
