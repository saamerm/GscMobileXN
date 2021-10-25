using System;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
	public class GSSelectionButton : GSButton
	{

		public GSSelectionButton ()
		{
			this.BackgroundColor = Colors.LightGrayColor;
			this.Layer.BorderColor = Colors.Clear.CGColor;
			this.Layer.BorderWidth = 0;
			this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			this.SetTitleColor (Colors.DARK_GREY_COLOR, UIControlState.Normal);

			this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			this.TitleLabel.TextAlignment = UITextAlignment.Center;
			this.TitleLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_SELECTION_BUTTON);
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
				this.BackgroundColor = Colors.LightGrayColor;
				this.SetTitleColor (Colors.DARK_GREY_COLOR, UIControlState.Normal);
			}
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

		}
	}
}

