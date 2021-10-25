using UIKit;

namespace MobileClaims.iOS.Views
{
    public interface IContentPresenter
    {
        void ShowNavigation();
        void HideNavigation();
        UIWindow Window{ get; set;}
    }
}

