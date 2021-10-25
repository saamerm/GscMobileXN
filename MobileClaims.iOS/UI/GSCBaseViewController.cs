using FluentValidation.Internal;
using Microsoft.AppCenter.Analytics;
using MobileClaims.iOS.UI;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace MobileClaims.iOS
{
    public class GSCBaseViewController : MvxViewController
    {
        public override void ViewWillAppear(bool animated)
        {
            Analytics.TrackEvent("PageView: " + this.GetType().Name.Replace("ViewController", string.Empty).SplitPascalCase());
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.NavigationController?.NavigationItem?.SetHidesBackButton(false, false);
            base.NavigationItem.BackBarButtonItem = new UIBarButtonItem(" ".tr(), UIBarButtonItemStyle.Plain, null, null);
        }
    }

    public interface IWebViewSupportedViewController 
    {
        UIActivityIndicatorView ActivityIndicatorView { get; }
        UIButton Back { get; }
        UIButton Forward { get; }
    }

    public class GSCBaseViewPaddingController : GSCBaseViewController
    {
        protected float ViewContentYPositionPadding { get; set; }
        protected float ViewContainerWidth { get; set; }
        protected float ViewContainerHeight { get; set; }
        protected float BottomPadding { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (this is IGSCBaseViewImplementor)
            {
                ViewContainerWidth = ((IGSCBaseViewImplementor)this).GetViewContainerWidth();
                ViewContainerHeight = ((IGSCBaseViewImplementor)this).GetViewContainerHeight();
                ViewContentYPositionPadding = ((IGSCBaseViewImplementor)this).ViewContentYPositionPadding();
            }
        }

        public virtual float GetBottomPadding()
        {
            return GetBottomPadding(Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING, 0);
        }

        public virtual float GetBottomPadding(float extraPos, float customPadding = 0)
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return extraPos;
            }
            return customPadding;
        }
    }

    public class GSCBaseViewController<T> : MvxViewController<T>
        where T : MvxViewModel
    {
        public GSCBaseViewController()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            Analytics.TrackEvent("PageView: " + this.GetType().Name.Replace("ViewController", string.Empty).SplitPascalCase());
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController?.NavigationItem?.SetHidesBackButton(false, false);
            base.NavigationItem.BackBarButtonItem = new UIBarButtonItem(" ".tr(), UIBarButtonItemStyle.Plain, null, null);
        }

        protected virtual void SetNavigationAfterViewDidLoad(string titleName, bool hideBackButton, bool animateButton)
        {
            SetNavigation(titleName, hideBackButton, animateButton);
        }

        protected virtual void SetNavigation(string titleName, bool hideBackButton, bool animateButton)
        {
            View.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.NavigationBarHidden = !hideBackButton;
            base.NavigationItem.Title = titleName.tr();
            base.NavigationItem.SetHidesBackButton(hideBackButton, animateButton);
        }
    }
}
