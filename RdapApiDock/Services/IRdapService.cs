using System.Threading.Tasks;

namespace RdapApiDock.Services
{
    public interface IRdapService
    {
        Task<object> GetRdapInfoByIpAddress(string ipAddress);
    }
}