using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Android.Runtime;
using MobileClaims.Core;
using MobileClaims.Droid.Views.Fragments;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;
using System;
using System.Threading;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MobileClaims.Droid
{
    [Application]
    public partial class Application : MvxAppCompatApplication<Setup, App>, Android.App.Application.IActivityLifecycleCallbacks
    {
        CancellationTokenSource token;

        public Application(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            System.Diagnostics.Debug.WriteLine("Executing custom application ctor!");
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);
            UserDialogs.Init(this);
            RegisterActivityLifecycleCallbacks(this);
            System.Diagnostics.Debug.WriteLine("setting activity resolver");
            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
            CrossFingerprint.SetDialogFragmentType<CustomFingerprintDialogFragment>();
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait;
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}