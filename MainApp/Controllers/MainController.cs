using MainApp.Models;
using MainApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MainApp.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMultiService _multiSvc;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MainController(IMultiService multiService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _multiSvc = multiService;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<object>> GetAggregatedInfo(string ipAddress, List<string> services = null)
        {
            try
            {
                IpInfo aggregatedInfo = new IpInfo();
                services = services.Select(x => x.ToUpper()).ToList();

                services ??= new string[] { "GEOIP", "RDAP" }.ToList();

                if (_multiSvc.ValidateIPv4(ipAddress))
                {
                    string httpUrl = null;
                    if (services.Contains("GEOIP"))
                    {
                        httpUrl = string.Concat(_configuration.GetValue<string>("GEOIP_URL"), string.Format("?ipAddress={0}", ipAddress));
                        aggregatedInfo.GeoIp = await ProxyTo(httpUrl);
                    }

                    if (services.Contains("RDAP"))
                    {
                        httpUrl = string.Concat(_configuration.GetValue<string>("RDAP_URL"), string.Format("?ipAddress={0}", ipAddress));
                        aggregatedInfo.RDAP = await ProxyTo(httpUrl);
                    }
                }

                return aggregatedInfo;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<ActionResult<object>> ProxyTo(string url)
        {
            var obj = await _httpClient.GetStringAsync(url);
            return obj;
        }
    }
}
