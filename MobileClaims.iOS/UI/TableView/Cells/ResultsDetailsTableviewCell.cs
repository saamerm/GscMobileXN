using System;
using UIKit;
using Foundation;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Entities;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;


namespace MobileClaims.iOS
{

	public class ResultsDetailsTableviewCell: MvxTableViewCell
	{

		public static readonly NSString CellIdentifier = new NSString ("ResultsDetailsTableviewCell");
		public static readonly UINib Nib = UINib.FromName ("ResultsDetailsTableviewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ResultsDetailsTableviewCell");

		public UILabel lblDateOfExpense;
		public UILabel lblExpenseType;
		public UILabel lblClaimAmount;
		public UILabel lblAmtPreviouslyPaid;

        public LabelNunitoSemiBold13_NunitoBold12 lblDateOfExpenseValue;
        public LabelNunitoSemiBold13_NunitoBold12 lblExpenseTypeValue;
        public LabelNunitoSemiBold13_NunitoBold12 lblClaimAmountValue;
        public LabelNunitoSemiBold13_NunitoBold12 lblAmtPreviouslyPaidValue;

		public UILabel lblGrayLine;

 		public UILabel lblFormNumber;//
 		public UILabel lblPaidAmount;//
		public UILabel lblClaimStatus;//
        public LabelNunitoSemiBold13_NunitoBold12 lblPaidAmountValue;//
        public LabelNunitoSemiBold13_NunitoBold12 lblClaimStatusValue;//
        public LabelNunitoSemiBold13_NunitoBold12 lblFormNumberValue;//
        public string eOBContent=string.Empty;
        private int eoBContentLines=0;
        private UILabel lblMessages;
        private LabelNunitoSemiBold13_NunitoBold12 lblMessagesValue;
        private float cellHeigth=Constants.RESULTSDETAILSTABLEVIEWCELLHEIGHT;
        private float messageHeight=Constants.EOBMESSAGE_LINEHEIGHT ;
        private int linesOfExpenseType=0;
        private float topAdjustment=4f;

