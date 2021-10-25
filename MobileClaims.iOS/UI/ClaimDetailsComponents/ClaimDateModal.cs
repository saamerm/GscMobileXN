using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class ClaimDateModal : UIViewController
	{
		public UIDatePicker datePicker;
		public GSButton dismissButton;

		private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
		private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

		public ClaimDateModal ()
		{
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

			float viewwidth = (float)base.View.Bounds.Width;
			float viewHeight = (float)base.View.Bounds.Height;

			if (Constants.IsPhone () && datePicker != null && dismissButton != null) {
				datePicker.Frame = new CGRect (viewwidth / 2 - (float)datePicker.Frame.Width / 2, viewHeight/2 - (float)datePicker.Frame.Height/2 - BUTTON_HEIGHT /2, (float)datePicker.Frame.Width, (float)datePicker.Frame.Height);
				dismissButton.Frame = new CGRect (viewwidth / 2 - BUTTON_WIDTH / 2, (float)datePicker.Frame.Y + (float)datePicker.Frame.Height + BUTTON_HEIGHT/2, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}
}

