using System.Threading.Tasks;
using MobileClaims.Core.Services;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MobileClaims.iOS.Services
{
    public class GpsAvailableService : IGpsAvailableService
    {
        public async Task<GeolocationStatus> GetStatus()
        {
            var isServiceEnabled = CrossGeolocator.Current.IsGeolocationEnabled;

            if (isServiceEnabled)
            {
                var isPermissionGranted = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                if (isPermissionGranted == PermissionStatus.Granted)
                {
                    return GeolocationStatus.Available;
                }
                else
                {
                    return GeolocationStatus.PermissionNotGranted;
                }
            }

            return GeolocationStatus.FeatureDisabled;
        }
    }
}
