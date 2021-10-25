using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IGpsAvailableService
    {
        Task<GeolocationStatus> GetStatus();
    }
}
