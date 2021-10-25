using System;
using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class ClaimBoolResultDisplay : UIView
	{

		UILabel titleLabel;
		UILabel boolLabel;

		public ClaimBoolResultDisplay (string title, bool boolForText )
		{
			titleLabel = new UILabel();
			titleLabel.Font = UIFont.FromName (Constants.NUNITO_BLACK, (nfloat)Constants.SMALL_FONT_SIZE);
			titleLabel.TextColor = Colors.DARK_GREY_COLOR;
			titleLabel.TextAlignment = UITextAlignment.Left;
			titleLabel.BackgroundColor = Colors.Clear;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			titleLabel.Text = title;
			AddSubview (titleLabel);

			boolLabel = new UILabel();
			boolLabel.Font = UIFont.FromName (Constants.NUNITO_BLACK, (nfloat)Constants.SMALL_FONT_SIZE);
			boolLabel.TextColor = Colors.DARK_GREY_COLOR;
			boolLabel.TextAlignment = UITextAlignment.Left;
			boolLabel.BackgroundColor = Colors.Clear;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			boolLabel.Text = boolForText ? "yes".tr () : "no".tr ();
			AddSubview (boolLabel);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = (float)this.Frame.Width;
			float topPadding = 15;
			float innerPadding = 15;


			titleLabel.Frame = new CGRect (sidePadding, topPadding, contentWidth/4 * 2 , (float)titleLabel.Frame.Height); 
			titleLabel.SizeToFit ();
			titleLabel.Frame = new CGRect (sidePadding, topPadding, contentWidth/4 * 2 , (float)titleLabel.Frame.Height); 

			_componentHeight = topPadding * 2 + (float)titleLabel.Frame.Height;

			boolLabel.Frame = new CGRect (contentWidth/4 * 3, _componentHeight/2 - (float)boolLabel.Frame.Height/2, contentWidth/4 , (float)boolLabel.Frame.Height); 
			boolLabel.SizeToFit ();
			boolLabel.Frame = new CGRect (contentWidth/4 * 3, _componentHeight/2 - (float)boolLabel.Frame.Height/2, contentWidth/4 , (float)boolLabel.Frame.Height); 
		}

		float _componentHeight;
		public float ComponentHeight
		{
			get {
				float contentWidth = (float)this.Frame.Width;
				float titlePadding = 15;

				titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titlePadding, contentWidth/4 * 3, (float)titleLabel.Frame.Height); 
				titleLabel.SizeToFit ();
				titleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, titlePadding, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height);
				return titlePadding * 2 + (float)titleLabel.Frame.Height;
			}
			set{
				_componentHeight = value;
			}

		}
	}
}

