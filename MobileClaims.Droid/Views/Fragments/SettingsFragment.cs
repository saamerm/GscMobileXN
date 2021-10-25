using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Droid.Services;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.ADD)]
    public class SettingsFragment_ : BaseFragment, IMvxView
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.SettingsView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var value = await (new BiometricsService()).BiometricsAvailable();
            TextView textView = this.Activity.FindViewById<TextView>(Resource.Id.setting_name);
            textView.Alpha = value ? 1 : 0.5f;

            var previousValue = this.Activity.FindViewById<Switch>(Resource.Id.setting_value).Checked;

            this.Activity.FindViewById<Switch>(Resource.Id.setting_value).Checked = previousValue && value;

            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            {
                this.Activity.FindViewById<Switch>(Resource.Id.setting_value).Enabled = value;
            }
        }
    }
}