using System;
using CoreGraphics;
using UIKit;

namespace MobileClaims.iOS
{
	public class SimpleMessageView : UIViewController
	{
		protected UILabel messageLabel;

		protected string messageString = "";

		public SimpleMessageView()
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View = new UIView() { BackgroundColor = Colors.BACKGROUND_COLOR };

			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.SetHidesBackButton (true, false);

			messageLabel = new UILabel ();
			messageLabel.LineBreakMode = UILineBreakMode.WordWrap;
			messageLabel.TextColor = Colors.VERY_DARK_GREY_COLOR;
			messageLabel.BackgroundColor = Colors.Clear;
			messageLabel.Text = messageString;
			messageLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.TEXT_FIELD_HEADING_SIZE);
			messageLabel.TextAlignment = UITextAlignment.Center;
			messageLabel.Lines = 2;
			View.AddSubview (messageLabel);
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

			float topPadding = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

			float msgWidth = (float)((float)base.View.Bounds.Width * 0.7);
			messageLabel.SizeToFit ();
			messageLabel.Frame = new CGRect ((float)View.Frame.Width / 2 - msgWidth / 2, (float)base.View.Bounds.Height / 2 - (Constants.DRUG_LOOKUP_LABEL_HEIGHT * 2) / 2 - topPadding, msgWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT * 2);
		}

		public void SetMessageText(string msg)
		{
			messageString = msg;

			if (messageLabel != null) {
				messageLabel.Text = msg;
				View.SetNeedsLayout ();
			}

		}
	}
}

