using System;
using Foundation;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.HCSA;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	public class ClaimConfirmationHCSATableViewCell: MvxTableViewCell
	{

		public static readonly NSString CellIdentifier = new NSString ("ClaimConfirmationHCSATableViewCell");
		public static readonly UINib Nib = UINib.FromName ("ClaimConfirmationHCSATableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ClaimConfirmationHCSATableViewCell");

		public UILabel lblDateOfExpense;
		public UILabel lblExpenseType;
		public UILabel lblClaimAmount;
		public UILabel lblAmtPreviouslyPaid;

		public UILabel lblDateOfExpenseValue;
		public UILabel lblExpenseTypeValue;
		public UILabel lblClaimAmountValue;
		public UILabel lblAmtPreviouslyPaidValue; 
    
		UILabel lblGrayLine;

		public static ClaimConfirmationHCSATableViewCell Create ()
		{
			return (ClaimConfirmationHCSATableViewCell)Nib.Instantiate (null, null) [0];
		}
		public ClaimConfirmationHCSATableViewCell (IntPtr handle) : base (handle)
		{

			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			lblDateOfExpense = new UILabel();
//			lblDateOfExpense.Text = "dateOfExpense".tr();
			lblDateOfExpense.BackgroundColor = Colors.Clear;
			lblDateOfExpense.TextAlignment = UITextAlignment.Left;
			lblDateOfExpense.Lines = 1;
			lblDateOfExpense.LineBreakMode = UILineBreakMode.WordWrap;
			lblDateOfExpense.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblDateOfExpense.TextColor = Colors.DARK_GREY_COLOR;

			lblExpenseType = new UILabel();
//			lblExpenseType.Text = "typeOfExpense".tr();
            lblExpenseType.BackgroundColor = Colors.Clear;
			lblExpenseType.TextAlignment = UITextAlignment.Left;
			lblExpenseType.Lines = 1;
			lblExpenseType.LineBreakMode = UILineBreakMode.WordWrap;
			lblExpenseType.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblExpenseType.TextColor = Colors.DARK_GREY_COLOR;


			lblClaimAmount = new UILabel();
//			lblClaimAmount.Text = "ClaimAmount";//"HCSA".tr();
			lblClaimAmount.BackgroundColor = Colors.Clear;
			lblClaimAmount.TextAlignment = UITextAlignment.Right;
			lblClaimAmount.Lines = 0;
			lblClaimAmount.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimAmount.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblClaimAmount.TextColor = Colors.DARK_GREY_COLOR;


			lblAmtPreviouslyPaid = new UILabel();
//			lblAmtPreviouslyPaid.Text = "PaidByInsurerGovt".tr();
			lblAmtPreviouslyPaid.BackgroundColor = Colors.Clear;
			lblAmtPreviouslyPaid.TextAlignment = UITextAlignment.Left;
			lblAmtPreviouslyPaid.Lines = 2;
			lblAmtPreviouslyPaid.LineBreakMode = UILineBreakMode.WordWrap;
			lblAmtPreviouslyPaid.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblAmtPreviouslyPaid.TextColor = Colors.DARK_GREY_COLOR;

			//------------

			lblDateOfExpenseValue = new UILabel();
//			lblDateOfExpenseValue.Text = "2015-10-15";//"HCSA".tr();
            lblDateOfExpenseValue.BackgroundColor = Colors.Clear;
			lblDateOfExpenseValue.TextAlignment = UITextAlignment.Left;
			lblDateOfExpenseValue.Lines = 0;
            lblDateOfExpenseValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblDateOfExpenseValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblDateOfExpenseValue.TextColor = Colors.DarkGrayColor;

			lblExpenseTypeValue = new UILabel();
//			lblExpenseTypeValue.Text = "Dental Service";//"HCSA".tr();
            lblExpenseTypeValue.BackgroundColor = Colors.Clear;
			lblExpenseTypeValue.TextAlignment = UITextAlignment.Left;
			lblExpenseTypeValue.Lines = 0;
			lblExpenseTypeValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblExpenseTypeValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblExpenseTypeValue.TextColor = Colors.DarkGrayColor;

			lblClaimAmountValue = new UILabel();
//			lblClaimAmountValue.Text = "$100";//"HCSA".tr();
			lblClaimAmountValue.BackgroundColor = Colors.Clear;
			lblClaimAmountValue.TextAlignment = UITextAlignment.Right;
			lblClaimAmountValue.Lines = 2;
			lblClaimAmountValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimAmountValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblClaimAmountValue.TextColor = Colors.DarkGrayColor;

			lblAmtPreviouslyPaidValue = new UILabel();
//			lblAmtPreviouslyPaidValue.Text = "$50";//"HCSA".tr();
			lblAmtPreviouslyPaidValue.BackgroundColor = Colors.Clear;
			lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Left;
			lblAmtPreviouslyPaidValue.Lines = 1;
			lblAmtPreviouslyPaidValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblAmtPreviouslyPaidValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblAmtPreviouslyPaidValue.TextColor = Colors.DarkGrayColor;

			lblGrayLine = new UILabel();
			lblGrayLine.BackgroundColor = Colors.MED_GREY_COLOR;

			if (Constants.IsPhone ()) {
				
				lblClaimAmountValue.TextAlignment = UITextAlignment.Left;
				lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Left;
				lblClaimAmount.TextAlignment = UITextAlignment.Left;

				ContentView.AddSubview (lblDateOfExpense);
				ContentView.AddSubview (lblExpenseType);
				ContentView.AddSubview (lblClaimAmount);
				ContentView.AddSubview (lblAmtPreviouslyPaid);
				ContentView.AddSubview (lblGrayLine);


			} else {
//				BackgroundColor = Colors.MED_GREY_COLOR;
				lblDateOfExpenseValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
				lblExpenseTypeValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
				lblClaimAmountValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);
				lblAmtPreviouslyPaidValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,13);

				lblClaimAmountValue.TextAlignment = UITextAlignment.Right;
				lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Right;
			}

			ContentView.AddSubview (lblDateOfExpenseValue);
			ContentView.AddSubview (lblExpenseTypeValue);
			ContentView.AddSubview (lblClaimAmountValue);
			ContentView.AddSubview (lblAmtPreviouslyPaidValue);

			SetConstraints	();  

			this.DelayBind (() => {
				var set = this.CreateBindingSet<ClaimConfirmationHCSATableViewCell,ClaimDetail> ();


				set.Bind (this.lblDateOfExpense).To (vm =>vm.DateOfExpenseLabel); 
				set.Bind (this.lblExpenseType).To (vm =>vm.TypeExpenseLabel); 
				set.Bind (this.lblClaimAmount).To (vm =>vm.ClaimedAmountLabel); 
				set.Bind (this.lblAmtPreviouslyPaid).To (vm =>vm.OtherPaidLabel); 

				set.Bind (this.lblDateOfExpenseValue).To (vm =>vm.ExpenseDate).WithConversion("StringFromDate");
				set.Bind (this.lblExpenseTypeValue).To (vm =>vm.ExpenseTypeDescription);//.ExpenseType.Name
				set.Bind (this.lblClaimAmountValue).To (vm =>vm.ClaimAmount).WithConversion("DollarSignDoublePrefix");
				set.Bind (this.lblAmtPreviouslyPaidValue).To (vm =>vm.OtherPaidAmount).WithConversion("DollarSignDoublePrefix");

				set.Apply ();
			});

          
		}

		private void SetConstraints()
		{  
			ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
              
			if (Constants.IsPhone ()) {
				this.AddConstraints (

					ContentView.AtLeftOf (this, 1),
					ContentView.WithSameWidth (this),

					lblDateOfExpense.AtTopOf (ContentView, 8),
					lblDateOfExpense.AtLeftOf (ContentView, 0),
					lblDateOfExpense.Height ().EqualTo (22),
					lblDateOfExpense.WithRelativeWidth(ContentView,0.5f), 
                   
                    lblDateOfExpenseValue.WithSameCenterY(lblDateOfExpense), 
                    lblDateOfExpenseValue.ToRightOf (lblDateOfExpense, 12),//10
                    lblDateOfExpenseValue.WithSameWidth(lblDateOfExpense).Minus (12), 
				 
                    lblExpenseType.WithSameLeft(lblDateOfExpense),
                    lblExpenseType.WithSameWidth(lblDateOfExpense), 
                    lblExpenseType.Below (lblDateOfExpense, 5),
  
                    lblExpenseTypeValue.WithSameTop(lblExpenseType), 
					lblExpenseTypeValue.ToRightOf (lblExpenseType, 12),//16 
                    lblExpenseTypeValue.WithSameWidth(lblExpenseType).Minus (12),

					lblClaimAmount.Below (lblExpenseTypeValue, 5),
                    lblClaimAmount.WithSameLeft(lblExpenseType), 
					lblClaimAmount.WithRelativeWidth(ContentView,0.5f),


                    lblClaimAmountValue.WithSameCenterY(lblClaimAmount),
					lblClaimAmountValue.ToRightOf (lblClaimAmount, 12),//16
					lblClaimAmountValue.AtRightOf (ContentView, 5),
					lblClaimAmountValue.WithSameHeight (lblClaimAmount),
					//	------------

					lblAmtPreviouslyPaid.Below (lblClaimAmount, 5),
					lblAmtPreviouslyPaid.AtLeftOf (ContentView, 0),
					lblAmtPreviouslyPaid.WithRelativeWidth(ContentView,0.5f),


					lblAmtPreviouslyPaidValue.Below (lblClaimAmount, 5),
					lblAmtPreviouslyPaidValue.ToRightOf (lblAmtPreviouslyPaid, 12),//16
					lblAmtPreviouslyPaidValue.AtRightOf (ContentView, 5),//16
					lblAmtPreviouslyPaidValue.WithSameHeight (lblAmtPreviouslyPaid),
//					lblAmtPreviouslyPaidValue.AtBottomOf (ContentView, 20)

					lblGrayLine.Below (lblAmtPreviouslyPaidValue, 5),
					lblGrayLine.Height ().EqualTo (1),
					lblGrayLine.AtRightOf (ContentView, 0),
					lblGrayLine.AtLeftOf (ContentView, 0),
					lblGrayLine.AtBottomOf (ContentView, 1)

				);

			} else {//ipad

				this.AddConstraints (

					ContentView.AtLeftOf (this, 1),
					ContentView.WithSameWidth(this),

					lblDateOfExpenseValue.AtTopOf (ContentView,2),
					lblDateOfExpenseValue.AtLeftOf (ContentView,2),
					lblDateOfExpenseValue.Height ().EqualTo (18),
					lblDateOfExpenseValue.Width ().EqualTo (150),

					lblExpenseTypeValue.AtTopOf (ContentView,2),
					lblExpenseTypeValue.ToRightOf (lblDateOfExpenseValue,15),
//					lblExpenseTypeValue.Height ().EqualTo (18),
					lblExpenseTypeValue.Width ().EqualTo (240),

					lblClaimAmountValue.AtTopOf (ContentView,2),
					lblClaimAmountValue.ToRightOf (lblExpenseTypeValue,5),
					lblClaimAmountValue.Height ().EqualTo (18),
					lblClaimAmountValue.ToLeftOf (lblAmtPreviouslyPaidValue,15),
					lblClaimAmountValue.AtBottomOf (ContentView, 8),


					lblAmtPreviouslyPaidValue.AtTopOf (ContentView,2),
					lblAmtPreviouslyPaidValue.Width ().EqualTo (230),
					lblAmtPreviouslyPaidValue.AtRightOf (ContentView,10),
					lblAmtPreviouslyPaidValue.Height ().EqualTo (18)

				);
			}
             
		} 
	}
}

