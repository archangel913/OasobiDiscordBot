using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IHttp
    {
        public Task<HttpResponseMessage> GetAsync(string request);
    }
}
