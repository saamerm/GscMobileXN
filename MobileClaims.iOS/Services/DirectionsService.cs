using System.Collections.Generic;
using CoreLocation;
using MapKit;
using MobileClaims.Core.Models;
using MobileClaims.Core.Services;

namespace MobileClaims.iOS.Services
{
    public class DirectionsService : IDirectionsService
    {
        public bool ShowDirectionsInMaps(string address, Coordinates coordinatesTo = null)
        {
            var mapItems = new List<MKMapItem>();
            mapItems.Add(MKMapItem.MapItemForCurrentLocation());

            var destinationMapItem =
                new MKMapItem(
                    new MKPlacemark(new CLLocationCoordinate2D(coordinatesTo.Latitude, coordinatesTo.Longitude)));

            if (!string.IsNullOrEmpty(address))
                destinationMapItem.Name = address;

            mapItems.Add(destinationMapItem);
            var launchOptions = new MKLaunchOptions { DirectionsMode = MKDirectionsMode.Default };

            return MKMapItem.OpenMaps(mapItems.ToArray(), launchOptions);
        }
    }
}
