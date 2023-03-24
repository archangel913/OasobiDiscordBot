using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models
{
    internal class OperatingTime
    {
        public int Second { get; private set; }

        public int Minute { get; private set; }

        public int Hour { get; private set; }

        public void Increment()
        {
            if (this.Second == 59 && this.Minute == 59)
            {
                this.Second = 0;
                this.Minute = 0;
                this.Hour++;
            }
            else if (this.Second == 59)
            {
                this.Second = 0;
                this.Minute++;
            }
            else
            {
                this.Second++;
            }
        }

        public override string ToString()
        {
            return $"{this.Hour:00}:{this.Minute:00}:{this.Second:00}";
        }
    }
}
