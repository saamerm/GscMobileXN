using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MobileClaims.Core;
using MvvmCross.Platforms.Android.Views;

namespace MobileClaims.Droid
{
    [Activity(
        MainLauncher = true
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /*protected override void TriggerFirstNavigate()
		{
			// override the default implementation to start the MainActivity, the host for the IMultiRegionPresenter.
			// when the activity is created it will start the initial navigation of the MvxApp.

			StartActivity(typeof(MainActivity));

			// do not call the base implementation
			//base.TriggerFirstNavigate();
		}*/

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            StartAppCenterServices();
        }

        private void StartAppCenterServices()
        {
            if (!string.IsNullOrEmpty(GSCHelper.AppCenterSecret))
            {
                AppCenter.Start(GSCHelper.AppCenterSecret, typeof(Analytics), typeof(Crashes));
            }
        }
    }
}