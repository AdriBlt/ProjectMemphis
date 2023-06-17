using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneFlow.Data;

namespace OneFlow.Clients
{
    public class OneFlowClient : IOneFlowClient
    {
        private readonly string _baseUrl;

        private readonly string _httpHost;

        public OneFlowClient()
        {
            _baseUrl = "http://oneflow-int/api/1/";
            _httpHost = "oneflow-int";
        }

        public Task<OneFlowFetchResult> FetchGraph(Guid id) 
            => GetAsync<OneFlowFetchResult>($"graph/tsg/{id}");

        public Task<OneFlowSearchResult> SearchGraphs(string query) 
            => GetAsync<OneFlowSearchResult>($"graphs/tsg/searchByTag?tag={query}");

        private async Task<T> GetAsync<T>(string path)
        {
            using (var client = new HttpClient(new WebRequestHandler()))
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(_httpHost))
                {
                    client.DefaultRequestHeaders.Host = _httpHost;
                }

                using (var response = await client.GetAsync(path).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<T>(result);
                    }
                }
            }

            return default(T);
        }
    }
}