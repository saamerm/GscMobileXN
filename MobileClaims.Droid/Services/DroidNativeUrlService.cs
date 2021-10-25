using Android.Content;
using Android.Util;
using MobileClaims.Core.Services;
using Plugin.CurrentActivity;

namespace MobileClaims.Droid.Services
{
	public class DroidNativeUrlService : INativeUrlService
    {
        public void OpenUrl(string url)
        {
            try
            {
                Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                CrossCurrentActivity.Current.Activity.StartActivity(browserIntent);
            }
            catch(ActivityNotFoundException e)
            {
                Log.Error("DroidNativeUrlService", e.ToString());
            }
        }
    }
}
