using Domain.Interface;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using VideoLibrary;

namespace Infrastructure.Http
{
    internal class Http : IHttp
    {
        public async Task<HttpResponseMessage> GetAsync(string request)
        {
            using var client = new HttpClient();
            return await client.GetAsync(request);
        }
    }
}
