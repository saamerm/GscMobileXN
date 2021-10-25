using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using System.Collections.Generic;
using System.Linq;
using MobileClaims.Droid.Presenter;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class ConfirmationOfPaymentUploadView : ActivityBase, IMultiRegionHost
    {
        ConfirmationOfPaymentUploadViewModel vm;

        private readonly int[] _validRegionIds = new int[]
        {
            Resource.Id.right_region,
            Resource.Id.left_region,
            Resource.Id.nav_region,
            Resource.Id.phone_main_region
        };

        private View _transparentPopupView;
        private List<MvxFragment> CustomFragmentsBackStack { get; } = new List<MvxFragment>();

        protected override void OnResume()
        {
            base.OnResume();
            vm.ActivityIsPaused = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            vm.ActivityIsPaused = true;
        }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);
            vm = (ConfirmationOfPaymentUploadViewModel)ViewModel;

            var cvf = new ConfirmationOfPaymentUploadViewFragment_
            {
                ViewModel = (ConfirmationOfPaymentUploadViewModel)ViewModel
            };
            Show(cvf, true);

            presenter.ShowBottomNavigation();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;
            int prevbackStackCount = backStackCount;

            var lastFragment = (MvxFragment)FragmentManager.Fragments.Last();

            if (lastFragment is ConfirmationOfPaymentCompletedView confirmationOfPaymentCompletedView)
            {
                ((ConfirmationOfPaymentCompletedViewModel)(confirmationOfPaymentCompletedView.ViewModel)).BackToMyClaimsCommand.Execute();
                return;
            }

            if (CustomFragmentsBackStack.Count == 2)
            {
                FragmentManager.PopBackStack();
                CloseAll();
                ((ConfirmationOfPaymentUploadViewModel)ViewModel).GoBackCommand.Execute(null);
                base.OnBackPressed();
                return;
            }

            CustomFragmentsBackStack.Remove(CustomFragmentsBackStack.Last());

            if (backStackCount == 1)
            {
                CloseAll();
                ((ConfirmationOfPaymentUploadViewModel)ViewModel).GoBackCommand.Execute(null);
                base.OnBackPressed();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        private void CloseSplitFragments()
        {
            int backStackCount = FragmentManager.BackStackEntryCount;

            for (int i = backStackCount; i > 0; i--)
            {
                var fragmentTag = FragmentManager.GetBackStackEntryAt(i - 1).Name;
                var currentFragment = (MvxFragment)FragmentManager.FindFragmentByTag(fragmentTag);
                int regionResourceId = currentFragment.GetRegionId();
                if (regionResourceId == Resource.Id.right_region)
                {
                    CloseFragment(currentFragment);
                    return;
                }
                else if (regionResourceId == Resource.Id.left_region)
                {
                    CloseFragment(currentFragment);
                    base.OnBackPressed();
                }
            }
        }

        public void Show(MvxFragment fragment)
        {
            Show(fragment, true);
        }

        protected void Show(MvxFragment fragment, bool backStack)
        {
            int regionResourceId = fragment.GetRegionId();
            bool hidesNav = fragment.GetHidesNav();

            CustomFragmentsBackStack.Add(fragment);

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
            //ft.SetCustomAnimations(Resource.Animation.enter, Resource.Animation.exit, Resource.Animation.popenter, Resource.Animation.popexit);

            if (regionResourceId == Resource.Id.nav_region)
            {
                ft.Replace(Resource.Id.nav_region, fragment);
            }
            else
            {
                ft.Replace(Resource.Id.phone_main_region, fragment);
            }

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

        private void CloseModal()
        {
            var viewFragment = FragmentManager.FindFragmentById(Resource.Id.phone_main_region) as MvxFragment;
            if (viewFragment != null)
            {
                CloseFragment(viewFragment);
            }
        }

        private void CloseRight()
        {
            var rightFragment = FragmentManager.FindFragmentById(Resource.Id.right_region) as MvxFragment;
            if (rightFragment != null)
            {
                CloseFragment(rightFragment);
            }
        }

        private void CloseSplit()
        {
            CloseRight();

            var leftFragment = FragmentManager.FindFragmentById(Resource.Id.left_region) as MvxFragment;
            if (leftFragment != null)
            {
                CloseFragment(leftFragment);
            }
        }
    }
}