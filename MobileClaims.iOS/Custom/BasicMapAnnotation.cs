using CoreLocation;
using MapKit;

namespace MobileClaims.iOS.Custom
{
    public class BasicMapAnnotation : MKAnnotation
    {
        public override CLLocationCoordinate2D Coordinate { get; }
        public override string Title { get; }
        public override string Subtitle { get; }

        public BasicMapAnnotation(CLLocationCoordinate2D coordinate, string title, string subtitle)
        {
            Coordinate = coordinate;
            Title = title;
            Subtitle = subtitle;
        }
    }
}