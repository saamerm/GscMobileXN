using System;
using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimResultDetailTableCell")]
	public class ClaimResultDetailTableCell : MvxTableViewCell
	{
		public UILabel formNumTitleLabel;
		public UILabel serviceDateTitleLabel;
		public UILabel serviceDescriptionTitleLabel;
		public UILabel claimedAmountTitleLabel;
		public UILabel otherPaidAmountTitleLabel;
//		public UILabel deductibleTitleLabel;
//		public UILabel copayTitleLabel;
		public UILabel paidAmountTitleLabel;
		public UILabel statusTitleLabel;

		public UILabel formNumLabel;
		public UILabel serviceDateLabel;
		public UILabel serviceDescriptionLabel;
		public UILabel claimedAmountLabel;
		public UILabel otherPaidAmountLabel;
//		public UILabel deductibleLabel;
//		public UILabel copayLabel;
		public UILabel paidAmountLabel;
		public UILabel statusLabel;
        public UILabel eobLabel;


		protected UIView cellBackingView;

		public ClaimResultDetailTableCell () : base () {
			CreateLayout();
			InitializeBindings();
		}
		public ClaimResultDetailTableCell (IntPtr handle) : base (handle) {
			CreateLayout();
			InitializeBindings();
		}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public void CreateLayout()
		{
            this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float fieldPadding = 20;
			float innerPadding = 10;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = (float)this.Frame.Width;
			float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth/2 + fieldPadding;
            float resultWidth = contentWidth / 2 - Constants.DRUG_LOOKUP_SIDE_PADDING - fieldPadding;
			float topPadding = 15;
			float yPos = 0;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (formNumLabel == null)
				formNumLabel = new UILabel ();
			formNumLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			formNumLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			formNumLabel.TextColor = Colors.DARK_GREY_COLOR;
			formNumLabel.TextAlignment = UITextAlignment.Left;
			formNumLabel.BackgroundColor = Colors.Clear;
			formNumLabel.Lines = 0;
			formNumLabel.LineBreakMode = UILineBreakMode.WordWrap;
			formNumLabel.SizeToFit ();
			formNumLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (formNumLabel);

			if (formNumTitleLabel == null)
				formNumTitleLabel = new UILabel ();
			formNumTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			formNumTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			formNumTitleLabel.TextAlignment = UITextAlignment.Left;
			formNumTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			formNumTitleLabel.BackgroundColor = Colors.Clear;
			formNumTitleLabel.Lines = 0;
			formNumTitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			formNumTitleLabel.Text="formNumber".tr();
			formNumTitleLabel.SizeToFit ();
			formNumTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (formNumTitleLabel);

			yPos += (float)formNumTitleLabel.Frame.Height + fieldPadding;

			if (serviceDateLabel == null)
				serviceDateLabel = new UILabel ();
			serviceDateLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			serviceDateLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			serviceDateLabel.TextColor = Colors.DARK_GREY_COLOR;
			serviceDateLabel.TextAlignment = UITextAlignment.Left;
			serviceDateLabel.BackgroundColor = Colors.Clear;
			serviceDateLabel.Lines = 0;
			serviceDateLabel.SizeToFit ();
			serviceDateLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (serviceDateLabel);

			if (serviceDateTitleLabel == null)
				serviceDateTitleLabel = new UILabel ();
			serviceDateTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			serviceDateTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			serviceDateTitleLabel.TextAlignment = UITextAlignment.Left;
			serviceDateTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			serviceDateTitleLabel.BackgroundColor = Colors.Clear;
			serviceDateTitleLabel.Lines = 0;
			serviceDateTitleLabel.Text= HasHCSADetails ? "dateOfExpense".tr() : "serviceDate".tr();
			serviceDateTitleLabel.SizeToFit ();
			serviceDateTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (serviceDateTitleLabel);

			yPos += (float)serviceDateTitleLabel.Frame.Height + fieldPadding;

			if (serviceDescriptionLabel == null)
				serviceDescriptionLabel = new UILabel ();
            serviceDescriptionLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			serviceDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			serviceDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
			serviceDescriptionLabel.TextAlignment = UITextAlignment.Left;
            serviceDescriptionLabel.BackgroundColor = Colors.Clear;
			serviceDescriptionLabel.Lines = 0;
			serviceDescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			serviceDescriptionLabel.SizeToFit ();
			serviceDescriptionLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (serviceDescriptionLabel);

			if (serviceDescriptionTitleLabel == null)
				serviceDescriptionTitleLabel = new UILabel ();
			serviceDescriptionTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			serviceDescriptionTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			serviceDescriptionTitleLabel.TextAlignment = UITextAlignment.Left;
			serviceDescriptionTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            serviceDescriptionTitleLabel.BackgroundColor = Colors.Clear;
			serviceDescriptionTitleLabel.Lines = 0;
			serviceDescriptionTitleLabel.Text= HasHCSADetails ? "typeOfExpense".tr() :"serviceDescription".tr();
			serviceDescriptionTitleLabel.SizeToFit ();
			serviceDescriptionTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (serviceDescriptionTitleLabel);

			yPos += (float)serviceDescriptionTitleLabel.Frame.Height + fieldPadding;

			if (claimedAmountLabel == null)
				claimedAmountLabel = new UILabel ();
			claimedAmountLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			claimedAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			claimedAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			claimedAmountLabel.TextAlignment = UITextAlignment.Left;
			claimedAmountLabel.BackgroundColor = Colors.Clear;
			claimedAmountLabel.Lines = 0;
			claimedAmountLabel.SizeToFit ();
			claimedAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (claimedAmountLabel);

			if (claimedAmountTitleLabel == null)
				claimedAmountTitleLabel = new UILabel ();
			claimedAmountTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			claimedAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			claimedAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			claimedAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			claimedAmountTitleLabel.BackgroundColor = Colors.Clear;
			claimedAmountTitleLabel.Lines = 0;
			claimedAmountTitleLabel.Text="claimedAmount".tr();
			claimedAmountTitleLabel.SizeToFit ();
			claimedAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (claimedAmountTitleLabel);

			yPos += (float)claimedAmountTitleLabel.Frame.Height + fieldPadding;

			if (otherPaidAmountLabel == null)
				otherPaidAmountLabel = new UILabel ();
			otherPaidAmountLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			otherPaidAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			otherPaidAmountLabel.TextAlignment = UITextAlignment.Left;
			otherPaidAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			otherPaidAmountLabel.BackgroundColor = Colors.Clear;
			otherPaidAmountLabel.Lines = 0;
			otherPaidAmountLabel.SizeToFit ();
			otherPaidAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (otherPaidAmountLabel);

			if (otherPaidAmountTitleLabel == null)
				otherPaidAmountTitleLabel = new UILabel ();
			otherPaidAmountTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			otherPaidAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			otherPaidAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			otherPaidAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			otherPaidAmountTitleLabel.BackgroundColor = Colors.Clear;
			otherPaidAmountTitleLabel.Lines = 0;
			otherPaidAmountTitleLabel.Text="otherPaidAmount".tr();
			otherPaidAmountTitleLabel.SizeToFit ();
			otherPaidAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (otherPaidAmountTitleLabel);

			yPos += (float)otherPaidAmountTitleLabel.Frame.Height + fieldPadding;

			if (paidAmountLabel == null)
				paidAmountLabel = new UILabel ();
			paidAmountLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			paidAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			paidAmountLabel.TextAlignment = UITextAlignment.Left;
			paidAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			paidAmountLabel.BackgroundColor = Colors.Clear;
			paidAmountLabel.Lines = 0;
			paidAmountLabel.SizeToFit ();
			paidAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (paidAmountLabel);

			if (paidAmountTitleLabel == null)
				paidAmountTitleLabel = new UILabel ();
			paidAmountTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			paidAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			paidAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			paidAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			paidAmountTitleLabel.BackgroundColor = Colors.Clear;
			paidAmountTitleLabel.Lines = 0;
			paidAmountTitleLabel.Text="paidAmount".tr();
			paidAmountTitleLabel.SizeToFit ();
			paidAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (paidAmountTitleLabel);

			yPos += (float)paidAmountTitleLabel.Frame.Height + fieldPadding;

			if (statusLabel == null)
				statusLabel = new UILabel ();
			statusLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			statusLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			statusLabel.TextAlignment = UITextAlignment.Left;
			statusLabel.TextColor = Colors.DARK_GREY_COLOR;
			statusLabel.BackgroundColor = Colors.Clear;
			statusLabel.Lines = 0;
			statusLabel.SizeToFit ();
			statusLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (statusLabel);

			if (statusTitleLabel == null)
				statusTitleLabel = new UILabel ();
			statusTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			statusTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			statusTitleLabel.TextAlignment = UITextAlignment.Left;
			statusTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			statusTitleLabel.BackgroundColor = Colors.Clear;
			statusTitleLabel.Lines = 0;
			statusTitleLabel.Text="claimStatus".tr();
			statusTitleLabel.SizeToFit ();
			statusTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (statusTitleLabel);

			yPos += (float)statusTitleLabel.Frame.Height + fieldPadding;

            if (eobLabel == null)
                eobLabel = new UILabel();
            eobLabel.Frame = new CGRect(sidePadding, yPos, contentWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            eobLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            eobLabel.TextAlignment = UITextAlignment.Left;
            eobLabel.TextColor = Colors.DARK_GREY_COLOR;
            eobLabel.BackgroundColor = Colors.Clear;
            eobLabel.Lines = 0;
            eobLabel.SizeToFit();
            AddSubview(eobLabel);

		}

		public virtual bool ShowsDeleteButton()
		{
			return false;
		}


		public virtual void InitializeBindings ()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimResultDetailTableCell,ClaimResultDetailGSC>();
					set.Bind(this.formNumLabel).To(item => item.ClaimFormID).WithConversion("LongToString").OneWay();
					set.Bind(this.serviceDateLabel).To(item => item.ServiceDate).WithConversion("DateToString").OneWay();
					set.Bind(this.serviceDescriptionLabel).To(item => item.ServiceDescription).OneWay();
//					set.Bind(this.deductibleLabel).To(item => item.DeductibleAmount).WithConversion("DollarSignDoublePrefix").OneWay();
					set.Bind(this.claimedAmountLabel).To(item => item.ClaimedAmount).WithConversion("DollarSignDoublePrefix").OneWay();
					set.Bind(this.otherPaidAmountLabel).To(item => item.OtherPaidAmount).WithConversion("DollarSignDoublePrefix").OneWay();
//					set.Bind(this.copayLabel).To(item => item.CopayAmount).WithConversion("DollarSignDoublePrefix").OneWay();
					set.Bind(this.paidAmountLabel).To(item => item.PaidAmount).WithConversion("DollarSignDoublePrefix").OneWay();
					set.Bind(this.statusLabel).To(item => item.ClaimStatus).OneWay();
                    set.Bind(eobLabel).For(v => v.Text).To(vm => vm.EOBMessagesText);
					set.Bind (this).For (v => v.HasHCSADetails).To (item => item.HasHCSADetails);
					set.Apply();


				});
		}

		private bool _hasHCSADetails = false;
		public bool HasHCSADetails {
			get {
				return _hasHCSADetails;
			}
			set {

				_hasHCSADetails = value;

				if (value ) {

					this.SetNeedsLayout ();

				}


			}

		}

	}
}


