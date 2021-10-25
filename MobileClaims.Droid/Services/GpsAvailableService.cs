using System.Threading.Tasks;
using MobileClaims.Core.Services;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MobileClaims.Droid.Services
{
    public class GpsAvailableService : IGpsAvailableService
    {
        public async Task<GeolocationStatus> GetStatus()
        {
            var isPermissionGranted = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);

            if (isPermissionGranted == PermissionStatus.Granted)
            {                
                var isServiceEnabled = CrossGeolocator.Current.IsGeolocationEnabled;
                if (isServiceEnabled)
                {
                    return GeolocationStatus.Available;
                }
                else
                {
                    return GeolocationStatus.FeatureDisabled;
                }
            }

            return GeolocationStatus.PermissionNotGranted;
        }
    }
}
