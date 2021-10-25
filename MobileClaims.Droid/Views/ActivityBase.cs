using Android.Content;
using Android.Content.PM;
using Android.OS;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using Microsoft.AppCenter.Analytics;
using FluentValidation.Internal;
using MobileClaims.Droid.Interfaces;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Droid.Views
{
    public class ActivityBase : MvxActivity
    {
        private DateTime _lastSeen;

        public List<MvxFragment> CustomFragmentsBackStack { get; } = new List<MvxFragment>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Analytics.TrackEvent("PageView: " + this.GetType().Name.Replace("Activity", string.Empty).SplitPascalCase());
            Xamarin.Essentials.Platform.Init(this, bundle); // add this line to your code, it may also be called: bundle

            RequestedOrientation = ScreenOrientation.Portrait;
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            var loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            if (level == TrimMemory.Background || level == TrimMemory.UiHidden)
            {
                loginservice.LastLogin = DateTime.Now;
            }

            base.OnTrimMemory(level);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            if (!(this is FindHealthProviderView) && CustomFragmentsBackStack.Count > 0)
            {
                // FindHealthProviderView has its own implementation of OnBackPressed
                CustomFragmentsBackStack.Remove(CustomFragmentsBackStack.Last());
            }
        }

        protected override void OnResume()
        {
            var resources = Resources;
			var configuration = resources.Configuration;
			var locale = configuration.Locale;
			var langService = Mvx.IoCProvider.Resolve<ILanguageService>();

            if (!langService.CurrentLanguage.Contains(locale.Language))
            {
                string loc = (locale.ToString()).Replace("_", "-");
                langService.SetCurrentLanguage(loc);
            }

            var loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            _lastSeen = loginservice.LastLogin;
            if (_lastSeen == DateTime.MinValue)
            {
                base.OnResume();
                return;
            }
            if(_lastSeen != DateTime.MinValue && DateTime.Now > _lastSeen.AddMinutes(15))
            {
                var loginService = Mvx.IoCProvider.Resolve<ILoginService>();
                loginService.Logout(); //we hit this after logging in!!  Not good
                loginservice.LastLogin = DateTime.MinValue;
            }
            base.OnResume();
        }

        /// <summary>
        /// Function to solve the issue where the app was crashing when you would tap the Menu button twice
        /// </summary>
        /// <param name="fragment">The fragment that the app is trying to open</param>
        /// <returns>True if the app was trying too open the Menu page again</returns>
        public bool IsLandingPageView(MvxFragment fragment)
        {
            if (fragment is LandingPageView)
            {
                // If the selected activity is trying to open the menu page, just quit that activity and the menu page will automatically display
                ((ViewModelBase)this.ViewModel).GoBackCommand.Execute(null);
                return true;
            }
            return false;
        }
    }
}