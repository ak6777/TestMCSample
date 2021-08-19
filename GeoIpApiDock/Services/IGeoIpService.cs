using System.Threading.Tasks;

namespace GeoIpApiDock.Services
{
    public interface IGeoIpService
    {
        Task<object> GetGeoIpInfoByIpAddress(string ipAddress);
    }
}