using System;
using System.Threading.Tasks;
using CoreGraphics;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.Claims;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimSubmissionConfirmationView1")]
    public class ClaimSubmissionConfirmationView1 : GSCBaseViewPaddingController, IRehydrating, IGSCBaseViewImplementor
    {
        #region IRehydrating
        public bool Rehydrating
        {
            get;
            set;
        }
        public bool FinishedRehydrating
        {
            get;
            set;
        }
        #endregion
        ClaimSubmissionConfirmationViewModel _model;

        protected UIScrollView scrollContainer;

        protected UIScrollView detailsScrollContainer;

        protected GSButton submitButton;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        protected UILabel planInformationLabel;
        protected UILabel claimDetailsLabel;
        protected UILabel treatmentDetailsLabel;

        protected UILabel submittedToGSC;

        private ClaimFieldResultDisplay gscIDResult;
        private ClaimFieldResultDisplay participantNameResult;
        private ClaimFieldResultDisplay providerResult;


        private ClaimBoolResultDisplay anotherPlanCoverageResult;
        private ClaimBoolResultDisplay otherBenefitsWithGSCResult;
        private ClaimBoolResultDisplay otherBenefitsSubmittedResult;
        private ClaimBoolResultDisplay otherBenefitsPayBalanceThroughOtherGSCResult;
        private ClaimFieldResultRightDisplay otherGSCNumberResult;

        private ClaimBoolResultDisplay hcsaResult;

        private ClaimBoolResultDisplay gstResult;

        private ClaimBoolResultDisplay motorVehicleAccidentResult;
        private ClaimFieldResultRightDisplay motorVehicleDateResult;

        private ClaimBoolResultDisplay workRelatedInuryResult;
        private ClaimFieldResultRightDisplay workInjuryDateResult;
        private ClaimFieldResultRightDisplay workInjuryCaseNumberResult;


        private ClaimBoolResultDisplay referralPreviouslySubmittedResult;
        private ClaimFieldResultRightDisplay dateofReferralResult;
        private ClaimFieldResultRightDisplay typeOfMedicalProfessionalResult;
        private ClaimBoolResultDisplay sportPurposesResult;

        private bool isNoTreatmentType;
        private bool isLengthType;

        UITableView treatmentDetailsList;
        MvxConfirmationTableViewSource treatmentDetailSource;
        protected UIBarButtonItem cancelButton;

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
        public UILabel orthFeeLabel;
        public UILabel altCarrierTitleLabel;
        public UILabel gstLabel;
        public UILabel pstLabel;
        public UILabel dateOfReferralTitleLabel;
        public UILabel typeOfProfessionalTitleLabel;
        //Vision Changes
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
        protected UIView addSeparator;

        //ClaimTreatmentDetailsTitleAndTextField gscNumberField;
        ClaimTreatmentDetailsTitleAndList participantList;

        protected float labelsCount = 0;

        public ClaimSubmissionConfirmationView1()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (Rehydrating)
            {
                rehydrationservice.Rehydrating = true;
            }
            base.NavigationItem.Title = "claimSubmissionConfirmation".tr();

            if (Constants.IS_OS_7_OR_LATER())
            {
                base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
                base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
                base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
            }
            else
            {
                base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            }

            if (Constants.IS_OS_7_OR_LATER())
                this.AutomaticallyAdjustsScrollViewInsets = false;

            _model = (ClaimSubmissionConfirmationViewModel)this.ViewModel;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(true, false);
            cancelButton = new UIBarButtonItem();
            cancelButton.Style = UIBarButtonItemStyle.Plain;
            cancelButton.Clicked += HandleCancelButton;
            cancelButton.Title = "cancel".tr();
            cancelButton.TintColor = Colors.HIGHLIGHT_COLOR;
            UITextAttributes attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
            cancelButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
            base.NavigationItem.LeftBarButtonItem = cancelButton;


            scrollContainer = new UIScrollView();
            scrollContainer.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            planInformationLabel = new UILabel();
            planInformationLabel.Text = "planInformationTitle".tr();
            planInformationLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            planInformationLabel.TextColor = Colors.DARK_GREY_COLOR;
            planInformationLabel.BackgroundColor = Colors.Clear;
            planInformationLabel.TextAlignment = UITextAlignment.Left;
            scrollContainer.AddSubview(planInformationLabel);

            gscIDResult = new ClaimFieldResultDisplay("greenShieldId".FormatWithBrandKeywords(LocalizableBrand.GreenShield), _model.Claim.Participant.PlanMemberID);
            scrollContainer.AddSubview(gscIDResult);

            participantNameResult = new ClaimFieldResultDisplay("participantName".tr(), _model.Claim.Participant.FullName);
            scrollContainer.AddSubview(participantNameResult);

            providerResult = new ClaimFieldResultDisplay("healthProvider".tr(), _model.Claim.Provider.DoctorName + "\r\n" + _model.Claim.Provider.Address + "\r\n" + _model.Claim.Provider.Phone);
            scrollContainer.AddSubview(providerResult);

            claimDetailsLabel = new UILabel();
            claimDetailsLabel.Text = "claimDetailsTitle".tr();
            claimDetailsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            claimDetailsLabel.TextColor = Colors.DARK_GREY_COLOR;
            claimDetailsLabel.BackgroundColor = Colors.Clear;
            claimDetailsLabel.TextAlignment = UITextAlignment.Left;
            scrollContainer.AddSubview(claimDetailsLabel);

            anotherPlanCoverageResult = new ClaimBoolResultDisplay("otherBenefitsTitle".tr(), _model.Claim.CoverageUnderAnotherBenefitsPlan);
            scrollContainer.AddSubview(anotherPlanCoverageResult);

            if (_model.Claim.IsIsOtherCoverageWithGSCVisible)
            {
                otherBenefitsWithGSCResult = new ClaimBoolResultDisplay("otherBenefitsWithGSC".FormatWithBrandKeywords(LocalizableBrand.GSC), _model.Claim.IsOtherCoverageWithGSC);
                scrollContainer.AddSubview(otherBenefitsWithGSCResult);
            }

            if (_model.Claim.IsHasClaimBeenSubmittedToOtherBenefitPlanVisible)
            {
                otherBenefitsSubmittedResult = new ClaimBoolResultDisplay("otherBenefitsSubmitted".tr(), _model.Claim.HasClaimBeenSubmittedToOtherBenefitPlan);
                scrollContainer.AddSubview(otherBenefitsSubmittedResult);
            }


            if (_model.Claim.IsPayAnyUnpaidBalanceThroughOtherGSCPlanVisible)
            {
                otherBenefitsPayBalanceThroughOtherGSCResult = new ClaimBoolResultDisplay("otherBenefitsPayBalanceThroughOtherGSC".FormatWithBrandKeywords(LocalizableBrand.GSC), _model.Claim.PayAnyUnpaidBalanceThroughOtherGSCPlan);
                scrollContainer.AddSubview(otherBenefitsPayBalanceThroughOtherGSCResult);
            }


            if (_model.Claim.IsOtherGSCNumberVisible)
            {
                otherGSCNumberResult = new ClaimFieldResultRightDisplay("otherBenefitsGSCNum".FormatWithBrandKeywords(LocalizableBrand.GSC), _model.Claim.OtherGSCNumber);
                scrollContainer.AddSubview(otherGSCNumberResult);
            }


            if (_model.Claim.IsPayUnderHCSAVisible)
            {
                hcsaResult = new ClaimBoolResultDisplay("otherBenefitsHSCA".tr() + "?", _model.Claim.PayUnderHCSA);
                scrollContainer.AddSubview(hcsaResult);
            }

            if (_model.Claim.IsIsGSTHSTIncludedVisible)
            {
                gstResult = new ClaimBoolResultDisplay("gstIncludedQ".tr(), _model.Claim.IsGSTHSTIncluded);
                scrollContainer.AddSubview(gstResult);
            }

            motorVehicleAccidentResult = new ClaimBoolResultDisplay("motorVehicleTitle".tr(), _model.Claim.IsTreatmentDueToAMotorVehicleAccident);
            scrollContainer.AddSubview(motorVehicleAccidentResult);

            if (_model.Claim.IsDateOfMotorVehicleAccidentVisible)
            {
                motorVehicleDateResult = new ClaimFieldResultRightDisplay("motorVehicleDate".tr(), _model.Claim.DateOfMotorVehicleAccident.ToShortDateString());
                scrollContainer.AddSubview(motorVehicleDateResult);
            }

            workRelatedInuryResult = new ClaimBoolResultDisplay("workInjuryTitle".tr(), _model.Claim.IsTreatmentDueToAWorkRelatedInjury);
            scrollContainer.AddSubview(workRelatedInuryResult);

            if (_model.Claim.IsDateOfWorkRelatedInjuryVisible)
            {
                workInjuryDateResult = new ClaimFieldResultRightDisplay("workInjuryDate".tr(), _model.Claim.DateOfWorkRelatedInjury.ToShortDateString());
                scrollContainer.AddSubview(workInjuryDateResult);
            }

            if (_model.Claim.IsWorkRelatedInjuryCaseNumberVisible)
            {
                workInjuryCaseNumberResult = new ClaimFieldResultRightDisplay("workInjuryCaseNumber".tr(), _model.Claim.WorkRelatedInjuryCaseNumber.ToString());
                scrollContainer.AddSubview(workInjuryCaseNumberResult);
            }


            referralPreviouslySubmittedResult = new ClaimBoolResultDisplay("medicalItemTitle".tr(), _model.Claim.HasReferralBeenPreviouslySubmitted);
            scrollContainer.AddSubview(referralPreviouslySubmittedResult);

            if (_model.Claim.IsDateOfReferralVisible)
            {
                dateofReferralResult = new ClaimFieldResultRightDisplay("medicalItemReferralDate".tr(), _model.Claim.DateOfReferral.ToShortDateString());
                scrollContainer.AddSubview(dateofReferralResult);
            }

            if (_model.Claim.IsTypeOfMedicalProfessionalVisible)
            {
                typeOfMedicalProfessionalResult = new ClaimFieldResultRightDisplay("medicalItemProfessional".tr(), _model.Claim.TypeOfMedicalProfessional.Name);
                scrollContainer.AddSubview(typeOfMedicalProfessionalResult);
            }

            if (_model.Claim.IsIsMedicalItemForSportsOnlyVisible)
            {
                sportPurposesResult = new ClaimBoolResultDisplay("medicalItemSport".tr(), _model.Claim.IsMedicalItemForSportsOnly);
                scrollContainer.AddSubview(sportPurposesResult);
            }

            treatmentDetailsLabel = new UILabel();
            treatmentDetailsLabel.Text = "treatmentDetailsTitle".tr();
            treatmentDetailsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            treatmentDetailsLabel.TextColor = Colors.DARK_GREY_COLOR;
            treatmentDetailsLabel.BackgroundColor = Colors.Clear;
            treatmentDetailsLabel.TextAlignment = UITextAlignment.Left;
            scrollContainer.AddSubview(treatmentDetailsLabel);





            detailsScrollContainer = new UIScrollView();
            detailsScrollContainer.BackgroundColor = Colors.Clear;
            detailsScrollContainer.ShowsHorizontalScrollIndicator = true;
            scrollContainer.AddSubview(detailsScrollContainer);

            switch (_model.Claim.Type.ID)
            {
                case "ACUPUNCTURE":
                case "CHIROPODY":
                case "CHIRO":
                case "PODIATRY":
                case "PHYSIO":
                    isNoTreatmentType = false;
                    break;
                case "MI":
                case "ORTHODONTIC":
                case "CONTACTS":
                case "GLASSES":
                case "EYEEXAM":
                    isNoTreatmentType = true;
                    break;
                case "SPEECH":
                case "PSYCHOLOGY":
                case "MASSAGE":
                case "NATUROPATHY":
                    isLengthType = true;
                    isNoTreatmentType = false;
                    break;
            }

            treatmentDetailsList = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            treatmentDetailsList.SeparatorColor = Colors.Black;
            treatmentDetailsList.SeparatorInset = new UIEdgeInsets(350, 5, 350, 5);
            treatmentDetailsList.UserInteractionEnabled = false;
            treatmentDetailsList.BackgroundColor = Colors.Clear;
            detailsScrollContainer.AddSubview(treatmentDetailsList);



            //detailsScrollContainer.Scrolled += ScrollEvent;
            treatmentDetailsList.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
            treatmentDetailsList.ClipsToBounds = false;

            if (Constants.IsPhone())
            {

                treatmentDetailSource = new MvxConfirmationTableViewSource(_model, treatmentDetailsList, "ClaimConfirmationTableViewCell", typeof(ClaimConfirmationTableViewCell));


            }
            else
            {

                treatmentDetailSource = new MvxConfirmationTableViewSource(_model, treatmentDetailsList, "ClaimConfirmationIPadTableViewCell", typeof(ClaimConfirmationIPadTableViewCell));

                if (_model.Claim.IsTypeOfTreatmentVisible)
                {
                    typeTitleLabel = new UILabel();
                    typeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    typeTitleLabel.TextAlignment = UITextAlignment.Left;
                    typeTitleLabel.BackgroundColor = Colors.Clear;
                    typeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    typeTitleLabel.Lines = 0;
                    typeTitleLabel.Text = "typeOfTreatmentTitle".tr();
                    typeTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(typeTitleLabel);
                    labelsCount++;
                }

                //TODO: what to bind to here?
                if (_model.Claim.IsTreatmentDurationVisible)
                {
                    lengthTitleLabel = new UILabel();
                    lengthTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    lengthTitleLabel.TextAlignment = UITextAlignment.Left;
                    lengthTitleLabel.BackgroundColor = Colors.Clear;
                    lengthTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    lengthTitleLabel.Lines = 0;
                    lengthTitleLabel.Text = "lengthOfTreatmentTitle".tr();
                    lengthTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(lengthTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsItemDescriptionVisible)
                {
                    itemDescriptionTitleLabel = new UILabel();
                    itemDescriptionTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    itemDescriptionTitleLabel.TextAlignment = UITextAlignment.Left;
                    itemDescriptionTitleLabel.BackgroundColor = Colors.Clear;
                    itemDescriptionTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    itemDescriptionTitleLabel.Lines = 0;
                    itemDescriptionTitleLabel.Text = "itemDescription".tr();
                    itemDescriptionTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(itemDescriptionTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsTreatementDateVisible)
                {

                    dateOfTreatmentTitleLabel = new UILabel();
                    dateOfTreatmentTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    dateOfTreatmentTitleLabel.TextAlignment = UITextAlignment.Left;
                    dateOfTreatmentTitleLabel.BackgroundColor = Colors.Clear;
                    dateOfTreatmentTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    dateOfTreatmentTitleLabel.Lines = 0;
                    dateOfTreatmentTitleLabel.Text = "dateOfTreatmentTitle".tr();
                    dateOfTreatmentTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(dateOfTreatmentTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsDateOfMonthlyTreatmentVisible)
                {

                    dateOfMonthlyTreatmentTitleLabel = new UILabel();
                    dateOfMonthlyTreatmentTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    dateOfMonthlyTreatmentTitleLabel.TextAlignment = UITextAlignment.Left;
                    dateOfMonthlyTreatmentTitleLabel.BackgroundColor = Colors.Clear;
                    dateOfMonthlyTreatmentTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    dateOfMonthlyTreatmentTitleLabel.Lines = 0;
                    dateOfMonthlyTreatmentTitleLabel.Text = "dateOfMonthlyTreatmentTitle".tr();
                    dateOfMonthlyTreatmentTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(dateOfMonthlyTreatmentTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsDateOfPurchaseVisible)
                {

                    dateOfPurchaseTitleLabel = new UILabel();
                    dateOfPurchaseTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    dateOfPurchaseTitleLabel.TextAlignment = UITextAlignment.Left;
                    dateOfPurchaseTitleLabel.BackgroundColor = Colors.Clear;
                    dateOfPurchaseTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    dateOfPurchaseTitleLabel.Lines = 0;
                    dateOfPurchaseTitleLabel.Text = "dateOfPurchaseService".tr();
                    dateOfPurchaseTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(dateOfPurchaseTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsPickupDateVisible)
                {

                    dateOfPickupTitleLabel = new UILabel();
                    dateOfPickupTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    dateOfPickupTitleLabel.TextAlignment = UITextAlignment.Left;
                    dateOfPickupTitleLabel.BackgroundColor = Colors.Clear;
                    dateOfPickupTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    dateOfPickupTitleLabel.Lines = 0;
                    dateOfPickupTitleLabel.Text = "pickupDate".tr();
                    dateOfPickupTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(dateOfPickupTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsQuantityVisible)
                {

                    quantityTitleLabel = new UILabel();
                    quantityTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    quantityTitleLabel.TextAlignment = UITextAlignment.Left;
                    quantityTitleLabel.BackgroundColor = Colors.Clear;
                    quantityTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    quantityTitleLabel.Lines = 0;
                    quantityTitleLabel.Text = "quantity".tr();
                    quantityTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(quantityTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsTreatmentAmountVisible)
                {
                    amountTitleLabel = new UILabel();
                    amountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    amountTitleLabel.TextAlignment = UITextAlignment.Left;
                    amountTitleLabel.BackgroundColor = Colors.Clear;
                    amountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    amountTitleLabel.Lines = 0;
                    amountTitleLabel.Text = "totalAmountOfVisit".tr();
                    amountTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(amountTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsTotalAmountChargedForMIVisible)
                {
                    totalAmountMedicalTitleLabel = new UILabel();
                    totalAmountMedicalTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    totalAmountMedicalTitleLabel.TextAlignment = UITextAlignment.Left;
                    totalAmountMedicalTitleLabel.BackgroundColor = Colors.Clear;
                    totalAmountMedicalTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    totalAmountMedicalTitleLabel.Lines = 0;
                    totalAmountMedicalTitleLabel.Text = "totalAmountMedicalItems".tr();
                    totalAmountMedicalTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(totalAmountMedicalTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsOrthodonticMonthlyFeeVisible)
                {
                    orthFeeLabel = new UILabel();
                    orthFeeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    orthFeeLabel.TextAlignment = UITextAlignment.Left;
                    orthFeeLabel.BackgroundColor = Colors.Clear;
                    orthFeeLabel.TextColor = Colors.DARK_GREY_COLOR;
                    orthFeeLabel.Lines = 0;
                    orthFeeLabel.Text = "omf".tr();
                    orthFeeLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(orthFeeLabel);
                    labelsCount++;
                }


                if (_model.Claim.IsAlternateCarrierPaymentVisible)
                {
                    altCarrierTitleLabel = new UILabel();
                    altCarrierTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    altCarrierTitleLabel.TextAlignment = UITextAlignment.Left;
                    altCarrierTitleLabel.BackgroundColor = Colors.Clear;
                    altCarrierTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    altCarrierTitleLabel.Lines = 0;
                    altCarrierTitleLabel.Text = "amountPaidAlt".tr();
                    altCarrierTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(altCarrierTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsGSTHSTIncludedInTotalVisible)
                {
                    gstLabel = new UILabel();
                    gstLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    gstLabel.TextAlignment = UITextAlignment.Left;
                    gstLabel.BackgroundColor = Colors.Clear;
                    gstLabel.TextColor = Colors.DARK_GREY_COLOR;
                    gstLabel.Lines = 0;
                    gstLabel.Text = "gstIncluded".tr();
                    gstLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(gstLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsPSTIncludedInTotalVisible)
                {
                    pstLabel = new UILabel();
                    pstLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    pstLabel.TextAlignment = UITextAlignment.Left;
                    pstLabel.BackgroundColor = Colors.Clear;
                    pstLabel.TextColor = Colors.DARK_GREY_COLOR;
                    pstLabel.Lines = 0;
                    pstLabel.Text = "pstIncluded".tr();
                    pstLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(pstLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsDateOfReferralVisibleForTreatment)
                {
                    dateOfReferralTitleLabel = new UILabel();
                    dateOfReferralTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    dateOfReferralTitleLabel.TextAlignment = UITextAlignment.Left;
                    dateOfReferralTitleLabel.BackgroundColor = Colors.Clear;
                    dateOfReferralTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    dateOfReferralTitleLabel.Lines = 0;
                    dateOfReferralTitleLabel.Text = "referralDate".tr();
                    dateOfReferralTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(dateOfReferralTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsTypeOfMedicalProfessionalVisibleForTreatment)
                {
                    typeOfProfessionalTitleLabel = new UILabel();
                    typeOfProfessionalTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    typeOfProfessionalTitleLabel.TextAlignment = UITextAlignment.Left;
                    typeOfProfessionalTitleLabel.BackgroundColor = Colors.Clear;
                    typeOfProfessionalTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    typeOfProfessionalTitleLabel.Lines = 0;
                    typeOfProfessionalTitleLabel.Text = "typeOfProfessional".tr();
                    typeOfProfessionalTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(typeOfProfessionalTitleLabel);
                    labelsCount++;
                }

                //VISION CHANGES

                if (_model.Claim.IsTypeOfEyewearVisible)
                {
                    eyewearTypeTitleLabel = new UILabel();
                    eyewearTypeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    eyewearTypeTitleLabel.TextAlignment = UITextAlignment.Left;
                    eyewearTypeTitleLabel.BackgroundColor = Colors.Clear;
                    eyewearTypeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    eyewearTypeTitleLabel.Lines = 0;
                    eyewearTypeTitleLabel.Text = "typeOfEyewearConf".tr();
                    eyewearTypeTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(eyewearTypeTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsTypeOfLensVisible)
                {
                    lensTypeTitleLabel = new UILabel();
                    lensTypeTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    lensTypeTitleLabel.TextAlignment = UITextAlignment.Left;
                    lensTypeTitleLabel.BackgroundColor = Colors.Clear;
                    lensTypeTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    lensTypeTitleLabel.Lines = 0;
                    lensTypeTitleLabel.Text = "typeOfLensConf".tr();
                    lensTypeTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(lensTypeTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsFrameAmountVisible)
                {
                    frameAmountTitleLabel = new UILabel();
                    frameAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    frameAmountTitleLabel.TextAlignment = UITextAlignment.Left;
                    frameAmountTitleLabel.BackgroundColor = Colors.Clear;
                    frameAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    frameAmountTitleLabel.Lines = 0;
                    frameAmountTitleLabel.Text = "frameAmount".tr();
                    frameAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(frameAmountTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsFeeAmountVisible)
                {
                    feeAmountTitleLabel = new UILabel();
                    feeAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    feeAmountTitleLabel.TextAlignment = UITextAlignment.Left;
                    feeAmountTitleLabel.BackgroundColor = Colors.Clear;
                    feeAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    feeAmountTitleLabel.Lines = 0;
                    feeAmountTitleLabel.Text = "feeAmount".tr();
                    feeAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(feeAmountTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsEyeglassLensesAmountVisible)
                {
                    eyeglassLensAmountTitleLabel = new UILabel();
                    eyeglassLensAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    eyeglassLensAmountTitleLabel.TextAlignment = UITextAlignment.Left;
                    eyeglassLensAmountTitleLabel.BackgroundColor = Colors.Clear;
                    eyeglassLensAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    eyeglassLensAmountTitleLabel.Lines = 0;
                    eyeglassLensAmountTitleLabel.Text = "lensAmount".tr();
                    eyeglassLensAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(eyeglassLensAmountTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsTotalAmountChargedVisible)
                {
                    totalAmountTitleLabel = new UILabel();
                    totalAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                    totalAmountTitleLabel.TextAlignment = UITextAlignment.Left;
                    totalAmountTitleLabel.BackgroundColor = Colors.Clear;
                    totalAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                    totalAmountTitleLabel.Lines = 0;
                    totalAmountTitleLabel.Text = "totalAmountCharged".tr();
                    totalAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
                    detailsScrollContainer.AddSubview(totalAmountTitleLabel);
                    labelsCount++;
                }

                if (_model.Claim.IsPrescriptionDetailsVisible)
                {

                    if (_model.Claim.IsRightSphereVisible)
                    {
                        rightSphereTitleLabel = new UILabel();
                        rightSphereTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        rightSphereTitleLabel.TextAlignment = UITextAlignment.Left;
                        rightSphereTitleLabel.BackgroundColor = Colors.Clear;
                        rightSphereTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        rightSphereTitleLabel.Lines = 0;
                        rightSphereTitleLabel.Text = "rightSphere".tr();
                        rightSphereTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(rightSphereTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsLeftSphereVisible)
                    {
                        leftSphereTitleLabel = new UILabel();
                        leftSphereTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        leftSphereTitleLabel.TextAlignment = UITextAlignment.Left;
                        leftSphereTitleLabel.BackgroundColor = Colors.Clear;
                        leftSphereTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        leftSphereTitleLabel.Lines = 0;
                        leftSphereTitleLabel.Text = "leftSphere".tr();
                        leftSphereTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(leftSphereTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsRightCylinderVisible)
                    {
                        rightCylinderTitleLabel = new UILabel();
                        rightCylinderTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        rightCylinderTitleLabel.TextAlignment = UITextAlignment.Left;
                        rightCylinderTitleLabel.BackgroundColor = Colors.Clear;
                        rightCylinderTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        rightCylinderTitleLabel.Lines = 0;
                        rightCylinderTitleLabel.Text = "rightCylinder".tr();
                        rightCylinderTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(rightCylinderTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsLeftCylinderVisible)
                    {
                        leftCylinderTitleLabel = new UILabel();
                        leftCylinderTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        leftCylinderTitleLabel.TextAlignment = UITextAlignment.Left;
                        leftCylinderTitleLabel.BackgroundColor = Colors.Clear;
                        leftCylinderTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        leftCylinderTitleLabel.Lines = 0;
                        leftCylinderTitleLabel.Text = "leftCylinder".tr();
                        leftCylinderTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(leftCylinderTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsRightAxisVisible)
                    {
                        rightAxisTitleLabel = new UILabel();
                        rightAxisTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        rightAxisTitleLabel.TextAlignment = UITextAlignment.Left;
                        rightAxisTitleLabel.BackgroundColor = Colors.Clear;
                        rightAxisTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        rightAxisTitleLabel.Lines = 0;
                        rightAxisTitleLabel.Text = "rightAxis".tr();
                        rightAxisTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(rightAxisTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsLeftAxisVisible)
                    {
                        leftAxisTitleLabel = new UILabel();
                        leftAxisTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        leftAxisTitleLabel.TextAlignment = UITextAlignment.Left;
                        leftAxisTitleLabel.BackgroundColor = Colors.Clear;
                        leftAxisTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        leftAxisTitleLabel.Lines = 0;
                        leftAxisTitleLabel.Text = "leftAxis".tr();
                        leftAxisTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(leftAxisTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsRightPrismVisible)
                    {
                        rightPrismTitleLabel = new UILabel();
                        rightPrismTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        rightPrismTitleLabel.TextAlignment = UITextAlignment.Left;
                        rightPrismTitleLabel.BackgroundColor = Colors.Clear;
                        rightPrismTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        rightPrismTitleLabel.Lines = 0;
                        rightPrismTitleLabel.Text = "rightPrism".tr();
                        rightPrismTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(rightPrismTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsLeftPrismVisible)
                    {
                        leftPrismTitleLabel = new UILabel();
                        leftPrismTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        leftPrismTitleLabel.TextAlignment = UITextAlignment.Left;
                        leftPrismTitleLabel.BackgroundColor = Colors.Clear;
                        leftPrismTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        leftPrismTitleLabel.Lines = 0;
                        leftPrismTitleLabel.Text = "leftPrism".tr();
                        leftPrismTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(leftPrismTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsRightBifocalVisible)
                    {
                        rightBifocalsTitleLabel = new UILabel();
                        rightBifocalsTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        rightBifocalsTitleLabel.TextAlignment = UITextAlignment.Left;
                        rightBifocalsTitleLabel.BackgroundColor = Colors.Clear;
                        rightBifocalsTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        rightBifocalsTitleLabel.Lines = 0;
                        rightBifocalsTitleLabel.Text = "rightBifocal".tr();
                        rightBifocalsTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(rightBifocalsTitleLabel);
                        labelsCount++;
                    }

                    if (_model.Claim.IsLeftBifocalVisible)
                    {
                        leftBifocalsTitleLabel = new UILabel();
                        leftBifocalsTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
                        leftBifocalsTitleLabel.TextAlignment = UITextAlignment.Left;
                        leftBifocalsTitleLabel.BackgroundColor = Colors.Clear;
                        leftBifocalsTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
                        leftBifocalsTitleLabel.Lines = 0;
                        leftBifocalsTitleLabel.Text = "leftBifocal".tr();
                        leftBifocalsTitleLabel.AdjustsFontSizeToFitWidth = true;
                        detailsScrollContainer.AddSubview(leftBifocalsTitleLabel);
                        labelsCount++;
                    }
                }
            }

            treatmentDetailsList.Source = treatmentDetailSource;
            participantList = new ClaimTreatmentDetailsTitleAndList(this, "choosePlanParticipantLower".tr());
            scrollContainer.AddSubview(participantList);

            DismmssVCTableViewSource participantSource = new DismmssVCTableViewSource(participantList.popoverController, participantList.listController.tableView, "ClaimParticipantGSCTableCell", typeof(ClaimParticipantGSCTableCell));
            participantList.listController.tableView.Source = participantSource;

            submittedToGSC = new UILabel();
            submittedToGSC.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            submittedToGSC.TextColor = Colors.DARK_GREY_COLOR;
            submittedToGSC.TextAlignment = UITextAlignment.Left;
            submittedToGSC.BackgroundColor = Colors.Clear;
            submittedToGSC.LineBreakMode = UILineBreakMode.WordWrap;
            submittedToGSC.Lines = 0;
            submittedToGSC.Text = "submittedToGSC".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            scrollContainer.AddSubview(submittedToGSC);

            submitButton = new GSButton();
            submitButton.SetTitle("submitClaim".tr(), UIControlState.Normal);
            scrollContainer.AddSubview(submitButton);

            setupErrorAlerts();

            var set = this.CreateBindingSet<ClaimSubmissionConfirmationView1, Core.ViewModels.ClaimSubmissionConfirmationViewModel>();
            set.Bind(treatmentDetailSource).To(vm => vm.Claim.TreatmentDetails);
            set.Bind(participantSource).To(vm => vm.Participants);
            set.Bind(participantSource).For(s => s.SelectedItem).To(vm => vm.SelectedParticipant);
            set.Bind(participantList.detailsLabel).To(vm => vm.SelectedParticipant.FullNameWithID);
            set.Bind(participantList).For(v => v.Hidden).To(vm => vm.IsParticipantsListVisible).WithConversion("BoolOpposite");
            //set.Bind(gscNumberField.textField).To(vm => vm.Claim.OtherGSCNumber);
            //set.Bind(gscNumberField).For(v => v.Hidden).To(vm => vm.IsParticipantsListVisible).WithConversion("BoolOpposite");
            set.Bind(this.submitButton).To(vm => vm.SubmitClaimCommand);
            set.Apply();

            if (!Constants.IsPhone())
            {
                ((GSCBaseView)View).subscribeToBusyIndicator();
                if (detailsScrollContainer != null)
                {
                    StartFlashDelay();
                }
            }
        }

        private void ScrollEvent(object sender, System.EventArgs e)
        {
            treatmentDetailsList.ReloadData();
            treatmentDetailsList.SetNeedsLayout();
        }

        async void StartFlashDelay()
        {
            await Task.Delay(500);
            detailsScrollContainer.FlashScrollIndicators();
        }

        void HandleCancelButton(object sender, EventArgs e)
        {
            int backTo = 2;

            if (base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo].GetType() == typeof(ClaimSubmitTermsAndConditionsView))
            {
                backTo++;
            }
            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo], true);
        }

        protected void setupErrorAlerts()
        {
            _model.OnInvalidClaim += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "error".tr(), null, "ok".tr(), null);
                    _error.Show();
                });

            };

            _model.OnInvalidGSCNumber += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "invalidGSCNumber".FormatWithBrandKeywords(LocalizableBrand.GSC), null, "ok".tr(), null);
                    _error.Show();
                });

            };

            _model.OnNoMatchedDependent += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "errorNoMatchedDependant".FormatWithBrandKeywords(LocalizableBrand.GSC), null, "ok".tr(), null);
                    _error.Show();
                });

            };

            _model.OnInvalidSecondaryPlanNumber += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "errorSecondaryClaim".tr(), null, "ok".tr(), null);
                    _error.Show();
                });

            };

            _model.OnInvalidOnlineClaim += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "errorOnlineSubmision".tr(), null, "ok".tr(), null);
                    _error.Show();
                });

            };

            _model.OnMultipleMatch += (object sender, EventArgs e) =>
            {
                InvokeOnMainThread(() =>
                {
                    UIAlertView _error = new UIAlertView("", "errorMultipleMatchDropDown".FormatWithBrandKeywords(LocalizableBrand.GSC), null, "ok".tr(), null);
                    _error.Show();
                    redrawCount = 0;
                    participantList.showError("errorMultipleMatchDropDown".FormatWithBrandKeywords(LocalizableBrand.GSC));
                    this.View.SetNeedsLayout();
                });

            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (Constants.IsPhone())
            {
                ((GSCBaseView)View).subscribeToBusyIndicator();
            }

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (detailsScrollContainer != null)
            {
                detailsScrollContainer.FlashScrollIndicators();
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate(toInterfaceOrientation, duration);

            redrawCount = 0;
            View.SetNeedsLayout();
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            redrawCount = 0;
            View.SetNeedsLayout();
        }

        int redrawCount = 0;
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            // float ViewContainerWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float ViewContainerHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float yPos = ViewContentYPositionPadding;

            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float scrollContainerPadding = 70;

            string currentCulture = System.Globalization.CultureInfo.CurrentUICulture.Name.ToString();

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight + itemPadding);

            if (!Constants.IsPhone())
            {
                yPos = 85;
            }

            if (currentCulture.Contains("fr") || currentCulture.Contains("Fr"))
            {
                #region layout for fr

                planInformationLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)planInformationLabel.Frame.Height);
                planInformationLabel.SizeToFit();
                yPos += ((float)planInformationLabel.Frame.Height / 2) + itemPadding;

                gscIDResult.Frame = new CGRect(0, yPos, ViewContainerWidth, gscIDResult.ComponentHeight);
                yPos += (gscIDResult.ComponentHeight / 2) + 8;
                participantNameResult.Frame = new CGRect(0, yPos, ViewContainerWidth, participantNameResult.ComponentHeight);
                yPos += participantNameResult.ComponentHeight / 2;
                providerResult.Frame = new CGRect(0, yPos, ViewContainerWidth, providerResult.ComponentHeight);
                yPos += providerResult.ComponentHeight + itemPadding;

                claimDetailsLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)claimDetailsLabel.Frame.Height);
                claimDetailsLabel.SizeToFit();
                yPos += (float)claimDetailsLabel.Frame.Height / 2;


                anotherPlanCoverageResult.Frame = new CGRect(0, yPos, ViewContainerWidth, anotherPlanCoverageResult.ComponentHeight);
                yPos += (anotherPlanCoverageResult.ComponentHeight) - itemPadding * 2;
                if (_model.Claim.IsIsOtherCoverageWithGSCVisible)
                {
                    otherBenefitsWithGSCResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherBenefitsWithGSCResult.ComponentHeight);
                    yPos += otherBenefitsWithGSCResult.ComponentHeight;
                }

                if (_model.Claim.IsHasClaimBeenSubmittedToOtherBenefitPlanVisible)
                {
                    otherBenefitsSubmittedResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherBenefitsSubmittedResult.ComponentHeight);
                    yPos += otherBenefitsSubmittedResult.ComponentHeight;
                }
                if (_model.Claim.IsPayAnyUnpaidBalanceThroughOtherGSCPlanVisible)
                {
                    otherBenefitsPayBalanceThroughOtherGSCResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherBenefitsPayBalanceThroughOtherGSCResult.ComponentHeight);
                    yPos += otherBenefitsPayBalanceThroughOtherGSCResult.ComponentHeight;
                }
                if (_model.Claim.IsOtherGSCNumberVisible)
                {
                    otherGSCNumberResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherGSCNumberResult.ComponentHeight);
                    yPos += otherGSCNumberResult.ComponentHeight;
                }

                if (_model.Claim.IsPayUnderHCSAVisible)
                {
                    hcsaResult.Frame = new CGRect(0, yPos, ViewContainerWidth, hcsaResult.ComponentHeight);
                    yPos += hcsaResult.ComponentHeight + itemPadding;
                }

                if (_model.Claim.IsIsGSTHSTIncludedVisible)
                {
                    gstResult.Frame = new CGRect(0, yPos, ViewContainerWidth, gstResult.ComponentHeight);
                    yPos += gstResult.ComponentHeight + itemPadding;
                }

                motorVehicleAccidentResult.Frame = new CGRect(0, yPos, ViewContainerWidth, motorVehicleAccidentResult.ComponentHeight);
                yPos += motorVehicleAccidentResult.ComponentHeight;
                if (_model.Claim.IsDateOfMotorVehicleAccidentVisible)
                {
                    motorVehicleDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, motorVehicleDateResult.ComponentHeight);
                    yPos += motorVehicleDateResult.ComponentHeight;
                }

                workRelatedInuryResult.Frame = new CGRect(0, yPos, ViewContainerWidth, workRelatedInuryResult.ComponentHeight);
                yPos += workRelatedInuryResult.ComponentHeight;
                if (_model.Claim.IsDateOfWorkRelatedInjuryVisible)
                {
                    workInjuryDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, workInjuryDateResult.ComponentHeight);
                    yPos += workInjuryDateResult.ComponentHeight;
                }
                if (_model.Claim.IsWorkRelatedInjuryCaseNumberVisible)
                {
                    workInjuryCaseNumberResult.Frame = new CGRect(0, yPos, ViewContainerWidth, workInjuryCaseNumberResult.ComponentHeight);
                    yPos += workInjuryCaseNumberResult.ComponentHeight;
                }

                if (_model.Claim.IsHasReferralBeenPreviouslySubmittedVisible)
                {
                    referralPreviouslySubmittedResult.Frame = new CGRect(0, yPos, ViewContainerWidth, referralPreviouslySubmittedResult.ComponentHeight);
                    yPos += referralPreviouslySubmittedResult.ComponentHeight;
                }

                if (_model.Claim.IsDateOfReferralVisible)
                {
                    dateofReferralResult.Frame = new CGRect(0, yPos, ViewContainerWidth, dateofReferralResult.ComponentHeight);
                    yPos += dateofReferralResult.ComponentHeight;
                    System.Console.WriteLine(dateofReferralResult.ComponentHeight + " date");
                }

                if (_model.Claim.IsTypeOfMedicalProfessionalVisible)
                {
                    typeOfMedicalProfessionalResult.Frame = new CGRect(0, yPos, ViewContainerWidth, typeOfMedicalProfessionalResult.ComponentHeight);
                    yPos += typeOfMedicalProfessionalResult.ComponentHeight;
                    System.Console.WriteLine(typeOfMedicalProfessionalResult.ComponentHeight + " notDate");
                }

                if (_model.Claim.IsIsMedicalItemForSportsOnlyVisible)
                {
                    sportPurposesResult.Frame = new CGRect(0, yPos, ViewContainerWidth, sportPurposesResult.ComponentHeight);
                    yPos += sportPurposesResult.ComponentHeight;
                }
                #endregion
            }
            else
            {
                #region layout for en

                planInformationLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)planInformationLabel.Frame.Height);
                planInformationLabel.SizeToFit();
                yPos += ((float)planInformationLabel.Frame.Height / 2) + itemPadding;

                gscIDResult.Frame = new CGRect(0, yPos, ViewContainerWidth, gscIDResult.ComponentHeight);
                yPos += gscIDResult.ComponentHeight / 2;
                participantNameResult.Frame = new CGRect(0, yPos, ViewContainerWidth, participantNameResult.ComponentHeight);
                yPos += participantNameResult.ComponentHeight / 2;
                providerResult.Frame = new CGRect(0, yPos, ViewContainerWidth, providerResult.ComponentHeight);
                yPos += providerResult.ComponentHeight + itemPadding;

                claimDetailsLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)claimDetailsLabel.Frame.Height);
                claimDetailsLabel.SizeToFit();
                yPos += (float)claimDetailsLabel.Frame.Height / 2;

                anotherPlanCoverageResult.Frame = new CGRect(0, yPos, ViewContainerWidth, anotherPlanCoverageResult.ComponentHeight);
                yPos += (anotherPlanCoverageResult.ComponentHeight) - itemPadding * 2;
                if (_model.Claim.IsIsOtherCoverageWithGSCVisible)
                {
                    otherBenefitsWithGSCResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherBenefitsWithGSCResult.ComponentHeight);
                    yPos += otherBenefitsWithGSCResult.ComponentHeight - (itemPadding * 2);
                }
                if (_model.Claim.IsHasClaimBeenSubmittedToOtherBenefitPlanVisible)
                {
                    otherBenefitsSubmittedResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherBenefitsSubmittedResult.ComponentHeight);
                    yPos += otherBenefitsSubmittedResult.ComponentHeight;
                }
                if (_model.Claim.IsPayAnyUnpaidBalanceThroughOtherGSCPlanVisible)
                {
                    otherBenefitsPayBalanceThroughOtherGSCResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherBenefitsPayBalanceThroughOtherGSCResult.ComponentHeight);
                    yPos += otherBenefitsPayBalanceThroughOtherGSCResult.ComponentHeight;
                }
                if (_model.Claim.IsOtherGSCNumberVisible)
                {
                    otherGSCNumberResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherGSCNumberResult.ComponentHeight);
                    yPos += otherGSCNumberResult.ComponentHeight;
                }

                if (_model.Claim.IsPayUnderHCSAVisible)
                {
                    hcsaResult.Frame = new CGRect(0, yPos, ViewContainerWidth, hcsaResult.ComponentHeight);
                    yPos += (hcsaResult.ComponentHeight - itemPadding) + 5;
                }

                if (_model.Claim.IsIsGSTHSTIncludedVisible)
                {
                    gstResult.Frame = new CGRect(0, yPos, ViewContainerWidth, gstResult.ComponentHeight);
                    yPos += (gstResult.ComponentHeight / 2) + itemPadding * 2;
                }

                motorVehicleAccidentResult.Frame = new CGRect(0, yPos, ViewContainerWidth, motorVehicleAccidentResult.ComponentHeight);
                yPos += (motorVehicleAccidentResult.ComponentHeight / 2) + itemPadding * 3;
                if (_model.Claim.IsDateOfMotorVehicleAccidentVisible)
                {
                    motorVehicleDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, motorVehicleDateResult.ComponentHeight);
                    yPos += (motorVehicleDateResult.ComponentHeight / 2) + itemPadding * (float)1.5;
                }

                workRelatedInuryResult.Frame = new CGRect(0, yPos, ViewContainerWidth, workRelatedInuryResult.ComponentHeight);
                yPos += (workRelatedInuryResult.ComponentHeight / 2) + itemPadding * 3;
                if (_model.Claim.IsDateOfWorkRelatedInjuryVisible)
                {
                    workInjuryDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, workInjuryDateResult.ComponentHeight);
                    yPos += (workInjuryDateResult.ComponentHeight / 2) + itemPadding * (float)1.5;
                }
                if (_model.Claim.IsWorkRelatedInjuryCaseNumberVisible)
                {
                    workInjuryCaseNumberResult.Frame = new CGRect(0, yPos, ViewContainerWidth, workInjuryCaseNumberResult.ComponentHeight);
                    yPos += (workInjuryCaseNumberResult.ComponentHeight / 2) + itemPadding * (float)1.5;
                }

                if (_model.Claim.IsHasReferralBeenPreviouslySubmittedVisible)
                {
                    referralPreviouslySubmittedResult.Frame = new CGRect(0, yPos, ViewContainerWidth, referralPreviouslySubmittedResult.ComponentHeight);
                    yPos += (referralPreviouslySubmittedResult.ComponentHeight / 2) + itemPadding * 3;
                }

                if (_model.Claim.IsDateOfReferralVisible)
                {
                    dateofReferralResult.Frame = new CGRect(0, yPos, ViewContainerWidth, dateofReferralResult.ComponentHeight);
                    yPos += (dateofReferralResult.ComponentHeight / 2) + itemPadding * 2;
                    System.Console.WriteLine(dateofReferralResult.ComponentHeight + " date");
                }

                if (_model.Claim.IsTypeOfMedicalProfessionalVisible)
                {
                    typeOfMedicalProfessionalResult.Frame = new CGRect(0, yPos, ViewContainerWidth, typeOfMedicalProfessionalResult.ComponentHeight);
                    yPos += (typeOfMedicalProfessionalResult.ComponentHeight / 2) + itemPadding * 2;
                    System.Console.WriteLine(typeOfMedicalProfessionalResult.ComponentHeight + " notDate");
                }

                if (_model.Claim.IsIsMedicalItemForSportsOnlyVisible)
                {
                    sportPurposesResult.Frame = new CGRect(0, yPos, ViewContainerWidth, sportPurposesResult.ComponentHeight);
                    yPos += (sportPurposesResult.ComponentHeight / 2) + itemPadding * 2;
                }
                #endregion
            }

            treatmentDetailsLabel.Frame = new CGRect(sidePadding, yPos + itemPadding * 2, ViewContainerWidth - sidePadding * 2, (float)treatmentDetailsLabel.Frame.Height);
            treatmentDetailsLabel.SizeToFit();

            yPos += (float)treatmentDetailsLabel.Frame.Height + itemPadding;

            //          switch (_model.Claim.Type.ID)
            //          {
            //          case "ACUPUNCTURE":
            //          case "CHIROPODY":
            //          case "CHIRO":
            //          case "PODIATRY":
            //          case "PHYSIO":
            //              isNoTreatmentType = false;
            //              break;
            //          case "MI":
            //          case "ORTHODONTIC":
            //          case "CONTACTS":
            //          case "GLASSES":
            //          case "EYEEXAM":
            //              isNoTreatmentType = true;
            //              break;
            //          case "SPEECH":
            //          case "PSYCHOLOGY":
            //          case "MASSAGE":
            //          case "NATUROPATHY":
            //              isLengthType = true;
            //              isNoTreatmentType = false;
            //              break;
            //          }

            float detailsScrollWidth = ViewContainerWidth;
            float innerYPos = 0;

            if (!Constants.IsPhone())
            {

                float fieldPadding = Constants.CLAIMS_RESULTS_IPAD_PADDING;
                float sectionWidth = 150;
                float rowHeight = Constants.CLAIMS_RESULTS_IPAD_HEIGHT;
                float xPos = fieldPadding;

                if (typeTitleLabel != null)
                {
                    typeTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)typeTitleLabel.Frame.Height / 2, sectionWidth, (float)typeTitleLabel.Frame.Height);
                    typeTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (itemDescriptionTitleLabel != null)
                {
                    itemDescriptionTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)itemDescriptionTitleLabel.Frame.Height / 2, sectionWidth, (float)itemDescriptionTitleLabel.Frame.Height);
                    itemDescriptionTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (lengthTitleLabel != null)
                {
                    lengthTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)lengthTitleLabel.Frame.Height / 2, sectionWidth, (float)lengthTitleLabel.Frame.Height);
                    lengthTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (dateOfTreatmentTitleLabel != null)
                {
                    dateOfTreatmentTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfTreatmentTitleLabel.Frame.Height / 2, sectionWidth, (float)dateOfTreatmentTitleLabel.Frame.Height);
                    dateOfTreatmentTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (dateOfMonthlyTreatmentTitleLabel != null)
                {
                    dateOfMonthlyTreatmentTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfMonthlyTreatmentTitleLabel.Frame.Height / 2, sectionWidth, (float)dateOfMonthlyTreatmentTitleLabel.Frame.Height);
                    dateOfMonthlyTreatmentTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (dateOfPurchaseTitleLabel != null)
                {
                    dateOfPurchaseTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfPurchaseTitleLabel.Frame.Height / 2, sectionWidth, (float)dateOfPurchaseTitleLabel.Frame.Height);
                    dateOfPurchaseTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (dateOfPickupTitleLabel != null)
                {
                    dateOfPickupTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfPickupTitleLabel.Frame.Height / 2, sectionWidth, (float)dateOfPickupTitleLabel.Frame.Height);
                    dateOfPickupTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (quantityTitleLabel != null)
                {
                    quantityTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)quantityTitleLabel.Frame.Height / 2, sectionWidth, (float)quantityTitleLabel.Frame.Height);
                    quantityTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (amountTitleLabel != null)
                {
                    amountTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)amountTitleLabel.Frame.Height / 2, sectionWidth, (float)amountTitleLabel.Frame.Height);
                    amountTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (totalAmountMedicalTitleLabel != null)
                {
                    totalAmountMedicalTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)totalAmountMedicalTitleLabel.Frame.Height / 2, sectionWidth, (float)totalAmountMedicalTitleLabel.Frame.Height);
                    totalAmountMedicalTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (orthFeeLabel != null)
                {
                    orthFeeLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)orthFeeLabel.Frame.Height / 2, sectionWidth, (float)orthFeeLabel.Frame.Height);
                    orthFeeLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (altCarrierTitleLabel != null)
                {
                    altCarrierTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)altCarrierTitleLabel.Frame.Height / 2, sectionWidth, (float)altCarrierTitleLabel.Frame.Height);
                    altCarrierTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (gstLabel != null)
                {
                    gstLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)gstLabel.Frame.Height / 2, sectionWidth, (float)gstLabel.Frame.Height);
                    gstLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (pstLabel != null)
                {
                    pstLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)pstLabel.Frame.Height / 2, sectionWidth, (float)pstLabel.Frame.Height);
                    pstLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (dateOfReferralTitleLabel != null)
                {
                    dateOfReferralTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)dateOfReferralTitleLabel.Frame.Height / 2, sectionWidth, (float)dateOfReferralTitleLabel.Frame.Height);
                    dateOfReferralTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (typeOfProfessionalTitleLabel != null)
                {
                    typeOfProfessionalTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)typeOfProfessionalTitleLabel.Frame.Height / 2, sectionWidth, (float)typeOfProfessionalTitleLabel.Frame.Height);
                    typeOfProfessionalTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (eyewearTypeTitleLabel != null)
                {
                    eyewearTypeTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)eyewearTypeTitleLabel.Frame.Height / 2, sectionWidth, (float)eyewearTypeTitleLabel.Frame.Height);
                    eyewearTypeTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (lensTypeTitleLabel != null)
                {
                    lensTypeTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)lensTypeTitleLabel.Frame.Height / 2, sectionWidth, (float)lensTypeTitleLabel.Frame.Height);
                    lensTypeTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (frameAmountTitleLabel != null)
                {
                    frameAmountTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)frameAmountTitleLabel.Frame.Height / 2, sectionWidth, (float)frameAmountTitleLabel.Frame.Height);
                    frameAmountTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (feeAmountTitleLabel != null)
                {
                    feeAmountTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)feeAmountTitleLabel.Frame.Height / 2, sectionWidth, (float)feeAmountTitleLabel.Frame.Height);
                    feeAmountTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (eyeglassLensAmountTitleLabel != null)
                {
                    eyeglassLensAmountTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)eyeglassLensAmountTitleLabel.Frame.Height / 2, sectionWidth, (float)eyeglassLensAmountTitleLabel.Frame.Height);
                    eyeglassLensAmountTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (totalAmountTitleLabel != null)
                {
                    totalAmountTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)totalAmountTitleLabel.Frame.Height / 2, sectionWidth, (float)totalAmountTitleLabel.Frame.Height);
                    totalAmountTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (rightSphereTitleLabel != null)
                {
                    rightSphereTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightSphereTitleLabel.Frame.Height / 2, sectionWidth, (float)rightSphereTitleLabel.Frame.Height);
                    rightSphereTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (leftSphereTitleLabel != null)
                {
                    leftSphereTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftSphereTitleLabel.Frame.Height / 2, sectionWidth, (float)leftSphereTitleLabel.Frame.Height);
                    leftSphereTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (rightCylinderTitleLabel != null)
                {
                    rightCylinderTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightCylinderTitleLabel.Frame.Height / 2, sectionWidth, (float)rightCylinderTitleLabel.Frame.Height);
                    rightCylinderTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (leftCylinderTitleLabel != null)
                {
                    leftCylinderTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftCylinderTitleLabel.Frame.Height / 2, sectionWidth, (float)leftCylinderTitleLabel.Frame.Height);
                    leftCylinderTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (rightAxisTitleLabel != null)
                {
                    rightAxisTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightAxisTitleLabel.Frame.Height / 2, sectionWidth, (float)rightAxisTitleLabel.Frame.Height);
                    rightAxisTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (leftAxisTitleLabel != null)
                {
                    leftAxisTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftAxisTitleLabel.Frame.Height / 2, sectionWidth, (float)leftAxisTitleLabel.Frame.Height);
                    leftAxisTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (rightPrismTitleLabel != null)
                {
                    rightPrismTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightPrismTitleLabel.Frame.Height / 2, sectionWidth, (float)rightPrismTitleLabel.Frame.Height);
                    rightPrismTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (leftPrismTitleLabel != null)
                {
                    leftPrismTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftPrismTitleLabel.Frame.Height / 2, sectionWidth, (float)leftPrismTitleLabel.Frame.Height);
                    leftPrismTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (rightBifocalsTitleLabel != null)
                {
                    rightBifocalsTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)rightBifocalsTitleLabel.Frame.Height / 2, sectionWidth, (float)rightBifocalsTitleLabel.Frame.Height);
                    rightBifocalsTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                if (leftBifocalsTitleLabel != null)
                {
                    leftBifocalsTitleLabel.Frame = new CGRect(xPos, rowHeight / 2 - (float)leftBifocalsTitleLabel.Frame.Height / 2, sectionWidth, (float)leftBifocalsTitleLabel.Frame.Height);
                    leftBifocalsTitleLabel.SizeToFit();
                    xPos += fieldPadding + sectionWidth;
                }

                innerYPos += rowHeight;
                detailsScrollWidth = xPos;
            }

            //float listHeight = (Constants.IsPhone() ?  ( isNoTreatmentType ?  Constants.CLAIM_CONFIRMATION_DETAILS_NT_HEIGHT : ( isLengthType ? Constants.CLAIM_CONFIRMATION_DETAILS_LT_HEIGHT : Constants.CLAIM_CONFIRMATION_DETAILS_HEIGHT)) : Constants.CLAIMS_RESULTS_IPAD_HEIGHT )* _model.Claim.TreatmentDetails.Count + itemPadding;

            treatmentDetailsList.SizeToFit();
            treatmentDetailsList.Frame = new CGRect(0, innerYPos, ViewContainerWidth, (float)treatmentDetailsList.ContentSize.Height + 20);

            //addSeparator = new UIView();
            //addSeparator.Frame = new CGRect(0, innerYPos, ViewContainerWidth, 3);
            //addSeparator.BackgroundColor = Colors.Clear;
            //treatmentDetailsList.AddSubview(addSeparator);

            detailsScrollContainer.Frame = new CGRect(0, yPos + itemPadding, ViewContainerWidth, innerYPos + (float)treatmentDetailsList.Frame.Height);
            detailsScrollContainer.ContentSize = new CGSize(detailsScrollWidth, innerYPos + (float)treatmentDetailsList.Frame.Height);

            yPos += innerYPos + (float)treatmentDetailsList.Frame.Height + 5;

            //          if (!gscNumberField.Hidden) {
            //              gscNumberField.Frame = new RectangleF (sidePadding, yPos, ViewContainerWidth - sidePadding*2, gscNumberField.ComponentHeight);
            //              yPos += gscNumberField.Frame.Height + itemPadding;
            //          }

            if (!participantList.Hidden)
            {
                participantList.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, participantList.ComponentHeight);
                yPos += (float)(participantList.Frame.Height + itemPadding);
            }

            submittedToGSC.SizeToFit();
            submittedToGSC.Frame = new CGRect(sidePadding, yPos + 5, ViewContainerWidth - sidePadding * 2, (float)submittedToGSC.Frame.Height + 5);

            yPos += (float)submittedToGSC.Frame.Height + itemPadding;

            submitButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, BUTTON_HEIGHT);
            yPos += (float)(submitButton.Frame.Height + itemPadding);


            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + scrollContainerPadding);


            if (redrawCount < 2)
            {
                redrawCount++;
                this.View.SetNeedsLayout();
            }
        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)base.View.Frame.Width;
            }
        }

        public float GetViewContainerHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            }
            else
            {
                return (float)base.View.Frame.Height - Helpers.BottomNavHeight();
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return 10;
            }
            else
            {
                return (Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING);
            }

        }
    }
}

