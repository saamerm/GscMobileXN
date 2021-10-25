using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
	public class LandingPageButton : UIButton
	{
		protected UIImageView imageIcon;
		protected UIImage _defaultImage;
		protected UIImage _highlightedImage;

		protected UIView dividerLine;

		private const float DIVIDER_WIDTH = 2.0f;

		public LandingPageButton (UIImage defaultImage, UIImage highlightImage)
		{
			_defaultImage = defaultImage;
			_highlightedImage = highlightImage;

			this.BackgroundColor = Colors.LightGrayColor;
			this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			this.SetTitleColor (Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
			this.VerticalAlignment = UIControlContentVerticalAlignment.Center;

			this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			this.TitleLabel.TextAlignment = UITextAlignment.Left;
			this.TitleLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LANDING_BUTTON_FONT_SIZE);

			this.TitleEdgeInsets = new UIEdgeInsets (0, Constants.LANDING_BUTTON_LABEL_X, 0, 0);

			imageIcon = new UIImageView(_defaultImage);
			imageIcon.BackgroundColor = Colors.Clear;
			imageIcon.Opaque = false;
			imageIcon.UserInteractionEnabled = false;
			AddSubview(imageIcon);

			dividerLine = new UIView ();
			dividerLine.BackgroundColor = Colors.BACKGROUND_COLOR;
			AddSubview (dividerLine);
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
				imageIcon.Image = _highlightedImage;
			} else {
				this.BackgroundColor = Colors.LightGrayColor;
				this.SetTitleColor (Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
				imageIcon.Image = _defaultImage;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (imageIcon != null) {
				float imageWidth = (float)imageIcon.Image.Size.Width;
				float imageHeight = (float)imageIcon.Image.Size.Height;
				float imageX = Constants.LANDING_BUTTON_LOGO_AREA_WIDTH /2 - imageWidth/2;
				float imageY = (float)this.Frame.Height/2 - imageHeight/2;

				imageIcon.Frame = new CGRect (imageX, imageY, imageWidth, imageHeight);
			}

			if (dividerLine != null) {
				dividerLine.Frame = new CGRect (Constants.LANDING_BUTTON_LOGO_AREA_WIDTH, 0, DIVIDER_WIDTH, (float)this.Frame.Size.Height);
			}
		}
	}
}

