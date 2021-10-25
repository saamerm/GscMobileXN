using System;
using CoreGraphics;
using UIKit;

namespace MobileClaims.iOS
{
	public class InformationHeadingAndDetailsComponent : UIView
	{
		protected UIView contentView;
		protected UIView bottomBorder;

		protected UILabel headingLabel;
		protected UILabel descriptionLabel;

		private const float TOP_PADDING = 10;

		public InformationHeadingAndDetailsComponent (Boolean hasBottomBorder, string headingText, string descriptionText)
		{

			this.BackgroundColor = Colors.BACKGROUND_COLOR;
			headingLabel = new UILabel();
			headingLabel.Text = headingText;
			headingLabel.TextColor = Colors.DARK_GREY_COLOR;
			headingLabel.TextAlignment = UITextAlignment.Left;
			headingLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
			headingLabel.LineBreakMode = UILineBreakMode.WordWrap;
			headingLabel.Lines = 0;
			Add(this.headingLabel);

//			UIFont font = headingLabel.Font;
//			NSString headingString = new NSString (headingLabel.Text);
//
//			SizeF.
//
//			headingString.GetSizeUsingAttributes

			descriptionLabel = new UILabel();
			descriptionLabel.Text = descriptionText;
			descriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
			descriptionLabel.TextAlignment = UITextAlignment.Left;
			descriptionLabel.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD, (nfloat)Constants.HEADING_FONT_SIZE);
			descriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			descriptionLabel.Lines = 0;
			Add(this.descriptionLabel);

			if (hasBottomBorder) {
				this.bottomBorder = new DottedLine();
				this.AddSubview (this.bottomBorder);
			}
		}

		private float redrawCount = 0;
		private float _componentHeight;
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			headingLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, TOP_PADDING, (float)base.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET *2, (float)headingLabel.Frame.Height + 15);
			headingLabel.SizeToFit ();
			headingLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, TOP_PADDING, (float)headingLabel.Frame.Width, (float)headingLabel.Frame.Height + 15);

			descriptionLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, TOP_PADDING + (float)headingLabel.Frame.Height, (float)base.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET *2, (float)descriptionLabel.Frame.Height + 10);
			descriptionLabel.SizeToFit ();
			descriptionLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, TOP_PADDING + (float)headingLabel.Frame.Height, (float)base.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET *2, (float)descriptionLabel.Frame.Height + 10);


			this.Frame = new CGRect ((float)this.Frame.X, (float)this.Frame.Y, (float)this.Frame.Width, TOP_PADDING + (float)headingLabel.Frame.Height + (float)descriptionLabel.Frame.Height + 15);
		
			_componentHeight = (float)this.Frame.Height;

			if (this.bottomBorder != null) {
				this.bottomBorder.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, (float)this.Frame.Height - 1, (float)this.Frame.Width - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 1);
			}

			if (redrawCount < 2) {
				redrawCount++;
				this.SetNeedsLayout();
			}

		}

		public float ComponentHeight
		{
			get{
				return _componentHeight;
			}

			set{
				_componentHeight = value;
			}
		}
	}
}

