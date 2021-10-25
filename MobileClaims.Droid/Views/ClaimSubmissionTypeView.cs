using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Plugin.Messenger;
using Plugin.Permissions;
using System;
using MobileClaims.Droid.Presenter;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class ClaimSubmissionTypeView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = { Resource.Id.phone_main_region, Resource.Id.nav_region };
        private bool dialogShown;
        private IMvxMessenger _messenger;
        private IMvxMessenger _messenger1;
        private object _sync = new object();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);

            if (!Mvx.IoCProvider.Resolve<IRehydrationService>().Rehydrating)
            {
                _messenger.Publish(new ClaimSubmissionStarted(this));
            }

            _messenger1 = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _messenger1.Subscribe<NotEligibleForVisionClaimsMessage>((message =>
            {
                ShowNotEligibleForVisionClaimsMessage();
            }));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            Intent i = new Intent("mobileclaims.droid.CONFIG_CHANGED");
            SendBroadcast(i);
        }

        public override void OnBackPressed()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;
            if (backStackCount == 1)
            {
                ((ClaimSubmissionTypeViewModel)ViewModel).GoBackCommand.Execute(null);
                base.OnBackPressed();
                Finish();
            }
            else
            {
                var fragmentTag = FragmentManager.GetBackStackEntryAt(backStackCount - 1).Name;

                if (fragmentTag.Contains("ClaimSubmissionResultView"))
                {
                    ((ClaimSubmissionTypeViewModel)ViewModel).GoBackCommand.Execute(null);
                    base.OnBackPressed();
                    Finish();
                }
                else
                {
                    base.OnBackPressed();
                }
            }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new ClaimSubmissionTypeFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

            CustomFragmentsBackStack.Add(cvf);

            presenter.ShowBottomNavigation();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ((ClaimSubmissionTypeViewModel)ViewModel).FragmentActivityBuilt?.Invoke(this, EventArgs.Empty);
        }

        public void ShowNotEligibleForVisionClaimsMessage()
        {
            if (!dialogShown)
            {

                RunOnUiThread(() =>
                {
                    dialogShown = true;
                    Resources res = Resources;
                    var builder = new AlertDialog.Builder(this);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
                    builder.SetMessage(string.Format(res.GetString(Resource.String.notEligibleVisionClaimsMessage)));
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                    {
                        dialogShown = false;
                    });
                    builder.Show();

                });
            }
        }

        [Export("errorClickHandler")] // The value found in android:onClick attribute.
        public void errorClickHandler(View v) // Does not need to match value in above attribute.
        {
            if (v.Tag != null)
            {
                var toast = Toast.MakeText(this, v.Tag.ToString(), ToastLength.Long);
                toast.SetGravity(Gravity.GetAbsoluteGravity(GravityFlags.Center, GravityFlags.Center), 0, 0);
                toast.Show();
            }
        }

        public void Show(MvxFragment fragment)
        {
            Show(fragment, true);
        }

        protected void Show(MvxFragment fragment, bool backStack)
        {
            lock (_sync)
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
                    ft.Replace(Resource.Id.phone_main_region, fragment, fragment.Class.Name);
                }

                //MANAGE BACKSTACK BEHAVIOUR HERE
                int backstackBehaviour = fragment.GetBackstackBehaviour();

                if (backstackBehaviour == BackstackTypes.FIRST_ITEM)
                {
                    FragmentManager.PopBackStack(null,PopBackStackFlags.Inclusive);
                    ft.AddToBackStack(fragment.Class.Name);
                }
                else if (backstackBehaviour == BackstackTypes.ADD)
                {
                    ft.AddToBackStack(fragment.Class.Name);
                }

                ft.Commit();
            }
        }

        /// <summary>
        /// Closes the view associated with the given ViewModel.
        /// Called from the IMultiRegionPresenter.
        /// </summary>
        /// <param name="viewModel"></param>
        public void CloseViewModel(IMvxViewModel viewModel)
        {
            var fragment = GetActiveFragmentForViewModel(viewModel);
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
        }

        /// <summary>
        /// Closes all active views.
        /// Called from the IMultiRegionPresenter.
        /// </summary>
        public void CloseAll()
        {
            MvxFragment fragment;
            fragment = FragmentManager.FindFragmentById(Resource.Id.right_region) as MvxFragment;
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
            fragment = FragmentManager.FindFragmentById(Resource.Id.left_region) as MvxFragment;
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
            fragment = FragmentManager.FindFragmentById(Resource.Id.phone_main_region) as MvxFragment;
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

        private void CloseFragment(MvxFragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Remove(fragment);
            ft.Commit();
        }
    }
}