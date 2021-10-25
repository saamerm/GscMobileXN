using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
	public class GSSelectionButtonSmall : GSButton
	{
		private float TEXT_HEIGHT = Constants.IsPhone() ? 25 : 45;

		public GSSelectionButtonSmall ()
		{
			this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
			this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
			this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			this.SetTitleColor (Colors.BACKGROUND_COLOR, UIControlState.Normal);

			this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			this.TitleLabel.TextAlignment = UITextAlignment.Center;
			this.TitleLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_BUTTON_FONT_SIZE);
			this.ContentEdgeInsets = new UIEdgeInsets (3, 0, 0, 0);
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
				this.BackgroundColor = Colors.LightGrayColor;
				this.SetTitleColor (Colors.DARK_GREY_COLOR, UIControlState.Normal);
			} else {
				this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
				this.SetTitleColor (Colors.BACKGROUND_COLOR, UIControlState.Normal);
			}
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (this.TitleLabel.Text != null) {

				UIFont sizedFont = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_BUTTON_FONT_SIZE);

				int i;

				for(i = (int)Constants.GS_BUTTON_FONT_SIZE; i > 10; i=i-2) {
					sizedFont = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)i);

					CGSize constraintSize = new CGSize (260.0f, float.MaxValue);

					//CGSize labelSize = this.TitleLabel.StringSize (this.TitleLabel.Text, sizedFont, constraintSize, UILineBreakMode.WordWrap);
                    CGSize labelSize = this.TitleLabel.Text.StringSize(sizedFont);
					if(labelSize.Height <= TEXT_HEIGHT)
						break;
				}

				this.TitleLabel.Font = sizedFont;
			}

		}
	}
}

