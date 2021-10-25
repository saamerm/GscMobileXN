using UIKit;
using MobileClaims.iOS.Views.ClaimsHistory;

namespace MobileClaims.iOS
{
    public class PresenterNavigator : UINavigationController
    {
        public PresenterNavigator()
        {
        }
        public override bool ShouldAutorotate()
        {
            if (this.NavigationController != null && 
                this.NavigationController.TopViewController != null && 
                this.NavigationController.TopViewController.GetType() == typeof(ClaimsHistoryResultDetailView))
            {
                return false;
            }
            return base.ShouldAutorotate();
        }
        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            if (this.NavigationController.TopViewController != null && this.NavigationController.TopViewController.GetType() == typeof(ClaimsHistoryResultDetailView))
            {
                return UIInterfaceOrientation.Portrait;
            }
            return base.PreferredInterfaceOrientationForPresentation();
        }
        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            toInterfaceOrientation = UIInterfaceOrientation.Portrait;
            base.WillRotate(toInterfaceOrientation, duration);
        }
    }
}

