using System;
using Foundation;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.HCSA;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	public class ClaimListResultsTableViewCell: MvxTableViewCell
	{

		public static readonly NSString CellIdentifier = new NSString ("ClaimListResultsTableViewCell");
		public static readonly UINib Nib = UINib.FromName ("ClaimListResultsTableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ClaimListResultsTableViewCell");

		public UILabel lblDateOfExpense;
		public UILabel lblExpenseType;
		public UILabel lblClaimAmount;
		public UILabel lblAmtPreviouslyPaid;
		public UILabel lblFormNumber;//

		public UILabel lblPaidAmount;//
		public UILabel lblClaimStatus;//
		public UILabel lblPaidAmountValue;//
		public UILabel lblClaimStatusValue;//
		public UILabel lblFormNumberValue;//
		public UILabel lblDateOfExpenseValue;
		public UILabel lblExpenseTypeValue;
		public UILabel lblClaimAmountValue;
		public UILabel lblAmtPreviouslyPaidValue;


		public static ClaimListResultsTableViewCell Create ()
		{
			return (ClaimListResultsTableViewCell)Nib.Instantiate (null, null) [0];
		}
		public ClaimListResultsTableViewCell (IntPtr handle) : base (handle)
		{

			lblFormNumber = new UILabel();
			lblFormNumber.Text = "Form Number";//"HCSA".tr();
			lblFormNumber.BackgroundColor = Colors.Clear;
			lblFormNumber.TextAlignment = UITextAlignment.Left;
			lblFormNumber.Lines = 1;
			lblFormNumber.LineBreakMode = UILineBreakMode.WordWrap;
			lblFormNumber.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblFormNumber.TextColor = Colors.DARK_GREY_COLOR;

			lblDateOfExpense = new UILabel();
			lblDateOfExpense.Text = "Date of Expense";//"HCSA".tr();
			lblDateOfExpense.BackgroundColor = Colors.Clear;
			lblDateOfExpense.TextAlignment = UITextAlignment.Left;
			lblDateOfExpense.Lines = 1;
			lblDateOfExpense.LineBreakMode = UILineBreakMode.WordWrap;
			lblDateOfExpense.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblDateOfExpense.TextColor = Colors.DARK_GREY_COLOR;

			lblExpenseType = new UILabel();
			lblExpenseType.Text = "Type of Expense";//"HCSA".tr();
			lblExpenseType.BackgroundColor = Colors.Clear;
			lblExpenseType.TextAlignment = UITextAlignment.Left;
			lblExpenseType.Lines = 1;
			lblExpenseType.LineBreakMode = UILineBreakMode.WordWrap;
			lblExpenseType.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblExpenseType.TextColor = Colors.DARK_GREY_COLOR;


			lblClaimAmount = new UILabel();
			lblClaimAmount.Text = "Claim Amount";//"HCSA".tr();
			lblClaimAmount.BackgroundColor = Colors.Clear;
			lblClaimAmount.TextAlignment = UITextAlignment.Right;
			lblClaimAmount.Lines = 1;
			lblClaimAmount.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimAmount.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblClaimAmount.TextColor = Colors.DARK_GREY_COLOR;


			lblAmtPreviouslyPaid = new UILabel();
			lblAmtPreviouslyPaid.Text = "Other Paid Amount";//"HCSA".tr();
			lblAmtPreviouslyPaid.BackgroundColor = Colors.Clear;
			lblAmtPreviouslyPaid.TextAlignment = UITextAlignment.Left;
			lblAmtPreviouslyPaid.Lines = 0;
			lblAmtPreviouslyPaid.LineBreakMode = UILineBreakMode.WordWrap;
			lblAmtPreviouslyPaid.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblAmtPreviouslyPaid.TextColor = Colors.DARK_GREY_COLOR;

			lblPaidAmount = new UILabel();
			lblPaidAmount.Text = "Paid Amount";//"HCSA".tr();
			lblPaidAmount.BackgroundColor = Colors.Clear;
			lblPaidAmount.TextAlignment = UITextAlignment.Left;
			lblPaidAmount.Lines = 0;
			lblPaidAmount.LineBreakMode = UILineBreakMode.WordWrap;
			lblPaidAmount.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblPaidAmount.TextColor = Colors.DARK_GREY_COLOR;

			lblClaimStatus = new UILabel();
			lblClaimStatus.Text = "Claim Status";//"HCSA".tr();
			lblClaimStatus.BackgroundColor = Colors.Clear;
			lblClaimStatus.TextAlignment = UITextAlignment.Left;
			lblClaimStatus.Lines = 0;
			lblClaimStatus.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimStatus.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblClaimStatus.TextColor = Colors.DARK_GREY_COLOR;

			//------------

			lblFormNumberValue = new UILabel();
			lblFormNumberValue.Text = "222458543";//"HCSA".tr();
			lblFormNumberValue.BackgroundColor = Colors.Clear;
			lblFormNumberValue.TextAlignment = UITextAlignment.Left;
			lblFormNumberValue.Lines = 1;
			lblFormNumberValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblFormNumberValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblFormNumberValue.TextColor = Colors.DarkGrayColor;

			lblDateOfExpenseValue = new UILabel();
			lblDateOfExpenseValue.Text = "2015-10-15";//"HCSA".tr();
			lblDateOfExpenseValue.BackgroundColor = Colors.Clear;
			lblDateOfExpenseValue.TextAlignment = UITextAlignment.Left;
			lblDateOfExpenseValue.Lines = 1;
			lblDateOfExpenseValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblDateOfExpenseValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblDateOfExpenseValue.TextColor = Colors.DarkGrayColor;

			lblExpenseTypeValue = new UILabel();
			lblExpenseTypeValue.Text = "Dental Service";//"HCSA".tr();
			lblExpenseTypeValue.BackgroundColor = Colors.Clear;
			lblExpenseTypeValue.TextAlignment = UITextAlignment.Left;
			lblExpenseTypeValue.Lines = 1;
			lblExpenseTypeValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblExpenseTypeValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblExpenseTypeValue.TextColor = Colors.DarkGrayColor;

			lblClaimAmountValue = new UILabel();
			lblClaimAmountValue.Text = "$100";//"HCSA".tr();
			lblClaimAmountValue.BackgroundColor = Colors.Clear;
			lblClaimAmountValue.TextAlignment = UITextAlignment.Right;
			lblClaimAmountValue.Lines = 1;
			lblClaimAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimAmountValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblClaimAmountValue.TextColor = Colors.DarkGrayColor;

			lblAmtPreviouslyPaidValue = new UILabel();
			lblAmtPreviouslyPaidValue.Text = "$50";//"HCSA".tr();
			lblAmtPreviouslyPaidValue.BackgroundColor = Colors.Clear;
			lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Left;
			lblAmtPreviouslyPaidValue.Lines = 1;
			lblAmtPreviouslyPaidValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblAmtPreviouslyPaidValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblAmtPreviouslyPaidValue.TextColor = Colors.DarkGrayColor;

			lblClaimStatusValue = new UILabel();
			lblClaimStatusValue.Text = "Awaiting Payment";//"HCSA".tr();
			lblClaimStatusValue.BackgroundColor = Colors.Clear;
			lblClaimStatusValue.TextAlignment = UITextAlignment.Left;
			lblClaimStatusValue.Lines = 1;
			lblClaimStatusValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimStatusValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblClaimStatusValue.TextColor = Colors.DarkGrayColor;

			lblPaidAmountValue = new UILabel();
			lblPaidAmountValue.Text = "0";//"HCSA".tr();
			lblPaidAmountValue.BackgroundColor = Colors.Clear;
			lblPaidAmountValue.TextAlignment = UITextAlignment.Left;
			lblPaidAmountValue.Lines = 1;
			lblPaidAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblPaidAmountValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblPaidAmountValue.TextColor = Colors.DarkGrayColor;


//			if (Constants.IsPhone ()) {
				lblClaimAmountValue.TextAlignment = UITextAlignment.Left;
				lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Left;
				lblClaimAmount.TextAlignment = UITextAlignment.Left;

				ContentView.AddSubview (lblDateOfExpense);
				ContentView.AddSubview (lblExpenseType);
				ContentView.AddSubview (lblClaimAmount);
				ContentView.AddSubview (lblAmtPreviouslyPaid);
//			} else {
//				
//				lblDateOfExpenseValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
//				lblExpenseTypeValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
//				lblClaimAmountValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
//				lblAmtPreviouslyPaidValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
//
//				lblClaimAmountValue.TextAlignment = UITextAlignment.Right;
//				lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Right;
//			}
			ContentView.AddSubview (lblFormNumber);
			ContentView.AddSubview (lblPaidAmount);
			ContentView.AddSubview (lblClaimStatus);
			ContentView.AddSubview (lblFormNumberValue);
			ContentView.AddSubview (lblClaimStatusValue);

			ContentView.AddSubview (lblDateOfExpenseValue);
			ContentView.AddSubview (lblExpenseTypeValue);
			ContentView.AddSubview (lblClaimAmountValue);
			ContentView.AddSubview (lblAmtPreviouslyPaidValue);
			ContentView.AddSubview (lblPaidAmountValue);

			SetConstraints	();  

			this.DelayBind (() => {
				var set = this.CreateBindingSet<ClaimListResultsTableViewCell,ClaimDetail> ();
//
//				set.Bind (this.lblDateOfExpenseValue).To (vm =>vm.ExpenseDate);
//				set.Bind (this.lblExpenseTypeValue).To (vm =>vm.ExpenseType.Name);
//				set.Bind (this.lblClaimAmountValue).To (vm =>vm.ClaimAmount);
//				set.Bind (this.lblAmtPreviouslyPaidValue).To (vm =>vm.OtherPaidAmount);
//			
				set.Apply ();
			});
		}

		private void SetConstraints()
		{


			ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

//			if (Constants.IsPhone ()) {
				this.AddConstraints (

					ContentView.AtLeftOf (this, 1),
					ContentView.WithSameWidth (this),

					lblFormNumber.AtTopOf (ContentView, 8),
					lblFormNumber.AtLeftOf (ContentView, 0),
					lblFormNumber.Height ().EqualTo (22),
					lblFormNumber.Width ().EqualTo (132),

					lblFormNumberValue.AtTopOf (ContentView, 8),
					lblFormNumberValue.AtRightOf (ContentView, 5),
					lblFormNumberValue.ToRightOf (lblFormNumber, 29),
					lblFormNumberValue.WithSameHeight (lblFormNumber),

					//

					lblDateOfExpense.Below (lblFormNumber, 5),
					lblDateOfExpense.AtLeftOf (ContentView, 0),
					lblDateOfExpense.Height ().EqualTo (22),
					lblDateOfExpense.Width ().EqualTo (132),

					lblDateOfExpenseValue.Below (lblFormNumber, 8),
					lblDateOfExpenseValue.AtRightOf (ContentView, 5),
					lblDateOfExpenseValue.ToRightOf (lblDateOfExpense, 29),
					lblDateOfExpenseValue.WithSameHeight (lblDateOfExpense),

					//------

					lblExpenseType.Below (lblDateOfExpense, 5),
					lblExpenseType.AtLeftOf (ContentView, 0),
					lblExpenseType.WithSameHeight (lblDateOfExpense),
					lblExpenseType.Width ().EqualTo (132),


					lblExpenseTypeValue.Below (lblDateOfExpense, 8),
					lblExpenseTypeValue.AtRightOf (ContentView, 5),
					lblExpenseTypeValue.ToRightOf (lblExpenseType, 29),//16
//					lblExpenseTypeValue.WithSameHeight (lblDateOfExpense),
					//-------

			    	lblClaimAmount.Below (lblExpenseTypeValue, 5),
					lblClaimAmount.AtLeftOf (ContentView, 0),
					lblClaimAmount.WithSameHeight (lblDateOfExpense),
					lblClaimAmount.Width ().EqualTo (132),

				    lblClaimAmountValue.Below (lblExpenseTypeValue, 8),
					lblClaimAmountValue.ToRightOf (lblClaimAmount, 29),//16
					lblClaimAmountValue.AtRightOf (ContentView, 5),
					lblClaimAmountValue.WithSameHeight (lblDateOfExpense),
					//	------------

					lblAmtPreviouslyPaid.Below (lblClaimAmount, 5),
					lblAmtPreviouslyPaid.AtLeftOf (ContentView, 0),
					lblAmtPreviouslyPaid.Width ().EqualTo (132),
			    	lblAmtPreviouslyPaid.WithSameHeight (lblDateOfExpense),

					lblAmtPreviouslyPaidValue.Below (lblClaimAmount, 5),
					lblAmtPreviouslyPaidValue.ToRightOf (lblAmtPreviouslyPaid, 29),//16
					lblAmtPreviouslyPaidValue.AtRightOf (ContentView, 5),//16
			    	lblAmtPreviouslyPaidValue.WithSameHeight (lblDateOfExpense),
				//
				   lblPaidAmount.Below (lblAmtPreviouslyPaid, 5),
			       lblPaidAmount.AtLeftOf (ContentView, 0),
				   lblPaidAmount.Width ().EqualTo (132),
				   lblPaidAmount.WithSameHeight (lblDateOfExpense),

				   lblPaidAmountValue.Below (lblAmtPreviouslyPaid, 5),
				   lblPaidAmountValue.ToRightOf (lblAmtPreviouslyPaid, 29),//16
				   lblPaidAmountValue.AtRightOf (ContentView, 5),//16
				   lblPaidAmountValue.WithSameHeight (lblDateOfExpense),
				   lblPaidAmountValue.AtBottomOf (ContentView, 10)

			);


		} 

	}
}

