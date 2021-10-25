 using System;
 using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
 using MvvmCross.Binding.BindingContext;

 namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimConfirmationIPadTableViewCell")]
	public class ClaimConfirmationIPadTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel typeLabel;
		public UILabel itemDescriptionLabel;
		public UILabel lengthLabel;
		public UILabel dateOfTreatmentLabel;
		public UILabel dateOfMonthlyTreatmentLabel;
		public UILabel dateofPurchaseLabel;
		public UILabel dateOfPickupLabel;
		public UILabel quantityLabel;
		public UILabel amountLabel;
		public UILabel totalAmountMedicalLabel;
		public UILabel orthFeeLabel;
		public UILabel altCarrierLabel;
		public UILabel gstLabel;
		public UILabel pstLabel;
		public UILabel dateOfReferralLabel;
		public UILabel typeOfProfessionalLabel;
		//Vision Properties
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

		public ClaimConfirmationIPadTableViewCell () : base () {}
		public ClaimConfirmationIPadTableViewCell (IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

        public void reorderCellWithModel(ClaimSubmissionConfirmationViewModel _model)
        {
            typeLabel.Hidden = !_model.Claim.IsTypeOfTreatmentVisible;
            itemDescriptionLabel.Hidden = !_model.Claim.IsItemDescriptionVisible;
            lengthLabel.Hidden = !_model.Claim.IsTreatmentDurationVisible;
            dateOfTreatmentLabel.Hidden =!_model.Claim.IsTreatementDateVisible;
            dateOfMonthlyTreatmentLabel.Hidden = !_model.Claim.IsDateOfMonthlyTreatmentVisible;
            dateofPurchaseLabel.Hidden = !_model.Claim.IsDateOfPurchaseVisible;
            dateOfPickupLabel.Hidden = !_model.Claim.IsPickupDateVisible;
            quantityLabel.Hidden = !_model.Claim.IsQuantityVisible;
            amountLabel.Hidden = !_model.Claim.IsTreatmentAmountVisible;
            totalAmountMedicalLabel.Hidden = !_model.Claim.IsTotalAmountChargedForMIVisible;
            orthFeeLabel.Hidden = !_model.Claim.IsOrthodonticMonthlyFeeVisible;
			altCarrierLabel.Hidden = !_model.Claim.IsAlternateCarrierPaymentVisible;
            gstLabel.Hidden = !_model.Claim.IsGSTHSTIncludedInTotalVisible;
			pstLabel.Hidden = !_model.Claim.IsPSTIncludedInTotalVisible;
            dateOfReferralLabel.Hidden = !_model.Claim.IsDateOfReferralVisibleForTreatment;
            typeOfProfessionalLabel.Hidden = !_model.Claim.IsTypeOfMedicalProfessionalVisibleForTreatment;
		    eyewearTypeLabel.Hidden = !_model.Claim.IsTypeOfEyewearVisible;
            lensTypeLabel.Hidden = !_model.Claim.IsTypeOfLensVisible;
            frameAmountLabel.Hidden = !_model.Claim.IsFrameAmountVisible;
            feeAmountLabel.Hidden = !_model.Claim.IsFeeAmountVisible;
            eyeglassLensAmountLabel.Hidden = !_model.Claim.IsEyeglassLensesAmountVisible;
            totalAmountLabel.Hidden = !_model.Claim.IsTotalAmountChargedVisible;
			rightSphereLabel.Hidden = !_model.Claim.IsRightSphereVisible;
			leftSphereLabel.Hidden = !_model.Claim.IsLeftSphereVisible;
            rightCylinderLabel.Hidden = !_model.Claim.IsRightCylinderVisible;
            leftCylinderLabel.Hidden = !_model.Claim.IsLeftCylinderVisible;
            rightAxisLabel.Hidden = !_model.Claim.IsRightAxisVisible;
            leftAxisLabel.Hidden = !_model.Claim.IsLeftAxisVisible;
            rightPrismLabel.Hidden = !_model.Claim.IsRightPrismVisible;
            leftPrismLabel.Hidden = !_model.Claim.IsLeftPrismVisible;
            rightBifocalsLabel.Hidden = !_model.Claim.IsRightBifocalVisible;
            leftBifocalsLabel.Hidden = !_model.Claim.IsLeftBifocalVisible;



            CreateLayout();
        }

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float topPadding = 15;
			float fieldPadding = Constants.CLAIMS_RESULTS_IPAD_PADDING;
			float rowHeight = Constants.CLAIMS_RESULTS_IPAD_HEIGHT;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

			float sectionWidth = 150;

			float xPos = fieldPadding;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (typeLabel == null)
            {
				typeLabel = new UILabel ();
                typeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
                typeLabel.TextAlignment = UITextAlignment.Left;
                typeLabel.BackgroundColor = Colors.Clear;
                typeLabel.TextColor = Colors.DARK_GREY_COLOR;
                typeLabel.Lines = 0;
                typeLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(typeLabel);
            }
            typeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)typeLabel.Frame.Height / 2, sectionWidth, (float)typeLabel.Frame.Height);
            typeLabel.SizeToFit();
            typeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)typeLabel.Frame.Height / 2, sectionWidth, (float)typeLabel.Frame.Height);
            if (!typeLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (itemDescriptionLabel == null)
            {
                itemDescriptionLabel = new UILabel();
                itemDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
                itemDescriptionLabel.TextAlignment = UITextAlignment.Left;
                itemDescriptionLabel.BackgroundColor = Colors.Clear;
                itemDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
                itemDescriptionLabel.Lines = 0;
                itemDescriptionLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(itemDescriptionLabel);
            }
            itemDescriptionLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)itemDescriptionLabel.Frame.Height / 2, sectionWidth, (float)itemDescriptionLabel.Frame.Height);
            itemDescriptionLabel.SizeToFit();
            itemDescriptionLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)itemDescriptionLabel.Frame.Height / 2, sectionWidth, (float)itemDescriptionLabel.Frame.Height);
            if (!itemDescriptionLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (lengthLabel == null)
            {
                lengthLabel = new UILabel();
                lengthLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
                lengthLabel.TextAlignment = UITextAlignment.Left;
                lengthLabel.BackgroundColor = Colors.Clear;
                lengthLabel.TextColor = Colors.DARK_GREY_COLOR;
                lengthLabel.Lines = 0;
                lengthLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(lengthLabel);            
            }
            lengthLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)lengthLabel.Frame.Height / 2, sectionWidth, (float)lengthLabel.Frame.Height);
            lengthLabel.SizeToFit();
            lengthLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)lengthLabel.Frame.Height / 2, (float)lengthLabel.Frame.Width, (float)lengthLabel.Frame.Height);
			if(!lengthLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (dateOfTreatmentLabel == null)
            {
                dateOfTreatmentLabel = new UILabel();
                dateOfTreatmentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                dateOfTreatmentLabel.TextAlignment = UITextAlignment.Left;
                dateOfTreatmentLabel.BackgroundColor = Colors.Clear;
                dateOfTreatmentLabel.TextColor = Colors.DARK_GREY_COLOR;
                dateOfTreatmentLabel.Lines = 0;
                dateOfTreatmentLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(dateOfTreatmentLabel);
            }
            dateOfTreatmentLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfTreatmentLabel.Frame.Height / 2, sectionWidth, (float)dateOfTreatmentLabel.Frame.Height);
            dateOfTreatmentLabel.SizeToFit();
            dateOfTreatmentLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfTreatmentLabel.Frame.Height / 2, sectionWidth, (float)dateOfTreatmentLabel.Frame.Height);
            if (!dateOfTreatmentLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (dateOfMonthlyTreatmentLabel == null)
            {
                dateOfMonthlyTreatmentLabel = new UILabel();
                dateOfMonthlyTreatmentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                dateOfMonthlyTreatmentLabel.TextAlignment = UITextAlignment.Left;
                dateOfMonthlyTreatmentLabel.BackgroundColor = Colors.Clear;
                dateOfMonthlyTreatmentLabel.TextColor = Colors.DARK_GREY_COLOR;
                dateOfMonthlyTreatmentLabel.Lines = 0;
                dateOfMonthlyTreatmentLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(dateOfMonthlyTreatmentLabel);
            }
            dateOfMonthlyTreatmentLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfMonthlyTreatmentLabel.Frame.Height / 2, sectionWidth, (float)dateOfMonthlyTreatmentLabel.Frame.Height);
            dateOfMonthlyTreatmentLabel.SizeToFit();
            dateOfMonthlyTreatmentLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfMonthlyTreatmentLabel.Frame.Height / 2, sectionWidth, (float)dateOfMonthlyTreatmentLabel.Frame.Height);
            if (!dateOfMonthlyTreatmentLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (dateofPurchaseLabel == null)
            {
                dateofPurchaseLabel = new UILabel();                
                dateofPurchaseLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                dateofPurchaseLabel.TextAlignment = UITextAlignment.Left;
                dateofPurchaseLabel.BackgroundColor = Colors.Clear;
                dateofPurchaseLabel.TextColor = Colors.DARK_GREY_COLOR;
                dateofPurchaseLabel.Lines = 0;
                dateofPurchaseLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(dateofPurchaseLabel);
            }
            dateofPurchaseLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateofPurchaseLabel.Frame.Height / 2, sectionWidth, (float)dateofPurchaseLabel.Frame.Height);
            dateofPurchaseLabel.SizeToFit();
            dateofPurchaseLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateofPurchaseLabel.Frame.Height / 2, sectionWidth, (float)dateofPurchaseLabel.Frame.Height); 
            if (!dateofPurchaseLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (dateOfPickupLabel == null)
            {
                dateOfPickupLabel = new UILabel();
                dateOfPickupLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                dateOfPickupLabel.TextAlignment = UITextAlignment.Left;
                dateOfPickupLabel.BackgroundColor = Colors.Clear;
                dateOfPickupLabel.TextColor = Colors.DARK_GREY_COLOR;
                dateOfPickupLabel.Lines = 0;
                dateOfPickupLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(dateOfPickupLabel);
            }
            dateOfPickupLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfPickupLabel.Frame.Height / 2, sectionWidth, (float)dateOfPickupLabel.Frame.Height);
            dateOfPickupLabel.SizeToFit();
            dateOfPickupLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfPickupLabel.Frame.Height / 2, sectionWidth, (float)dateOfPickupLabel.Frame.Height);
            if (!dateOfPickupLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (quantityLabel == null)
            {
                quantityLabel = new UILabel();
                quantityLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                quantityLabel.TextAlignment = UITextAlignment.Left;
                quantityLabel.BackgroundColor = Colors.Clear;
                quantityLabel.TextColor = Colors.DARK_GREY_COLOR;
                quantityLabel.Lines = 0;
                quantityLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(quantityLabel);
            }
            quantityLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)quantityLabel.Frame.Height / 2, sectionWidth, (float)quantityLabel.Frame.Height);
            quantityLabel.SizeToFit();
            quantityLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)quantityLabel.Frame.Height / 2, sectionWidth, (float)quantityLabel.Frame.Height);
            if (!quantityLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (amountLabel == null)
            {
                amountLabel = new UILabel();                
                amountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                amountLabel.TextAlignment = UITextAlignment.Left;
                amountLabel.BackgroundColor = Colors.Clear;
                amountLabel.TextColor = Colors.DARK_GREY_COLOR;
                amountLabel.Lines = 0;
                amountLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(amountLabel);
            }
            amountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)amountLabel.Frame.Height / 2, sectionWidth, (float)amountLabel.Frame.Height);
            amountLabel.SizeToFit();
            amountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)amountLabel.Frame.Height / 2, sectionWidth, (float)amountLabel.Frame.Height);
			if(!amountLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (totalAmountMedicalLabel == null)
            {
                totalAmountMedicalLabel = new UILabel();
                totalAmountMedicalLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                totalAmountMedicalLabel.TextAlignment = UITextAlignment.Left;
                totalAmountMedicalLabel.BackgroundColor = Colors.Clear;
                totalAmountMedicalLabel.TextColor = Colors.DARK_GREY_COLOR;
                totalAmountMedicalLabel.Lines = 0;
                totalAmountMedicalLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(totalAmountMedicalLabel);
            }
            totalAmountMedicalLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)totalAmountMedicalLabel.Frame.Height / 2, sectionWidth, (float)totalAmountMedicalLabel.Frame.Height);
            totalAmountMedicalLabel.SizeToFit();
            totalAmountMedicalLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)totalAmountMedicalLabel.Frame.Height / 2, sectionWidth, (float)totalAmountMedicalLabel.Frame.Height);                
			if(!totalAmountMedicalLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (orthFeeLabel == null)
            {
                orthFeeLabel = new UILabel();
                orthFeeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                orthFeeLabel.TextAlignment = UITextAlignment.Left;
                orthFeeLabel.BackgroundColor = Colors.Clear;
                orthFeeLabel.TextColor = Colors.DARK_GREY_COLOR;
                orthFeeLabel.Lines = 0;
                orthFeeLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(orthFeeLabel);
            }
            orthFeeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)orthFeeLabel.Frame.Height / 2, sectionWidth, (float)orthFeeLabel.Frame.Height);
            orthFeeLabel.SizeToFit();
            orthFeeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)orthFeeLabel.Frame.Height / 2, sectionWidth, (float)orthFeeLabel.Frame.Height);
            if (!orthFeeLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (altCarrierLabel == null)
            {
                altCarrierLabel = new UILabel();
                altCarrierLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                altCarrierLabel.TextAlignment = UITextAlignment.Left;
                altCarrierLabel.BackgroundColor = Colors.Clear;
                altCarrierLabel.TextColor = Colors.DARK_GREY_COLOR;
                altCarrierLabel.Lines = 0;
                altCarrierLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(altCarrierLabel);
            }
            altCarrierLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)altCarrierLabel.Frame.Height / 2, sectionWidth, (float)altCarrierLabel.Frame.Height);
            altCarrierLabel.SizeToFit();
            altCarrierLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)altCarrierLabel.Frame.Height / 2, sectionWidth, (float)altCarrierLabel.Frame.Height);
            if (!altCarrierLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (gstLabel == null)
            {
                gstLabel = new UILabel();
                gstLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                gstLabel.TextAlignment = UITextAlignment.Left;
                gstLabel.BackgroundColor = Colors.Clear;
                gstLabel.TextColor = Colors.DARK_GREY_COLOR;
                gstLabel.Lines = 0;
                gstLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(gstLabel);
            }
            gstLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)gstLabel.Frame.Height / 2, sectionWidth, (float)gstLabel.Frame.Height);
            gstLabel.SizeToFit();
            gstLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)gstLabel.Frame.Height / 2, sectionWidth, (float)gstLabel.Frame.Height);
            if (!gstLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (pstLabel == null)
            {
                pstLabel = new UILabel();
                pstLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                pstLabel.TextAlignment = UITextAlignment.Left;
                pstLabel.BackgroundColor = Colors.Clear;
                pstLabel.TextColor = Colors.DARK_GREY_COLOR;
                pstLabel.Lines = 0;
                pstLabel.ClipsToBounds = false;
                AddSubview(pstLabel);
            }
            pstLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)pstLabel.Frame.Height / 2, sectionWidth, (float)pstLabel.Frame.Height);
            pstLabel.SizeToFit();
            pstLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)pstLabel.Frame.Height / 2, sectionWidth, (float)pstLabel.Frame.Height);
            if (!pstLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (dateOfReferralLabel == null)
            {
                dateOfReferralLabel = new UILabel();
                dateOfReferralLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                dateOfReferralLabel.TextAlignment = UITextAlignment.Left;
                dateOfReferralLabel.BackgroundColor = Colors.Clear;
                dateOfReferralLabel.TextColor = Colors.DARK_GREY_COLOR;
                dateOfReferralLabel.Lines = 0;
                dateOfReferralLabel.AdjustsFontSizeToFitWidth = true;
                AddSubview(dateOfReferralLabel);
            }
            dateOfReferralLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfReferralLabel.Frame.Height / 2, sectionWidth, (float)dateOfReferralLabel.Frame.Height);
            dateOfReferralLabel.SizeToFit();
            dateOfReferralLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfReferralLabel.Frame.Height / 2, sectionWidth, (float)dateOfReferralLabel.Frame.Height);
            if (!dateOfReferralLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (typeOfProfessionalLabel == null)
            {
                typeOfProfessionalLabel = new UILabel();
                typeOfProfessionalLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                typeOfProfessionalLabel.TextAlignment = UITextAlignment.Left;
                typeOfProfessionalLabel.BackgroundColor = Colors.Clear;
                typeOfProfessionalLabel.TextColor = Colors.DARK_GREY_COLOR;
                typeOfProfessionalLabel.Lines = 0;
                typeOfProfessionalLabel.ClipsToBounds = false;
                AddSubview(typeOfProfessionalLabel);
            }
            typeOfProfessionalLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)typeOfProfessionalLabel.Frame.Height / 2, sectionWidth, (float)typeOfProfessionalLabel.Frame.Height);
            typeOfProfessionalLabel.SizeToFit();
            typeOfProfessionalLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)typeOfProfessionalLabel.Frame.Height / 2, sectionWidth, (float)typeOfProfessionalLabel.Frame.Height);
            if (!typeOfProfessionalLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			//Vision Properties

            if (eyewearTypeLabel == null)
            {
                eyewearTypeLabel = new UILabel();
                eyewearTypeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                eyewearTypeLabel.TextAlignment = UITextAlignment.Left;
                eyewearTypeLabel.BackgroundColor = Colors.Clear;
                eyewearTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
                eyewearTypeLabel.Lines = 0;
                eyewearTypeLabel.ClipsToBounds = false;
                AddSubview(eyewearTypeLabel);
            }
            eyewearTypeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)eyewearTypeLabel.Frame.Height / 2, sectionWidth, (float)eyewearTypeLabel.Frame.Height);
            eyewearTypeLabel.SizeToFit();
            eyewearTypeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)eyewearTypeLabel.Frame.Height / 2, sectionWidth, (float)eyewearTypeLabel.Frame.Height);
            if (!eyewearTypeLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (lensTypeLabel == null)
            {
                lensTypeLabel = new UILabel();
                lensTypeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                lensTypeLabel.TextAlignment = UITextAlignment.Left;
                lensTypeLabel.BackgroundColor = Colors.Clear;
                lensTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
                lensTypeLabel.Lines = 0;
                lensTypeLabel.ClipsToBounds = false;
                AddSubview(lensTypeLabel);
            }
            lensTypeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)lensTypeLabel.Frame.Height / 2, sectionWidth, (float)lensTypeLabel.Frame.Height);
            lensTypeLabel.SizeToFit();
            lensTypeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)lensTypeLabel.Frame.Height / 2, sectionWidth, (float)lensTypeLabel.Frame.Height);
            if (!lensTypeLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (frameAmountLabel == null)
            {
                frameAmountLabel = new UILabel();
                frameAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                frameAmountLabel.TextAlignment = UITextAlignment.Left;
                frameAmountLabel.BackgroundColor = Colors.Clear;
                frameAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
                frameAmountLabel.Lines = 0;
                frameAmountLabel.ClipsToBounds = false;
                AddSubview(frameAmountLabel);
            }
            frameAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)frameAmountLabel.Frame.Height / 2, sectionWidth, (float)frameAmountLabel.Frame.Height);
            frameAmountLabel.SizeToFit();
            frameAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)frameAmountLabel.Frame.Height / 2, sectionWidth, (float)frameAmountLabel.Frame.Height);
            if (!frameAmountLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (feeAmountLabel == null)
            {
                feeAmountLabel = new UILabel();
                feeAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                feeAmountLabel.TextAlignment = UITextAlignment.Left;
                feeAmountLabel.BackgroundColor = Colors.Clear;
                feeAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
                feeAmountLabel.Lines = 0;
                feeAmountLabel.ClipsToBounds = false;
                AddSubview(feeAmountLabel);
            }
            feeAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)feeAmountLabel.Frame.Height / 2, sectionWidth, (float)feeAmountLabel.Frame.Height);
            feeAmountLabel.SizeToFit();
            feeAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)feeAmountLabel.Frame.Height / 2, sectionWidth, (float)feeAmountLabel.Frame.Height);
            if (!feeAmountLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (eyeglassLensAmountLabel == null)
            {
                eyeglassLensAmountLabel = new UILabel();
                eyeglassLensAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                eyeglassLensAmountLabel.TextAlignment = UITextAlignment.Left;
                eyeglassLensAmountLabel.BackgroundColor = Colors.Clear;
                eyeglassLensAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
                eyeglassLensAmountLabel.Lines = 0;
                eyeglassLensAmountLabel.ClipsToBounds = false;
                AddSubview(eyeglassLensAmountLabel);
            }
            eyeglassLensAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)eyeglassLensAmountLabel.Frame.Height / 2, sectionWidth, (float)eyeglassLensAmountLabel.Frame.Height);
            eyeglassLensAmountLabel.SizeToFit();
            eyeglassLensAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)eyeglassLensAmountLabel.Frame.Height / 2, sectionWidth, (float)eyeglassLensAmountLabel.Frame.Height);
            if (!eyeglassLensAmountLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (totalAmountLabel == null)
            {
                totalAmountLabel = new UILabel();
                totalAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                totalAmountLabel.TextAlignment = UITextAlignment.Left;
                totalAmountLabel.BackgroundColor = Colors.Clear;
                totalAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
                totalAmountLabel.Lines = 0;
                totalAmountLabel.ClipsToBounds = false;
                AddSubview(totalAmountLabel);
            }
            totalAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)totalAmountLabel.Frame.Height / 2, sectionWidth, (float)totalAmountLabel.Frame.Height);
            totalAmountLabel.SizeToFit();
            totalAmountLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)totalAmountLabel.Frame.Height / 2, sectionWidth, (float)totalAmountLabel.Frame.Height);
            if (!totalAmountLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (rightSphereLabel == null)
            {
                rightSphereLabel = new UILabel();
                rightSphereLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                rightSphereLabel.TextAlignment = UITextAlignment.Left;
                rightSphereLabel.BackgroundColor = Colors.Clear;
                rightSphereLabel.TextColor = Colors.DARK_GREY_COLOR;
                rightSphereLabel.Lines = 0;
                rightSphereLabel.ClipsToBounds = false;
                AddSubview(rightSphereLabel);
            }
            rightSphereLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightSphereLabel.Frame.Height / 2, sectionWidth, (float)rightSphereLabel.Frame.Height);
            rightSphereLabel.SizeToFit();
            rightSphereLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightSphereLabel.Frame.Height / 2, sectionWidth, (float)rightSphereLabel.Frame.Height);
            if (!rightSphereLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (leftSphereLabel == null)
            {
                leftSphereLabel = new UILabel();
                leftSphereLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                leftSphereLabel.TextAlignment = UITextAlignment.Left;
                leftSphereLabel.BackgroundColor = Colors.Clear;
                leftSphereLabel.TextColor = Colors.DARK_GREY_COLOR;
                leftSphereLabel.Lines = 0;
                leftSphereLabel.ClipsToBounds = false;
                AddSubview(leftSphereLabel);
            }
            leftSphereLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftSphereLabel.Frame.Height / 2, sectionWidth, (float)leftSphereLabel.Frame.Height);
            leftSphereLabel.SizeToFit();
            leftSphereLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftSphereLabel.Frame.Height / 2, sectionWidth, (float)leftSphereLabel.Frame.Height);
            if (!leftSphereLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (rightCylinderLabel == null)
            {
                rightCylinderLabel = new UILabel();
                rightCylinderLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                rightCylinderLabel.TextAlignment = UITextAlignment.Left;
                rightCylinderLabel.BackgroundColor = Colors.Clear;
                rightCylinderLabel.TextColor = Colors.DARK_GREY_COLOR;
                rightCylinderLabel.Lines = 0;
                rightCylinderLabel.ClipsToBounds = false;
                AddSubview(rightCylinderLabel);
            }
            rightCylinderLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightCylinderLabel.Frame.Height / 2, sectionWidth, (float)rightCylinderLabel.Frame.Height);
            rightCylinderLabel.SizeToFit();
            rightCylinderLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightCylinderLabel.Frame.Height / 2, sectionWidth, (float)rightCylinderLabel.Frame.Height);
            if (!rightCylinderLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (leftCylinderLabel == null)
            {
                leftCylinderLabel = new UILabel();
                leftCylinderLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                leftCylinderLabel.TextAlignment = UITextAlignment.Left;
                leftCylinderLabel.BackgroundColor = Colors.Clear;
                leftCylinderLabel.TextColor = Colors.DARK_GREY_COLOR;
                leftCylinderLabel.Lines = 0;
                leftCylinderLabel.ClipsToBounds = false;
                AddSubview(leftCylinderLabel);
            }
            leftCylinderLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftCylinderLabel.Frame.Height / 2, sectionWidth, (float)leftCylinderLabel.Frame.Height);
            leftCylinderLabel.SizeToFit();
            leftCylinderLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftCylinderLabel.Frame.Height / 2, sectionWidth, (float)leftCylinderLabel.Frame.Height);
            if (!leftCylinderLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (rightAxisLabel == null)
            {
                rightAxisLabel = new UILabel();
                rightAxisLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                rightAxisLabel.TextAlignment = UITextAlignment.Left;
                rightAxisLabel.BackgroundColor = Colors.Clear;
                rightAxisLabel.TextColor = Colors.DARK_GREY_COLOR;
                rightAxisLabel.Lines = 0;
                rightAxisLabel.ClipsToBounds = false;
                AddSubview(rightAxisLabel);
            }
            rightAxisLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightAxisLabel.Frame.Height / 2, sectionWidth, (float)rightAxisLabel.Frame.Height);
            rightAxisLabel.SizeToFit();
            rightAxisLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightAxisLabel.Frame.Height / 2, sectionWidth, (float)rightAxisLabel.Frame.Height);
            if (!rightAxisLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (leftAxisLabel == null)
            {
                leftAxisLabel = new UILabel();
                leftAxisLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                leftAxisLabel.TextAlignment = UITextAlignment.Left;
                leftAxisLabel.BackgroundColor = Colors.Clear;
                leftAxisLabel.TextColor = Colors.DARK_GREY_COLOR;
                leftAxisLabel.Lines = 0;
                leftAxisLabel.ClipsToBounds = false;
                AddSubview(leftAxisLabel);
            }
            leftAxisLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftAxisLabel.Frame.Height / 2, sectionWidth, (float)leftAxisLabel.Frame.Height);
            leftAxisLabel.SizeToFit();
            leftAxisLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftAxisLabel.Frame.Height / 2, sectionWidth, (float)leftAxisLabel.Frame.Height);
            if (!leftAxisLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (rightPrismLabel == null)
            {
                rightPrismLabel = new UILabel();
                rightPrismLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                rightPrismLabel.TextAlignment = UITextAlignment.Left;
                rightPrismLabel.BackgroundColor = Colors.Clear;
                rightPrismLabel.TextColor = Colors.DARK_GREY_COLOR;
                rightPrismLabel.Lines = 0;
                rightPrismLabel.ClipsToBounds = false;
                AddSubview(rightPrismLabel);
            }
            rightPrismLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightPrismLabel.Frame.Height / 2, sectionWidth, (float)rightPrismLabel.Frame.Height);
            rightPrismLabel.SizeToFit();
            rightPrismLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightPrismLabel.Frame.Height / 2, sectionWidth, (float)rightPrismLabel.Frame.Height);
            if (!rightPrismLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (leftPrismLabel == null)
            {
                leftPrismLabel = new UILabel();
                leftPrismLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                leftPrismLabel.TextAlignment = UITextAlignment.Left;
                leftPrismLabel.BackgroundColor = Colors.Clear;
                leftPrismLabel.TextColor = Colors.DARK_GREY_COLOR;
                leftPrismLabel.Lines = 0;
                leftPrismLabel.ClipsToBounds = false;
                AddSubview(leftPrismLabel);
            }
            leftPrismLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftPrismLabel.Frame.Height / 2, sectionWidth, (float)leftPrismLabel.Frame.Height);
            leftPrismLabel.SizeToFit();
            leftPrismLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftPrismLabel.Frame.Height / 2, sectionWidth, (float)leftPrismLabel.Frame.Height);
            if (!leftPrismLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (rightBifocalsLabel == null)
            {
                rightBifocalsLabel = new UILabel();
                rightBifocalsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                rightBifocalsLabel.TextAlignment = UITextAlignment.Left;
                rightBifocalsLabel.BackgroundColor = Colors.Clear;
                rightBifocalsLabel.TextColor = Colors.DARK_GREY_COLOR;
                rightBifocalsLabel.Lines = 0;
                rightBifocalsLabel.ClipsToBounds = false;
                AddSubview(rightBifocalsLabel);
            }
            rightBifocalsLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightBifocalsLabel.Frame.Height / 2, sectionWidth, (float)rightBifocalsLabel.Frame.Height);
            rightBifocalsLabel.SizeToFit();
            rightBifocalsLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightBifocalsLabel.Frame.Height / 2, sectionWidth, (float)rightBifocalsLabel.Frame.Height);
            if (!rightBifocalsLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

            if (leftBifocalsLabel == null)
            {
                leftBifocalsLabel = new UILabel();
                leftBifocalsLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
                leftBifocalsLabel.TextAlignment = UITextAlignment.Left;
                leftBifocalsLabel.BackgroundColor = Colors.Clear;
                leftBifocalsLabel.TextColor = Colors.DARK_GREY_COLOR;
                leftBifocalsLabel.Lines = 0;
                leftBifocalsLabel.ClipsToBounds = false;
                AddSubview(leftBifocalsLabel);
            }
            leftBifocalsLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftBifocalsLabel.Frame.Height / 2, sectionWidth, (float)leftBifocalsLabel.Frame.Height);
            leftBifocalsLabel.SizeToFit();
            leftBifocalsLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftBifocalsLabel.Frame.Height / 2, sectionWidth, (float)leftBifocalsLabel.Frame.Height);
            if (!leftBifocalsLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

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
					var set = this.CreateBindingSet<ClaimConfirmationIPadTableViewCell,TreatmentDetail>();
					set.Bind(this.typeLabel).To(item => item.TypeOfTreatmentListViewItem.Name);
                    //set.Bind(typeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfTreatmentVisible).WithConversion("BoolOpposite");
                    set.Bind(this).For(v => v.ItemDescription).To(item => item.ItemDescription.Name);
					//set.Bind(itemDescriptionLabel).For(v => v.Hidden).To(vm => vm.IsItemDescriptionVisible).WithConversion("BoolOpposite");

					set.Bind(this.lengthLabel).To(item => item.TreatmentDurationListViewItem.Name);
					//set.Bind(lengthLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentDurationVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateOfTreatmentLabel).To(item => item.TreatmentDate).WithConversion("DateToString");
					//set.Bind(dateOfTreatmentLabel).For(v => v.Hidden).To(vm => vm.IsTreatementDateVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateOfMonthlyTreatmentLabel).To(item => item.DateOfMonthlyTreatment).WithConversion("DateToString");
					//set.Bind(dateOfMonthlyTreatmentLabel).For(v => v.Hidden).To(vm => vm.IsDateOfMonthlyTreatmentVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateofPurchaseLabel).To(item => item.DateOfPurchase).WithConversion("DateToString");
					//set.Bind(dateofPurchaseLabel).For(v => v.Hidden).To(vm => vm.IsDateOfPurchaseVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateOfPickupLabel).To(item => item.PickupDate).WithConversion("DateToString");
					//set.Bind(dateOfPickupLabel).For(v => v.Hidden).To(vm => vm.IsPickupDateVisible).WithConversion("BoolOpposite");

					set.Bind(this.quantityLabel).To(item => item.Quantity);
					//set.Bind(quantityLabel).For(v => v.Hidden).To(vm => vm.IsQuantityVisible).WithConversion("BoolOpposite");

					set.Bind(this.amountLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");
					//set.Bind(amountLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentAmountVisible).WithConversion("BoolOpposite");

					set.Bind(this.totalAmountMedicalLabel).To(item => item.TreatmentAmount).WithConversion("DollarSignDoublePrefix");
					//set.Bind(totalAmountMedicalLabel).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedForMIVisible).WithConversion("BoolOpposite");

					set.Bind(this.orthFeeLabel).To(item => item.OrthodonticMonthlyFee).WithConversion("DollarSignDoublePrefix");
					//set.Bind(orthFeeLabel).For(v => v.Hidden).To(vm => vm.IsOrthodonticMonthlyFeeVisible).WithConversion("BoolOpposite");

					set.Bind(this.altCarrierLabel).To(item => item.AlternateCarrierPayment).WithConversion("DollarSignDoublePrefix");
					//set.Bind(altCarrierLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");

					set.Bind(this.gstLabel).To(item => item.GSTHSTIncludedInTotal).WithConversion("BoolToString");
					//set.Bind(gstLabel).For(v => v.Hidden).To(vm => vm.IsGSTHSTIncludedInTotalVisible).WithConversion("BoolOpposite");

					set.Bind(this.pstLabel).To(item => item.PSTIncludedInTotal).WithConversion("BoolToString");
					//set.Bind(pstLabel).For(v => v.Hidden).To(vm => vm.IsPSTIncludedInTotalVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateOfReferralLabel).To(item => item.DateOfReferral).WithConversion("DateToString");
					//set.Bind(dateOfReferralLabel).For(v => v.Hidden).To(vm => vm.IsDateOfReferralVisible).WithConversion("BoolOpposite");

					set.Bind(this.typeOfProfessionalLabel).To(item => item.TypeOfMedicalProfessional.Name);
					//set.Bind(typeOfProfessionalLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfMedicalProfessionalVisible).WithConversion("BoolOpposite");

					//Vision Properties

					set.Bind(this.eyewearTypeLabel).To(item => item.TypeOfEyewear.Name);
					//set.Bind(eyewearTypeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfEyewearVisible).WithConversion("BoolOpposite");

					set.Bind(this.lensTypeLabel).To(item => item.TypeOfLens.Name);
					//set.Bind(lensTypeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfLensVisible).WithConversion("BoolOpposite");

					set.Bind(this.frameAmountLabel).To(item => item.FrameAmount);
					//set.Bind(frameAmountLabel).For(v => v.Hidden).To(vm => vm.IsFrameAmountVisible).WithConversion("BoolOpposite");

					set.Bind(this.feeAmountLabel).To(item => item.FeeAmount);
					//set.Bind(feeAmountLabel).For(v => v.Hidden).To(vm => vm.IsFeeAmountVisible).WithConversion("BoolOpposite");

					set.Bind(this.eyeglassLensAmountLabel).To(item => item.EyeglassLensesAmount);
					//set.Bind(eyeglassLensAmountLabel).For(v => v.Hidden).To(vm => vm.IsEyeglassLensesAmountVisible).WithConversion("BoolOpposite");

					set.Bind(this.totalAmountLabel).To(item => item.TotalAmountCharged);
					//set.Bind(totalAmountLabel).For(v => v.Hidden).To(vm => vm.IsTotalAmountChargedVisible).WithConversion("BoolOpposite");

					set.Bind(this.rightSphereLabel).To(item => item.RightSphere.Name);
					//set.Bind(rightSphereLabel).For(v => v.Hidden).To(vm => vm.IsRightSphereEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftSphereLabel).To(item => item.LeftSphere.Name);
					//set.Bind(leftSphereLabel).For(v => v.Hidden).To(vm => vm.IsLeftSphereEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightCylinderLabel).To(item => item.RightCylinder.Name);
					//set.Bind(rightCylinderLabel).For(v => v.Hidden).To(vm => vm.IsRightCylinderEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftCylinderLabel).To(item => item.LeftCylinder.Name);
					//set.Bind(leftCylinderLabel).For(v => v.Hidden).To(vm => vm.IsLeftCylinderEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightAxisLabel).To(item => item.RightAxis.Name);
					//set.Bind(rightAxisLabel).For(v => v.Hidden).To(vm => vm.IsRightAxisEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftAxisLabel).To(item => item.LeftAxis.Name);
					//set.Bind(leftAxisLabel).For(v => v.Hidden).To(vm => vm.IsLeftAxisEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightPrismLabel).To(item => item.RightPrism.Name);
					//set.Bind(rightPrismLabel).For(v => v.Hidden).To(vm => vm.IsRightPrismEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftPrismLabel).To(item => item.LeftPrism.Name);
					//set.Bind(leftPrismLabel).For(v => v.Hidden).To(vm => vm.IsLeftPrismEnabled).WithConversion("BoolOpposite");

					set.Bind(this.rightBifocalsLabel).To(item => item.RightBifocal.Name);
					//set.Bind(rightBifocalsLabel).For(v => v.Hidden).To(vm => vm.IsRightBifocalEnabled).WithConversion("BoolOpposite");

					set.Bind(this.leftBifocalsLabel).To(item => item.LeftBifocal.Name);
					//set.Bind(leftBifocalsLabel).For(v => v.Hidden).To(vm => vm.IsLeftBifocalEnabled).WithConversion("BoolOpposite");

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

			}
		}

        private string _itemDescription;
        public string ItemDescription {
            get
            {
                return _itemDescription;
            }
            set
            {
                _itemDescription = value;
                itemDescriptionLabel.Text = value;
            }
        }


	}
}
