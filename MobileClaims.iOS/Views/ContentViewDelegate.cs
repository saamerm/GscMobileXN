using System.Linq;
using MobileClaims.iOS.Helper;
using ObjCRuntime;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public class ContentViewDelegate : UINavigationControllerDelegate
    {
        public override void DidShowViewController(UINavigationController navigationController, [Transient] UIViewController viewController, bool animated)
        {
            if(navigationController.ViewControllers.Length > 2)
            {
                var vc = navigationController.ViewControllers[navigationController.ViewControllers.Length - 2];
                if(vc.GetType().HasAttribute(typeof(RemoveAfterViewDidDisappearAttribute)))
                {
                    var viewControllersList = navigationController.ViewControllers.ToList();
                    viewControllersList.Remove(vc);
                    navigationController.ViewControllers = viewControllersList.ToArray();
                }
            }
        }
    }
}