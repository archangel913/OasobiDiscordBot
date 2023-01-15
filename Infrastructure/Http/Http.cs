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
        private readonly HttpClient HttpClient;

        public Http()
        {
            this.HttpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string request)
        {
            return await this.HttpClient.GetAsync(request);
        }
    }
}
