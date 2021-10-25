using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using System;
using System.Linq;
using MobileClaims.Droid.Presenter;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MobileClaims.Droid.Interfaces;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "DashboardView", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTop)]
    public class DashboardView : ActivityBase, IMultiRegionHost
    {
        private const int InitialNumberOfFragments = 2;

        private readonly int[] _validRegionIds = {
            Resource.Id.phone_main_region,
            Resource.Id.nav_region,
        };

        private DashboardViewModel _model;
        private bool _closeAll;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainLayout);

        }

        protected override void OnResume()
        {
            base.OnResume();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.ChangePresentation(new MvxClosePresentationHint(null));

            if (_closeAll)
            {
                while (FragmentManager.Fragments.Count > InitialNumberOfFragments)
                {
                    FragmentManager.PopBackStackImmediate();
                    if (CustomFragmentsBackStack.Count > 1)
                    {
                        CustomFragmentsBackStack.RemoveAt(CustomFragmentsBackStack.Count - 1);
                    }
                }

                _closeAll = false;
            }

            _model?.DashboardEntered?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            _model = (DashboardViewModel)ViewModel;

            _model.IsTabletDeviceType = IsTablet(this);

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new DashboardViewFragment_
            {
                ViewModel = _model
            };
            Show(cvf, true);

            CustomFragmentsBackStack.Add(cvf);

            presenter.ShowBottomNavigation();
        }

        public override void OnBackPressed()
        {
            if (CustomFragmentsBackStack.Count <= InitialNumberOfFragments)
            {
                CloseAll();
                ((DashboardViewModel)ViewModel).GoBackCommand.Execute(null);
                return;
            }

            base.OnBackPressed();
        }

        public void Show(MvxFragment fragment)
        {
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
            if (navRegion != null)
            {
                _ = fragment.GetHidesNav() ? ViewStates.Gone : ViewStates.Visible;
            }

            Fragment currentFragment = GetCurrentFragmentFromBackStack();
            if (currentFragment != null)
            {
                if (fragment.GetType() == currentFragment.GetType())
                {
                    return;
                }
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
                FragmentManager.PopBackStack(null,PopBackStackFlags.Inclusive);
                ft.AddToBackStack(fragment.Class.Name);
            }
            else if (backstackBehaviour == BackstackTypes.ADD)
            {
                ft.AddToBackStack(fragment.Class.Name);
            }

            ft.CommitAllowingStateLoss();
        }

        public void CloseViewModel(IMvxViewModel viewModel)
        {
            var fragment = GetActiveFragmentForViewModel(viewModel);
            if (fragment != null)
            {
                CloseFragment(fragment);
            }
        }

        private Fragment GetCurrentFragmentFromBackStack()
        {
            Fragment currentFragmentType = null;
            if (FragmentManager.BackStackEntryCount != 0)
            {
                currentFragmentType = FragmentManager.Fragments.LastOrDefault();
            }
            return currentFragmentType;
        }

        public void CloseAll()
        {
            _closeAll = true;
        }

        public bool IsTablet(Context context)
        {
            return Resources.GetBoolean(Resource.Boolean.isTablet);
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
    }
}