using UIKit;

namespace MobileClaims.iOS
{
	public static class Helpers
	{
		public static bool IsInPortraitMode()
		{
			return UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.Portrait || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.PortraitUpsideDown;			
		}

		public static bool IsInLandscapeMode()
		{
			return UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight;			
		}

		public static float BottomNavHeight()
		{
            float navSize = Constants.NAV_BUTTON_SIZE_IPHONE;

            if (Constants.Bottom > 0)
            {
                navSize += Constants.Bottom / 2;
            }

            return Constants.IsPhone () ? navSize : Constants.NAV_BUTTON_SIZE_IPAD;
		}

		public static float StatusBarHeight()
		{
			return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
		}

        public static float OrientationAwareScreenWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(8, 0))
                return (float)UIScreen.MainScreen.Bounds.Width;
            else
                return IsInPortraitMode() ? (float)UIScreen.MainScreen.Bounds.Width : (float)UIScreen.MainScreen.Bounds.Height;
        }

        public static float OrientationAwareScreenHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(8, 0))
                return (float)UIScreen.MainScreen.Bounds.Height;
            else
                return IsInPortraitMode() ? (float)UIScreen.MainScreen.Bounds.Height : (float)UIScreen.MainScreen.Bounds.Width;
        }  

		public static float OrientationAwareViewHeight()
		{
			return OrientationAwareScreenHeight() - BottomNavHeight() - StatusBarHeight();
		}
	}
}

