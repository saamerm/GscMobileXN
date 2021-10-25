using System;
using CoreGraphics;
using UIKit;
using Foundation;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	public class SpendingAccountDetailSection : UIView
	{
		SpendingAccountPeriodRollup _account;

		UILabel contributionYearLabel;
//		UILabel totalRemainingTitleLabel;
//		UILabel totalRemainingAmountLabel;

		UIView vewContributionBorder;//totalRemainingContainer,

		NSMutableArray accountDetailSubSections;
		private const float PADDING = 10;

		public SpendingAccountDetailSection (SpendingAccountPeriodRollup account)
		{
			_account = account;
			this.BackgroundColor = Colors.BACKGROUND_COLOR;

			contributionYearLabel = new UILabel();
			var yearText1 = "   "+"contributionYear".tr() + account.Year + "   ";

			NSDateFormatter dtf = new NSDateFormatter ();
			dtf.DateFormat = "MMMM dd, yyyy";
			NSDate dtStart = dtf.Parse (account.StartDateAsString);
			NSDate dtEnd = dtf.Parse (account.EndDateAsString);

			dtf.DateFormat = "MMM dd, yyyy";
			string strStart = dtf.ToString(dtStart);
			string strEnd = dtf.ToString(dtEnd);

//			var yearText2 = "(" + account.StartDateAsString + "-" + account.EndDateAsString + ")";
//			var yearText2 = "(" + strStart + " - " + strEnd + ")";
			var yearText = yearText1;// + yearText2;
			contributionYearLabel.TextColor = Colors.DARK_GREY_COLOR;
            contributionYearLabel.BackgroundColor = Colors.LightGrayColor;
			contributionYearLabel.TextAlignment = UITextAlignment.Left;
            contributionYearLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_HEADING_SIZE); 
			var textAttributed1 = new NSMutableAttributedString(yearText,new UIStringAttributes(){ForegroundColor = Colors.VERY_DARK_GREY_COLOR, Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_HEADING_SIZE )});
            var colourAttribute1 = new UIStringAttributes()
            {
                ForegroundColor = Colors.DARK_GREY_COLOR, 
                Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_LIST_ITEM_SIZE),
                BaselineOffset=3
                    
            };
