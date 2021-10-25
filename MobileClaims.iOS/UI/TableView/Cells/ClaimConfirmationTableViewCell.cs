 using System;
 using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
 using MvvmCross.Binding.BindingContext;

 namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimConfirmationTableViewCell")]
	public class ClaimConfirmationTableViewCell : MvxDeleteTableViewCell
	{
		//TITLES
		public UILabel typeTitleLabel;
		public UILabel itemDescriptionTitleLabel;
		public UILabel lengthTitleLabel;
		public UILabel dateOfTreatmentTitleLabel;
		public UILabel dateOfMonthlyTreatmentTitleLabel;
		public UILabel dateOfPurchaseTitleLabel;
		public UILabel dateOfPickupTitleLabel;
		public UILabel quantityTitleLabel;
		public UILabel amountTitleLabel;
		public UILabel totalAmountMedicalTitleLabel;
		public UILabel orthFeeTitleLabel;
		public UILabel altCarrierTitleLabel;
		public UILabel gstTitleLabel;
		public UILabel pstTitleLabel;
        public UILabel HasReferralBeenPreviouslySubmittedTitleLabel;
        public UILabel dateOfReferralTitleLabel;
		public UILabel typeOfProfessionalTitleLabel;
		//New Vision Properties
		public UILabel eyewearTypeTitleLabel;
		public UILabel lensTypeTitleLabel;
		public UILabel frameAmountTitleLabel;
		public UILabel feeAmountTitleLabel;
		public UILabel eyeglassLensAmountTitleLabel;
		public UILabel totalAmountTitleLabel;
		public UILabel rightSphereTitleLabel;
		public UILabel leftSphereTitleLabel;
		public UILabel rightCylinderTitleLabel;
		public UILabel leftCylinderTitleLabel;
		public UILabel rightAxisTitleLabel;
		public UILabel leftAxisTitleLabel;
		public UILabel rightPrismTitleLabel;
		public UILabel leftPrismTitleLabel;
		public UILabel rightBifocalsTitleLabel;
		public UILabel leftBifocalsTitleLabel;

        public UILabel emptyLabel;

		//LABELS
		public UILabel typeLabel;
		public UILabel itemDescriptionLabel;
		public UILabel lengthLabel;
		public UILabel dateOfTreatmentLabel;
		public UILabel dateOfMonthlyTreatmentLabel;
		public UILabel dateOfPurchaseLabel;
		public UILabel dateOfPickupLabel;
		public UILabel quantityLabel;
		public UILabel amountLabel;
		public UILabel totalAmountMedicalLabel;
		public UILabel orthFeeLabel;
		public UILabel altCarrierLabel;
		public UILabel gstLabel;
		public UILabel pstLabel;
        public UILabel HasReferralBeenPreviouslySubmittedLabel;
        public UILabel dateOfReferralLabel;
		public UILabel typeOfProfessionalLabel;
		//New Vision Properties
		public UILabel eyewearTypeLabel;
		public UILabel lensTypeLabel;
		public UILabel frameAmountLabel;
		public UILabel feeAmountLabel;
		public UILabel eyeglassLensAmountLabel;
		public UILabel totalAmountLabel;
		public UILabel rightSphereLabel;
		public UILabel leftSphereLabel;
		public UILabel rightCylinderLabel;
		public UILabel leftCylinderLabel;
		public UILabel rightAxisLabel;
		public UILabel leftAxisLabel;
		public UILabel rightPrismLabel;
		public UILabel leftPrismLabel;
		public UILabel rightBifocalsLabel;
		public UILabel leftBifocalsLabel;

		protected UIView cellBackingView;

		public ClaimConfirmationTableViewCell () : base () {}
		public ClaimConfirmationTableViewCell (IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float topPadding = 15;
			float fieldPadding = 10;
			float yPos = 5;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float totalSidePadding = sidePadding * 2;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
            cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height + yPos);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (typeTitleLabel == null)
				typeTitleLabel = new UILabel ();
			typeTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)typeTitleLabel.Frame.Height);
			typeTitleLabel.SizeToFit ();
			typeTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)typeTitleLabel.Frame.Width, (float)typeTitleLabel.Frame.Height);
			typeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			typeTitleLabel.TextAlignment = UITextAlignment.Left;
			typeTitleLabel.BackgroundColor = Colors.Clear;
			typeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			typeTitleLabel.Lines = 0;
			typeTitleLabel.Text="typeOfTreatmentTitle".tr();
			AddSubview (typeTitleLabel);

			if (typeLabel == null)
				typeLabel = new UILabel ();
			typeLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)typeLabel.Frame.Width, yPos, Frame.Width - (float)typeTitleLabel.Frame.Width - totalSidePadding, (float)typeLabel.Frame.Height);
			typeLabel.SizeToFit ();
			typeLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)typeLabel.Frame.Width, yPos, (float)typeLabel.Frame.Width, (float)typeLabel.Frame.Height);
			typeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			typeLabel.TextAlignment = UITextAlignment.Right;
			typeLabel.BackgroundColor = Colors.Clear;
			typeLabel.TextColor = Colors.DARK_GREY_COLOR;
			typeLabel.Lines = 0;
			AddSubview (typeLabel);

			if(!typeLabel.Hidden)	
				yPos += Math.Max((float)typeTitleLabel.Frame.Height, (float)typeLabel.Frame.Height) + fieldPadding;

			if (itemDescriptionTitleLabel == null)
				itemDescriptionTitleLabel = new UILabel ();
			itemDescriptionTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)itemDescriptionTitleLabel.Frame.Height);
			itemDescriptionTitleLabel.SizeToFit ();
			itemDescriptionTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)itemDescriptionTitleLabel.Frame.Width, (float)itemDescriptionTitleLabel.Frame.Height);
			itemDescriptionTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			itemDescriptionTitleLabel.TextAlignment = UITextAlignment.Left;
			itemDescriptionTitleLabel.BackgroundColor = Colors.Clear;
			itemDescriptionTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			itemDescriptionTitleLabel.Lines = 0;
			itemDescriptionTitleLabel.Text="itemDescription".tr();
			AddSubview (itemDescriptionTitleLabel);

			if (itemDescriptionLabel == null)
				itemDescriptionLabel = new UILabel ();
			itemDescriptionLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)itemDescriptionLabel.Frame.Width, yPos, Frame.Width - (float)itemDescriptionTitleLabel.Frame.Width - totalSidePadding - 20, (float)itemDescriptionLabel.Frame.Height);
			itemDescriptionLabel.SizeToFit ();
			itemDescriptionLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)itemDescriptionLabel.Frame.Width, yPos, (float)itemDescriptionLabel.Frame.Width, (float)itemDescriptionLabel.Frame.Height);
			itemDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			itemDescriptionLabel.TextAlignment = UITextAlignment.Right;
			itemDescriptionLabel.BackgroundColor = Colors.Clear;
			itemDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
			itemDescriptionLabel.Lines = 0;
			AddSubview (itemDescriptionLabel);

			if(!itemDescriptionLabel.Hidden)	
				yPos += Math.Max((float)itemDescriptionLabel.Frame.Height, (float)itemDescriptionTitleLabel.Frame.Height ) + fieldPadding;

			if (lengthTitleLabel == null)
				lengthTitleLabel = new UILabel ();
			lengthTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-totalSidePadding, (float)lengthTitleLabel.Frame.Height);
			lengthTitleLabel.SizeToFit ();
			lengthTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)lengthTitleLabel.Frame.Width, (float)lengthTitleLabel.Frame.Height);
			lengthTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			lengthTitleLabel.TextAlignment = UITextAlignment.Left;
			lengthTitleLabel.BackgroundColor = Colors.Clear;
			lengthTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			lengthTitleLabel.Lines = 0;
			lengthTitleLabel.Text="lengthOfTreatmentTitle".tr();
			AddSubview (lengthTitleLabel);

			if (lengthLabel == null)
				lengthLabel = new UILabel ();
			lengthLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)lengthLabel.Frame.Width, yPos, Frame.Width - (float)lengthTitleLabel.Frame.Width - totalSidePadding, (float)lengthLabel.Frame.Height);
			lengthLabel.SizeToFit ();
			lengthLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)lengthLabel.Frame.Width, yPos, (float)lengthLabel.Frame.Width, (float)lengthLabel.Frame.Height);
			lengthLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			lengthLabel.TextAlignment = UITextAlignment.Right;
			lengthLabel.BackgroundColor = Colors.Clear;
			lengthLabel.TextColor = Colors.DARK_GREY_COLOR;
			lengthLabel.Lines = 0;
			AddSubview (lengthLabel);

			if(!lengthLabel.Hidden)
				yPos += Math.Max((float)lengthLabel.Frame.Height, (float)lengthTitleLabel.Frame.Height) + fieldPadding;

			if (dateOfTreatmentTitleLabel == null)
				dateOfTreatmentTitleLabel = new UILabel ();
			dateOfTreatmentTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)dateOfTreatmentTitleLabel.Frame.Height);
			dateOfTreatmentTitleLabel.SizeToFit ();
			dateOfTreatmentTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)dateOfTreatmentTitleLabel.Frame.Width, (float)dateOfTreatmentTitleLabel.Frame.Height);
			dateOfTreatmentTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfTreatmentTitleLabel.TextAlignment = UITextAlignment.Left;
			dateOfTreatmentTitleLabel.BackgroundColor = Colors.Clear;
			dateOfTreatmentTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfTreatmentTitleLabel.Lines = 0;
			dateOfTreatmentTitleLabel.Text="dateOfTreatmentTitle".tr();
			AddSubview (dateOfTreatmentTitleLabel);

			if (dateOfTreatmentLabel == null)
				dateOfTreatmentLabel = new UILabel ();
			dateOfTreatmentLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)dateOfTreatmentLabel.Frame.Width, yPos, Frame.Width - (float)dateOfTreatmentTitleLabel.Frame.Width - totalSidePadding, (float)dateOfTreatmentLabel.Frame.Height);
			dateOfTreatmentLabel.SizeToFit ();
			dateOfTreatmentLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)dateOfTreatmentLabel.Frame.Width, yPos, (float)dateOfTreatmentLabel.Frame.Width, (float)dateOfTreatmentLabel.Frame.Height);
			dateOfTreatmentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfTreatmentLabel.TextAlignment = UITextAlignment.Right;
			dateOfTreatmentLabel.BackgroundColor = Colors.Clear;
			dateOfTreatmentLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfTreatmentLabel.Lines = 0;
			AddSubview (dateOfTreatmentLabel);

			if(!dateOfTreatmentLabel.Hidden)
				yPos += Math.Max((float)dateOfTreatmentLabel.Frame.Height, (float)dateOfTreatmentTitleLabel.Frame.Height) + fieldPadding;

			if (dateOfMonthlyTreatmentTitleLabel == null)
				dateOfMonthlyTreatmentTitleLabel = new UILabel ();
			dateOfMonthlyTreatmentTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)dateOfMonthlyTreatmentTitleLabel.Frame.Height);
			dateOfMonthlyTreatmentTitleLabel.SizeToFit ();
			dateOfMonthlyTreatmentTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)dateOfMonthlyTreatmentTitleLabel.Frame.Width, (float)dateOfMonthlyTreatmentTitleLabel.Frame.Height);
			dateOfMonthlyTreatmentTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfMonthlyTreatmentTitleLabel.TextAlignment = UITextAlignment.Left;
			dateOfMonthlyTreatmentTitleLabel.BackgroundColor = Colors.Clear;
			dateOfMonthlyTreatmentTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfMonthlyTreatmentTitleLabel.Lines = 0;
			dateOfMonthlyTreatmentTitleLabel.Text="dateOfMonthlyTreatmentTitle".tr();
			AddSubview (dateOfMonthlyTreatmentTitleLabel);

			if (dateOfMonthlyTreatmentLabel == null)
				dateOfMonthlyTreatmentLabel = new UILabel ();
			dateOfMonthlyTreatmentLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)dateOfMonthlyTreatmentLabel.Frame.Width, yPos, Frame.Width - (float)dateOfMonthlyTreatmentTitleLabel.Frame.Width - totalSidePadding, (float)dateOfMonthlyTreatmentLabel.Frame.Height);
			dateOfMonthlyTreatmentLabel.SizeToFit ();
			dateOfMonthlyTreatmentLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)dateOfMonthlyTreatmentLabel.Frame.Width, yPos, (float)dateOfMonthlyTreatmentLabel.Frame.Width, (float)dateOfMonthlyTreatmentLabel.Frame.Height);
			dateOfMonthlyTreatmentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfMonthlyTreatmentLabel.TextAlignment = UITextAlignment.Right;
			dateOfMonthlyTreatmentLabel.BackgroundColor = Colors.Clear;
			dateOfMonthlyTreatmentLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfMonthlyTreatmentLabel.Lines = 0;
			AddSubview (dateOfMonthlyTreatmentLabel);

			if(!dateOfMonthlyTreatmentLabel.Hidden)
				yPos += Math.Max((float)dateOfMonthlyTreatmentTitleLabel.Frame.Height, (float)dateOfMonthlyTreatmentLabel.Frame.Height) + fieldPadding;

			if (dateOfPurchaseTitleLabel == null)
				dateOfPurchaseTitleLabel = new UILabel ();
			dateOfPurchaseTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)dateOfPurchaseTitleLabel.Frame.Height);
			dateOfPurchaseTitleLabel.SizeToFit ();
			dateOfPurchaseTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)dateOfPurchaseTitleLabel.Frame.Width, (float)dateOfPurchaseTitleLabel.Frame.Height);
			dateOfPurchaseTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfPurchaseTitleLabel.TextAlignment = UITextAlignment.Left;
			dateOfPurchaseTitleLabel.BackgroundColor = Colors.Clear;
			dateOfPurchaseTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfPurchaseTitleLabel.Lines = 0;
			dateOfPurchaseTitleLabel.Text="dateOfPurchaseService".tr();
			AddSubview (dateOfPurchaseTitleLabel);

			if (dateOfPurchaseLabel == null)
				dateOfPurchaseLabel = new UILabel ();
			dateOfPurchaseLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)dateOfPurchaseLabel.Frame.Width, yPos, Frame.Width - (float)dateOfPurchaseTitleLabel.Frame.Width - totalSidePadding, (float)dateOfPurchaseLabel.Frame.Height);
			dateOfPurchaseLabel.SizeToFit ();
			dateOfPurchaseLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)dateOfPurchaseLabel.Frame.Width, yPos, (float)dateOfPurchaseLabel.Frame.Width, (float)dateOfPurchaseLabel.Frame.Height);
			dateOfPurchaseLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfPurchaseLabel.TextAlignment = UITextAlignment.Right;
			dateOfPurchaseLabel.BackgroundColor = Colors.Clear;
			dateOfPurchaseLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfPurchaseLabel.Lines = 0;
			AddSubview (dateOfPurchaseLabel);

			if(!dateOfPurchaseLabel.Hidden)
				yPos += Math.Max((float)dateOfPurchaseTitleLabel.Frame.Height, (float)dateOfPurchaseLabel.Frame.Height) + fieldPadding;

			if (dateOfPickupLabel == null)
				dateOfPickupLabel = new UILabel ();
			dateOfPickupLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)dateOfPickupLabel.Frame.Height);
			dateOfPickupLabel.SizeToFit ();
			dateOfPickupLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)dateOfPickupLabel.Frame.Height);
			dateOfPickupLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfPickupLabel.TextAlignment = UITextAlignment.Right;
			dateOfPickupLabel.BackgroundColor = Colors.Clear;
			dateOfPickupLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfPickupLabel.Lines = 0;
			dateOfPickupLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateOfPickupLabel);

			if (dateOfPickupTitleLabel == null)
				dateOfPickupTitleLabel = new UILabel ();
			dateOfPickupTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)dateOfPickupTitleLabel.Frame.Height);
			dateOfPickupTitleLabel.SizeToFit ();
			dateOfPickupTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)dateOfPickupTitleLabel.Frame.Height);
			dateOfPickupTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			dateOfPickupTitleLabel.TextAlignment = UITextAlignment.Left;
			dateOfPickupTitleLabel.BackgroundColor = Colors.Clear;
			dateOfPickupTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			dateOfPickupTitleLabel.Lines = 0;
			dateOfPickupTitleLabel.Text="pickupDate".tr();
			dateOfPickupTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateOfPickupTitleLabel);

			if(!dateOfPickupLabel.Hidden)
				yPos += (float)dateOfPickupLabel.Frame.Height + fieldPadding;

			if (quantityLabel == null)
				quantityLabel = new UILabel ();
			quantityLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)quantityLabel.Frame.Height);
			quantityLabel.SizeToFit ();
			quantityLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)quantityLabel.Frame.Height);
			quantityLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			quantityLabel.TextAlignment = UITextAlignment.Right;
			quantityLabel.BackgroundColor = Colors.Clear;
			quantityLabel.TextColor = Colors.DARK_GREY_COLOR;
			quantityLabel.Lines = 0;
			quantityLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (quantityLabel);

			if (quantityTitleLabel == null)
				quantityTitleLabel = new UILabel ();
			quantityTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)quantityTitleLabel.Frame.Height);
			quantityTitleLabel.SizeToFit ();
			quantityTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)quantityTitleLabel.Frame.Height);
			quantityTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			quantityTitleLabel.TextAlignment = UITextAlignment.Left;
			quantityTitleLabel.BackgroundColor = Colors.Clear;
			quantityTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			quantityTitleLabel.Lines = 0;
			quantityTitleLabel.Text="quantity".tr();
			quantityTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (quantityTitleLabel);

			if(!quantityLabel.Hidden)
				yPos += (float)quantityLabel.Frame.Height + fieldPadding;

			if (amountLabel == null)
				amountLabel = new UILabel ();
			amountLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)amountLabel.Frame.Height);
			amountLabel.SizeToFit ();
			amountLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)amountLabel.Frame.Height);
			amountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			amountLabel.TextAlignment = UITextAlignment.Right;
			amountLabel.BackgroundColor = Colors.Clear;
			amountLabel.TextColor = Colors.DARK_GREY_COLOR;
			amountLabel.Lines = 0;
			amountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (amountLabel);

			if (amountTitleLabel == null)
				amountTitleLabel = new UILabel ();
			amountTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2 - 40, (float)amountTitleLabel.Frame.Height);
			amountTitleLabel.SizeToFit ();
			amountTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2 - 40, (float)amountTitleLabel.Frame.Height);
			amountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			amountTitleLabel.TextAlignment = UITextAlignment.Left;
			amountTitleLabel.BackgroundColor = Colors.Clear;
			amountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			amountTitleLabel.Lines = 0;
			amountTitleLabel.Text= (isMI ? "totalAmountMedicalItems".tr() : "totalAmountOfVisit".tr());

			AddSubview (amountTitleLabel);

			if(!amountLabel.Hidden)
				yPos += (float)amountLabel.Frame.Height + fieldPadding;

			if (totalAmountMedicalTitleLabel == null)
				totalAmountMedicalTitleLabel = new UILabel ();
			totalAmountMedicalTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width/2, (float)totalAmountMedicalTitleLabel.Frame.Height);
			totalAmountMedicalTitleLabel.SizeToFit ();
			totalAmountMedicalTitleLabel.Frame = new CGRect (sidePadding, yPos, (float)totalAmountMedicalTitleLabel.Frame.Width, (float)totalAmountMedicalTitleLabel.Frame.Height);
			totalAmountMedicalTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			totalAmountMedicalTitleLabel.TextAlignment = UITextAlignment.Left;
			totalAmountMedicalTitleLabel.BackgroundColor = Colors.Clear;
			totalAmountMedicalTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			totalAmountMedicalTitleLabel.Lines = 0;
			totalAmountMedicalTitleLabel.Text= ("totalAmountMedicalItems".tr());

			AddSubview (totalAmountMedicalTitleLabel);

			if (totalAmountMedicalLabel == null)
				totalAmountMedicalLabel = new UILabel ();
			totalAmountMedicalLabel.Frame = new CGRect (Frame.Width - totalSidePadding - (float)totalAmountMedicalLabel.Frame.Width, yPos, Frame.Width - (float)totalAmountMedicalTitleLabel.Frame.Width - totalSidePadding - 20, (float)totalAmountMedicalLabel.Frame.Height);
			totalAmountMedicalLabel.SizeToFit ();
			totalAmountMedicalLabel.Frame = new CGRect (Frame.Width - sidePadding - (float)totalAmountMedicalLabel.Frame.Width, yPos, (float)totalAmountMedicalLabel.Frame.Width, (float)totalAmountMedicalLabel.Frame.Height);
			totalAmountMedicalLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			totalAmountMedicalLabel.TextAlignment = UITextAlignment.Right;
			totalAmountMedicalLabel.BackgroundColor = Colors.Clear;
			totalAmountMedicalLabel.TextColor = Colors.DARK_GREY_COLOR;
			totalAmountMedicalLabel.Lines = 0;
			AddSubview (totalAmountMedicalLabel);

			if(!totalAmountMedicalLabel.Hidden)
				yPos += Math.Max((float)totalAmountMedicalTitleLabel.Frame.Height, (float)totalAmountMedicalLabel.Frame.Height) + fieldPadding;

			if (orthFeeLabel == null)
				orthFeeLabel = new UILabel ();
			orthFeeLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)orthFeeLabel.Frame.Height);
			orthFeeLabel.SizeToFit ();
			orthFeeLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding* 2, (float)orthFeeLabel.Frame.Height);
			orthFeeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			orthFeeLabel.TextAlignment = UITextAlignment.Right;
			orthFeeLabel.BackgroundColor = Colors.Clear;
			orthFeeLabel.TextColor = Colors.DARK_GREY_COLOR;
			orthFeeLabel.Lines = 0;
			orthFeeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (orthFeeLabel);

			if (orthFeeTitleLabel == null)
				orthFeeTitleLabel = new UILabel ();
			orthFeeTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)orthFeeTitleLabel.Frame.Height);
			orthFeeTitleLabel.SizeToFit ();
			orthFeeTitleLabel.Frame = new CGRect (sidePadding, yPos, Frame.Width-sidePadding * 2, (float)orthFeeTitleLabel.Frame.Height);
			orthFeeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			orthFeeTitleLabel.TextAlignment = UITextAlignment.Left;
			orthFeeTitleLabel.BackgroundColor = Colors.Clear;
			orthFeeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			orthFeeTitleLabel.Lines = 0;
			orthFeeTitleLabel.Text="omf".tr();
			orthFeeTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (orthFeeTitleLabel);

			if(!orthFeeLabel.Hidden)
				yPos += (float)orthFeeLabel.Frame.Height + fieldPadding;


			if (altCarrierLabel == null)
				altCarrierLabel = new UILabel ();
			altCarrierLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)altCarrierLabel.Frame.Height );
			altCarrierLabel.SizeToFit ();
			altCarrierLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)altCarrierLabel.Frame.Height );
			altCarrierLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			altCarrierLabel.TextAlignment = UITextAlignment.Right;
			altCarrierLabel.BackgroundColor = Colors.Clear;
			altCarrierLabel.TextColor = Colors.DARK_GREY_COLOR;
			altCarrierLabel.Lines = 0;
			altCarrierLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altCarrierLabel);

			if (altCarrierTitleLabel == null)
				altCarrierTitleLabel = new UILabel ();
			altCarrierTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)altCarrierTitleLabel.Frame.Height );
			altCarrierTitleLabel.SizeToFit ();
			altCarrierTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)altCarrierTitleLabel.Frame.Height );
			altCarrierTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			altCarrierTitleLabel.TextAlignment = UITextAlignment.Left;
			altCarrierTitleLabel.BackgroundColor = Colors.Clear;
			altCarrierTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			altCarrierTitleLabel.Lines = 0;
			altCarrierTitleLabel.Text="amountPaidAlt".tr();
			altCarrierTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altCarrierTitleLabel);

