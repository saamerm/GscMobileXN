using System;
using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class ClaimFieldResultRightDisplay : UIView
	{

		public UILabel titleLabel;
		public UILabel fieldLabel;

		public ClaimFieldResultRightDisplay (string title, string fieldText = null )
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

			fieldLabel = new UILabel();
			fieldLabel.Font = UIFont.FromName (Constants.NUNITO_BLACK, (nfloat)Constants.SMALL_FONT_SIZE);
			fieldLabel.TextColor = Colors.DARK_GREY_COLOR;
			fieldLabel.TextAlignment = UITextAlignment.Left;
			fieldLabel.LineBreakMode = UILineBreakMode.WordWrap;
			fieldLabel.Lines = 0;
			fieldLabel.BackgroundColor = Colors.Clear;
			if(fieldText != null)
				fieldLabel.Text = fieldText;
			AddSubview (fieldLabel);
		}

		int redrawCount = 0;
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = (float)this.Frame.Width;
			float titlePadding = 10;
			float innerPadding = 15;

			titleLabel.Frame = new CGRect (sidePadding, titlePadding, contentWidth/2 - innerPadding, (float)titleLabel.Frame.Height); 
			titleLabel.SizeToFit ();
			titleLabel.Frame = new CGRect (sidePadding, titlePadding, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height); 

			float tempHeight = titlePadding * 2 + (float)titleLabel.Frame.Height;

			fieldLabel.Frame = new CGRect (contentWidth/4 * 3,  _componentHeight/2 - (float)fieldLabel.Frame.Height/2, contentWidth/4 , (float)fieldLabel.Frame.Height); 
			fieldLabel.SizeToFit ();
			fieldLabel.Frame = new CGRect (contentWidth/4 * 3,  _componentHeight/2 - (float)fieldLabel.Frame.Height/2, (float)fieldLabel.Frame.Width, (float)fieldLabel.Frame.Height); 

			_componentHeight = titlePadding * 2 +  Math.Max((float)titleLabel.Frame.Height, (float)fieldLabel.Frame.Height);

//			RectangleF titleFrame = titleLabel.Frame;
//			RectangleF fieldFrame = fieldLabel.Frame;
//
//			if (fieldLabel.Frame.Height > titleLabel.Frame.Height) {
//
//				titleFrame.Y = titlePadding + fieldFrame.Height / 2 - titleFrame.Height/2;
//				titleLabel.Frame = titleFrame;
//			} else {
//				fieldFrame.Y = titlePadding + titleFrame.Height / 2 - fieldFrame.Height/2;
//				fieldLabel.Frame = fieldFrame;
//			}

			if (redrawCount < 2) {
				redrawCount++;
				this.SetNeedsLayout();
			}

		}

		float _componentHeight;
		public float ComponentHeight
		{
			get {
				float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
				float contentWidth = (float)this.Frame.Width;
				float titlePadding = 10;
				float innerPadding = 15;

				titleLabel.Frame = new CGRect (sidePadding, titlePadding, contentWidth/2 - innerPadding, (float)titleLabel.Frame.Height); 
				titleLabel.SizeToFit ();
				titleLabel.Frame = new CGRect (sidePadding, titlePadding, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height); 

				float tempHeight = titlePadding * 2 + (float)titleLabel.Frame.Height;

				fieldLabel.Frame = new CGRect (contentWidth/4 * 3, _componentHeight/2 - (float)fieldLabel.Frame.Height/2, contentWidth/4 , (float)fieldLabel.Frame.Height); 
				fieldLabel.SizeToFit ();
				fieldLabel.Frame = new CGRect (contentWidth/4 * 3, _componentHeight/2 - (float)fieldLabel.Frame.Height/2, (float)fieldLabel.Frame.Width, (float)fieldLabel.Frame.Height);  

//				RectangleF titleFrame = titleLabel.Frame;
//				RectangleF fieldFrame = fieldLabel.Frame;
//
//				if (fieldLabel.Frame.Height > titleLabel.Frame.Height) {
//
//					titleFrame.Y = titlePadding + fieldFrame.Height / 2 - titleFrame.Height/2;
//					titleLabel.Frame = titleFrame;
//				} else {
//					fieldFrame.Y = titlePadding + titleFrame.Height / 2 - fieldFrame.Height/2;
//					fieldLabel.Frame = fieldFrame;
//				}

				return (titlePadding * 2) + Math.Max( (float)titleLabel.Frame.Height, (float)fieldLabel.Frame.Height);
			}
			set{
				_componentHeight = value;
			}

		}
	}
}

