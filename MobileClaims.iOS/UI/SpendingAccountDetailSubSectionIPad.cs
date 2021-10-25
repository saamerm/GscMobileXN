using System;
using CoreGraphics;
using UIKit;
using Foundation;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	public class SpendingAccountDetailSubSectionIPad : UIView
	{

		SpendingAccountDetail _accountDetail;

		UILabel accountTitle;
		UILabel depositedAmount;
		UILabel usedToDateAmount;
		UILabel remainingAmount,forfeitureTitle;

		private const float PADDING = 10;

		public SpendingAccountDetailSubSectionIPad (SpendingAccountDetail accountDetail)
		{
			_accountDetail = accountDetail;

			accountTitle = new UILabel();
			string name = accountDetail.AccountName;
			if (name != null)
				name = name.ToUpper ();
			accountTitle.Text = name;
			accountTitle.TextColor = Colors.VERY_DARK_GREY_COLOR;
			accountTitle.BackgroundColor = Colors.Clear;
			accountTitle.TextAlignment = UITextAlignment.Left;
			accountTitle.Lines = 0;
			accountTitle.LineBreakMode = UILineBreakMode.WordWrap;
			accountTitle.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.BUTTON_FONT_SIZE );

			depositedAmount = new UILabel();
			depositedAmount.Text = "$" + accountDetail.Deposited.ToString ();
			depositedAmount.TextColor = Colors.HIGHLIGHT_COLOR;
			depositedAmount.BackgroundColor = Colors.Clear;
			depositedAmount.TextAlignment = UITextAlignment.Right;
			depositedAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );

			usedToDateAmount = new UILabel();
			usedToDateAmount.Text = "$" + accountDetail.UsedToDate.ToString ();
			usedToDateAmount.TextColor = Colors.DARK_GREY_COLOR;
			usedToDateAmount.BackgroundColor = Colors.Clear;
			usedToDateAmount.TextAlignment = UITextAlignment.Right;
			usedToDateAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );

			remainingAmount = new UILabel();
			remainingAmount.Text ="$" + accountDetail.Remaining.ToString ();
			remainingAmount.TextColor = Colors.HIGHLIGHT_COLOR;
			remainingAmount.BackgroundColor = Colors.Clear;
			remainingAmount.TextAlignment = UITextAlignment.Right;
			remainingAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );

			forfeitureTitle = new UILabel();
			var stdText = "forfeited".tr () + " ";
			NSDateFormatter dtf = new NSDateFormatter ();
			dtf.DateFormat = "MMMM dd, yyyy";
			NSDate dtStart = dtf.Parse (accountDetail.UseByDateAsString);
			dtf.DateFormat = "MMM dd, yyyy";
			string strUseBy = dtf.ToString(dtStart);

			var forfeitText = stdText + strUseBy;

			forfeitureTitle.TextColor =Colors.DARK_GREY_COLOR;
			forfeitureTitle.BackgroundColor = Colors.Clear;
			forfeitureTitle.TextAlignment = UITextAlignment.Left;
//			forfeitureTitle.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.FONT_SIZE_10 );
			var textAttributed = new NSMutableAttributedString(forfeitText,new UIStringAttributes(){ForegroundColor = Colors.DARK_GREY_COLOR, Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.SIZE_10 )});
			var colourAttribute = new UIStringAttributes()
			{
				ForegroundColor = Colors.DARK_GREY_COLOR, Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SIZE_10 )
			};
			textAttributed.SetAttributes(colourAttribute, new NSRange(stdText.Length, forfeitText.Length-stdText.Length));
			forfeitureTitle.AttributedText = textAttributed;
			this.AddSubview (accountTitle);
//			accountTitle.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (depositedAmount);
//			depositedAmount.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (usedToDateAmount);
//			usedToDateAmount.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (remainingAmount);
//			remainingAmount.BackgroundColor = Constants.LIGHT_BLUE_COLOR;
			this.AddSubview (forfeitureTitle);
		}

		public float getHeight ()
		{
			return (float)forfeitureTitle.Frame.GetMaxY();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			float tableStartPos = Constants.BALANCES_SIDE_PADDING;
			if (Helpers.IsInLandscapeMode ())
				tableStartPos += Constants.BALANCES_SIDE_PADDING;
			float tableColumnWidth = ((float)this.Frame.Width - (tableStartPos *2)) /4;

			accountTitle.SizeToFit ();
			depositedAmount.SizeToFit ();
			usedToDateAmount.SizeToFit ();
			remainingAmount.SizeToFit ();
			forfeitureTitle.SizeToFit ();

			accountTitle.Frame = new CGRect (tableStartPos, -2, tableColumnWidth, (float)accountTitle.Frame.Height);
			accountTitle.SizeToFit ();
			depositedAmount.Frame = new CGRect (tableStartPos+ tableColumnWidth*2-(float)depositedAmount.Frame.Width-5, 0, (float)depositedAmount.Frame.Width, (float)depositedAmount.Frame.Height);
			usedToDateAmount.Frame = new CGRect (tableStartPos+ tableColumnWidth *3-(float)usedToDateAmount.Frame.Width, 0, (float)usedToDateAmount.Frame.Width, (float)usedToDateAmount.Frame.Height);
			remainingAmount.Frame = new CGRect (tableStartPos + tableColumnWidth *4-(float)remainingAmount.Frame.Width, 0, (float)remainingAmount.Frame.Width, (float)remainingAmount.Frame.Height);
			forfeitureTitle.Frame = new CGRect (tableStartPos, accountTitle.Frame.GetMaxY()+Constants.SIZE_10/2, forfeitureTitle.Frame.Width, (float)forfeitureTitle.Frame.Height);
		}
	}
}

