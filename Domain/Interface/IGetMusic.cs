using Domain.Musics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IGetMusic
    {
        Task<List<Music>> GetMusicsAsync(string url);
    }
}
