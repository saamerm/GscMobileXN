	using System;
using CoreGraphics;
    using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
	public class SelectionMenuItemStackedLabels : UIButton
	{
		UIView bottomBorder;
		UITableViewCell check;
		UITableViewCell disclosure;

		UILabel topLabel;
		UILabel middleLabel;
		UILabel bottomLabel;

		private const float CHECK_SIZE = 30;
		private const float CHECK_PADDING = 3;
		public SelectionMenuItemStackedLabels (Boolean hasBottomBorder)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;
			this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			this.ContentEdgeInsets = new UIEdgeInsets (0, Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 0, 0);
			this.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);

			this.check = new UITableViewCell ();
			this.check.Accessory = UITableViewCellAccessory.Checkmark;
			this.check.UserInteractionEnabled = false;
            this.check.Frame = (CGRect)this.Bounds;
			this.check.Alpha = 0;
			this.AddSubview (this.check);


			this.disclosure = new UITableViewCell ();
			this.disclosure.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			this.disclosure.UserInteractionEnabled = false;
            this.disclosure.Frame = (CGRect)this.Bounds;
			this.disclosure.Alpha = 0;
			this.AddSubview (this.disclosure);

			topLabel = new UILabel ();
			topLabel.TextColor = Colors.DARK_GREY_COLOR;
			topLabel.BackgroundColor = Colors.Clear;
			topLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			topLabel.TextAlignment = UITextAlignment.Left;
			AddSubview (topLabel);

			middleLabel = new UILabel ();
			middleLabel.TextColor = Colors.DARK_GREY_COLOR;
			middleLabel.BackgroundColor = Colors.Clear;
			middleLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			middleLabel.TextAlignment = UITextAlignment.Left;
			AddSubview (middleLabel);

			bottomLabel = new UILabel ();
			bottomLabel.TextColor = Colors.DARK_GREY_COLOR;
			bottomLabel.BackgroundColor = Colors.Clear;
			bottomLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			bottomLabel.TextAlignment = UITextAlignment.Left;
			AddSubview (bottomLabel);


			if (hasBottomBorder) {
				this.bottomBorder = new UIView();
				this.bottomBorder.BackgroundColor = UIColor.LightGray;
				this.AddSubview (this.bottomBorder);
			}

		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			this.BackgroundColor = Colors.BACKGROUND_COLOR;
		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);

			this.BackgroundColor = Colors.BACKGROUND_COLOR;
		}

		public void showCheck()
		{
			this.check.Alpha = 1;
			this.disclosure.Alpha = 0;
		}

		public void hideCheck ()
		{
			this.check.Alpha = 0;

		}

		public void showDisclosureIndicator()
		{
			this.disclosure.Alpha = 1;
			this.check.Alpha = 0;
		}

		public void hideDisclosureIndicator()
		{
			this.disclosure.Alpha = 0;
		}

		public void setTopText(string text)
		{
			topLabel.Text = text;
		}

		public void setMiddleText(string text)
		{
			middleLabel.Text = text;
		}
			
		public void setBottomText(string text)
		{
			bottomLabel.Text = text;
		}

		public override void SetTitleColor (UIColor color, UIControlState forState)
		{
			base.SetTitleColor (color, forState);
			topLabel.TextColor = color;
			middleLabel.TextColor = color;
			bottomLabel.TextColor = color;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

            topLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, Constants.STACKED_MENU_LABEL_INSET, (float)this.Bounds.Width, Constants.STACKED_MENU_LABEL_HEIGHT);
            middleLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, (float)topLabel.Frame.Y + (float)topLabel.Frame.Height + Constants.STACKED_MENU_LABEL_PADDING, (float)this.Bounds.Width, Constants.STACKED_MENU_LABEL_HEIGHT);
            bottomLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, (float)middleLabel.Frame.Y + (float)middleLabel.Frame.Height + Constants.STACKED_MENU_LABEL_PADDING, (float)this.Bounds.Width, Constants.STACKED_MENU_LABEL_HEIGHT);
            this.Frame = new CGRect((float)this.Frame.X, (float)this.Frame.Y, (float)this.Frame.Width, (float)Constants.STACKED_MENU_TOTAL_HEIGHT);

			if (this.bottomBorder != null) {
                this.bottomBorder.Frame = new CGRect((float)Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, (float)this.Frame.Height - 1, (float)this.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 1);
			}

            this.check.Frame = (CGRect)this.Bounds;
            this.disclosure.Frame = (CGRect)this.Bounds;
		}
	}
}

