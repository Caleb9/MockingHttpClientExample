using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi
{
    public class HttpClientIsolationService
    {
        private readonly HttpClient _httpClient;

        public HttpClientIsolationService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetMessage()
        {
            var response = await _httpClient.GetAsync("http://fakeuri");
            return await response.Content.ReadAsStringAsync();
        }
    }
}