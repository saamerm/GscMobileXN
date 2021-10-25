using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
	public class WhiteListButton : UIButton
	{
		UIView bottomBorder;
		UITableViewCell disclosure;


		public WhiteListButton (Boolean hasBottomBorder)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;
			this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			this.ContentEdgeInsets = new UIEdgeInsets (0, Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 0, 0);
			this.Font = UIFont.SystemFontOfSize ((nfloat)16.0f);
			this.SetTitleColor (Colors.Black, UIControlState.Normal);

			this.disclosure = new UITableViewCell ();
			this.disclosure.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			this.disclosure.UserInteractionEnabled = false;
			this.disclosure.BackgroundColor = Colors.Clear;
			this.disclosure.Frame = (CGRect)this.Bounds;
			this.disclosure.Alpha = 1;
			this.AddSubview (this.disclosure);

			if (hasBottomBorder) {
				this.bottomBorder = new UIView();
				this.bottomBorder.BackgroundColor = UIColor.LightGray;
				this.AddSubview (this.bottomBorder);
			}

			this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			this.TitleLabel.TextAlignment = UITextAlignment.Center;

		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			setHighlight (true);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			setHighlight (this.Selected);
		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);

			setHighlight (this.Selected);
		}

		public override bool Selected {
			get {
				return base.Selected;
			}
			set {
				base.Selected = value;

				setHighlight (value);

			}
		}

		protected void setHighlight(bool highlighted)
		{
			if (highlighted) {
				this.BackgroundColor = Colors.HIGHLIGHT_COLOR;

				this.SetTitleColor (Colors.BACKGROUND_COLOR, UIControlState.Normal);
			} else {
				this.BackgroundColor = Colors.BACKGROUND_COLOR;

				this.SetTitleColor (Colors.Black, UIControlState.Normal);
			}
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			this.disclosure.Frame = (CGRect)this.Bounds;
			if (this.bottomBorder != null) {
				this.bottomBorder.Frame = new CGRect (Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, (float)this.Frame.Height - 1, (float)this.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 1);
			}
		}
	}
}

