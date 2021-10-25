using System;

namespace MobileClaims.Core.Helpers
{
    public static class MetricConvertionHelper
    {
        public static double KilometresToLatitudeDegrees(this double kms)
        {
            double earthRadius = 6371.0; // in kms
            double radiansToDegrees = 180.0 / Math.PI;
            return (kms / earthRadius) * radiansToDegrees;
        }

        /// <summary>Converts kilometres to longitudinal degrees at a specified latitude</summary>
        public static double KilometresToLongitudeDegrees(this double kms, double atLatitude)
        {
            double earthRadius = 6371.0; // in kms
            double degreesToRadians = Math.PI / 180.0;
            double radiansToDegrees = 180.0 / Math.PI;
            // derive the earth's radius at that point in latitude
            double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
            return (kms / radiusAtLatitude) * radiansToDegrees;
        }
    }
}
