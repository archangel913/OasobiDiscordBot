using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Musics
{
    public class Music : IComparable<Music>
    {
        public Music(string title, string url)
        {
            if (!url.Contains("https://www.youtube.com/"))
            {
                throw new ArgumentException("source is not youtube url in Music class.");
            }
            this.Id = NextId;
            this.Title = title;
            this.Url = url;
            NextId++;
        }

        public int CompareTo(Music? other)
        {
            if (other is null) return 0;
            return this.Id - other.Id;
        }

        static private int NextId = 0;

        public int Id { get; }

        public string Title { get; }

        public string Url { get; }
    }
}
