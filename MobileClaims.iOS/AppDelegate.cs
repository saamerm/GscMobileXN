using CoreLocation;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MobileClaims.Core;
using MobileClaims.Core.Services;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Platform.Platform;
using MvvmCross.Platforms.Ios.Core;
using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using UIKit;
using Google.Maps;

namespace MobileClaims.iOS
{
    [Foundation.Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        // class-level declarations
        DateTime whenSentToBackGround;

        public override UIWindow Window { get; set; }

        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        //public override void WillEnterForeground(UIApplication application)
        //{
        //    base.WillEnterForeground(application);
        //    var loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
        //    if ((DateTime.Now - loginservice.LastLogin).TotalMinutes >= 15)
        //    {
        //        loginservice.Logout();
        //    }
        //}

        //public override void DidEnterBackground(UIApplication application)
        //{
        //    base.DidEnterBackground(application);
        //    var loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
        //    loginservice.LastLogin = DateTime.Now;
        //    whenSentToBackGround = DateTime.Now;
        //}

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<Setup>(this.GetType().Assembly);
        }

        public override void ReceiveMemoryWarning(UIApplication application)
        {
            MvxTrace.Trace("GC in AppDelegate ReceiveMemoryWarning");
            GC.Collect();
            //base.ReceiveMemoryWarning(application);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
#if DEBUG
            // Code for starting up the Xamarin Test Cloud Agent
            // This is not necessary for Android applications
            Xamarin.Calabash.Start();
#endif
            StartAppCenterServices();

            // TODO: Uncomment following code
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            var result = base.FinishedLaunching(application, launchOptions);

            //var setup = new Setup();
            ////setup.Initialize();

            var loginService = Mvx.IoCProvider.Resolve<ILoginService>();
            loginService.IsLoggedIn = false;
            loginService.Logout();

            // TODO: Fix the issue where Mvx.IoCProvider is null
            var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
            startup.Start();

            var key = Mvx.IoCProvider.Resolve<IConfigurationService>().GetMapsApiKey();
            MapServices.ProvideAPIKey(key);

            Mvx.IoCProvider.Resolve<IDeviceService>().IsTablet = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;

            Window.MakeKeyAndVisible();
            return result;
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