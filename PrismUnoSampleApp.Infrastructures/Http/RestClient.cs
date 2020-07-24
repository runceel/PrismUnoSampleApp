using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.Infrastructures.Http
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;

        public RestClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public Task<HttpResponseMessage> GetAsync(string uri) => _client.GetAsync(uri);
        public Task<HttpResponseMessage> PostAsync(string uri, HttpContent content) => _client.PostAsync(uri, content);
    }
}
