using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Musics.YouTubeMusics.YoutubePlaylists
{
    public class YoutubePlaylists : YoutubeApiStructure
    {
        public IList<PlaylistItems> items { get; set; }
    }
}
