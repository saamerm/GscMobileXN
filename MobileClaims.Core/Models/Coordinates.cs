namespace MobileClaims.Core.Models
{
    public class Coordinates
    {
        public Coordinates(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
