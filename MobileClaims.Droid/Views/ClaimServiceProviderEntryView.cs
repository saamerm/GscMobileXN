using System;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimServiceProviderEntryView : BaseFragment
    {
        private ClaimServiceProviderEntryViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimServiceProviderEntryView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimServiceProviderEntryViewModel)ViewModel;

            Button useThisServiceProvider = Activity.FindViewById<Button>(Resource.Id.useThisServiceProvider);
            useThisServiceProvider.Text = string.Format(Resources.GetString(Resource.String.claimServiceProviderEntryUsetThisLabel), (_model.ClaimSubmissionType.ID).ToLower());

            if (Resources.GetBoolean(Resource.Boolean.isTablet))
            {
                _model.CloseServiceProviderEntryPopup += HandleCloseServiceProviderEntryPopup;
            }
        }

        void HandleCloseServiceProviderEntryPopup(object sender, EventArgs e)
        {
            Activity.RunOnUiThread(() =>
                {
                    //FrameLayout popup_background = this.Activity.FindViewById<FrameLayout>(Resource.Id.popup_background);
                    //popup_background.Visibility = ViewStates.Gone;
                    Activity.FragmentManager.PopBackStack();
                }
            );
        }
    }
}