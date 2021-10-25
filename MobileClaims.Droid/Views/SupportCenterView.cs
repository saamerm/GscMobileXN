using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Presenter;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using Plugin.Permissions;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class SupportCenterView : ActivityBase, IMultiRegionHost
    {
        private bool _dialogShown;
        private readonly int[] _validRegionIds = {
            Resource.Id.phone_main_region,
            Resource.Id.nav_region,
        };
        private object _sync = new object();

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;
            if (backStackCount == 1)
            {
                ((SupportCenterViewModel)this.ViewModel).GoBackCommand.Execute(null);
                base.OnBackPressed();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new SupportCenterFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

            presenter.ShowBottomNavigation();
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
                    ft.Replace(Resource.Id.phone_main_region, fragment);
                }

                //MANAGE BACKSTACK BEHAVIOUR HERE
                int backstackBehaviour = fragment.GetBackstackBehaviour();

                if (backstackBehaviour == BackstackTypes.FIRST_ITEM)
                {
                    FragmentManager.PopBackStackImmediate(null, PopBackStackFlags.Inclusive);
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