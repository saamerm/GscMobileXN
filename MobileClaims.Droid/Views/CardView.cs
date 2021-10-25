using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.Messages;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Presenter;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class CardView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = new int[] { Resource.Id.right_region, Resource.Id.left_region, Resource.Id.phone_main_region, Resource.Id.nav_region, };

        public static ScaleGestureDetector scaleGestureDetector;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == 1204)
            {
                var messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
                if (grantResults[0] == Permission.Granted)
                {
                    messenger.Publish(new PermissionsStorageGrantedMessage(this));
                }
                else
                {
                    //permission denied
                    messenger.Publish(new PermissionsStorageDeniedMessage(this));
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Set the layout
            SetContentView(Resource.Layout.MainLayout);
            //RequestedOrientation = ScreenOrientation.User;
            // must call base implementation to create the activity before starting MvxApp.
            base.OnCreate(savedInstanceState);
        }

        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount == 1)
            {
                ((CardViewModel)this.ViewModel).GoBackCommand.Execute(null);
            }
            base.OnBackPressed();
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new CardViewFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

            FrameLayout navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);
            if (!((CardViewModel)ViewModel).FromLoginScreen)
            {
                navRegion.Visibility = ViewStates.Visible;
                presenter.ShowBottomNavigation();
            }
            else
            {
                navRegion.Visibility = ViewStates.Gone;
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
            if (IsLandingPageView(fragment))
                return;
            
            Show(fragment, true);
        }

        protected void Show(MvxFragment fragment, bool backStack)
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
                ft.Add(Resource.Id.phone_main_region, fragment);
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