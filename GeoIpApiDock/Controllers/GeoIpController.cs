using GeoIpApiDock.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GeoIpApiDock.Controllers
{
    [Produces("application/json")]
    //[Route("api/v1/[controller]")]
    [ApiController]
    public class GeoIpController : ControllerBase
    {
        private readonly IGeoIpService _geoIpService;

        public GeoIpController(IGeoIpService geoIpService)
        {
            _geoIpService = geoIpService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("api/v1/GeoIp/GetGeoIpInfo")]
        public async Task<ActionResult<object>> GetGeoIpInfo(string ipAddress)
        {
            try
            {
                return await _geoIpService.GetGeoIpInfoByIpAddress(ipAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
