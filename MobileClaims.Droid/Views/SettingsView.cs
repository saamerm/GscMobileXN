using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Services;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using System;
using MobileClaims.Droid.Presenter;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class SettingsView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = { Resource.Id.phone_main_region, Resource.Id.nav_region };
        public void CloseAll()
        {
            var fragment = FragmentManager.FindFragmentById(Resource.Id.phone_main_region) as MvxFragment;
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
        }

        private MvxFragment GetActiveFragmentForViewModel(IMvxViewModel viewModel)
        {
            foreach (int regionId in _validRegionIds)
            {
                MvxFragment fragment = FragmentManager.FindFragmentById(regionId) as MvxFragment;
                if (fragment != null && fragment.ViewModel == viewModel)
                    return fragment;
            }

            return null;
        }

        public void CloseViewModel(IMvxViewModel viewModel)
        {
            var fragment = GetActiveFragmentForViewModel(viewModel);
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
        }

        public void Show(MvxFragment fragment)
        {
            int regionResourceId = fragment.GetRegionId();
            bool hidesNav = fragment.GetHidesNav();

            //NavRegion RegionAttribute DICTATES WHETHER THE NAVIGATION REGION SHOULD SHOW OR NOT
            FrameLayout navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);

            if (hidesNav)
            {
                navRegion.Visibility = ViewStates.Gone;
            }
            else
            {
                navRegion.Visibility = ViewStates.Visible;
            }

            var ft = FragmentManager.BeginTransaction();
            //ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit, Resource.Animation.popenter, Resource.Animation.popexit);

            if (regionResourceId == Resource.Id.nav_region)
            {
                ft.Replace(Resource.Id.nav_region, fragment);
            }
            else
            {
                ft.Replace(Resource.Id.phone_main_region, fragment);
            }

            //MANAGE BACKSTACK BEHAVIOUR HERE
            int backstackBehaviour = fragment.GetBackstackBehaviour();

            if (backstackBehaviour == BackstackTypes.FIRST_ITEM)
            {
                FragmentManager.PopBackStack(null,PopBackStackFlags.Inclusive);
                ft.AddToBackStack(null);
            }
            else if (backstackBehaviour == BackstackTypes.ADD)
            {
                ft.AddToBackStack(null);
            }

            ft.Commit();
        }

        private void CloseFragment(MvxFragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Remove(fragment);
            ft.Commit();
        }

        protected override void OnCreate(Bundle bundle)
        {
            SetContentView(Resource.Layout.MainLayout);
            RequestedOrientation = ScreenOrientation.User;
            base.OnCreate(bundle);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var settingsFragment = new SettingsFragment_
            {
                ViewModel = ViewModel
            };
            Show(settingsFragment);

            FrameLayout navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);
            navRegion.Visibility = ViewStates.Visible;

            presenter.ShowBottomNavigation();
        }

        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount == 1)
            {
                ((SettingsViewModel)this.ViewModel).GoBackCommand.Execute(null);
            }
            base.OnBackPressed();
        }

        [Export("setting_value_click_handler")]
        public void setting_value_click_handler(View v)
        {
            var bioAvailableTask = (new BiometricsService()).BiometricsAvailable();
            bioAvailableTask.Wait();
            var value = bioAvailableTask.Result;

            if (value)
            {
                return;
            }

            var previousValue = this.FindViewById<Switch>(Resource.Id.setting_value).Checked;

            //this.FindViewById<Switch>(Resource.Id.setting_value).Checked = previousValue && value;

            this.RunOnUiThread(() =>
            {
                this.FindViewById<Switch>(Resource.Id.setting_value).Checked = false;

                var builder = new AlertDialog.Builder(this);
                builder.SetMessage(Resource.String.fingerprintSettingMessageContent);
                builder.SetCancelable(true);

                builder.SetNegativeButton(Resource.String.OK, (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();

                // Show the dialog. This is important to do before accessing the buttons.
                dialog.Show();

                // Get the buttons.                
                var okBtn = dialog.GetButton((int)DialogButtonType.Negative);

                // Assign our handlers.              
                okBtn.Click += (sender, args) =>
                {
                    GC.Collect();
                    // dialogShown = false;
                    dialog.Dismiss();
                                        
                    this.FindViewById<Switch>(Resource.Id.setting_value).Checked = previousValue && value;
                };
            });
        }
    }
}