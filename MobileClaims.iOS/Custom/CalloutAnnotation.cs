using CoreLocation;
using MapKit;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS.Custom
{
    public class CalloutAnnotation : MKAnnotation
    {
        public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }
        public CLLocationCoordinate2D Coords;

        public CalloutAnnotation(ServiceProvider provider)
        {
            if (provider != null)
            {
                Provider = provider;
                var coord = new CLLocationCoordinate2D(provider.Latitude, provider.Longitude);
                this.Coords = coord;
            }
        }

        private ServiceProvider _provider;
        public ServiceProvider Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                _provider = value;
            }
        }
    }
}