		public static ResultsDetailsTableviewCell Create ()
		{
			return (ResultsDetailsTableviewCell)Nib.Instantiate (null, null) [0];
		}
		public ResultsDetailsTableviewCell (IntPtr handle) : base (handle)
        {    
            this.SelectionStyle = UITableViewCellSelectionStyle.None; 
            CountMessages();

            lblFormNumber = new UILabel(); 
            lblFormNumber.BackgroundColor = Colors.Clear;
            lblFormNumber.TextAlignment = UITextAlignment.Left;
            lblFormNumber.Lines = 0;
            lblFormNumber.LineBreakMode = UILineBreakMode.WordWrap;
            lblFormNumber.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblFormNumber.TextColor = Colors.DARK_GREY_COLOR; 

            lblPaidAmount = new UILabel(); 
            lblPaidAmount.BackgroundColor = Colors.Clear;
            lblPaidAmount.TextAlignment = UITextAlignment.Left;
            lblPaidAmount.Lines = 0;
            lblPaidAmount.LineBreakMode = UILineBreakMode.WordWrap;
            lblPaidAmount.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblPaidAmount.TextColor = Colors.DARK_GREY_COLOR;

            lblClaimStatus = new UILabel();
//			lblClaimStatus.Text = "Claim Status";//"HCSA".tr();
            lblClaimStatus.BackgroundColor = Colors.Clear;
            lblClaimStatus.TextAlignment = UITextAlignment.Left;
            lblClaimStatus.Lines = 0;
            lblClaimStatus.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimStatus.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblClaimStatus.TextColor = Colors.DARK_GREY_COLOR;

            lblGrayLine = new UILabel();
            lblGrayLine.BackgroundColor = Colors.MED_GREY_COLOR;

            lblFormNumberValue = new LabelNunitoSemiBold13_NunitoBold12();
 
            lblClaimStatusValue = new LabelNunitoSemiBold13_NunitoBold12(); 

            lblPaidAmountValue = new LabelNunitoSemiBold13_NunitoBold12();  

            lblDateOfExpense = new UILabel();
//			lblDateOfExpense.Text = "Date of Expense";//"HCSA".tr();
            lblDateOfExpense.BackgroundColor = Colors.Clear;
            lblDateOfExpense.TextAlignment = UITextAlignment.Left;
            lblDateOfExpense.Lines = 0;
            lblDateOfExpense.LineBreakMode = UILineBreakMode.WordWrap;
            lblDateOfExpense.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblDateOfExpense.TextColor = Colors.DARK_GREY_COLOR;

            lblExpenseType = new UILabel();
//			lblExpenseType.Text = "Type of Expense";//"HCSA".tr();
            lblExpenseType.BackgroundColor = Colors.Clear;
            lblExpenseType.TextAlignment = UITextAlignment.Left;
            lblExpenseType.Lines = 0;
            lblExpenseType.LineBreakMode = UILineBreakMode.WordWrap;
            lblExpenseType.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblExpenseType.TextColor = Colors.DARK_GREY_COLOR;


            lblClaimAmount = new UILabel();
//			lblClaimAmount.Text = "Claim Amount";//"HCSA".tr();
            lblClaimAmount.BackgroundColor = Colors.Clear;
            lblClaimAmount.TextAlignment = UITextAlignment.Right;
            lblClaimAmount.Lines = 0;
            lblClaimAmount.LineBreakMode = UILineBreakMode.WordWrap;
            lblClaimAmount.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblClaimAmount.TextColor = Colors.DARK_GREY_COLOR;


            lblAmtPreviouslyPaid = new UILabel(); 
            lblAmtPreviouslyPaid.BackgroundColor = Colors.Clear;
            lblAmtPreviouslyPaid.TextAlignment = UITextAlignment.Left;
            lblAmtPreviouslyPaid.Lines = 0;
            lblAmtPreviouslyPaid.LineBreakMode = UILineBreakMode.WordWrap;
            lblAmtPreviouslyPaid.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 12);
            lblAmtPreviouslyPaid.TextColor = Colors.DARK_GREY_COLOR;

            //------------

            lblDateOfExpenseValue = new LabelNunitoSemiBold13_NunitoBold12();
 
            lblDateOfExpenseValue.TextColor = Colors.DarkGrayColor;

            lblExpenseTypeValue = new LabelNunitoSemiBold13_NunitoBold12(); 
 
            lblClaimAmountValue = new LabelNunitoSemiBold13_NunitoBold12(); 

            lblAmtPreviouslyPaidValue = new LabelNunitoSemiBold13_NunitoBold12();  

            lblClaimAmountValue.TextAlignment = UITextAlignment.Left;
            lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Left;
            lblClaimAmount.TextAlignment = UITextAlignment.Left;

            lblMessages = new UILabel(); 
            lblMessages.BackgroundColor = Colors.Clear; 
            lblMessages.TextAlignment = UITextAlignment.Left; 
            lblMessages.Lines = 0;
            lblMessages.LineBreakMode = UILineBreakMode.WordWrap;
            lblMessages.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 13);
            lblMessages.TextColor = Colors.DARK_GREY_COLOR; 
          

            lblMessagesValue = new LabelNunitoSemiBold13_NunitoBold12();  

            ContentView.AddSubview(lblDateOfExpense);
            ContentView.AddSubview(lblExpenseType);
            ContentView.AddSubview(lblClaimAmount);
            ContentView.AddSubview(lblAmtPreviouslyPaid);
            ContentView.AddSubview(lblFormNumber);
            ContentView.AddSubview(lblPaidAmount); 
            ContentView.AddSubview(lblClaimStatus);
		    