//			textAttributed1.SetAttributes(colourAttribute1, new NSRange(yearText1.Length, yearText2.Length));
			contributionYearLabel.AttributedText = textAttributed1;

			this.AddSubview (contributionYearLabel);

			vewContributionBorder = new UIView ();
            vewContributionBorder.BackgroundColor = Colors.SpendingAccountMediumGrayColor;
			contributionYearLabel.AddSubview (vewContributionBorder);
			/*
			totalRemainingContainer = new UIView();
			totalRemainingContainer.BackgroundColor = Colors.BACKGROUND_COLOR;
			totalRemainingContainer.Layer.BorderColor = Colors.Black.CGColor;
			totalRemainingContainer.Layer.BorderWidth = 2;
			totalRemainingContainer.Layer.MasksToBounds = true;

			this.AddSubview (totalRemainingContainer);

			totalRemainingTitleLabel = new UILabel();
			totalRemainingTitleLabel.Text = "totalRemaining".tr();
			totalRemainingTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			totalRemainingTitleLabel.BackgroundColor = Colors.Clear;
			totalRemainingTitleLabel.TextAlignment = UITextAlignment.Left;
			totalRemainingTitleLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE );


			totalRemainingAmountLabel = new UILabel();
			totalRemainingAmountLabel.Text ="$" + account.TotalRemaining.ToString();
			totalRemainingAmountLabel.TextColor = Colors.HIGHLIGHT_COLOR;
			totalRemainingAmountLabel.BackgroundColor = Colors.Clear;
			totalRemainingAmountLabel.TextAlignment = UITextAlignment.Right;
			totalRemainingAmountLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );//UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE );

			totalRemainingContainer.AddSubview (totalRemainingTitleLabel);
			totalRemainingContainer.AddSubview (totalRemainingAmountLabel);
			*/
			accountDetailSubSections = new NSMutableArray ();

			for (var i = 0; i < account.SpendingAccounts.Count; i++) {

				SpendingAccountDetailSubSection subSection = new SpendingAccountDetailSubSection (account.SpendingAccounts [i]);

				accountDetailSubSections.Add (subSection);

				this.AddSubview (subSection);

			}
		}



		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			setLocalFrames ();
		}

		public void setLocalFrames() {

			contributionYearLabel.SizeToFit ();
			if (contributionYearLabel.Frame.Width > this.Frame.Width) {
				contributionYearLabel.AdjustsFontSizeToFitWidth = true;
			}
            contributionYearLabel.Frame = new CGRect (0, 0, (float)this.Frame.Width, Constants.DRUG_LOOKUP_PADDED_LABEL_HEIGHT);
			vewContributionBorder.Frame = new CGRect (0, contributionYearLabel.Frame.GetMaxY () - 1, contributionYearLabel.Frame.Width, 1);

			float subSectionY = (float)contributionYearLabel.Frame.GetMaxY();
			for (var i = 0; i < (uint)accountDetailSubSections.Count; i++) {
				SpendingAccountDetailSubSection subSection = accountDetailSubSections.GetItem<SpendingAccountDetailSubSection>((nuint)i);
				subSection.setLocalFrames ();
				subSection.Frame = new CGRect (0, subSectionY, (float)this.Frame.Width, subSection.getHeight());

				subSectionY += subSection.getHeight() + Constants.DRUG_LOOKUP_TOP_PADDING;
			} 
			height = subSectionY;
			/*
			totalRemainingTitleLabel.SizeToFit ();
			totalRemainingAmountLabel.SizeToFit ();
			totalRemainingContainer.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, subSectionY, (float)this.Frame.Width-2*Constants.DRUG_LOOKUP_SIDE_PADDING, totalRemainingTitleLabel.Frame.Height+Constants.DRUG_LOOKUP_TOP_PADDING);

			totalRemainingTitleLabel.Frame = new CGRect (Constants.BALANCES_ITEM_SIDE_PADDING, Constants.DRUG_LOOKUP_TOP_PADDING/2, totalRemainingTitleLabel.Frame.Width, totalRemainingTitleLabel.Frame.Height);
			totalRemainingAmountLabel.Frame = new CGRect ((float)totalRemainingContainer.Frame.Width - totalRemainingAmountLabel.Frame.Width-(Constants.BALANCES_ITEM_SIDE_PADDING-Constants.DRUG_LOOKUP_SIDE_PADDING), Constants.DRUG_LOOKUP_TOP_PADDING/2, totalRemainingAmountLabel.Frame.Width, totalRemainingTitleLabel.Frame.Height);
*/
		}

		private float _height;
		public float height
		{
			get
			{
				float tempHeight = _height;//(float)totalRemainingContainer.Frame.GetMaxY()+Constants.DRUG_LOOKUP_TOP_PADDING;
				return tempHeight;

				//ADD HEIGHTS OF INITIAL LABELS
				tempHeight += Constants.DRUG_LOOKUP_LABEL_HEIGHT * 2 + Constants.DRUG_LOOKUP_PADDED_LABEL_HEIGHT + Constants.DRUG_LOOKUP_TOP_PADDING;

				for (var i = 0; i < (uint)accountDetailSubSections.Count; i++) {
					SpendingAccountDetailSubSection subSection = accountDetailSubSections.GetItem<SpendingAccountDetailSubSection>((nuint)i);
					tempHeight += subSection.getHeight () + Constants.DRUG_LOOKUP_TOP_PADDING;
				}

				return tempHeight;
			}
			set 
			{ 
				_height = value;
			}
		}

	}
}

