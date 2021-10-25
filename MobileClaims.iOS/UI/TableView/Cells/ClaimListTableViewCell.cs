using System;
using Foundation;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.HCSA;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	public partial class ClaimListTableViewCell : MvxTableViewCell
	{
		public static readonly NSString CellIdentifier = new NSString ("ClaimListTableViewCell");
		public static readonly UINib Nib = UINib.FromName ("ClaimListTableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ClaimListTableViewCell");

		public UIView BackgroundView;
		public UILabel lblDate;
		public UILabel lblClaimedService;
		public UILabel lblOrignalAmt;
		public UILabel lblAmtPreviouslyPaid;
		public UILabel lblOrignalAmtValue;
		public UILabel lblAmtPreviouslyPaidValue; 
        public UITableViewCellState previousMask;
        private float borderWidth=Constants.HCSA_CellBorder;

        public static ClaimListTableViewCell Create ()
		{
			return (ClaimListTableViewCell)Nib.Instantiate (null, null) [0];
		}
		public ClaimListTableViewCell (IntPtr handle) : base (handle)
		{

			BackgroundView = new UIView();
			ContentView.BackgroundColor = Colors.LightGrayColor;

			lblDate = new UILabel();
			lblDate.Text = "2015-10-15";//"HCSA".tr();
			lblDate.BackgroundColor = Colors.Clear;
			lblDate.TextAlignment = UITextAlignment.Left;
			lblDate.Lines = 1;
			lblDate.LineBreakMode = UILineBreakMode.WordWrap;
			lblDate.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblDate.TextColor = Colors.DARK_GREY_COLOR;

			lblClaimedService = new UILabel();
			lblClaimedService.Text = "Dental Services";//"HCSA".tr();
			lblClaimedService.BackgroundColor = Colors.Clear;
			lblClaimedService.TextAlignment = UITextAlignment.Left;
			lblClaimedService.Lines = 0;
			lblClaimedService.LineBreakMode = UILineBreakMode.WordWrap;
			lblClaimedService.Font = UIFont.FromName (Constants.NUNITO_BOLD,16);
			lblClaimedService.TextColor = Colors.DarkGrayColor;

			lblOrignalAmt = new UILabel(); 
			lblOrignalAmt.BackgroundColor = Colors.Clear;
			lblOrignalAmt.TextAlignment = UITextAlignment.Left;
			lblOrignalAmt.Lines = 1;
			lblOrignalAmt.LineBreakMode = UILineBreakMode.WordWrap;
			lblOrignalAmt.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblOrignalAmt.TextColor = Colors.DARK_GREY_COLOR;

			lblAmtPreviouslyPaid = new UILabel(); 
			lblAmtPreviouslyPaid.BackgroundColor = Colors.Clear;
			lblAmtPreviouslyPaid.TextAlignment = UITextAlignment.Left;
			lblAmtPreviouslyPaid.Lines = 1;
			lblAmtPreviouslyPaid.LineBreakMode = UILineBreakMode.WordWrap;
			lblAmtPreviouslyPaid.Font = UIFont.FromName (Constants.NUNITO_SEMIBOLD,13);
			lblAmtPreviouslyPaid.TextColor = Colors.DARK_GREY_COLOR;

			lblOrignalAmtValue = new UILabel();
			lblOrignalAmtValue.Text = "$100";//"HCSA".tr();
			lblOrignalAmtValue.BackgroundColor = Colors.Clear;
			lblOrignalAmtValue.TextAlignment = UITextAlignment.Right;
			lblOrignalAmtValue.Lines = 1;
			lblOrignalAmtValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblOrignalAmtValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblOrignalAmtValue.TextColor = Colors.DarkGrayColor;

			lblAmtPreviouslyPaidValue = new UILabel();
			lblAmtPreviouslyPaidValue.Text = "$50";//"HCSA".tr();
			lblAmtPreviouslyPaidValue.BackgroundColor = Colors.Clear;
			lblAmtPreviouslyPaidValue.TextAlignment = UITextAlignment.Right;
			lblAmtPreviouslyPaidValue.Lines = 1;
			lblAmtPreviouslyPaidValue.LineBreakMode = UILineBreakMode.WordWrap;
			lblAmtPreviouslyPaidValue.Font = UIFont.FromName (Constants.NUNITO_BOLD,12);
			lblAmtPreviouslyPaidValue.TextColor = Colors.DarkGrayColor;

			ContentView.AddSubview (lblDate);
			ContentView.AddSubview (lblClaimedService);
			ContentView.AddSubview (lblOrignalAmt);
			ContentView.AddSubview (lblAmtPreviouslyPaid);
			ContentView.AddSubview (lblOrignalAmtValue);
			ContentView.AddSubview (lblAmtPreviouslyPaidValue);

			SetConstraints	();  

			this.DelayBind (() => {
				var set = this.CreateBindingSet<ClaimListTableViewCell,ClaimDetail>();

				set.Bind (this.lblDate).To (vm =>vm.ExpenseDate).WithConversion("StringFromDate");//.TwoWay().Apply(;
				set.Bind (this.lblClaimedService).To (vm =>vm.ExpenseTypeDescription);
                set.Bind(lblOrignalAmt).To(e => e.ClaimedAmountLabel);
				set.Bind (this.lblOrignalAmtValue).To (vm =>vm.ClaimAmount).WithConversion("DollarSignDoublePrefix");
                set.Bind(lblAmtPreviouslyPaid).To(e => e.OtherPaidLabel);
				set.Bind (this.lblAmtPreviouslyPaidValue).To (vm =>vm.OtherPaidAmount).WithConversion("DollarSignDoublePrefix");

				set.Apply ();
			});
		}

		private void SetConstraints()
        {
            float Side_Spacing = 20;

            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            this.AddConstraints(

                ContentView.AtLeftOf(this, 1),
                ContentView.AtRightOf(this, 1),
                ContentView.WithSameHeight(this).Minus(borderWidth * 2),
                ContentView.WithSameTop(this),

                lblDate.AtLeftOf(ContentView, 10),
                lblDate.AtRightOf(ContentView, 10),
                lblDate.AtTopOf(ContentView, 8),//50+15
                lblDate.Height().EqualTo(18),

                lblClaimedService.Below(lblDate, 3),
                lblClaimedService.AtRightOf(ContentView, 10),
                lblClaimedService.AtLeftOf(ContentView, 10),
//				lblClaimedService.Height().EqualTo(22), 

                lblOrignalAmt.Below(lblClaimedService, 5),
                lblOrignalAmt.AtLeftOf(ContentView, 10),
//				lblOrignalAmt.AtRightOf (ContentView,5),
                lblOrignalAmt.WithSameHeight(lblDate),

                lblAmtPreviouslyPaid.Below(lblOrignalAmt, 5),
                lblAmtPreviouslyPaid.AtLeftOf(ContentView, 10),
//				lblAmtPreviouslyPaid.AtRightOf (ContentView,5),
                lblAmtPreviouslyPaid.WithSameHeight(lblDate),
                lblAmtPreviouslyPaid.Width().EqualTo(225), 

                lblOrignalAmtValue.Below(lblClaimedService, 5),
//				lblOrignalAmtValue.ToRightOf (lblOrignalAmt,5),
                lblOrignalAmtValue.AtRightOf(ContentView, 10),
                lblOrignalAmtValue.WithSameHeight(lblAmtPreviouslyPaid),

                lblAmtPreviouslyPaidValue.Below(lblOrignalAmtValue, 5),
//				lblAmtPreviouslyPaidValue.ToRightOf (lblAmtPreviouslyPaid,5),
                lblAmtPreviouslyPaidValue.AtRightOf(ContentView, 10),//16
                lblAmtPreviouslyPaidValue.WithSameHeight(lblAmtPreviouslyPaid),
                lblAmtPreviouslyPaidValue.AtBottomOf(ContentView, 10)

            );
        } 

        public virtual bool ShowsDeleteButton()
        {
            return true;
        } 
      

        public override void WillTransitionToState(UITableViewCellState mask)
        {
            if (mask==UITableViewCellState.DefaultMask  ||mask == UITableViewCellState.ShowingDeleteConfirmationMask)
            {   
                if (this.Subviews.Length == 3)
                {
                    UIView vDelete = (UIView)this.Subviews[0];
                    UIView vContent = (UIView)this.Subviews[1];
                    CGRect fDeleteView = vDelete.Frame;
                    CGRect fContentView = vContent.Frame;
                    fDeleteView.Height = fContentView.Height;
                    vDelete.Frame = fDeleteView;
                    vDelete.Hidden = true;
                    vDelete.Alpha = 0;
                }
            }
            base.WillTransitionToState(mask);
        }

        public override void DidTransitionToState(UITableViewCellState mask)
        {  
            base.DidTransitionToState(mask);
            if (mask == UITableViewCellState.ShowingDeleteConfirmationMask)
            {   
                if (this.Subviews.Length == 3)
                {
                    UIView vDelete = (UIView)this.Subviews[0];
                    UIView vContent = (UIView)this.Subviews[1];
                    CGRect fDeleteView = vDelete.Frame;
                    CGRect fContentView = vContent.Frame;
                    fDeleteView.Height = fContentView.Height;
                    vDelete.Frame = fDeleteView;
                    vDelete.Hidden = false;
                    vDelete.Alpha = 1;
                }
            }
            previousMask = mask; 
        } 
    }
}
	