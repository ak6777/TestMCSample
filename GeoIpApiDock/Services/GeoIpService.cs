using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoIpApiDock.Services
{
    public class GeoIpService : IGeoIpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private readonly string _httpUrl;

        public GeoIpService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _httpUrl = configuration.GetValue<string>("API_URL");
        }

        public async Task<object> GetGeoIpInfoByIpAddress(string ipAddress)
        {
            object geoIpContent = null;

            var urlWithParam = string.Concat(_httpUrl, ipAddress);

            var httpClient = _httpClientFactory.CreateClient();
            using (var response = await httpClient.GetAsync(urlWithParam, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                geoIpContent = await JsonSerializer.DeserializeAsync<object>(stream, _options);
            }

            return geoIpContent;
        }
    }
}
