using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Presenter;
using MobileClaims.Droid.Views.Fragments;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustResize)]
    public class ChangeForLifeView : ActivityBase, IMultiRegionHost
    {
        private readonly int[] _validRegionIds = {Resource.Id.phone_main_region, Resource.Id.nav_region  };

        protected override void OnCreate(Bundle bundle)
        {
            SetContentView(Resource.Layout.MainLayout);

            base.OnCreate(bundle);
        }

        public override void OnBackPressed()
        {             
            if (FragmentManager.BackStackEntryCount == 1)
            {
                ((ChangeForLifeViewModel)this.ViewModel).GoBackCommand.Execute(null);
            }
            base.OnBackPressed();
        }
        
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var presenter = Mvx.IoCProvider.Resolve<IMultiRegionPresenter>();
            presenter.RegisterMultiRegionHost(this);

            var cvf = new ChangeForLifeFragment_()
            {
                ViewModel = ViewModel
            };
            Show(cvf, true);

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
    public class ChangeForLifeViewClient : WebViewClient
    {
		public ChangeForLifeFragment_ c4l;

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            return false;
        }
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            return false;
        }

        public override void OnReceivedSslError(WebView view, SslErrorHandler handler, Android.Net.Http.SslError error)
        {
            base.OnReceivedSslError(view, handler, error);
            //handler.Proceed();
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url); 
          //  (view as ExtendedWebView).ViewCanGoBack = view.CanGoBack();
          //  (view as ExtendedWebView).ViewCanGoForward = view.CanGoForward();
//           (view as ExtendedWebView).BackButtVisible = view.CanGoBack() ? ViewStates.Visible : ViewStates.Gone;
//           (view as ExtendedWebView).ForwardButtVisible = view.CanGoForward () ? ViewStates.Visible : ViewStates.Gone;
//			(view as ExtendedWebView).BackButtVisible=ViewStates.Visible;
//			(view as ExtendedWebView).ForwardButtVisible=ViewStates.Visible;
			if (view.CanGoBack ()) {
				c4l._imgBack.SetImageResource (Resource.Drawable.back_active);
			} else {
				c4l._imgBack.SetImageResource (Resource.Drawable.back_inactive);
			}

			if (view.CanGoForward ()) {
				c4l._imgForward.SetImageResource (Resource.Drawable.forward_active);
			} else {
				c4l._imgForward.SetImageResource(Resource.Drawable.forward_inactive);
			}

            Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
//			(view as ExtendedWebView).BackButtVisible = view.CanGoBack() ? c4l._imgBack.SetImageResource(Resource.Drawable.back_active) : c4l._imgBack.SetImageResource(Resource.Drawable.back_inactive);
//			(view as ExtendedWebView).ForwardButtVisible = view.CanGoForward () ?  c4l._imgForward.SetImageResource(Resource.Drawable.forward_active) :c4l._imgForward.SetImageResource(Resource.Drawable.forward_inactive);
               
        }
    }
}