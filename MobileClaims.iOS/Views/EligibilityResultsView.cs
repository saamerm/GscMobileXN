using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("EligibilityResultsView")]
    public class EligibilityResultsView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {
        protected EligibilityResultsViewModel _model;

        protected UIScrollView scrollableContainer;

        protected UILabel submissionResultsForLabel;
        protected UILabel reasonsLabel;
        protected UILabel resultsNoteLabel;
        protected UILabel eligibilityNoteLabel;

        private ClaimFieldResultDisplay gscIDResult;
        private ClaimFieldResultDisplay participantNameResult;
        private ClaimFieldResultDisplay submissionDateResult;

        private ClaimFieldResultDisplay treatmentDateResult;
        private ClaimFieldResultDisplay examinationDateResult;
        private ClaimFieldResultDisplay purchaseServiceDateResult;
        private ClaimFieldResultDisplay serviceDescriptionResult;
        private ClaimFieldResultDisplay totalAmountResult;
        private ClaimFieldResultDisplay totalChargeExaminationResult;
        private ClaimFieldResultDisplay lensTypeResult;
        private ClaimFieldResultDisplay totalChargeForContactsResult;
        private ClaimFieldResultDisplay totalChargeForGlassesResult;
        private ClaimFieldResultDisplay provinceResult;
        private ClaimFieldResultDisplay planWouldPayResult;
        private ClaimFieldResultDisplay dateOfEligibilityResult;

        private ClaimFieldResultDisplay planWouldPayChanged1Result;
        private UILabel orLabel;
        private ClaimFieldResultDisplay planWouldPayChanged2Result;

        public UILabel limitationLabel;

        protected UITableView limitationsTableView;
        MvxLimitationsTableViewSource limitationsTableSource;

        private ClaimFieldResultDisplay deductibleAmountResult;
        private ClaimFieldResultDisplay copayAmountResult;
        private ClaimFieldResultDisplay otherResult;

        private float borderWidth = 2f;
        private bool _isVisionEnhancementApplicable;
        protected UIView VisionEnhancementView;
        protected CALayer VisionEnhancementViewTopBorder;
        protected CALayer VisionEnhancementViewBottomBorder;
        protected UIImageView WarningSignImageView;
        protected UILabel VisionEnhancementMessageLabel;
        protected GSButton doneButton;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        public EligibilityResultsView()
        {
        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "eligibilityResults".tr();
            base.NavigationItem.SetHidesBackButton(true, false);

            _model = (EligibilityResultsViewModel)ViewModel;

            this.AutomaticallyAdjustsScrollViewInsets = false;

            scrollableContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollableContainer);

            submissionResultsForLabel = new UILabel();
            submissionResultsForLabel.Text = "submissionResultsFor".tr();
            submissionResultsForLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
            submissionResultsForLabel.TextColor = Colors.DARK_GREY_COLOR;
            submissionResultsForLabel.BackgroundColor = Colors.Clear;
            submissionResultsForLabel.Lines = 0;
            submissionResultsForLabel.LineBreakMode = UILineBreakMode.WordWrap;
            submissionResultsForLabel.TextAlignment = UITextAlignment.Left;
            scrollableContainer.AddSubview(submissionResultsForLabel);

            gscIDResult = new ClaimFieldResultDisplay("greenShieldId".FormatWithBrandKeywords(LocalizableBrand.GreenShield), _model.EligibilityCheckResults.Result.PlanMemberDisplayID);
            scrollableContainer.AddSubview(gscIDResult);

            participantNameResult = new ClaimFieldResultDisplay("participantName".tr(), _model.EligibilityCheckResults.Result.ParticipantFullName);
            scrollableContainer.AddSubview(participantNameResult);

            submissionDateResult = new ClaimFieldResultDisplay("submissionDate".tr(), _model.EligibilityCheckResults.Result.SubmissionDate.ToShortDateString());
            scrollableContainer.AddSubview(submissionDateResult);

            treatmentDateResult = new ClaimFieldResultDisplay("dateOfTreatmentTitleColon".tr(), _model.EligibilityCheckResults.TreatmentDateAsDateTime.ToShortDateString());
            scrollableContainer.AddSubview(treatmentDateResult);

            examinationDateResult = new ClaimFieldResultDisplay("dateOfExaminationColon".tr(), _model.EligibilityCheckResults.TreatmentDateAsDateTime.ToShortDateString());
            scrollableContainer.AddSubview(examinationDateResult);

            purchaseServiceDateResult = new ClaimFieldResultDisplay("dateOfPurchaseService".tr(), _model.EligibilityCheckResults.TreatmentDateAsDateTime.ToShortDateString());
            scrollableContainer.AddSubview(purchaseServiceDateResult);

            serviceDescriptionResult = new ClaimFieldResultDisplay("serviceDescription".tr(), _model.EligibilityCheckResults.Result.ServiceDescription);
            scrollableContainer.AddSubview(serviceDescriptionResult);

            totalAmountResult = new ClaimFieldResultDisplay("totalAmountOfVisitColon".tr(), "$" + _model.EligibilityCheckResults.ClaimAmount.ToString());
            scrollableContainer.AddSubview(totalAmountResult);

            totalChargeExaminationResult = new ClaimFieldResultDisplay("totalChargeOfExaminationColon".tr(), "$" + _model.EligibilityCheckResults.ClaimAmount.ToString());
            scrollableContainer.AddSubview(totalChargeExaminationResult);

            lensTypeResult = new ClaimFieldResultDisplay("lensTypeResult".tr() + "contextColon".tr(), _model.EligibilityCheckResults.LensType != null ? _model.EligibilityCheckResults.LensType.Name : "");
            scrollableContainer.AddSubview(lensTypeResult);

            totalChargeForContactsResult = new ClaimFieldResultDisplay("totalChargeForContacts".tr() + "contextColon".tr(), "$" + _model.EligibilityCheckResults.ClaimAmount.ToString());
            scrollableContainer.AddSubview(totalChargeForContactsResult);

            totalChargeForGlassesResult = new ClaimFieldResultDisplay("totalChargeForGlasses".tr() + "contextColon".tr(), "$" + _model.EligibilityCheckResults.ClaimAmount.ToString());
            scrollableContainer.AddSubview(totalChargeForGlassesResult);

            provinceResult = new ClaimFieldResultDisplay("provinceColon".tr(), _model.EligibilityCheckResults.Province.Name);
            scrollableContainer.AddSubview(provinceResult);

            planWouldPayResult = new ClaimFieldResultDisplay("yourPlanWouldPay".tr(), "$" + _model.EligibilityCheckResults.Result.PaidAmount.ToString());
            scrollableContainer.AddSubview(planWouldPayResult);

            dateOfEligibilityResult = new ClaimFieldResultDisplay("dateOfEligibilityMaximum".tr() + "contextColon".tr(), _model.EligibilityCheckResults.Result.EligibilityDate.ToShortDateString());
            scrollableContainer.AddSubview(dateOfEligibilityResult);

            planWouldPayChanged1Result = new ClaimFieldResultDisplay("planWouldPayChanged1".tr(), "$" + _model.EligibilityCheckResults.Result.PaidAmount.ToString());
            scrollableContainer.AddSubview(planWouldPayChanged1Result);

            orLabel = new UILabel();
            orLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            orLabel.TextColor = Colors.DARK_GREY_COLOR;
            orLabel.TextAlignment = UITextAlignment.Left;
            orLabel.BackgroundColor = Colors.Clear;
            orLabel.LineBreakMode = UILineBreakMode.WordWrap;
            orLabel.Lines = 0;
            orLabel.Text = "or".tr();
            scrollableContainer.AddSubview(orLabel);

            planWouldPayChanged2Result = new ClaimFieldResultDisplay("planWouldPayChanged2".tr(), "$" + _model.EligibilityCheckResults.Result.RxChangedPaidAmount != null ? _model.EligibilityCheckResults.Result.RxChangedPaidAmount.ToString() : "");
            scrollableContainer.AddSubview(planWouldPayChanged2Result);

            treatmentDateResult.Hidden = true;
            examinationDateResult.Hidden = true;
            purchaseServiceDateResult.Hidden = true;
            serviceDescriptionResult.Hidden = true;
            totalAmountResult.Hidden = true;
            totalChargeExaminationResult.Hidden = true;
            lensTypeResult.Hidden = true;
            totalChargeForContactsResult.Hidden = true;
            totalChargeForGlassesResult.Hidden = true;
            provinceResult.Hidden = true;
            planWouldPayResult.Hidden = true;
            dateOfEligibilityResult.Hidden = true;
            planWouldPayChanged1Result.Hidden = true;
            orLabel.Hidden = true;
            planWouldPayChanged2Result.Hidden = true;

            reasonsLabel = new UILabel();
            reasonsLabel.Text = "reasonsReduced".tr();
            reasonsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            reasonsLabel.TextColor = Colors.DARK_GREY_COLOR;
            reasonsLabel.BackgroundColor = Colors.Clear;
            reasonsLabel.Lines = 0;
            reasonsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            reasonsLabel.TextAlignment = UITextAlignment.Left;
            scrollableContainer.AddSubview(reasonsLabel);

            string deductionString = "";

            if (_model.EligibilityCheckResults.Result.DeductionMessages != null)
            {
                for (var i = 0; i < _model.EligibilityCheckResults.Result.DeductionMessages.Count; i++)
                {

                    deductionString += _model.EligibilityCheckResults.Result.DeductionMessages[i] + " ";
                }
            }


            deductibleAmountResult = new ClaimFieldResultDisplay("deductibleAmount".tr(), "$" + _model.EligibilityCheckResults.Result.DeductibleAmount.ToString());
            scrollableContainer.AddSubview(deductibleAmountResult);
            copayAmountResult = new ClaimFieldResultDisplay("copayAmount".tr(), "$" + _model.EligibilityCheckResults.Result.CopayAmount.ToString());
            scrollableContainer.AddSubview(copayAmountResult);
            otherResult = new ClaimFieldResultDisplay("other".tr(), deductionString);
            scrollableContainer.AddSubview(otherResult);

            eligibilityNoteLabel = new UILabel();
            eligibilityNoteLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            eligibilityNoteLabel.TextColor = Colors.DARK_GREY_COLOR;
            eligibilityNoteLabel.TextAlignment = UITextAlignment.Left;
            eligibilityNoteLabel.BackgroundColor = Colors.Clear;
            eligibilityNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
            eligibilityNoteLabel.Lines = 0;
            if (_model.EligibilityCheckResults.Result.EligibilityNote != null)
            {
                eligibilityNoteLabel.Text = _model.EligibilityCheckResults.Result.EligibilityNote;
            }
            scrollableContainer.AddSubview(eligibilityNoteLabel);

            limitationLabel = new UILabel();
            limitationLabel.Text = "planLimitation".tr();
            limitationLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            limitationLabel.TextColor = Colors.DARK_GREY_COLOR;
            limitationLabel.BackgroundColor = Colors.Clear;
            limitationLabel.TextAlignment = UITextAlignment.Left;
            limitationLabel.Lines = 0;
            limitationLabel.SizeToFit();
            limitationLabel.AdjustsFontSizeToFitWidth = true;

            limitationsTableView = new UITableView();
            limitationsTableView.TableHeaderView = new UIView();
            limitationsTableView.SeparatorColor = Colors.Clear;
            limitationsTableView.UserInteractionEnabled = false;
            limitationsTableView.ShowsVerticalScrollIndicator = true;

            limitationsTableSource = new MvxLimitationsTableViewSource(_model.EligibilityCheckResults.Result.PlanLimitations, limitationsTableView, "EligibilityLimitationsTableCell", typeof(EligibilityLimitationsTableCell), true);
            limitationsTableView.Source = limitationsTableSource;

            if (_model.EligibilityCheckResults.Result.IsPlanLimitationsVisible)
            {
                scrollableContainer.AddSubview(limitationLabel);
                scrollableContainer.AddSubview(limitationsTableView);
            }

            resultsNoteLabel = new UILabel();
            resultsNoteLabel.TextAlignment = UITextAlignment.Left;
            resultsNoteLabel.BackgroundColor = Colors.Clear;
            resultsNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
            resultsNoteLabel.Lines = 0;

            var resultsNoteStringAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.DarkGrayColor2,
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.EligibilityResultVisionEnhancementFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.EligibilityResultVisionEnhancementFontLineSpace
                }
            };

            if (!_model.PhoneBusy)
            {
                if (!_model.NoPhoneAlteration)
                {
                    var attributedResultsNoteMessage =
                        new NSMutableAttributedString(
                            LocalizableBrand.EligibilityResultsNote
                                .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, _model.PhoneNumber.Text),
                            resultsNoteStringAttributes);
                    resultsNoteLabel.AttributedText = attributedResultsNoteMessage;
                }
                else
                {
                    var attributedResultsNoteMessage =
                        new NSMutableAttributedString(
                            LocalizableBrand.EligibilityResultsNote
                                .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumber),
                            resultsNoteStringAttributes);
                    resultsNoteLabel.AttributedText = attributedResultsNoteMessage;