            ContentView.AddSubview(lblFormNumberValue);
            ContentView.AddSubview(lblClaimStatusValue);
            ContentView.AddSubview(lblDateOfExpenseValue);
            ContentView.AddSubview(lblExpenseTypeValue);
            ContentView.AddSubview(lblClaimAmountValue);
            ContentView.AddSubview(lblPaidAmountValue);
            ContentView.AddSubview(lblAmtPreviouslyPaidValue);
            ContentView.AddSubview(lblGrayLine);
            ContentView.AddSubview(lblMessages);
            ContentView.AddSubview(lblMessagesValue);

            SetConstraints();   

            this.DelayBind(() =>
            {
				
                var set = this.CreateBindingSet<ResultsDetailsTableviewCell,ClaimDetail>();//ClaimResultDetailGSC
                set.Bind(this.lblFormNumber).To(vm => vm.FormNumberLabel);
                set.Bind(this.lblDateOfExpense).To(vm => vm.DateOfExpenseLabel);
                set.Bind(this.lblExpenseType).To(vm => vm.TypeExpenseLabel);
                set.Bind(this.lblClaimAmount).To(vm => vm.ClaimedAmountLabel);
                set.Bind(this.lblAmtPreviouslyPaid).To(vm => vm.OtherPaidLabel);
                set.Bind(this.lblPaidAmount).To(vm => vm.PaidAmountLabel);
                set.Bind(this.lblClaimStatus).To(vm => vm.ClaimStatusLabel);
                set.Bind(this.lblMessages).To(vm => vm.EOBLabel);

                set.Bind(this.lblFormNumberValue).For(l=>l.TextContent).To(vm => vm.ClaimFormID);
                set.Bind(this.lblDateOfExpenseValue).For(l=>l.TextContent).To(vm => vm.ExpenseDate).WithConversion("StringFromDate");
                set.Bind(this.lblExpenseTypeValue).For(l=>l.TextContent).To(vm => vm.ExpenseTypeDescription);
                set.Bind(this.lblClaimAmountValue).For(l=>l.TextContent).To(vm => vm.ClaimAmount).WithConversion("DollarSignDoublePrefix");
                set.Bind(this.lblAmtPreviouslyPaidValue).For(l=>l.TextContent).To(vm => vm.OtherPaidAmount).WithConversion("DollarSignDoublePrefix");
                set.Bind(this.lblPaidAmountValue).For(l=>l.TextContent).To(vm => vm.PaidAmount).WithConversion("DollarSignDoublePrefix");
                set.Bind(this.lblClaimStatusValue).For(l=>l.TextContent).To(vm => vm.ClaimStatus); 
                set.Bind(this).For(c => c.Messages).To(vm => vm.EOBMessages);

                set.Bind(this).For(c=>c.ExpenseType).To(vm=>vm.ExpenseTypeDescription);
                set.Apply();
            });
        }

		public void SetConstraints()
		{
            CountMessages();
            this.RemoveConstraints(this.Constraints);
            ContentView.RemoveConstraints(ContentView.Constraints);
            cellHeigth = Constants.RESULTSDETAILSTABLEVIEWCELLHEIGHT  + (eoBContentLines+linesOfExpenseType) * messageHeight;
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints(); 
             
			if (Constants.IsPhone ()) {
				this.AddConstraints ( 
                    ContentView .AtTopOf(this,1),
					ContentView.AtLeftOf (this, 1),
					ContentView.WithSameWidth (this), 
                    ContentView.Height().EqualTo (cellHeigth), 

					lblFormNumber.AtTopOf (ContentView, 8),
					lblFormNumber.AtLeftOf (ContentView, 0),
					lblFormNumber.Height ().EqualTo (22), 
					lblFormNumber.WithRelativeWidth(ContentView,0.5f).Plus(5),

                    lblFormNumberValue.WithSameTop(lblFormNumber).Plus(topAdjustment),
					lblFormNumberValue.AtRightOf (ContentView, 5),
					lblFormNumberValue.ToRightOf (lblFormNumber, 10),//29
					lblFormNumberValue.WithSameHeight (lblFormNumber), 
				 
                    lblDateOfExpense.Below (lblFormNumber,5),
					lblDateOfExpense.AtLeftOf (ContentView, 0),
					lblDateOfExpense.Height ().EqualTo (22),
					lblDateOfExpense.WithRelativeWidth(ContentView,0.5f).Plus(5), 
		 
                    lblDateOfExpenseValue.WithSameTop(lblDateOfExpense).Plus(topAdjustment),
					lblDateOfExpenseValue.AtRightOf (ContentView, 5),
					lblDateOfExpenseValue.ToRightOf (lblDateOfExpense, 10),
					lblDateOfExpenseValue.WithSameHeight (lblDateOfExpense),

					//------

					lblExpenseType.Below (lblDateOfExpense, 5),
					lblExpenseType.AtLeftOf (ContentView, 0), 
					lblExpenseType.WithRelativeWidth(ContentView,0.5f).Plus(5), 
				 
					lblExpenseTypeValue.AtRightOf (ContentView, 5),
                    lblExpenseTypeValue.WithSameTop(lblExpenseType).Plus(topAdjustment),
					lblExpenseTypeValue.ToRightOf (lblExpenseType, 10)//16
// 
					//-------
                );
                if (linesOfExpenseType<=1)
                {
                    this.AddConstraints(lblClaimAmount.Below(lblExpenseType, 5));
                }
                else
                {
                    this.AddConstraints(lblClaimAmount.Below(lblExpenseTypeValue, 5)); 
                }

                this.AddConstraints(
                    lblClaimAmount.AtLeftOf(ContentView, 0), 
                    lblClaimAmount.WithRelativeWidth(ContentView, 0.5f).Plus(5),

                    lblClaimAmountValue.WithSameTop(lblClaimAmount).Plus(topAdjustment),
                    lblClaimAmountValue.ToRightOf(lblClaimAmount, 10),//16
                    lblClaimAmountValue.AtRightOf(ContentView, 5), 
					//	------------

                    lblAmtPreviouslyPaid.Below(lblClaimAmount, 5),
                    lblAmtPreviouslyPaid.AtLeftOf(ContentView, 0), 
                    lblAmtPreviouslyPaid.WithRelativeWidth(ContentView, 0.5f).Plus(5),

                    lblAmtPreviouslyPaidValue.WithSameTop(lblAmtPreviouslyPaid).Plus(topAdjustment),
                    lblAmtPreviouslyPaidValue.ToRightOf(lblAmtPreviouslyPaid, 10),//16
                    lblAmtPreviouslyPaidValue.AtRightOf(ContentView, 5),//16  

                    lblPaidAmount.Below(lblAmtPreviouslyPaid, 5),
                    lblPaidAmount.AtLeftOf(ContentView, 0), 
                    lblPaidAmount.WithRelativeWidth(ContentView, 0.5f).Plus(5),

                    lblPaidAmountValue.WithSameTop(lblPaidAmount).Plus(topAdjustment),
                    lblPaidAmountValue.ToRightOf(lblPaidAmount, 10),//16
                    lblPaidAmountValue.AtRightOf(ContentView, 5),//16 

                    lblClaimStatus.Below(lblPaidAmount, 5),
                    lblClaimStatus.AtLeftOf(ContentView, 0), 
                    lblClaimStatus.WithRelativeWidth(ContentView, 0.5f).Plus(5),

                    lblClaimStatusValue.WithSameTop(lblClaimStatus).Plus(topAdjustment),
                    lblClaimStatusValue.ToRightOf(lblClaimStatus, 10),//16
                    lblClaimStatusValue.AtRightOf(ContentView, 5),//16
					 

                    lblMessages.Below(lblClaimStatus, 5),
                    lblMessages.AtLeftOf(ContentView, 0), 
                    lblMessages.WithRelativeWidth(ContentView, 0.5f).Plus(5),

                    lblMessagesValue.WithSameTop(lblMessages).Plus(topAdjustment),  
                    lblMessagesValue.ToRightOf(lblClaimStatus, 10),//16
                    lblMessagesValue.AtRightOf(ContentView, 5)
                );//16
                if (eoBContentLines == 0)
                {
                    this.AddConstraints( lblGrayLine.Below (lblMessages, 5));
                }
                else
                {
                    this.AddConstraints ( lblGrayLine.Below (lblMessagesValue, 5));
                }
                this.AddConstraints( 
					lblGrayLine.Height ().EqualTo (1),
					lblGrayLine.AtRightOf (ContentView,0),
					lblGrayLine.AtLeftOf (ContentView, 0)
					//lblGrayLine.AtBottomOf (ContentView, 1)

//					lblClaimStatusValue.AtBottomOf (ContentView, 10)

				);
			} 
            else // iPad
            { 
				this.AddConstraints ( 
					ContentView.AtLeftOf (this, 1),
					ContentView.WithSameWidth (this), 

					lblFormNumber.AtTopOf (ContentView, 8),
					lblFormNumber.AtLeftOf (ContentView, 15), 
					lblFormNumber.WithRelativeWidth(ContentView,0.5f).Minus(20),

                    lblFormNumberValue.WithSameTop(lblFormNumber).Plus(topAdjustment),
					lblFormNumberValue.AtRightOf (ContentView, 5),
					lblFormNumberValue.ToRightOf (lblFormNumber, 20),//140 

                    lblDateOfExpense.Below(lblFormNumber, 5),//8
					lblDateOfExpense.AtLeftOf (ContentView, 15), 
					lblDateOfExpense.WithRelativeWidth(ContentView,0.5f).Minus(20),

                    lblDateOfExpenseValue.WithSameTop(lblDateOfExpense).Plus(topAdjustment),
					lblDateOfExpenseValue.AtRightOf (ContentView, 5),
					lblDateOfExpenseValue.ToRightOf (lblDateOfExpense, 20), 

					lblExpenseType.Below (lblDateOfExpense, 5),
					lblExpenseType.AtLeftOf (ContentView, 15), 
					lblExpenseType.WithRelativeWidth(ContentView,0.5f).Minus(20),


                    lblExpenseTypeValue.WithSameTop(lblExpenseType).Plus(topAdjustment),
					lblExpenseTypeValue.AtRightOf (ContentView, 5),
					lblExpenseTypeValue.ToRightOf (lblExpenseType, 20)//16  
                );

//                if (linesOfExpenseType<=1)
//                {
//                    this.AddConstraints(lblClaimAmount.Below(lblExpenseType, 5));
//                }
//                else
//                {
                    this.AddConstraints(lblClaimAmount.Below(lblExpenseTypeValue, 5)); 
               // }

                this.AddConstraints (
					lblClaimAmount.AtLeftOf (ContentView, 15), 
					lblClaimAmount.WithRelativeWidth(ContentView,0.5f).Minus(20), 

                    lblClaimAmountValue.WithSameTop(lblClaimAmount).Plus(topAdjustment),
					lblClaimAmountValue.ToRightOf (lblClaimAmount, 20),//16
					lblClaimAmountValue.AtRightOf (ContentView, 5), 

					lblAmtPreviouslyPaid.Below (lblClaimAmount, 5),
					lblAmtPreviouslyPaid.AtLeftOf (ContentView, 15), 
					lblAmtPreviouslyPaid.WithRelativeWidth(ContentView,0.5f).Minus(20),

                    lblAmtPreviouslyPaidValue.WithSameTop(lblAmtPreviouslyPaid).Plus(topAdjustment),
					lblAmtPreviouslyPaidValue.ToRightOf (lblAmtPreviouslyPaid, 20),//16
					lblAmtPreviouslyPaidValue.AtRightOf (ContentView, 5),//16 

					lblPaidAmount.Below (lblAmtPreviouslyPaid, 5),
					lblPaidAmount.AtLeftOf (ContentView, 15), 
					lblPaidAmount.WithRelativeWidth(ContentView,0.5f).Minus(20),

                    lblPaidAmountValue.WithSameTop(lblPaidAmount).Plus(topAdjustment),
					lblPaidAmountValue.ToRightOf (lblPaidAmount, 20),//16
					lblPaidAmountValue.AtRightOf (ContentView, 5),//16 

                    lblClaimStatus.Below (lblPaidAmount, 5),
					lblClaimStatus.AtLeftOf (ContentView, 15), 
					lblClaimStatus.WithRelativeWidth(ContentView,0.5f).Minus(20),

                    lblClaimStatusValue.WithSameTop(lblClaimStatus).Plus(topAdjustment),
					lblClaimStatusValue.ToRightOf (lblClaimStatus, 20),//16
					lblClaimStatusValue.AtRightOf (ContentView, 5),//16
					//lblClaimStatusValue.WithSameHeight (lblAmtPreviouslyPaid),

                    lblMessages.Below (lblClaimStatusValue, 5),
                    lblMessages.AtLeftOf (ContentView, 15), 
                    lblMessages.WithRelativeWidth(ContentView,0.5f).Plus(5),

                    lblMessagesValue.WithSameTop(lblMessages).Plus(topAdjustment),
                    lblMessagesValue.ToRightOf (lblClaimStatus, 20),//16 
                    lblMessagesValue.AtRightOf (ContentView, 5)
                );//16 
                if (eoBContentLines == 0)
                {
                    this.AddConstraints( lblGrayLine.Below (lblMessages, 5));
                }
                else
                {
                    this.AddConstraints ( lblGrayLine.Below (lblMessagesValue, 5));
                }

                this.AddConstraints(   
					lblGrayLine.Height ().EqualTo (1),
					lblGrayLine.AtRightOf (ContentView, 0),
					lblGrayLine.AtLeftOf (ContentView, 15),
					lblGrayLine.AtBottomOf (ContentView, 1)

//					lblClaimStatusValue.AtBottomOf (ContentView, 10)

				); 
			} 
		}  

        private void CountMessages()
        {  
            eoBContentLines = 0; 
            if (Messages != null)
            {
                int countMessages = Messages.Count;
                if (countMessages > 0)
                {
                    int linesForeachMessage = 0;
                    for (int i = 0; i < countMessages; i++)
                    {
                        string MessageStr = Messages[i].Message.ToString();
                        if (!string.IsNullOrEmpty(MessageStr))
                        {
                            int longOfStr = MessageStr.Length;
                            if (longOfStr > 0)
                            {  
                                int messageLong =Helpers.IsInPortraitMode()? Constants.EOBMESSAGELIMATELENGTH_PORTRAIT :Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE ;
                                linesForeachMessage = (int)(longOfStr / messageLong ); 
                                linesForeachMessage += 1;
                                eOBContent +=  Messages[i].Message.ToString()+"\n";  
                            } 
                        } 
                        eoBContentLines += linesForeachMessage;
                    }  
                } 
            } 
        }

        private List<ClaimEOBMessageGSC> _messages;
        public List<ClaimEOBMessageGSC> Messages
        {
            get{ return _messages;}
            set{
                _messages = value; 
                CountMessages(); 
                lblMessagesValue.TextContent = eOBContent;
                SetConstraints();
            }
            
        } 

        private string _expenseType;
        public string ExpenseType
        {
            get{ return _expenseType;}
            set{
                _expenseType = value; 
                GetExpenseTypeLines();
                SetConstraints();
            } 
        } 

        private void GetExpenseTypeLines()
        {
            linesOfExpenseType = 0; 
            if (!string.IsNullOrEmpty(_expenseType))
            {
                int longOfStr = _expenseType.Length;
                if (longOfStr > 0)
                { 
                    int messageLong = Helpers.IsInPortraitMode() ? Constants.EOBMESSAGELIMATELENGTH_PORTRAIT : Constants.EOBMESSAGELIMATELENGTH_LANDSCAPE;
                    linesOfExpenseType = (int)(longOfStr / messageLong); 
                    linesOfExpenseType += 1;
                }
            }
        }
	}
}

