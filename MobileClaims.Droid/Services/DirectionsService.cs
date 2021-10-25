using System.Globalization;
using Android.Content;
using MobileClaims.Core.Models;
using MobileClaims.Core.Services;
using Plugin.CurrentActivity;

namespace MobileClaims.Droid.Services
{
    public class DirectionsService : IDirectionsService
    {
        public bool ShowDirectionsInMaps(string address, Coordinates coordinatesTo)
        {
            var toLatitude = coordinatesTo.Latitude.ToString(CultureInfo.InvariantCulture);
            var toLongitude = coordinatesTo.Longitude.ToString(CultureInfo.InvariantCulture);

            var gmmIntentUri = Android.Net.Uri.Parse($"google.navigation:q={toLatitude},{toLongitude}");
            var intent = new Intent(Intent.ActionView);
            intent.SetData(gmmIntentUri);
            intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity");
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            return true;
        }
    }
}