#if FPPM
                    attributedResultsNoteMessage =
                        new NSMutableAttributedString(
                           LocalizableBrand.EligibilityResultsNote
                                .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumberFPPM),
                           resultsNoteStringAttributes);
                    resultsNoteLabel.AttributedText = attributedResultsNoteMessage;
#endif
                }
            }
            else
            {
                ((GSCBaseView)View).startLoading();
            }
            scrollableContainer.AddSubview(resultsNoteLabel);

            doneButton = new GSButton();
            doneButton.SetTitle("submitAnotherEligibilityCheck".tr(), UIControlState.Normal);
            scrollableContainer.AddSubview(doneButton);

            bool isVision = false;

            switch (_model.EligibilityCheckType.ID)
            {
                case "RECALLEXAM":
                    {
                        treatmentDateResult.Hidden = false;
                        serviceDescriptionResult.Hidden = false;
                        totalAmountResult.Hidden = false;
                        provinceResult.Hidden = false;
                        planWouldPayResult.Hidden = false;
                        break;
                    }
                case "ORTHOTICS":
                    {
                        treatmentDateResult.Hidden = false;
                        serviceDescriptionResult.Hidden = false;
                        totalAmountResult.Hidden = false;
                        provinceResult.Hidden = false;
                        planWouldPayResult.Hidden = false;
                        break;
                    }
                case "CHIRO":
                case "PHYSIO":
                case "MASSAGE":
                    {
                        treatmentDateResult.Hidden = false;
                        serviceDescriptionResult.Hidden = false;
                        totalAmountResult.Hidden = false;
                        provinceResult.Hidden = false;
                        planWouldPayResult.Hidden = false;
                        break;
                    }
                case "GLASSES":
                    {
                        purchaseServiceDateResult.Hidden = false;
                        serviceDescriptionResult.Hidden = false;
                        lensTypeResult.Hidden = false;
                        totalChargeForGlassesResult.Hidden = false;
                        planWouldPayResult.Hidden = false;
                        dateOfEligibilityResult.Hidden = false;
                        isVision = true;
                        break;

                    }
                case "CONTACTS":
                    {
                        purchaseServiceDateResult.Hidden = false;
                        serviceDescriptionResult.Hidden = false;
                        totalChargeForContactsResult.Hidden = false;
                        planWouldPayResult.Hidden = false;
                        dateOfEligibilityResult.Hidden = false;
                        isVision = true;
                        break;
                    }

                case "EYEEXAM":
                    {
                        examinationDateResult.Hidden = false;
                        serviceDescriptionResult.Hidden = false;
                        totalChargeExaminationResult.Hidden = false;
                        provinceResult.Hidden = false;
                        planWouldPayResult.Hidden = false;
                        isVision = true;
                        break;
                    }
            }

            if (isVision)
            {
                switch (_model.EligibilityCheckResults.Result.PaidAmountIndicator)
                {
                    case 0:
                        planWouldPayResult.Hidden = false;
                        break;
                    case 1:
                        planWouldPayResult.Hidden = false;
                        break;
                    case 2:
                        planWouldPayChanged1Result.Hidden = false;
                        orLabel.Hidden = false;
                        planWouldPayChanged2Result.Hidden = false;
                        planWouldPayResult.Hidden = true;
                        break;
                }
            }

            var set = this.CreateBindingSet<EligibilityResultsView, EligibilityResultsViewModel>();
            set.Bind(doneButton).To(vm => vm.SubmitAnotherEligibilityCheckCommand);
            set.Bind(limitationsTableSource).To(vm => vm.EligibilityCheckResults.Result.PlanLimitations);
            set.Bind(this).For(v => v.PhoneBusy).To(vm => vm.PhoneBusy);
            set.Bind(this).For(x => x.IsVisionEnhancementApplicable).To(vm => vm.IsVisionEnhancementApplicable);
            set.Apply();

        }

        public void layoutComponents()
        {
            var resultsNoteStringAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.DarkGrayColor2,
                Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.EligibilityResultVisionEnhancementFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.EligibilityResultVisionEnhancementFontLineSpace
                }
            };

            if (!_model.NoPhoneAlteration)
            {
                var attributedResultsNoteMessage =
                    new NSMutableAttributedString(
                        LocalizableBrand.EligibilityResultsNote
                            .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, _model.PhoneNumber.Text),
                        resultsNoteStringAttributes);
                resultsNoteLabel.AttributedText = attributedResultsNoteMessage;
            }
            else
            {
                var attributedResultsNoteMessage =
                    new NSMutableAttributedString(
                        LocalizableBrand.EligibilityResultsNote
                            .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumber),
                        resultsNoteStringAttributes);
                resultsNoteLabel.AttributedText = attributedResultsNoteMessage;

