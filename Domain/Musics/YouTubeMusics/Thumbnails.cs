using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Musics.YouTubeMusics
{
    public class Thumbnails
    {
        // can't use "default"
        public Thumbnail medium { get; set; }

        public Thumbnail high { get; set; }

        public Thumbnail standard { get; set; }

        public Thumbnail maxres { get; set; }
    }
}