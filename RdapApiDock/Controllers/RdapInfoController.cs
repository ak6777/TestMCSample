using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RdapApiDock.Services;
using System;
using System.Threading.Tasks;

namespace RdapApiDock.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class RdapInfoController : ControllerBase
    {
        private readonly IRdapService _rdapService;

        public RdapInfoController(IRdapService rdapService)
        {
            _rdapService = rdapService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("api/v1/RdapInfo/GetRdapInfo")]
        public async Task<ActionResult<object>> GetRdapInfo(string ipAddress)
        {
            try
            {
                return await _rdapService.GetRdapInfoByIpAddress(ipAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