//			if (TreatmentDetailItem != null && !TreatmentDetailItem.IsAlternateCarrierPaymentVisible) {
//				altCarrierLabel.Alpha = 0;
//				altCarrierTitleLabel.Alpha = 0;
//			} else {
			if(!altCarrierLabel.Hidden)
				yPos += (float)altCarrierLabel.Frame.Height + fieldPadding;
			 
			//}
			if (gstLabel == null)
				gstLabel = new UILabel ();
			gstLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)gstLabel.Frame.Height );
			gstLabel.SizeToFit ();
			gstLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)gstLabel.Frame.Height );
			gstLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			gstLabel.TextAlignment = UITextAlignment.Right;
			gstLabel.BackgroundColor = Colors.Clear;
			gstLabel.TextColor = Colors.DARK_GREY_COLOR;
			gstLabel.Lines = 0;
			gstLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (gstLabel);

			if (gstTitleLabel == null)
				gstTitleLabel = new UILabel ();
			gstTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)gstTitleLabel.Frame.Height );
			gstTitleLabel.SizeToFit ();
			gstTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)gstTitleLabel.Frame.Height );
			gstTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			gstTitleLabel.TextAlignment = UITextAlignment.Left;
			gstTitleLabel.BackgroundColor = Colors.Clear;
			gstTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			gstTitleLabel.Lines = 0;
			gstTitleLabel.Text="gstIncluded".tr();
			gstTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (gstTitleLabel);

			if(!gstLabel.Hidden)
				yPos += (float)gstLabel.Frame.Height + fieldPadding;

			if (pstLabel == null)
				pstLabel = new UILabel ();
			pstLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)pstLabel.Frame.Height );
			pstLabel.SizeToFit ();
			pstLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)pstLabel.Frame.Height );
			pstLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			pstLabel.TextAlignment = UITextAlignment.Right;
			pstLabel.BackgroundColor = Colors.Clear;
			pstLabel.TextColor = Colors.DARK_GREY_COLOR;
			pstLabel.Lines = 0;
			pstLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (pstLabel);

			if (pstTitleLabel == null)
				pstTitleLabel = new UILabel ();
			pstTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)pstTitleLabel.Frame.Height );
			pstTitleLabel.SizeToFit ();
			pstTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)pstTitleLabel.Frame.Height );
			pstTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			pstTitleLabel.TextAlignment = UITextAlignment.Left;
			pstTitleLabel.BackgroundColor = Colors.Clear;
			pstTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			pstTitleLabel.Lines = 0;
			pstTitleLabel.Text="pstIncluded".tr();
			pstTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (pstTitleLabel);

			if(!pstLabel.Hidden)
				yPos += (float)pstLabel.Frame.Height + fieldPadding;

            if (HasReferralBeenPreviouslySubmittedLabel == null)
                HasReferralBeenPreviouslySubmittedLabel = new UILabel();
            HasReferralBeenPreviouslySubmittedLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)HasReferralBeenPreviouslySubmittedLabel.Frame.Height);
            HasReferralBeenPreviouslySubmittedLabel.SizeToFit();
            HasReferralBeenPreviouslySubmittedLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)HasReferralBeenPreviouslySubmittedLabel.Frame.Height);
            HasReferralBeenPreviouslySubmittedLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            HasReferralBeenPreviouslySubmittedLabel.TextAlignment = UITextAlignment.Right;
            HasReferralBeenPreviouslySubmittedLabel.BackgroundColor = Colors.Clear;
            HasReferralBeenPreviouslySubmittedLabel.TextColor = Colors.DARK_GREY_COLOR;
            HasReferralBeenPreviouslySubmittedLabel.Lines = 0;
            HasReferralBeenPreviouslySubmittedLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(HasReferralBeenPreviouslySubmittedLabel);

            if (HasReferralBeenPreviouslySubmittedTitleLabel == null)
                HasReferralBeenPreviouslySubmittedTitleLabel = new UILabel();
            HasReferralBeenPreviouslySubmittedTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2 - 60, (float)HasReferralBeenPreviouslySubmittedTitleLabel.Frame.Height);
            HasReferralBeenPreviouslySubmittedTitleLabel.SizeToFit();
            HasReferralBeenPreviouslySubmittedTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2 - 60, (float)HasReferralBeenPreviouslySubmittedTitleLabel.Frame.Height );
            HasReferralBeenPreviouslySubmittedTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            HasReferralBeenPreviouslySubmittedTitleLabel.TextAlignment = UITextAlignment.Left;
            HasReferralBeenPreviouslySubmittedTitleLabel.BackgroundColor = Colors.Clear;
            HasReferralBeenPreviouslySubmittedTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            HasReferralBeenPreviouslySubmittedTitleLabel.Lines = 0;
            HasReferralBeenPreviouslySubmittedTitleLabel.Text = "medicalItemTitle".tr();
            HasReferralBeenPreviouslySubmittedTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(HasReferralBeenPreviouslySubmittedTitleLabel);

            if (!HasReferralBeenPreviouslySubmittedLabel.Hidden)
                yPos += (float)HasReferralBeenPreviouslySubmittedLabel.Frame.Height + fieldPadding + 5;

            if (dateOfReferralLabel == null)
                dateOfReferralLabel = new UILabel();
            dateOfReferralLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)dateOfReferralLabel.Frame.Height);
            dateOfReferralLabel.SizeToFit();
            dateOfReferralLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)dateOfReferralLabel.Frame.Height);
            dateOfReferralLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            dateOfReferralLabel.TextAlignment = UITextAlignment.Right;
            dateOfReferralLabel.BackgroundColor = Colors.Clear;
            dateOfReferralLabel.TextColor = Colors.DARK_GREY_COLOR;
            dateOfReferralLabel.Lines = 0;
            dateOfReferralLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(dateOfReferralLabel);

            if (dateOfReferralTitleLabel == null)
                dateOfReferralTitleLabel = new UILabel();
            dateOfReferralTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)dateOfReferralTitleLabel.Frame.Height);
            dateOfReferralTitleLabel.SizeToFit();
            dateOfReferralTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width-sidePadding * 2, (float)dateOfReferralTitleLabel.Frame.Height );
            dateOfReferralTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            dateOfReferralTitleLabel.TextAlignment = UITextAlignment.Left;
            dateOfReferralTitleLabel.BackgroundColor = Colors.Clear;
            dateOfReferralTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            dateOfReferralTitleLabel.Lines = 0;
            dateOfReferralTitleLabel.Text = "referralDate".tr();
            dateOfReferralTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(dateOfReferralTitleLabel);

            if (!dateOfReferralLabel.Hidden)
                yPos += (float)dateOfReferralLabel.Frame.Height + fieldPadding;

            if (typeOfProfessionalTitleLabel == null)
				typeOfProfessionalTitleLabel = new UILabel ();
			//NOTE: Especially long title label. Cutting this one short to force multi line. Same adjustment made in MVXConfirmationTableViewSource.
			typeOfProfessionalTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width/2, (float)typeOfProfessionalTitleLabel.Frame.Height );
			typeOfProfessionalTitleLabel.SizeToFit ();
            typeOfProfessionalTitleLabel.Frame = new CGRect ( sidePadding, yPos, Frame.Width/1.8, (float)typeOfProfessionalTitleLabel.Frame.Height );
			typeOfProfessionalTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			typeOfProfessionalTitleLabel.TextAlignment = UITextAlignment.Left;
            typeOfProfessionalTitleLabel.BackgroundColor = Colors.Clear;
			typeOfProfessionalTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			typeOfProfessionalTitleLabel.Lines = 0;
			typeOfProfessionalTitleLabel.Text="typeOfProfessional".tr();
            typeOfProfessionalTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview (typeOfProfessionalTitleLabel);

			if (typeOfProfessionalLabel == null)
				typeOfProfessionalLabel = new UILabel ();
            typeOfProfessionalLabel.Frame = new CGRect(Frame.Width - totalSidePadding - (float)typeOfProfessionalLabel.Frame.Width, yPos, Frame.Width - (float)typeOfProfessionalTitleLabel.Frame.Width - totalSidePadding, (float)typeOfProfessionalLabel.Frame.Height);
			typeOfProfessionalLabel.SizeToFit ();
            typeOfProfessionalLabel.Frame = new CGRect(Frame.Width - sidePadding - (float)typeOfProfessionalLabel.Frame.Width, yPos, (float)typeOfProfessionalLabel.Frame.Width, (float)typeOfProfessionalTitleLabel.Frame.Height);
			typeOfProfessionalLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			typeOfProfessionalLabel.TextAlignment = UITextAlignment.Right;
            typeOfProfessionalLabel.BackgroundColor = Colors.Clear;
			typeOfProfessionalLabel.TextColor = Colors.DARK_GREY_COLOR;
			typeOfProfessionalLabel.Lines = 0;
            typeOfProfessionalLabel.AdjustsFontSizeToFitWidth = true;

            AddSubview(typeOfProfessionalLabel);

			if(!typeOfProfessionalLabel.Hidden)
				yPos += Math.Max((float)typeOfProfessionalLabel.Frame.Height, (float)typeOfProfessionalTitleLabel.Frame.Height) + fieldPadding + 5;

			//New Vision Properties
			if (eyewearTypeTitleLabel == null)
				eyewearTypeTitleLabel = new UILabel ();
            eyewearTypeTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)eyewearTypeTitleLabel.Frame.Height);
			eyewearTypeTitleLabel.SizeToFit ();
            eyewearTypeTitleLabel.Frame = new CGRect(sidePadding, yPos, (float)eyewearTypeTitleLabel.Frame.Width, (float)eyewearTypeTitleLabel.Frame.Height);
			eyewearTypeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			eyewearTypeTitleLabel.TextAlignment = UITextAlignment.Left;
			eyewearTypeTitleLabel.BackgroundColor = Colors.Clear;
			eyewearTypeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			eyewearTypeTitleLabel.Lines = 0;
			eyewearTypeTitleLabel.Text="typeOfEyewearConf".tr();
			eyewearTypeTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (eyewearTypeTitleLabel);

			if (eyewearTypeLabel == null)
				eyewearTypeLabel = new UILabel ();
            eyewearTypeLabel.Frame = new CGRect(Frame.Width - totalSidePadding - (float)eyewearTypeLabel.Frame.Width, yPos, Frame.Width - (float)eyewearTypeTitleLabel.Frame.Width - totalSidePadding - 20, (float)eyewearTypeLabel.Frame.Height);
			eyewearTypeLabel.SizeToFit ();
            eyewearTypeLabel.Frame = new CGRect(Frame.Width - sidePadding - (float)eyewearTypeLabel.Frame.Width, yPos, (float)eyewearTypeLabel.Frame.Width, (float)eyewearTypeLabel.Frame.Height);
			eyewearTypeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			eyewearTypeLabel.TextAlignment = UITextAlignment.Right;
			eyewearTypeLabel.BackgroundColor = Colors.Clear;
			eyewearTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
			eyewearTypeLabel.Lines = 0;
			eyewearTypeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (eyewearTypeLabel);

			if(!eyewearTypeTitleLabel.Hidden)
                yPos += Math.Max((float)eyewearTypeTitleLabel.Frame.Height, (float)eyewearTypeLabel.Frame.Height) + fieldPadding;

			if (lensTypeTitleLabel == null)
				lensTypeTitleLabel = new UILabel ();
            lensTypeTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)lensTypeTitleLabel.Frame.Height);
			lensTypeTitleLabel.SizeToFit ();
            lensTypeTitleLabel.Frame = new CGRect(sidePadding, yPos, (float)lensTypeTitleLabel.Frame.Width, (float)lensTypeTitleLabel.Frame.Height);
			lensTypeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			lensTypeTitleLabel.TextAlignment = UITextAlignment.Left;
			lensTypeTitleLabel.BackgroundColor = Colors.Clear;
			lensTypeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			lensTypeTitleLabel.Lines = 0;
			lensTypeTitleLabel.Text="typeOfLensConf".tr();
			lensTypeTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (lensTypeTitleLabel);

			if (lensTypeLabel == null)
				lensTypeLabel = new UILabel ();
            lensTypeLabel.Frame = new CGRect(Frame.Width - totalSidePadding - (float)lensTypeLabel.Frame.Width, yPos, Frame.Width - (float)lensTypeTitleLabel.Frame.Width - totalSidePadding - 20, (float)lensTypeLabel.Frame.Height);
			lensTypeLabel.SizeToFit ();
            lensTypeLabel.Frame = new CGRect(Frame.Width - sidePadding - (float)lensTypeLabel.Frame.Width, yPos, (float)lensTypeLabel.Frame.Width, (float)lensTypeLabel.Frame.Height);
			lensTypeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			lensTypeLabel.TextAlignment = UITextAlignment.Right;
			lensTypeLabel.BackgroundColor = Colors.Clear;
			lensTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
			lensTypeLabel.Lines = 0;
			lensTypeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (lensTypeLabel);

			if(!lensTypeTitleLabel.Hidden)
                yPos += Math.Max((float)lensTypeTitleLabel.Frame.Height, (float)lensTypeLabel.Frame.Height) + fieldPadding;

			if (frameAmountLabel == null)
				frameAmountLabel = new UILabel ();
            frameAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)frameAmountLabel.Frame.Height);
			frameAmountLabel.SizeToFit ();
            frameAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)frameAmountLabel.Frame.Height);
			frameAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			frameAmountLabel.TextAlignment = UITextAlignment.Right;
			frameAmountLabel.BackgroundColor = Colors.Clear;
			frameAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			frameAmountLabel.Lines = 0;
			frameAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (frameAmountLabel);

			if (frameAmountTitleLabel == null)
				frameAmountTitleLabel = new UILabel ();
            frameAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)frameAmountTitleLabel.Frame.Height);
			frameAmountTitleLabel.SizeToFit ();
            frameAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)frameAmountTitleLabel.Frame.Height);
			frameAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			frameAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			frameAmountTitleLabel.BackgroundColor = Colors.Clear;
			frameAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			frameAmountTitleLabel.Lines = 0;
			frameAmountTitleLabel.Text="frameAmount".tr();
			frameAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (frameAmountTitleLabel);

			if(!frameAmountLabel.Hidden)
                yPos += (float)frameAmountLabel.Frame.Height + fieldPadding;

			if (feeAmountLabel == null)
				feeAmountLabel = new UILabel ();
            feeAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)feeAmountLabel.Frame.Height);
			feeAmountLabel.SizeToFit ();
            feeAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)feeAmountLabel.Frame.Height);
			feeAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			feeAmountLabel.TextAlignment = UITextAlignment.Right;
			feeAmountLabel.BackgroundColor = Colors.Clear;
			feeAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			feeAmountLabel.Lines = 0;
			feeAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (feeAmountLabel);

			if (feeAmountTitleLabel == null)
				feeAmountTitleLabel = new UILabel ();
            feeAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)feeAmountTitleLabel.Frame.Height);
			feeAmountTitleLabel.SizeToFit ();
            feeAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)feeAmountTitleLabel.Frame.Height);
			feeAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			feeAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			feeAmountTitleLabel.BackgroundColor = Colors.Clear;
			feeAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			feeAmountTitleLabel.Lines = 0;
			feeAmountTitleLabel.Text="feeAmount".tr();
			feeAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (feeAmountTitleLabel);

			if(!feeAmountLabel.Hidden)
                yPos += (float)feeAmountLabel.Frame.Height + fieldPadding;

			if (eyeglassLensAmountLabel == null)
				eyeglassLensAmountLabel = new UILabel ();
            eyeglassLensAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)eyeglassLensAmountLabel.Frame.Height);
			eyeglassLensAmountLabel.SizeToFit ();
            eyeglassLensAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)eyeglassLensAmountLabel.Frame.Height);
			eyeglassLensAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			eyeglassLensAmountLabel.TextAlignment = UITextAlignment.Right;
			eyeglassLensAmountLabel.BackgroundColor = Colors.Clear;
			eyeglassLensAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			eyeglassLensAmountLabel.Lines = 0;
			eyeglassLensAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (eyeglassLensAmountLabel);

			if (eyeglassLensAmountTitleLabel == null)
				eyeglassLensAmountTitleLabel = new UILabel ();
            eyeglassLensAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)eyeglassLensAmountTitleLabel.Frame.Height);
			eyeglassLensAmountTitleLabel.SizeToFit ();
            eyeglassLensAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)eyeglassLensAmountTitleLabel.Frame.Height);
			eyeglassLensAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			eyeglassLensAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			eyeglassLensAmountTitleLabel.BackgroundColor = Colors.Clear;
			eyeglassLensAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			eyeglassLensAmountTitleLabel.Lines = 0;
			eyeglassLensAmountTitleLabel.Text="lensAmount".tr();
			eyeglassLensAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (eyeglassLensAmountTitleLabel);

			if(!eyeglassLensAmountLabel.Hidden)
                yPos += (float)eyeglassLensAmountLabel.Frame.Height + fieldPadding;

			if (totalAmountLabel == null)
				totalAmountLabel = new UILabel ();
            totalAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)totalAmountLabel.Frame.Height);
			totalAmountLabel.SizeToFit ();
            totalAmountLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)totalAmountLabel.Frame.Height);
			totalAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			totalAmountLabel.TextAlignment = UITextAlignment.Right;
			totalAmountLabel.BackgroundColor = Colors.Clear;
			totalAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
			totalAmountLabel.Lines = 0;
			totalAmountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (totalAmountLabel);

			if (totalAmountTitleLabel == null)
				totalAmountTitleLabel = new UILabel ();
            totalAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)totalAmountTitleLabel.Frame.Height);
			totalAmountTitleLabel.SizeToFit ();
            totalAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)totalAmountTitleLabel.Frame.Height);
			totalAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			totalAmountTitleLabel.TextAlignment = UITextAlignment.Left;
			totalAmountTitleLabel.BackgroundColor = Colors.Clear;
			totalAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			totalAmountTitleLabel.Lines = 0;
			totalAmountTitleLabel.Text="totalAmountCharged".tr();
			totalAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (totalAmountTitleLabel);

			if(!totalAmountLabel.Hidden)
                yPos += (float)totalAmountLabel.Frame.Height + fieldPadding;

			if (rightSphereLabel == null)
				rightSphereLabel = new UILabel ();
            rightSphereLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightSphereLabel.Frame.Height);
			rightSphereLabel.SizeToFit ();
            rightSphereLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightSphereLabel.Frame.Height);
			rightSphereLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightSphereLabel.TextAlignment = UITextAlignment.Right;
			rightSphereLabel.BackgroundColor = Colors.Clear;
			rightSphereLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightSphereLabel.Lines = 0;
			rightSphereLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightSphereLabel);

			if (rightSphereTitleLabel == null)
				rightSphereTitleLabel = new UILabel ();
            rightSphereTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightSphereTitleLabel.Frame.Height);
			rightSphereTitleLabel.SizeToFit ();
            rightSphereTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightSphereTitleLabel.Frame.Height);
			rightSphereTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightSphereTitleLabel.TextAlignment = UITextAlignment.Left;
			rightSphereTitleLabel.BackgroundColor = Colors.Clear;
			rightSphereTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightSphereTitleLabel.Lines = 0;
			rightSphereTitleLabel.Text="rightSphere".tr();
			rightSphereTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightSphereTitleLabel);

			if(!rightSphereTitleLabel.Hidden)
                yPos += (float)rightSphereTitleLabel.Frame.Height + fieldPadding;

			if (leftSphereLabel == null)
				leftSphereLabel = new UILabel ();
            leftSphereLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftSphereLabel.Frame.Height);
			leftSphereLabel.SizeToFit ();
            leftSphereLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftSphereLabel.Frame.Height);
			leftSphereLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftSphereLabel.TextAlignment = UITextAlignment.Right;
			leftSphereLabel.BackgroundColor = Colors.Clear;
			leftSphereLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftSphereLabel.Lines = 0;
			leftSphereLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftSphereLabel);

			if (leftSphereTitleLabel == null)
				leftSphereTitleLabel = new UILabel ();
            leftSphereTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftSphereTitleLabel.Frame.Height);
			leftSphereTitleLabel.SizeToFit ();
            leftSphereTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftSphereTitleLabel.Frame.Height);
			leftSphereTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftSphereTitleLabel.TextAlignment = UITextAlignment.Left;
			leftSphereTitleLabel.BackgroundColor = Colors.Clear;
			leftSphereTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftSphereTitleLabel.Lines = 0;
			leftSphereTitleLabel.Text="leftSphere".tr();
			leftSphereTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftSphereTitleLabel);

			if(!leftSphereTitleLabel.Hidden)
                yPos += (float)leftSphereTitleLabel.Frame.Height + fieldPadding;
				
			if (rightCylinderLabel == null)
				rightCylinderLabel = new UILabel ();
            rightCylinderLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightCylinderLabel.Frame.Height);
			rightCylinderLabel.SizeToFit ();
            rightCylinderLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightCylinderLabel.Frame.Height);
			rightCylinderLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightCylinderLabel.TextAlignment = UITextAlignment.Right;
			rightCylinderLabel.BackgroundColor = Colors.Clear;
			rightCylinderLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightCylinderLabel.Lines = 0;
			rightCylinderLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightCylinderLabel);

			if (rightCylinderTitleLabel == null)
				rightCylinderTitleLabel = new UILabel ();
            rightCylinderTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightCylinderTitleLabel.Frame.Height);
			rightCylinderTitleLabel.SizeToFit ();
            rightCylinderTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightCylinderTitleLabel.Frame.Height);
			rightCylinderTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightCylinderTitleLabel.TextAlignment = UITextAlignment.Left;
			rightCylinderTitleLabel.BackgroundColor = Colors.Clear;
			rightCylinderTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightCylinderTitleLabel.Lines = 0;
			rightCylinderTitleLabel.Text="rightCylinder".tr();
			rightCylinderTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightCylinderTitleLabel);

			if(!rightCylinderTitleLabel.Hidden)
                yPos += (float)rightCylinderTitleLabel.Frame.Height + fieldPadding;

			if (leftCylinderLabel == null)
				leftCylinderLabel = new UILabel ();
            leftCylinderLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftCylinderLabel.Frame.Height);
			leftCylinderLabel.SizeToFit ();
            leftCylinderLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftCylinderLabel.Frame.Height);
			leftCylinderLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftCylinderLabel.TextAlignment = UITextAlignment.Right;
			leftCylinderLabel.BackgroundColor = Colors.Clear;
			leftCylinderLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftCylinderLabel.Lines = 0;
			leftCylinderLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftCylinderLabel);

			if (leftCylinderTitleLabel == null)
				leftCylinderTitleLabel = new UILabel ();
            leftCylinderTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftCylinderTitleLabel.Frame.Height);
			leftCylinderTitleLabel.SizeToFit ();
            leftCylinderTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftCylinderTitleLabel.Frame.Height);
			leftCylinderTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftCylinderTitleLabel.TextAlignment = UITextAlignment.Left;
			leftCylinderTitleLabel.BackgroundColor = Colors.Clear;
			leftCylinderTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftCylinderTitleLabel.Lines = 0;
			leftCylinderTitleLabel.Text="leftCylinder".tr();
			leftCylinderTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftCylinderTitleLabel);

			if(!leftCylinderTitleLabel.Hidden)
                yPos += (float)leftCylinderTitleLabel.Frame.Height + fieldPadding;
				
			if (rightAxisLabel == null)
				rightAxisLabel = new UILabel ();
            rightAxisLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightAxisLabel.Frame.Height);
			rightAxisLabel.SizeToFit ();
            rightAxisLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightAxisLabel.Frame.Height);
			rightAxisLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightAxisLabel.TextAlignment = UITextAlignment.Right;
			rightAxisLabel.BackgroundColor = Colors.Clear;
			rightAxisLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightAxisLabel.Lines = 0;
			rightAxisLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightAxisLabel);

			if (rightAxisTitleLabel == null)
				rightAxisTitleLabel = new UILabel ();
            rightAxisTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightAxisTitleLabel.Frame.Height);
			rightAxisTitleLabel.SizeToFit ();
            rightAxisTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightAxisTitleLabel.Frame.Height);
			rightAxisTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightAxisTitleLabel.TextAlignment = UITextAlignment.Left;
			rightAxisTitleLabel.BackgroundColor = Colors.Clear;
			rightAxisTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightAxisTitleLabel.Lines = 0;
			rightAxisTitleLabel.Text="rightAxis".tr();
			rightAxisTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightAxisTitleLabel);

			if(!rightAxisTitleLabel.Hidden)
                yPos += (float)rightAxisTitleLabel.Frame.Height + fieldPadding;

			if (leftAxisLabel == null)
				leftAxisLabel = new UILabel ();
            leftAxisLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftAxisLabel.Frame.Height);
			leftAxisLabel.SizeToFit ();
            leftAxisLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftAxisLabel.Frame.Height);
			leftAxisLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftAxisLabel.TextAlignment = UITextAlignment.Right;
			leftAxisLabel.BackgroundColor = Colors.Clear;
			leftAxisLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftAxisLabel.Lines = 0;
			leftAxisLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftAxisLabel);

			if (leftAxisTitleLabel == null)
				leftAxisTitleLabel = new UILabel ();
            leftAxisTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftAxisTitleLabel.Frame.Height);
			leftAxisTitleLabel.SizeToFit ();
            leftAxisTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftAxisTitleLabel.Frame.Height);
			leftAxisTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftAxisTitleLabel.TextAlignment = UITextAlignment.Left;
			leftAxisTitleLabel.BackgroundColor = Colors.Clear;
			leftAxisTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftAxisTitleLabel.Lines = 0;
			leftAxisTitleLabel.Text="leftAxis".tr();
			leftAxisTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftAxisTitleLabel);

			if(!leftAxisTitleLabel.Hidden)
                yPos += (float)leftAxisTitleLabel.Frame.Height + fieldPadding;
				
			if (rightPrismLabel == null)
				rightPrismLabel = new UILabel ();
            rightPrismLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightPrismLabel.Frame.Height);
			rightPrismLabel.SizeToFit ();
            rightPrismLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightPrismLabel.Frame.Height);
			rightPrismLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightPrismLabel.TextAlignment = UITextAlignment.Right;
			rightPrismLabel.BackgroundColor = Colors.Clear;
			rightPrismLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightPrismLabel.Lines = 0;
			rightPrismLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightPrismLabel);

			if (rightPrismTitleLabel == null)
				rightPrismTitleLabel = new UILabel ();
            rightPrismTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightPrismTitleLabel.Frame.Height);
			rightPrismTitleLabel.SizeToFit ();
            rightPrismTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightPrismTitleLabel.Frame.Height);
			rightPrismTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightPrismTitleLabel.TextAlignment = UITextAlignment.Left;
			rightPrismTitleLabel.BackgroundColor = Colors.Clear;
			rightPrismTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightPrismTitleLabel.Lines = 0;
			rightPrismTitleLabel.Text="rightPrism".tr();
			rightPrismTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightPrismTitleLabel);

			if(!rightPrismTitleLabel.Hidden)
                yPos += (float)rightPrismTitleLabel.Frame.Height + fieldPadding;

			if (leftPrismLabel == null)
				leftPrismLabel = new UILabel ();
            leftPrismLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftPrismLabel.Frame.Height);
			leftPrismLabel.SizeToFit ();
            leftPrismLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftPrismLabel.Frame.Height);
			leftPrismLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftPrismLabel.TextAlignment = UITextAlignment.Right;
			leftPrismLabel.BackgroundColor = Colors.Clear;
			leftPrismLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftPrismLabel.Lines = 0;
			leftPrismLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftPrismLabel);

			if (leftPrismTitleLabel == null)
				leftPrismTitleLabel = new UILabel ();
            leftPrismTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftPrismTitleLabel.Frame.Height);
			leftPrismTitleLabel.SizeToFit ();
            leftPrismTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftPrismTitleLabel.Frame.Height);
			leftPrismTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftPrismTitleLabel.TextAlignment = UITextAlignment.Left;
			leftPrismTitleLabel.BackgroundColor = Colors.Clear;
			leftPrismTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftPrismTitleLabel.Lines = 0;
			leftPrismTitleLabel.Text="leftPrism".tr();
			leftPrismTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftPrismTitleLabel);

			if(!leftPrismTitleLabel.Hidden)
                yPos += (float)leftPrismTitleLabel.Frame.Height + fieldPadding;
				
			if (rightBifocalsLabel == null)
				rightBifocalsLabel = new UILabel ();
            rightBifocalsLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightBifocalsLabel.Frame.Height);
			rightBifocalsLabel.SizeToFit ();
            rightBifocalsLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightBifocalsLabel.Frame.Height);
			rightBifocalsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightBifocalsLabel.TextAlignment = UITextAlignment.Right;
			rightBifocalsLabel.BackgroundColor = Colors.Clear;
			rightBifocalsLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightBifocalsLabel.Lines = 0;
			rightBifocalsLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightBifocalsLabel);

			if (rightBifocalsTitleLabel == null)
				rightBifocalsTitleLabel = new UILabel ();
            rightBifocalsTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightBifocalsTitleLabel.Frame.Height);
			rightBifocalsTitleLabel.SizeToFit ();
            rightBifocalsTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)rightBifocalsTitleLabel.Frame.Height);
			rightBifocalsTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			rightBifocalsTitleLabel.TextAlignment = UITextAlignment.Left;
			rightBifocalsTitleLabel.BackgroundColor = Colors.Clear;
			rightBifocalsTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			rightBifocalsTitleLabel.Lines = 0;
			rightBifocalsTitleLabel.Text="rightBifocal".tr();
			rightBifocalsTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (rightBifocalsTitleLabel);

			if(!rightBifocalsTitleLabel.Hidden)
                yPos += (float)rightBifocalsTitleLabel.Frame.Height + fieldPadding;

			if (leftBifocalsLabel == null)
				leftBifocalsLabel = new UILabel ();
            leftBifocalsLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftBifocalsLabel.Frame.Height);
			leftBifocalsLabel.SizeToFit ();
            leftBifocalsLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftBifocalsLabel.Frame.Height);
			leftBifocalsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftBifocalsLabel.TextAlignment = UITextAlignment.Right;
			leftBifocalsLabel.BackgroundColor = Colors.Clear;
			leftBifocalsLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftBifocalsLabel.Lines = 0;
			leftBifocalsLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftBifocalsLabel);

			if (leftBifocalsTitleLabel == null)
				leftBifocalsTitleLabel = new UILabel ();
            leftBifocalsTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftBifocalsTitleLabel.Frame.Height);
			leftBifocalsTitleLabel.SizeToFit ();
            leftBifocalsTitleLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)leftBifocalsTitleLabel.Frame.Height);
			leftBifocalsTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			leftBifocalsTitleLabel.TextAlignment = UITextAlignment.Left;
			leftBifocalsTitleLabel.BackgroundColor = Colors.Clear;
			leftBifocalsTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			leftBifocalsTitleLabel.Lines = 0;
			leftBifocalsTitleLabel.Text="leftBifocal".tr();
			leftBifocalsTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (leftBifocalsTitleLabel);

			if(!leftBifocalsTitleLabel.Hidden)
                yPos += (float)leftBifocalsTitleLabel.Frame.Height + fieldPadding;


            //if (emptyLabel == null)
            //{
            //    emptyLabel = new UILabel();
            //}
            //emptyLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)emptyLabel.Frame.Height);
            //emptyLabel.SizeToFit();
            //emptyLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)emptyLabel.Frame.Height);
            //emptyLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            //emptyLabel.TextAlignment = UITextAlignment.Left;
            //emptyLabel.BackgroundColor = Colors.Clear;
            //emptyLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            //emptyLabel.Lines = 0;
            //emptyLabel.Text = "asdadasd";
            //emptyLabel.AdjustsFontSizeToFitWidth = true;
            //AddSubview(emptyLabel);

            //if (!emptyLabel.Hidden)
                //yPos += (float)emptyLabel.Frame.Height + fieldPadding;
            
            yPos += fieldPadding * 8;

		}

		public override void SetSelected (bool selected, bool animated)
		{
			SetHighlightColors (selected);
			base.SetSelected (selected, animated);
		}

		public override void SetHighlighted (bool highlighted, bool animated)
		{
			if (!PersistsSelection())
				SetHighlightColors (highlighted);
			base.SetHighlighted (highlighted, animated);
		}

		private void SetHighlightColors(bool selected)
		{

		}

		public override bool ShowsDeleteButton ()
		{
			return false;
		}

		public virtual bool PersistsSelection()
		{
			return true;
		}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimConfirmationTableViewCell,TreatmentDetail>();
					set.Bind(this.typeLabel).To(item => item.TypeOfTreatmentListViewItem.Name);
					set.Bind(typeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfTreatmentVisible).WithConversion("BoolOpposite");
					set.Bind(typeTitleLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfTreatmentVisible).WithConversion("BoolOpposite");

					set.Bind(this.itemDescriptionLabel).To(item => item.ItemDescription.Name);
					set.Bind(itemDescriptionLabel).For(v => v.Hidden).To(vm => vm.IsItemDescriptionVisible).WithConversion("BoolOpposite");
					set.Bind(itemDescriptionTitleLabel).For(v => v.Hidden).To(vm => vm.IsItemDescriptionVisible).WithConversion("BoolOpposite");


					set.Bind(this.lengthLabel).To(item => item.TreatmentDurationListViewItem.Name);
					set.Bind(lengthLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentDurationVisible).WithConversion("BoolOpposite");
					set.Bind(lengthTitleLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentDurationVisible).WithConversion("BoolOpposite");


					set.Bind(this.dateOfTreatmentLabel).To(item => item.TreatmentDate).WithConversion("DateToString");
					set.Bind(dateOfTreatmentLabel).For(v => v.Hidden).To(vm => vm.IsTreatementDateVisible).WithConversion("BoolOpposite");
					set.Bind(dateOfTreatmentTitleLabel).For(v => v.Hidden).To(vm => vm.IsTreatementDateVisible).WithConversion("BoolOpposite");


					set.Bind(this.dateOfMonthlyTreatmentLabel).To(item => item.DateOfMonthlyTreatment).WithConversion("DateToString");
					set.Bind(dateOfMonthlyTreatmentLabel).For(v => v.Hidden).To(vm => vm.IsDateOfMonthlyTreatmentVisible).WithConversion("BoolOpposite");
					set.Bind(dateOfMonthlyTreatmentTitleLabel).For(v => v.Hidden).To(vm => vm.IsDateOfMonthlyTreatmentVisible).WithConversion("BoolOpposite");



					set.Bind(this.dateOfPurchaseLabel).To(item => item.DateOfPurchase).WithConversion("DateToString");
					set.Bind(dateOfPurchaseLabel).For(v => v.Hidden).To(vm => vm.IsDateOfPurchaseVisible).WithConversion("BoolOpposite");
					set.Bind(dateOfPurchaseTitleLabel).For(v => v.Hidden).To(vm => vm.IsDateOfPurchaseVisible).WithConversion("BoolOpposite");



					set.Bind(this.dateOfPickupLabel).To(item => item.PickupDate).WithConversion("DateToString");
					set.Bind(dateOfPickupLabel).For(v => v.Hidden).To(vm => vm.IsPickupDateVisible).WithConversion("BoolOpposite");
					set.Bind(dateOfPickupTitleLabel).For(v => v.Hidden).To(vm => vm.IsPickupDateVisible).WithConversion("BoolOpposite");



					set.Bind(this.quantityLabel).To(item => item.Quantity);
					set.Bind(quantityLabel).For(v => v.Hidden).To(vm => vm.IsQuantityVisible).WithConversion("BoolOpposite");
					set.Bind(quantityTitleLabel).For(v => v.Hidden).To(vm => vm.IsQuantityVisible).WithConversion("BoolOpposite");



					set.Bind(this.amountLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");
					set.Bind(amountLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentAmountVisible).WithConversion("BoolOpposite");
					set.Bind(amountTitleLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentAmountVisible).WithConversion("BoolOpposite");



					set.Bind(this.totalAmountMedicalLabel).To(item => item.TreatmentAmount).WithConversion("DollarSignDoublePrefix");
					set.Bind(totalAmountMedicalLabel).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedForMIVisible).WithConversion("BoolOpposite");
					set.Bind(totalAmountMedicalTitleLabel).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedForMIVisible).WithConversion("BoolOpposite");


                    
					set.Bind(this.orthFeeLabel).To(item => item.OrthodonticMonthlyFee).WithConversion("DollarSignDoublePrefix");
					set.Bind(orthFeeLabel).For(v => v.Hidden).To(vm => vm.IsOrthodonticMonthlyFeeVisible).WithConversion("BoolOpposite");
					set.Bind(orthFeeTitleLabel).For(v => v.Hidden).To(vm => vm.IsOrthodonticMonthlyFeeVisible).WithConversion("BoolOpposite");




					set.Bind(this.altCarrierLabel).To(item => item.AlternateCarrierPayment).WithConversion("DollarSignDoublePrefix");
					//set.Bind (this).For (v => v.TreatmentDetailItem).To (vm => vm);
					set.Bind(altCarrierLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");
					set.Bind(altCarrierTitleLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");



					set.Bind(this.gstLabel).To(item => item.GSTHSTIncludedInTotal).WithConversion("BoolToString");
					set.Bind(gstLabel).For(v => v.Hidden).To(vm => vm.IsGSTHSTIncludedInTotalVisible).WithConversion("BoolOpposite");
					set.Bind(gstTitleLabel).For(v => v.Hidden).To(vm => vm.IsGSTHSTIncludedInTotalVisible).WithConversion("BoolOpposite");



					set.Bind(this.pstLabel).To(item => item.PSTIncludedInTotal).WithConversion("BoolToString");
					set.Bind(pstLabel).For(v => v.Hidden).To(vm => vm.IsPSTIncludedInTotalVisible).WithConversion("BoolOpposite");
					set.Bind(pstTitleLabel).For(v => v.Hidden).To(vm => vm.IsPSTIncludedInTotalVisible).WithConversion("BoolOpposite");



                    // label here
                    set.Bind(this.HasReferralBeenPreviouslySubmittedLabel).To(item => item.HasReferralBeenPreviouslySubmitted).WithConversion("BoolToString");
                    set.Bind(HasReferralBeenPreviouslySubmittedLabel).For(v => v.Hidden).To(vm => vm.IsHasReferralBeenPreviouslySubmittedVisible).WithConversion("BoolOpposite");
                    set.Bind(HasReferralBeenPreviouslySubmittedTitleLabel).For(v => v.Hidden).To(vm => vm.IsHasReferralBeenPreviouslySubmittedVisible).WithConversion("BoolOpposite");



                    set.Bind(this.dateOfReferralLabel).To(item => item.DateOfReferral).WithConversion("DateToString");
					set.Bind(dateOfReferralLabel).For(v => v.Hidden).To(vm => vm.IsDateOfReferralVisible).WithConversion("BoolOpposite");
					set.Bind(dateOfReferralTitleLabel).For(v => v.Hidden).To(vm => vm.IsDateOfReferralVisible).WithConversion("BoolOpposite");



					set.Bind(this.typeOfProfessionalLabel).To(item => item.TypeOfMedicalProfessional.Name);
					set.Bind(typeOfProfessionalLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfMedicalProfessionalVisible).WithConversion("BoolOpposite");
					set.Bind(typeOfProfessionalTitleLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfMedicalProfessionalVisible).WithConversion("BoolOpposite");

					//New Vision Properties




					set.Bind(this.eyewearTypeLabel).To(item => item.TypeOfEyewear.Name);
					set.Bind(eyewearTypeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfEyewearVisible).WithConversion("BoolOpposite");
					set.Bind(eyewearTypeTitleLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfEyewearVisible).WithConversion("BoolOpposite");

					set.Bind(this.lensTypeLabel).To(item => item.TypeOfLens.Name);
					set.Bind(lensTypeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfLensVisible).WithConversion("BoolOpposite");
					set.Bind(lensTypeTitleLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfLensVisible).WithConversion("BoolOpposite");

					set.Bind(this.frameAmountLabel).To(item => item.FrameAmount);
					set.Bind(frameAmountLabel).For(v => v.Hidden).To(vm => vm.IsFrameAmountVisible).WithConversion("BoolOpposite");
					set.Bind(frameAmountTitleLabel).For(v => v.Hidden).To(vm => vm.IsFrameAmountVisible).WithConversion("BoolOpposite");



					set.Bind(this.feeAmountLabel).To(item => item.FeeAmount);
					set.Bind(feeAmountLabel).For(v => v.Hidden).To(vm => vm.IsFeeAmountVisible).WithConversion("BoolOpposite");
					set.Bind(feeAmountTitleLabel).For(v => v.Hidden).To(vm => vm.IsFeeAmountVisible).WithConversion("BoolOpposite");



					set.Bind(this.eyeglassLensAmountLabel).To(item => item.EyeglassLensesAmount);
					set.Bind(eyeglassLensAmountLabel).For(v => v.Hidden).To(vm => vm.IsEyeglassLensesAmountVisible).WithConversion("BoolOpposite");
					set.Bind(eyeglassLensAmountTitleLabel).For(v => v.Hidden).To(vm => vm.IsEyeglassLensesAmountVisible).WithConversion("BoolOpposite");



					set.Bind(this.totalAmountLabel).To(item => item.TotalAmountCharged);
					set.Bind(totalAmountLabel).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedVisible).WithConversion("BoolOpposite");
					set.Bind(totalAmountTitleLabel).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedVisible).WithConversion("BoolOpposite");



					set.Bind(this.rightSphereLabel).To(item => item.RightSphere.Name);
					set.Bind(rightSphereLabel).For(v => v.Hidden).To(vm => vm.IsRightSphereEnabled).WithConversion("BoolOpposite");
					set.Bind(rightSphereTitleLabel).For(v => v.Hidden).To(vm => vm.IsRightSphereEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftSphereLabel).To(item => item.LeftSphere.Name);
					set.Bind(leftSphereLabel).For(v => v.Hidden).To(vm => vm.IsLeftSphereEnabled).WithConversion("BoolOpposite");
					set.Bind(leftSphereTitleLabel).For(v => v.Hidden).To(vm => vm.IsLeftSphereEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightCylinderLabel).To(item => item.RightCylinder.Name);
					set.Bind(rightCylinderLabel).For(v => v.Hidden).To(vm => vm.IsRightCylinderEnabled).WithConversion("BoolOpposite");
					set.Bind(rightCylinderTitleLabel).For(v => v.Hidden).To(vm => vm.IsRightCylinderEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftCylinderLabel).To(item => item.LeftCylinder.Name);
					set.Bind(leftCylinderLabel).For(v => v.Hidden).To(vm => vm.IsLeftCylinderEnabled).WithConversion("BoolOpposite");
					set.Bind(leftCylinderTitleLabel).For(v => v.Hidden).To(vm => vm.IsLeftCylinderEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightAxisLabel).To(item => item.RightAxis.Name);
					set.Bind(rightAxisLabel).For(v => v.Hidden).To(vm => vm.IsRightAxisEnabled).WithConversion("BoolOpposite");
					set.Bind(rightAxisTitleLabel).For(v => v.Hidden).To(vm => vm.IsRightAxisEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftAxisLabel).To(item => item.LeftAxis.Name);
					set.Bind(leftAxisLabel).For(v => v.Hidden).To(vm => vm.IsLeftAxisEnabled).WithConversion("BoolOpposite");
					set.Bind(leftAxisTitleLabel).For(v => v.Hidden).To(vm => vm.IsLeftAxisEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightPrismLabel).To(item => item.RightPrism.Name);
					set.Bind(rightPrismLabel).For(v => v.Hidden).To(vm => vm.IsRightPrismEnabled).WithConversion("BoolOpposite");
					set.Bind(rightPrismTitleLabel).For(v => v.Hidden).To(vm => vm.IsRightPrismEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftPrismLabel).To(item => item.LeftPrism.Name);
					set.Bind(leftPrismLabel).For(v => v.Hidden).To(vm => vm.IsLeftPrismEnabled).WithConversion("BoolOpposite");
					set.Bind(leftPrismTitleLabel).For(v => v.Hidden).To(vm => vm.IsLeftPrismEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightBifocalsLabel).To(item => item.RightBifocal.Name);
					set.Bind(rightBifocalsLabel).For(v => v.Hidden).To(vm => vm.IsRightBifocalEnabled).WithConversion("BoolOpposite");
					set.Bind(rightBifocalsTitleLabel).For(v => v.Hidden).To(vm => vm.IsRightBifocalEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftBifocalsLabel).To(item => item.LeftBifocal.Name);
					set.Bind(leftBifocalsLabel).For(v => v.Hidden).To(vm => vm.IsLeftBifocalEnabled).WithConversion("BoolOpposite");
					set.Bind(leftBifocalsTitleLabel).For(v => v.Hidden).To(vm => vm.IsLeftBifocalEnabled).WithConversion("BoolOpposite");

					set.Bind (this).For (v => v.ClaimSubmissionType).To (vm => vm.ClaimSubmissionType);

					set.Apply();


				});
		}

		private bool isMI;
		private ClaimSubmissionType _claimSubmissionType;
		public ClaimSubmissionType ClaimSubmissionType
		{
			get
			{
				return _claimSubmissionType;
			}
			set
			{
				_claimSubmissionType = value;
				switch (value.ID)
				{
				case "ACUPUNCTURE":
				case "CHIROPODY":
				case "CHIRO":
				case "PHYSIO":
				case "PODIATRY":
					break;
				case "PSYCHOLOGY":
				case "MASSAGE":
				case "NATUROPATHY":
				case "SPEECH":
					break;
				case "MI":
					isMI = true;
					break;
				case "ORTHODONTIC":
					break;
				case "CONTACTS":
					break;
				case "GLASSES":
					break;
				case "EYEEXAM":
					break;
				}
				SetNeedsLayout ();

//				if (!value.IsAlternateCarrierPaymentVisible) {
//					this.Hidden = true;
//				} else {
//					this.Hidden = false;
//					//altCarrierTitleLabel.Hidden = false;
//				}


			}
		}


	}
}
