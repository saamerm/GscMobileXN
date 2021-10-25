using System.Threading.Tasks;
using MobileClaims.Core.Helpers;

namespace MobileClaims.Core.Services
{
    public interface IGeolocationService
    {
        Task<GeolocationResult> GetGeolocationAsync(bool ipGeolocationEnabled, int timeoutMiliseconds = 5000);
    }

    public enum GeolocationStatus
    {
        SuccessGps,
        SuccessIp,
        FeatureDisabled,
        PermissionNotGranted,
        UnableToGetLocation,
        Available
    }
}