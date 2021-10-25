using System;

namespace MobileClaims.Core.Helpers
{
    public class GeolocationHelper
    {
        private const int EarthRadiusInKm = 6378;

        public static double ConvertLatitudeDifferenceToKm(double eastLatitude, double westLatitude)
        {
            double latitudeDifference = Math.Abs(eastLatitude - westLatitude);


            var km = latitudeDifference * EarthRadiusInKm / 180 * Math.PI;

            return km;
        }
    }
}