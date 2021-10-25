using CoreGraphics;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.iOS.Views;
using MobileClaims.iOS.Views.ClaimsHistory;
using MobileClaims.iOS.Views.Dashboard;
using MobileClaims.iOS.Views.Menu;
using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using System;
using System.Linq;
using UIKit;

namespace MobileClaims.iOS
{
    public class ContentNavViewController : UIViewController, IContentPresenter
    {
        private object _sync = new object();
        private bool _navInitialized;
        private bool _navAdded;
        private IMvxViewPresenter _presenter;

        public PresenterNavigator ContentView;

        public UINavigationController ViewNavigationController { get; }

        public UIWindow Window { get; set; }

        public IMvxViewPresenter Presenter
        {
            get => _presenter ?? Mvx.IoCProvider.Resolve<IMvxViewPresenter>();
            set => _presenter = value;
        }

        public ContentNavViewController()
        {
            ContentView = new PresenterNavigator();
            ViewNavigationController = new PresenterNavigator();

            ContentView.Delegate = new ContentViewDelegate();

            AddChildViewController(ContentView);
            View.AddSubview(ContentView.View);

            ViewNavigationController = new UINavigationController();

            ViewNavigationController.View.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            ViewNavigationController.NavigationBarHidden = true;

            var attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);

            UIBarButtonItem.Appearance.SetTitleTextAttributes(attributes, UIControlState.Normal);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            //Set frame of tabNav here to avoid overlapping content views
            var screenWidth = (float)base.View.Bounds.Width;
            var screenHeight = (float)base.View.Bounds.Height;

            if (ViewNavigationController != null)
            {
                var navSize = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;

                if (Constants.Bottom > 0)
                {
                    navSize += Constants.Bottom / 2;
                }

                ViewNavigationController.View.Frame = new CGRect(0, screenHeight - navSize, screenWidth, navSize);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!_navAdded)
            {
                ViewNavigationController.View.Alpha = 0;
                ViewNavigationController.View.UserInteractionEnabled = false;
            }
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (ContentView != null &&
                ContentView.TopViewController != null &&
                ContentView.TopViewController.GetType() == typeof(ClaimsHistoryResultDetailView))
            {
                //return UIInterfaceOrientationMask.Portrait;
                if (!ContentView.TopViewController.ShouldAutorotate())
                {
                    return base.GetSupportedInterfaceOrientations();
                    return UIInterfaceOrientationMask.Portrait;
                }

                return base.GetSupportedInterfaceOrientations();
            }
            else
            {
                return base.GetSupportedInterfaceOrientations();
            }
        }

        public void SetContent(UIViewController content, string requestedBy, bool hidesNav)
        {
            lock (_sync)
            {
                var animatesPush = true;

                var vc = content as MvxViewController;
                var rehydrationService = Mvx.IoCProvider.Resolve<IRehydrationService>();
                if (rehydrationService.Rehydrating)
                {
                    MvxTrace.Trace("Rehydrating!");
                    animatesPush = false;
                }

                //Multiple listeners of LoggedOutMessage causing several instances of
                //LoginViewModel to push onto stack. Preventing animation temporarily.
                if (hidesNav)
                {
                    animatesPush = false;
                }

                try
                {
                    ContentView.PushViewController(content, animatesPush);

                    // Removing views in between dashboard page view and the requested view controller
                    // Doing here because otherwise it would show landing page for a minute and show requested view-controller.
                    if (string.Equals(requestedBy, NavigationRequestTypes.BottomNavigationBar, StringComparison.OrdinalIgnoreCase))
                    {
                        var viewControllers = ContentView.ViewControllers.ToList();
                        var existingDashboardPageViewAt = viewControllers.FindIndex(x => x.GetType() == typeof(Dashboard));

                        if (existingDashboardPageViewAt > 0)
                        {
                            existingDashboardPageViewAt = content.GetType() == typeof(Dashboard)
                                ? existingDashboardPageViewAt
                                : existingDashboardPageViewAt + 1;
                            for (var i = viewControllers.Count - 1 - existingDashboardPageViewAt; i >= existingDashboardPageViewAt; i--)
                            {
                                ContentView.ViewControllers[i].RemoveFromParentViewController();
                            }
                        }
                    }

                    // Removing landing page from the navigation stack as it's now supposed to be treated as drawer. 
                    // Therefore, as soon as an option is selected from menu, I remove it from the stack
                    if (string.Equals(requestedBy, NavigationRequestTypes.Menu, StringComparison.OrdinalIgnoreCase))
                    {
                        var viewControllers = ContentView.ViewControllers.ToList();
                        var landingPageIndex = viewControllers.FindIndex(x => x.GetType() == typeof(LandingPageView));

                        if (landingPageIndex >= 0)
                        {
                            ContentView.ViewControllers[landingPageIndex].RemoveFromParentViewController();
                        }
                    }
                }
                catch
                {
                    ContentView.PopToViewController(content, animatesPush);
                }

                if (hidesNav && _navInitialized)
                {
                    RemoveNavigation();
                }
                else if (!hidesNav && _navInitialized && !_navAdded)
                {
                    AddNavigation();
                }

                var attributes = new UIStringAttributes();
                attributes.ForegroundColor = Colors.HIGHLIGHT_COLOR;
                attributes.BackgroundColor = Colors.BACKGROUND_COLOR;
                attributes.Font = UIFont.SystemFontOfSize((nfloat)Constants.NAV_BAR_FONT_SIZE);
                attributes.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.NAV_BAR_FONT_SIZE);

                ContentView.NavigationBar.TitleTextAttributes = (UIStringAttributes)attributes;
                ContentView.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
                ContentView.NavigationBar.BarTintColor = Colors.BACKGROUND_COLOR;
            }
        }

        public void SetNav(UIViewController nav)
        {
            ViewNavigationController.PushViewController(nav, false);
            ViewNavigationController.View.Alpha = 1;
            ViewNavigationController.View.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            AddNavigation();
            _navInitialized = true;
        }

        public void ShowNavigation()
        {
            AddNavigation();
        }

        public void HideNavigation()
        {
            RemoveNavigation();
        }

        private void AddNavigation()
        {
            AddChildViewController(ViewNavigationController);
            View.AddSubview(ViewNavigationController.View);

            ViewNavigationController.View.Alpha = 1;
            ViewNavigationController.View.UserInteractionEnabled = true;

            _navAdded = true;
        }

        private void RemoveNavigation()
        {
            ViewNavigationController.RemoveFromParentViewController();
            ViewNavigationController.View.RemoveFromSuperview();
            _navAdded = false;
        }
    }
}