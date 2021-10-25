using Foundation;
using MobileClaims.Core.Services;
using UIKit;

namespace MobileClaims.iOS.Services
{
	public class IosNativeUrlService : INativeUrlService
    {
		public void OpenUrl(string url)
		{
            var nsUrl = new NSUrl(url);
            if (UIApplication.SharedApplication.CanOpenUrl(nsUrl))
            {
                UIApplication.SharedApplication.OpenUrl(nsUrl);
            }
		}
    }
}
