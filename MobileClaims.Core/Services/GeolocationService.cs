using System;
using System.Threading.Tasks;
using MobileClaims.Core.Helpers;
using MvvmCross;
using MvvmCross.Logging;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace MobileClaims.Core.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IMvxLog _log;
        private readonly IGpsAvailableService _gpsAvailableService;
        private readonly IIpGeolocationService _ipGeolocationService;
        private readonly IGeolocator _geolocator = CrossGeolocator.Current;

        public GeolocationService(
            IGpsAvailableService gpsAvailableService,
            IIpGeolocationService ipGeolocationService,
            IMvxLog log)
        {
            _gpsAvailableService = gpsAvailableService;
            _ipGeolocationService = ipGeolocationService;
            _log = log;
        }


        public Task<GeolocationResult> GetGeolocationAsync(bool ipGeolocationEnabled = true, int timeoutMiliseconds = 5000)
        {
            return ProcessGeoLocationResult(ipGeolocationEnabled, timeoutMiliseconds);
        }

        private async Task<GeolocationResult> ProcessGeoLocationResult(bool ipGeolocationEnabled, int timeoutMiliseconds)
        {

#if DEBUGTestCloud
            var position = new Position(43.6477560, -79.3918450);
            return GeolocationResult.Success(position);
#endif
            try
            {
                var gpsLocationResult = await FetchGpsLocation(() => _geolocator.GetPositionAsync(TimeSpan.FromMilliseconds(timeoutMiliseconds)));
                if (!ipGeolocationEnabled 
                    || gpsLocationResult.Status == GeolocationStatus.SuccessIp 
                    || gpsLocationResult.Status == GeolocationStatus.SuccessGps)
                {
                    return gpsLocationResult;
                }

                var ipLocationResult = await _ipGeolocationService.GetIpLocation();
                if (ipLocationResult?.Position == null)
                {
                    return GeolocationResult.Failure(GeolocationStatus.UnableToGetLocation);
                }

                return GeolocationResult.SuccessIp(new Position(ipLocationResult.Position.Latitude, ipLocationResult.Position.Longitude));

            }
            catch (Exception)
            {
                return GeolocationResult.Failure(GeolocationStatus.UnableToGetLocation);
            }
        }

        private async Task<GeolocationResult> FetchGpsLocation(Func<Task<Position>> getLocationFunc)
        {
            var status = await _gpsAvailableService.GetStatus();
            if (status != GeolocationStatus.Available)
            {
                return GeolocationResult.Failure(status);
            }

            try
            {
                var location = await getLocationFunc.Invoke();
                if (location == null)
                {
                    return GeolocationResult.Failure(GeolocationStatus.UnableToGetLocation);
                }

                return GeolocationResult.SuccessGps(location);
            }
            catch (TaskCanceledException ex)
            {
                _log.Warn($"Time-out on fetching location. Exception: {ex.Message}");
                return GeolocationResult.Failure(GeolocationStatus.UnableToGetLocation);
            }
        }
    }
}
