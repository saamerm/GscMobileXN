
using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.Droid.Presenter;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class DirectDepositView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = {
            Resource.Id.phone_main_region,
            Resource.Id.nav_region,
        };

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);
        }

        public override void OnBackPressed()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;
            if (backStackCount == 1)
            {
                ((DirectDepositViewModel)this.ViewModel).GoBackCommand.Execute(null);
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

            var cvf = new DirectDepositViewFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);
            CustomFragmentsBackStack.Add(cvf);
            presenter.ShowBottomNavigation();
        }

        public void Show(MvxFragment fragment)
        {
            if (IsLandingPageView(fragment))
                return;

            Show(fragment, true);
        }

        protected void Show(MvxFragment fragment, bool backStack)
        {
            if (!fragment.HasRegionAttribute())
            {
                throw new InvalidOperationException("Fragment has no region attribute.");
            }

            int regionResourceId = fragment.GetRegionId();
            bool hidesNav = fragment.GetHidesNav();

            if (!_validRegionIds.Contains(regionResourceId))
            {
                throw new InvalidOperationException("Id specified in resource attribute is invalid.");
            }

            var navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);

            if (hidesNav)
            {
                navRegion.Visibility = ViewStates.Gone;
            }
            else
            {
                navRegion.Visibility = ViewStates.Visible;
            }

            var ft = FragmentManager.BeginTransaction();

            /*//ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit, Resource.Animation.popenter,
                    Resource.Animation.popexit);*/

            ////ft.SetCustomAnimations(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);

            if (regionResourceId == Resource.Id.nav_region)
            {
                ft.Replace(Resource.Id.nav_region, fragment);
            }
            else
            {
                ft.Add(Resource.Id.phone_main_region, fragment);
            }

            int backstackBehaviour = fragment.GetBackstackBehaviour();

            if (backstackBehaviour == BackstackTypes.FIRST_ITEM)
            {
                FragmentManager.PopBackStack(null, PopBackStackFlags.Inclusive);
                ft.AddToBackStack(fragment.Class.Name);
            }
            else if (backstackBehaviour == BackstackTypes.ADD)
            {
                ft.AddToBackStack(fragment.Class.Name);
            }

            ft.CommitAllowingStateLoss();
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
