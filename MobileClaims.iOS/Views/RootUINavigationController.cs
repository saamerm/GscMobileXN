using System;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public class RootUINavigationController: UINavigationController
    {
        public RootUINavigationController()
        {
        }
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {  
            return UIInterfaceOrientationMask.All;
        }

    }
}