#if FPPM
                attributedResultsNoteMessage =
                    new NSMutableAttributedString(
                        LocalizableBrand.EligibilityResultsNote
                            .FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.PhoneNumberFPPM),
                        resultsNoteStringAttributes);
                resultsNoteLabel.AttributedText = attributedResultsNoteMessage;
#endif
            }
          
            View.SetNeedsLayout();
        }

        public void AddVisionEnhancementMessage()
        {
            if (_isVisionEnhancementApplicable)
            {
                VisionEnhancementViewBottomBorder = new CALayer();
                VisionEnhancementViewBottomBorder.BorderColor = Colors.HIGHLIGHT_COLOR.CGColor;
                VisionEnhancementViewBottomBorder.BorderWidth = borderWidth;

                VisionEnhancementViewTopBorder = new CALayer();
                VisionEnhancementViewTopBorder.BorderColor = Colors.HIGHLIGHT_COLOR.CGColor;
                VisionEnhancementViewTopBorder.BorderWidth = borderWidth;

                VisionEnhancementView = new UIView();
                VisionEnhancementView.Layer.AddSublayer(VisionEnhancementViewTopBorder);
                VisionEnhancementView.Layer.AddSublayer(VisionEnhancementViewBottomBorder);

                WarningSignImageView = new UIImageView();
                WarningSignImageView.Image = UIImage.FromBundle("Warning");
                WarningSignImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

                VisionEnhancementMessageLabel = new UILabel();
                VisionEnhancementMessageLabel.Lines = 0;
                VisionEnhancementMessageLabel.LineBreakMode = UILineBreakMode.WordWrap;

                var visionEnhancementStringAttributes = new UIStringAttributes
                {
                    ForegroundColor = Colors.DarkGrayColor2,
                    Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.EligibilityResultVisionEnhancementFontSize),
                    ParagraphStyle = new NSMutableParagraphStyle()
                    {
                        LineSpacing = Constants.EligibilityResultVisionEnhancementFontLineSpace
                    }
                };
                var attributedVisionMessage = new NSMutableAttributedString(_model.EligibilityResultsPCAndPGText,
                    visionEnhancementStringAttributes);
                VisionEnhancementMessageLabel.AttributedText = attributedVisionMessage;
                VisionEnhancementView.AddSubview(WarningSignImageView);
                VisionEnhancementView.AddSubview(VisionEnhancementMessageLabel);
                scrollableContainer.AddSubview(VisionEnhancementView);
            }
            View.SetNeedsLayout();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ((GSCBaseView)View).subscribeToBusyIndicator();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ((GSCBaseView)View).unsubscribeFromBusyIndicator();
        }

        private bool _phonebusy = true;
        public bool PhoneBusy
        {
            get
            {
                return _phonebusy;
            }
            set
            {
                _phonebusy = value;
                if (!_phonebusy)
                {
                    layoutComponents();
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).stopLoading();
                    });
                }
                else
                {
                    InvokeOnMainThread(() =>
                    {
                        ((GSCBaseView)View).startLoading();
                    });
                }

            }
        }

        public bool IsVisionEnhancementApplicable
        {
            get => _isVisionEnhancementApplicable;
            set
            {
                _isVisionEnhancementApplicable = value;
                AddVisionEnhancementMessage();
            }
        }

        int redrawCount = 0;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float innerPadding = 10;
            float limitationPadding = 10;

            float centerX = ViewContainerWidth / 2;
            float yPos = ViewContentYPositionPadding;
            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float extraPos = Constants.IOS_7_TOP_PADDING;

            submissionResultsForLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)submissionResultsForLabel.Frame.Height);
            submissionResultsForLabel.SizeToFit();
            yPos += (float)submissionResultsForLabel.Frame.Height + innerPadding;

            gscIDResult.Frame = new CGRect(0, yPos, ViewContainerWidth, gscIDResult.ComponentHeight);
            yPos += gscIDResult.ComponentHeight;
            participantNameResult.Frame = new CGRect(0, yPos, ViewContainerWidth, participantNameResult.ComponentHeight);
            yPos += participantNameResult.ComponentHeight;
            submissionDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, submissionDateResult.ComponentHeight);
            yPos += submissionDateResult.ComponentHeight;

            if (!treatmentDateResult.Hidden)
            {
                treatmentDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, treatmentDateResult.ComponentHeight);
                yPos += treatmentDateResult.ComponentHeight;
            }
            if (!examinationDateResult.Hidden)
            {
                examinationDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, examinationDateResult.ComponentHeight);
                yPos += examinationDateResult.ComponentHeight;
            }
            if (!purchaseServiceDateResult.Hidden)
            {
                purchaseServiceDateResult.Frame = new CGRect(0, yPos, ViewContainerWidth, purchaseServiceDateResult.ComponentHeight);
                yPos += purchaseServiceDateResult.ComponentHeight;
            }
            if (!serviceDescriptionResult.Hidden)
            {
                serviceDescriptionResult.Frame = new CGRect(0, yPos, ViewContainerWidth, serviceDescriptionResult.ComponentHeight);
                yPos += serviceDescriptionResult.ComponentHeight;
            }
            if (!lensTypeResult.Hidden)
            {
                lensTypeResult.Frame = new CGRect(0, yPos, ViewContainerWidth, lensTypeResult.ComponentHeight);
                yPos += lensTypeResult.ComponentHeight;
            }
            if (!totalChargeForContactsResult.Hidden)
            {
                totalChargeForContactsResult.Frame = new CGRect(0, yPos, ViewContainerWidth, totalChargeForContactsResult.ComponentHeight);
                yPos += totalChargeForContactsResult.ComponentHeight;
            }
            if (!totalChargeForGlassesResult.Hidden)
            {
                totalChargeForGlassesResult.Frame = new CGRect(0, yPos, ViewContainerWidth, totalChargeForGlassesResult.ComponentHeight);
                yPos += totalChargeForGlassesResult.ComponentHeight;
            }
            if (!totalAmountResult.Hidden)
            {
                totalAmountResult.Frame = new CGRect(0, yPos, ViewContainerWidth, totalAmountResult.ComponentHeight);
                yPos += totalAmountResult.ComponentHeight;
            }
            if (!totalChargeExaminationResult.Hidden)
            {
                totalChargeExaminationResult.Frame = new CGRect(0, yPos, ViewContainerWidth, totalChargeExaminationResult.ComponentHeight);
                yPos += totalChargeExaminationResult.ComponentHeight;
            }
            if (!provinceResult.Hidden)
            {
                provinceResult.Frame = new CGRect(0, yPos, ViewContainerWidth, provinceResult.ComponentHeight);
                yPos += provinceResult.ComponentHeight;
            }
            if (!planWouldPayResult.Hidden)
            {
                planWouldPayResult.Frame = new CGRect(0, yPos, ViewContainerWidth, planWouldPayResult.ComponentHeight);
                yPos += planWouldPayResult.ComponentHeight;
            }
            if (!dateOfEligibilityResult.Hidden)
            {
                dateOfEligibilityResult.Frame = new CGRect(0, yPos, ViewContainerWidth, dateOfEligibilityResult.ComponentHeight);
                yPos += dateOfEligibilityResult.ComponentHeight;
            }

            if (!planWouldPayChanged1Result.Hidden)
            {
                planWouldPayChanged1Result.Frame = new CGRect(0, yPos, ViewContainerWidth, planWouldPayChanged1Result.ComponentHeight);
                yPos += planWouldPayChanged1Result.ComponentHeight;
            }
            if (!orLabel.Hidden)
            {
                orLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)orLabel.Frame.Height);
                orLabel.SizeToFit();
                yPos += (float)orLabel.Frame.Height;
            }
            if (!planWouldPayChanged2Result.Hidden)
            {
                planWouldPayChanged2Result.Frame = new CGRect(0, yPos, ViewContainerWidth, planWouldPayChanged2Result.ComponentHeight);
                yPos += planWouldPayChanged2Result.ComponentHeight;
            }

            yPos += itemPadding;

            eligibilityNoteLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)eligibilityNoteLabel.Frame.Height);
            eligibilityNoteLabel.SizeToFit();

            if ((float)eligibilityNoteLabel.Frame.Height > 1)
            {
                yPos += (float)eligibilityNoteLabel.Frame.Height;
            }

            reasonsLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)reasonsLabel.Frame.Height);
            reasonsLabel.SizeToFit();
            yPos += (float)reasonsLabel.Frame.Height + innerPadding;
            deductibleAmountResult.Frame = new CGRect(0, yPos, ViewContainerWidth, deductibleAmountResult.ComponentHeight);
            yPos += deductibleAmountResult.ComponentHeight;
            copayAmountResult.Frame = new CGRect(0, yPos, ViewContainerWidth, copayAmountResult.ComponentHeight);
            yPos += copayAmountResult.ComponentHeight;
            otherResult.Frame = new CGRect(0, yPos, ViewContainerWidth, otherResult.ComponentHeight);
            yPos += otherResult.ComponentHeight;

            yPos += itemPadding;

            if (_model.EligibilityCheckResults.Result.IsPlanLimitationsVisible)
            {
                limitationLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth, (float)limitationLabel.Frame.Height);
                limitationLabel.SizeToFit();

                yPos += (float)limitationLabel.Frame.Height + limitationPadding + innerPadding;

                limitationsTableView.Frame = new CGRect(0, yPos, ViewContainerWidth, (nfloat)limitationsTableView.ContentSize.Height);
                limitationsTableView.SetNeedsLayout();
                yPos += (float)limitationsTableView.Frame.Height + itemPadding;
            }

            if (IsVisionEnhancementApplicable)
            {
                var visionEnhancementLeftPadding = 63f;
                var visionEnhancementTopPadding = 20f;
                var warningSignLeftPadding = 20f;

                VisionEnhancementMessageLabel.Frame = new CGRect(visionEnhancementLeftPadding,
                    visionEnhancementTopPadding,
                    View.Frame.Width - visionEnhancementLeftPadding - WarningSignImageView.Image.Size.Width,
                    VisionEnhancementMessageLabel.Frame.Height);
                VisionEnhancementMessageLabel.SizeToFit();

                VisionEnhancementView.Frame = new CGRect(0, yPos,
                    View.Frame.Width,
                    VisionEnhancementMessageLabel.Frame.Height + visionEnhancementTopPadding * 2);

                WarningSignImageView.Frame = new CGRect(warningSignLeftPadding,
                       (VisionEnhancementView.Frame.Height - WarningSignImageView.Image.Size.Height) / 2,
                       WarningSignImageView.Image.Size.Width,
                       WarningSignImageView.Image.Size.Height);

                VisionEnhancementViewTopBorder.Frame = new CGRect(0, 0, View.Frame.Width, borderWidth);
                VisionEnhancementViewBottomBorder.Frame = new CGRect(0, VisionEnhancementView.Frame.Height - borderWidth, View.Frame.Width, borderWidth);

                yPos += (float)VisionEnhancementView.Frame.Height + itemPadding;
            }

            resultsNoteLabel.Frame = new CGRect(sidePadding, yPos, ViewContainerWidth - sidePadding * 2, (float)resultsNoteLabel.Frame.Height);
            resultsNoteLabel.SizeToFit();

            yPos += (float)resultsNoteLabel.Frame.Height + itemPadding;

            doneButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, BUTTON_HEIGHT);
            yPos += (float)doneButton.Frame.Height + itemPadding;

            scrollableContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollableContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + GetBottomPadding(extraPos));

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
                return (float)base.View.Bounds.Width;
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
                return (float)base.View.Bounds.Height - Helpers.BottomNavHeight();
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            }
            else
            {
                return Constants.IOS_7_TOP_PADDING;
            }
        }
    }
}