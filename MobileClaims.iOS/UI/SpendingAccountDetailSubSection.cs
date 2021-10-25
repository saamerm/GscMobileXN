using System;
using CoreGraphics;
using UIKit;
using Foundation;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	public class SpendingAccountDetailSubSection : UIView
	{

		SpendingAccountDetail _accountDetail;

		UILabel accountDetailName;

		UILabel depositedTitle;
		UILabel depositedAmount;
		UILabel usedToDateTitle;
		UILabel usedToDateAmount;
		UILabel remainingTitle;
		UILabel remainingAmount;

		UILabel forfeitureTitle;
		UIView vewGreen;

		UIView amountsContainer;

		private const float PADDING = 10;

		public SpendingAccountDetailSubSection (SpendingAccountDetail accountDetail)
		{
			_accountDetail = accountDetail;

			accountDetailName = new UILabel();
			string name = accountDetail.AccountName;
			if (name != null)
				name = name.ToUpper ();
			accountDetailName.Text = name;
			accountDetailName.TextColor = Colors.VERY_DARK_GREY_COLOR;
			accountDetailName.BackgroundColor = Colors.Clear;
			accountDetailName.TextAlignment = UITextAlignment.Left;
			accountDetailName.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.BUTTON_FONT_SIZE );

			depositedTitle = new UILabel();
			depositedTitle.Text = "deposited".tr ();
			depositedTitle.TextColor = Colors.DARK_GREY_COLOR;
			depositedTitle.BackgroundColor = Colors.Clear;
			depositedTitle.TextAlignment = UITextAlignment.Left;
			//depositedTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );

			depositedAmount = new UILabel();
			depositedAmount.Text = "$" + accountDetail.Deposited.ToString ();
			depositedAmount.TextColor = Colors.HIGHLIGHT_COLOR;
			depositedAmount.BackgroundColor = Colors.Clear;
			depositedAmount.TextAlignment = UITextAlignment.Right;
			depositedAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );

			usedToDateTitle = new UILabel();
			usedToDateTitle.Text = "usedToDate".tr ();
			usedToDateTitle.TextColor = Colors.DARK_GREY_COLOR;
			usedToDateTitle.BackgroundColor = Colors.Clear;
			usedToDateTitle.TextAlignment = UITextAlignment.Left;
			//usedToDateTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );

			usedToDateAmount = new UILabel();
			usedToDateAmount.Text = "$" + accountDetail.UsedToDate.ToString ();
			usedToDateAmount.TextColor = Colors.DARK_GREY_COLOR;
			usedToDateAmount.BackgroundColor = Colors.Clear;
			usedToDateAmount.TextAlignment = UITextAlignment.Right;
			usedToDateAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );

			vewGreen = new UIView ();
			vewGreen.BackgroundColor = Colors.MED_GREY_COLOR;

			remainingTitle = new UILabel();
			remainingTitle.Text = "remaining".tr ();
			remainingTitle.TextColor = Colors.VERY_DARK_GREY_COLOR;
			remainingTitle.BackgroundColor = Colors.Clear;
			remainingTitle.TextAlignment = UITextAlignment.Left;
			remainingTitle.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.HEADING_FONT_SIZE );

			remainingAmount = new UILabel();
			remainingAmount.Text ="$" + accountDetail.Remaining.ToString ();
			remainingAmount.TextColor = Colors.HIGHLIGHT_COLOR;
			remainingAmount.BackgroundColor = Colors.Clear;
			remainingAmount.TextAlignment = UITextAlignment.Right;
			remainingAmount.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.BUTTON_FONT_SIZE );

			forfeitureTitle = new UILabel();
			forfeitureTitle.BackgroundColor = Colors.Clear;
			forfeitureTitle.TextAlignment = UITextAlignment.Left;
			forfeitureTitle.Lines = 0;
			forfeitureTitle.LineBreakMode = UILineBreakMode.WordWrap;
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


			amountsContainer = new UIView();
			amountsContainer.BackgroundColor = Colors.BACKGROUND_COLOR;
			amountsContainer.Layer.MasksToBounds = true;

			this.AddSubview (accountDetailName);
			this.AddSubview (forfeitureTitle);
			this.AddSubview (amountsContainer);

			amountsContainer.AddSubview (depositedTitle);
			amountsContainer.AddSubview (depositedAmount);
			amountsContainer.AddSubview (usedToDateTitle);
			amountsContainer.AddSubview (usedToDateAmount);
			amountsContainer.AddSubview (vewGreen);
			amountsContainer.AddSubview (remainingTitle);
			amountsContainer.AddSubview (remainingAmount);
		}

		public float getHeight ()
		{
			return (float)forfeitureTitle.Frame.GetMaxY();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			setLocalFrames ();
		}

		public void setLocalFrames() {
			//float viewwidth = base.View.Bounds.Width;
			//float contentWidth = viewwidth - Constants.BALANCES_SIDE_PADDING * 2;
			//float viewHeight = base.View.Bounds.Height - startY;

			accountDetailName.Frame = new CGRect (Constants.BALANCES_SIDE_PADDING, Constants.DRUG_LOOKUP_SIDE_PADDING, (float)this.Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

			depositedTitle.SizeToFit ();
			depositedAmount.SizeToFit ();
			usedToDateTitle.SizeToFit ();
			usedToDateAmount.SizeToFit ();
			remainingTitle.SizeToFit ();
			remainingAmount.SizeToFit ();
			depositedTitle.SizeToFit ();

			float xPadding = Constants.BALANCES_ITEM_SIDE_PADDING;
			depositedTitle.Frame = new CGRect (xPadding, 0, depositedTitle.Frame.Width, depositedTitle.Frame.Height);
			depositedAmount.Frame = new CGRect ((float)this.Frame.Width - xPadding - depositedAmount.Frame.Width, 0, depositedAmount.Frame.Width, depositedAmount.Frame.Height);

			usedToDateTitle.Frame = new CGRect (xPadding, (float)depositedTitle.Frame.GetMaxY() +PADDING, usedToDateTitle.Frame.Width , usedToDateTitle.Frame.Height);
			usedToDateAmount.Frame = new CGRect ((float)this.Frame.Width - xPadding - usedToDateAmount.Frame.Width, usedToDateTitle.Frame.GetMinY(), usedToDateAmount.Frame.Width, usedToDateAmount.Frame.Height);

			vewGreen.Frame = new CGRect (xPadding, usedToDateTitle.Frame.GetMaxY() + PADDING, (float)this.Frame.Width - 2*xPadding, 1);

			remainingTitle.Frame = new CGRect (xPadding, (float)vewGreen.Frame.GetMaxY() +PADDING, remainingTitle.Frame.Width , remainingTitle.Frame.Height);
			remainingAmount.Frame = new CGRect ((float)this.Frame.Width - xPadding - remainingAmount.Frame.Width, remainingTitle.Frame.GetMinY(), remainingAmount.Frame.Width, remainingAmount.Frame.Height);

			amountsContainer.Frame = new CGRect (0, (float)accountDetailName.Frame.GetMaxY() + Constants.DRUG_LOOKUP_SIDE_PADDING, (float)this.Frame.Width, remainingAmount.Frame.GetMaxY());

			forfeitureTitle.SizeToFit ();
			forfeitureTitle.Frame = new CGRect (xPadding, (float)amountsContainer.Frame.Y + (float)amountsContainer.Frame.Height + PADDING, (float)this.Frame.Width - xPadding*2, (float)forfeitureTitle.Frame.Height);
		}
	}
}

