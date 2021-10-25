using System;
using CoreGraphics;
using UIKit;
using Foundation;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	public class SpendingAccountDetailSectionIPad : UIView
	{
		SpendingAccountPeriodRollup _account;
		UILabel contributionYearLabel;

		UILabel accountTitle;
		UILabel depositedTitle;
		UILabel usedToDateTitle;
		UILabel remainingTitle;

//		UILabel totalRemainingTitle;
//		UILabel totalRemainingAmount;

//		UILabel forfeitureTitle;
		UILabel forfeitureDate;
		UIView vewGreen,vewContributionBorder;//vewTotalRemaining,
		NSMutableArray accountDetailSubSections;


		private const float PADDING = 10;

		public SpendingAccountDetailSectionIPad (SpendingAccountPeriodRollup account)
		{
			_account = account;

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
			contributionYearLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_HEADING_SIZE );
			var textAttributed1 = new NSMutableAttributedString(yearText,new UIStringAttributes(){ForegroundColor = Colors.VERY_DARK_GREY_COLOR, Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_HEADING_SIZE )});
			var colourAttribute1 = new UIStringAttributes()
			{
				ForegroundColor = Colors.DARK_GREY_COLOR, Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_HEADING_SIZE )
			};
//			textAttributed1.SetAttributes(colourAttribute1, new NSRange(yearText1.Length, yearText2.Length));
			contributionYearLabel.AttributedText = textAttributed1;

			vewContributionBorder = new UIView ();
            vewContributionBorder.BackgroundColor = Colors.SpendingAccountMediumGrayColor;
			contributionYearLabel.AddSubview (vewContributionBorder);


			vewGreen = new UIView ();
			vewGreen.BackgroundColor = Colors.HIGHLIGHT_COLOR;

			accountTitle = new UILabel();
			accountTitle.Text = "accountNoC".tr ();
			accountTitle.TextColor = Colors.DARK_GREY_COLOR;
			accountTitle.BackgroundColor = Colors.Clear;
			accountTitle.TextAlignment = UITextAlignment.Left;
			accountTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );

			depositedTitle = new UILabel();
			depositedTitle.Text = "depositedNoC".tr ();
			depositedTitle.TextColor = Colors.DARK_GREY_COLOR;
			depositedTitle.LineBreakMode = UILineBreakMode.WordWrap;
			depositedTitle.Lines = 2;
			depositedTitle.BackgroundColor = Colors.Clear;
			depositedTitle.TextAlignment = UITextAlignment.Right;
			depositedTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );

			usedToDateTitle = new UILabel();
			usedToDateTitle.Text = "usedToDateNoC".tr ();
			usedToDateTitle.LineBreakMode = UILineBreakMode.WordWrap;
			usedToDateTitle.TextColor = Colors.DARK_GREY_COLOR;
			usedToDateTitle.Lines = 2;
			usedToDateTitle.BackgroundColor = Colors.Clear;
			usedToDateTitle.TextAlignment = UITextAlignment.Right;
			usedToDateTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );

			remainingTitle = new UILabel();
			remainingTitle.Text = "remainingNoC".tr ();
			remainingTitle.LineBreakMode = UILineBreakMode.WordWrap;
			remainingTitle.Lines = 2;
			remainingTitle.TextColor = Colors.VERY_DARK_GREY_COLOR;
			remainingTitle.BackgroundColor = Colors.Clear;
			remainingTitle.TextAlignment = UITextAlignment.Right;
			remainingTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );


			accountDetailSubSections = new NSMutableArray ();

			for (var i = 0; i < account.SpendingAccounts.Count; i++) {

				SpendingAccountDetailSubSectionIPad subSection = new SpendingAccountDetailSubSectionIPad (account.SpendingAccounts [i]);

				accountDetailSubSections.Add (subSection);

				this.AddSubview (subSection);

			}

			/*
			vewTotalRemaining = new UIView ();
			vewTotalRemaining.BackgroundColor = Colors.BACKGROUND_COLOR;
			vewTotalRemaining.Layer.BorderColor = Colors.Black.CGColor;
			vewTotalRemaining.Layer.BorderWidth = 2;
			this.AddSubview (vewTotalRemaining);

			totalRemainingTitle = new UILabel();
//			totalRemainingTitle.Text = "totalRemaining".tr ();
//			totalRemainingTitle.TextColor =Colors.DARK_GREY_COLOR;
			totalRemainingTitle.BackgroundColor = Colors.Clear;
			totalRemainingTitle.TextAlignment = UITextAlignment.Left;
//			totalRemainingTitle.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );

			var stdText = "totalRemaining".tr () + " ";
			var forfeitText = stdText + "$" + account.TotalRemaining.ToString();
			var textAttributed = new NSMutableAttributedString(forfeitText,new UIStringAttributes(){ForegroundColor = Colors.Black, Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE )});
			var colourAttribute = new UIStringAttributes()
			{
				ForegroundColor = Colors.HIGHLIGHT_COLOR, Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE )//;UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE )
			};
			textAttributed.SetAttributes(colourAttribute, new NSRange(stdText.Length, forfeitText.Length-stdText.Length));
			totalRemainingTitle.AttributedText = textAttributed;

//			totalRemainingAmount = new UILabel();
//			totalRemainingAmount.Text =  "$" + account.TotalRemaining.ToString();
//			totalRemainingAmount.TextColor = Colors.HIGHLIGHT_COLOR;
//			totalRemainingAmount.BackgroundColor = Colors.Clear;
//			totalRemainingAmount.TextAlignment = UITextAlignment.Left;
//			totalRemainingAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
*/
			this.AddSubview (contributionYearLabel);
