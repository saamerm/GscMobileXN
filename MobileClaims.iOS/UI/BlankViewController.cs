using UIKit;

namespace MobileClaims.iOS
{
	public class BlankViewController : UIViewController
	{

		public BlankViewController ()
		{


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			base.NavigationController.NavigationItem.SetHidesBackButton (true, false);
			base.NavigationItem.SetHidesBackButton (true, false);
		}
			
			
	}
}

