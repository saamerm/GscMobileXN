using MobileClaims.Core.Services;
using Plugin.Geolocator.Abstractions;

namespace MobileClaims.Core.Helpers
{
    public class GeolocationResult
    {
        public GeolocationResult(GeolocationStatus status, Position position)
        {
            Status = status;
            Position = position;
        }

        public static GeolocationResult Failure(GeolocationStatus status)
        {
            return new GeolocationResult(status, null);
        }

        public static GeolocationResult SuccessIp(Position location)
        {
            return new GeolocationResult(GeolocationStatus.SuccessIp, location);
        }

        public static GeolocationResult SuccessGps(Position location)
        {
            return new GeolocationResult(GeolocationStatus.SuccessGps, location);
        }

        public GeolocationStatus Status { get; private set; }

        public Position Position { get; private set; }
    }
}