//			contributionYearLabel.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (accountTitle);
//			accountTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (depositedTitle);
//			depositedTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (usedToDateTitle);
//			usedToDateTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (remainingTitle);
//			remainingTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
//			vewTotalRemaining.AddSubview (totalRemainingTitle);
//			totalRemainingTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
//			vewTotalRemaining.AddSubview (totalRemainingAmount);
//			totalRemainingAmount.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
//			this.AddSubview (forfeitureTitle);
//			forfeitureTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (vewGreen);
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			setLocalFrames ();
		}

		public void setLocalFrames() {
			float vPadding = 20;

			float tableStartPos = Constants.BALANCES_SIDE_PADDING;
			if (Helpers.IsInLandscapeMode ())
				tableStartPos += Constants.BALANCES_SIDE_PADDING;
			float tableColumnWidth = ((float)this.Frame.Width - (tableStartPos *2)) /4;

			contributionYearLabel.SizeToFit ();

//			totalRemainingTitle.SizeToFit ();

			float firstRowHeight = vPadding + (float)contributionYearLabel.Frame.Height;

			contributionYearLabel.Frame = new CGRect (Constants.BALANCES_SIDE_PADDING, 0, (float)this.Frame.Width-2*Constants.BALANCES_SIDE_PADDING, Constants.SIZE_30);
			vewContributionBorder.Frame = new CGRect (0, contributionYearLabel.Frame.GetMaxY () - 1, contributionYearLabel.Frame.Width, 1);

			accountTitle.Frame = new CGRect (tableStartPos, firstRowHeight, tableColumnWidth - 5, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			depositedTitle.Frame = new CGRect (tableStartPos + tableColumnWidth-5, firstRowHeight, tableColumnWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			usedToDateTitle.Frame = new CGRect (tableStartPos + tableColumnWidth *2, firstRowHeight, tableColumnWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			remainingTitle.Frame = new CGRect (tableStartPos + tableColumnWidth *3, firstRowHeight, tableColumnWidth , Constants.DRUG_LOOKUP_LABEL_HEIGHT);

			accountTitle.SizeToFit ();
			depositedTitle.SizeToFit ();
			usedToDateTitle.SizeToFit ();
			remainingTitle.SizeToFit ();

			// check if any label height is greater than drug lookup

			nfloat labelHeight = Constants.DRUG_LOOKUP_LABEL_HEIGHT;
			if (labelHeight < accountTitle.Frame.Height)
				labelHeight = accountTitle.Frame.Height;
			if (labelHeight < depositedTitle.Frame.Height)
				labelHeight = depositedTitle.Frame.Height;
			if (labelHeight < usedToDateTitle.Frame.Height)
				labelHeight = usedToDateTitle.Frame.Height;
			if (labelHeight < remainingTitle.Frame.Height)
				labelHeight = remainingTitle.Frame.Height;			
			
			accountTitle.Frame = new CGRect (tableStartPos, firstRowHeight, tableColumnWidth - 5, labelHeight);
			depositedTitle.Frame = new CGRect (tableStartPos + tableColumnWidth-5, firstRowHeight, tableColumnWidth, labelHeight);
			usedToDateTitle.Frame = new CGRect (tableStartPos + tableColumnWidth *2, firstRowHeight-2, tableColumnWidth, labelHeight+4);
			remainingTitle.Frame = new CGRect (tableStartPos + tableColumnWidth *3, firstRowHeight, tableColumnWidth , labelHeight);

			vewGreen.Frame = new CGRect (tableStartPos, accountTitle.Frame.GetMaxY (), (float)this.Frame.Width - tableStartPos-Constants.BALANCES_SIDE_PADDING, 1);
			float yPos = vPadding + (float)vewGreen.Frame.GetMaxY();

			for (var i = 0; i < (uint)accountDetailSubSections.Count; i++) {
				SpendingAccountDetailSubSectionIPad subSection = accountDetailSubSections.GetItem<SpendingAccountDetailSubSectionIPad>((nuint)i);

				subSection.Frame = new CGRect (0, yPos, (float)this.Frame.Width, subSection.getHeight());

				yPos += subSection.getHeight() + 2*PADDING;
			} 
			height = yPos;
//			vewTotalRemaining.Frame = new CGRect (contributionYearLabel.Frame.GetMaxX() - (float)totalRemainingTitle.Frame.Width-20, yPos, (float)totalRemainingTitle.Frame.Width+20, (float)totalRemainingTitle.Frame.Height+20);
//			totalRemainingTitle.Frame = new CGRect (10, 10, (float)totalRemainingTitle.Frame.Width, (float)totalRemainingTitle.Frame.Height);
		}

		private float _height;
		public float height
		{
			get
			{
				return _height;
				float tempHeight = 0;
				//ADD HEIGHTS OF INITIAL LABELS

//				tempHeight = (float)vewTotalRemaining.Frame.GetMaxY ();// + PADDING;


//				for (var i = 0; i < accountDetailSubSections.Count; i++) {
//					SpendingAccountDetailSubSection subSection = accountDetailSubSections.GetItem<SpendingAccountDetailSubSection>(i);
//					tempHeight += subSection.getHeight () + + Constants.DRUG_LOOKUP_TOP_PADDING;
//				}

				return tempHeight;
			}
			set 
			{ 
				_height = value;
			}
		}
	}
}

