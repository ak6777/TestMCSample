using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RdapApiDock.Services
{
    public class RdapService : IRdapService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private readonly string _httpUrl;

        public RdapService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _httpUrl = configuration.GetValue<string>("API_URL");
        }

        public async Task<object> GetRdapInfoByIpAddress(string ipAddress)
        {
            object rdapContent = null;

            var urlWithParam = string.Concat(_httpUrl, ipAddress);

            var httpClient = _httpClientFactory.CreateClient();
            using (var response = await httpClient.GetAsync(urlWithParam, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                rdapContent = await JsonSerializer.DeserializeAsync<object>(stream, _options);
            }

            return rdapContent;
        }
    }
}
