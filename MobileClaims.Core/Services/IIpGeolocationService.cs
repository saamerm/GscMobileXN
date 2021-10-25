using System.Threading.Tasks;
using MobileClaims.Core.Helpers;

namespace MobileClaims.Core.Services
{
    public interface IIpGeolocationService
    {
        Task<GeolocationResult> GetIpLocation();
    }
}
