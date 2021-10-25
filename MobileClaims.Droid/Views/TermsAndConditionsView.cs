using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Presenter;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustPan)]
    public class TermsAndConditionsView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = { Resource.Id.right_region, Resource.Id.left_region, Resource.Id.phone_main_region, Resource.Id.nav_region };
        private ILoginService _loginservice;
        private TermsAndConditionsViewModel _model;

        public static bool isExternalWebview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            isExternalWebview = false;
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent ev)
        {
            if (keyCode == Keycode.Back)
            {
                if (isExternalWebview)
                {
                    isExternalWebview = false;
                    return true;
                }

                ((TermsAndConditionsViewModel)ViewModel).GoBackCommand.Execute(null);
                return false;
            }
            return base.OnKeyDown(keyCode, ev);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _model = (TermsAndConditionsViewModel)ViewModel;
            _loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            if (_loginservice.IsLoggedIn)
            {
            }
            else
            {
                if (_model.AcceptedTC)
                {
                    _model.AcceptTermsAndConditionsDroid.Execute(null);
                }

            }

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new TermsAndConditionsFragment_
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

            _loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            var privacyButton = FindViewById<FrameLayout>(Resource.Id.nav_region);
            if (((TermsAndConditionsViewModel)ViewModel).AcceptedTC && _loginservice.IsLoggedIn)
            {
                privacyButton.Visibility = ViewStates.Visible;
                presenter.ShowBottomNavigation();
            }
            else
            {
                privacyButton.Visibility = ViewStates.Gone;
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
            int regionResourceId = fragment.GetRegionId();

            var navRegion = (FrameLayout)FindViewById(Resource.Id.nav_region);
            var phoneMainRegion = (FrameLayout)FindViewById(Resource.Id.phone_main_region);

            if (!_model.AcceptedTC)
            {
                navRegion.Visibility = ViewStates.Gone;
                (phoneMainRegion.LayoutParameters as ViewGroup.MarginLayoutParams).SetMargins(0, 0, 0, 0);
            }
            else
            {
                //var dim = (int) Resources.GetDimension(Resource.Dimension.tabbar_height);
                navRegion.Visibility = ViewStates.Visible;
                (phoneMainRegion.LayoutParameters as ViewGroup.MarginLayoutParams).SetMargins(0, 0, 0, (int)Resources.GetDimension(Resource.Dimension.tabbar_height));
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
                FragmentManager.PopBackStackImmediate(null, PopBackStackFlags.Inclusive);
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