using System;
using System.Collections.Generic;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS
{
    [Obsolete]
    [Foundation.Register("ClaimDetailsView1")]
    public class ClaimDetailsView1 : GSCBaseViewPaddingController, IRehydrating, IGSCBaseViewImplementor
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
        protected ClaimDetailsViewModel _model;

        protected UIScrollView scrollContainer;

        protected UIView participantBlockOverlay;

        //==IPHONE ONLY===
        protected UIView providerContainer;
        protected UILabel providerTitle;
        protected UILabel providerName;
        protected UILabel providerAddress;
        protected UILabel providerCity;
        protected UILabel providerPhone;
        protected UILabel registrationNumber;
        //==IPHONE ONLY===

        protected ClaimOtherBenefitsView claimOtherBenefits;//switch
        protected ClaimOtherBenefitsHCSAView claimOtherBenefitsHCSA;//switch //no
        protected ClaimOtherBenefitsGSTView claimOtherBenefitsGST;//switch
        protected ClaimMotorVehicleView claimMotorVehicle;//switch
        protected ClaimWorkInjuryView claimWorkInjury;//switch
        protected ClaimMedicalItemView claimMedicalItem;//date and textField

        protected GSButton treatmentDetailsButton;

        protected UILabel planParticpantLabel;
        protected UILabel claimOtherBenefitsHCSALabel;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        private bool medicalItemVisible;

        private bool vmHasAppearedDelay;

        private IMvxMessenger _messenger;

        private UILabel massageNotice;

        public ClaimDetailsView1()
        {

        }

        public override void ViewDidLoad()
        {
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (Rehydrating)
            {
                rehydrationservice.Rehydrating = true;
            }

            base.ViewDidLoad();

            this.AutomaticallyAdjustsScrollViewInsets = false;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "claimDetailsTitle".tr();
            base.NavigationItem.SetHidesBackButton(false, false);

            base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
            base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
            base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;

            _model = (ClaimDetailsViewModel)this.ViewModel;
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            scrollContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            ((GSCBaseView)View).ViewTapped += HandleViewTapped;

            participantBlockOverlay = new UIScrollView();
            participantBlockOverlay.BackgroundColor = Colors.LightGrayColor;
            participantBlockOverlay.Alpha = 0.5f;

            providerTitle = new UILabel();
            providerTitle.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            providerTitle.TextColor = Colors.DARK_GREY_COLOR;
            providerTitle.LineBreakMode = UILineBreakMode.WordWrap;
            providerTitle.Lines = 0;
            providerTitle.BackgroundColor = Colors.Clear;
            providerTitle.TextAlignment = UITextAlignment.Left;
            scrollContainer.AddSubview(providerTitle);

            providerContainer = new UIView();
            scrollContainer.AddSubview(providerContainer);

            providerName = new UILabel();
            providerName.TextColor = Colors.DARK_GREY_COLOR;
            providerName.BackgroundColor = Colors.Clear;
            providerName.TextAlignment = UITextAlignment.Left;
            providerName.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            providerContainer.AddSubview(providerName);

            providerAddress = new UILabel();
            providerAddress.TextColor = Colors.DARK_GREY_COLOR;
            providerAddress.BackgroundColor = Colors.Clear;
            providerAddress.TextAlignment = UITextAlignment.Left;
            providerAddress.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            providerContainer.AddSubview(providerAddress);

            providerCity = new UILabel();
            providerCity.TextColor = Colors.DARK_GREY_COLOR;
            providerCity.BackgroundColor = Colors.Clear;
            providerCity.TextAlignment = UITextAlignment.Left;
            providerCity.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            providerContainer.AddSubview(providerCity);

            providerPhone = new UILabel();
            providerPhone.TextColor = Colors.DARK_GREY_COLOR;
            providerPhone.BackgroundColor = Colors.Clear;
            providerPhone.TextAlignment = UITextAlignment.Left;
            providerPhone.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            providerContainer.AddSubview(providerPhone);

            registrationNumber = new UILabel();
            registrationNumber.TextColor = Colors.DARK_GREY_COLOR;
            registrationNumber.BackgroundColor = Colors.Clear;
            registrationNumber.TextAlignment = UITextAlignment.Left;
            registrationNumber.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            providerContainer.AddSubview(registrationNumber);

            planParticpantLabel = new UILabel();
            planParticpantLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            planParticpantLabel.TextColor = Colors.DARK_GREY_COLOR;
            planParticpantLabel.BackgroundColor = Colors.Clear;
            planParticpantLabel.TextAlignment = UITextAlignment.Left;
            planParticpantLabel.LineBreakMode = UILineBreakMode.WordWrap;
            planParticpantLabel.Lines = 0;
            scrollContainer.AddSubview(planParticpantLabel);

            claimOtherBenefits = new ClaimOtherBenefitsView();
            claimOtherBenefits.VisibilityToggled += HandleVisibilityToggled;
            scrollContainer.AddSubview(claimOtherBenefits.View);

            claimOtherBenefitsHCSALabel = new UILabel();
            claimOtherBenefitsHCSALabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);//Constants.NUNITO_BOLD//(nfloat)Constants.LG_HEADING_FONT_SIZE);
            claimOtherBenefitsHCSALabel.TextColor = Colors.DARK_GREY_COLOR;
            claimOtherBenefitsHCSALabel.BackgroundColor = Colors.Clear;
            claimOtherBenefitsHCSALabel.TextAlignment = UITextAlignment.Left;
            claimOtherBenefitsHCSALabel.LineBreakMode = UILineBreakMode.WordWrap;
            claimOtherBenefitsHCSALabel.Lines = 0;
            claimOtherBenefitsHCSALabel.Text = !string.IsNullOrEmpty(_model.ClaimOtherBenefitsViewModel.HCSASubtitle) ? _model.ClaimOtherBenefitsViewModel.HCSASubtitle.ToUpper() : string.Empty;
            scrollContainer.AddSubview(claimOtherBenefitsHCSALabel);

            claimOtherBenefitsHCSA = new ClaimOtherBenefitsHCSAView();
            claimOtherBenefitsHCSA.Title = _model.ClaimOtherBenefitsViewModel.HCSAQuestion;
            scrollContainer.AddSubview(claimOtherBenefitsHCSA.View);

            claimOtherBenefitsGST = new ClaimOtherBenefitsGSTView();
            scrollContainer.AddSubview(claimOtherBenefitsGST.View);

            claimMotorVehicle = new ClaimMotorVehicleView();
            claimMotorVehicle.VisibilityToggled += HandleVisibilityToggled;
            scrollContainer.AddSubview(claimMotorVehicle.View);

            claimWorkInjury = new ClaimWorkInjuryView();
            claimWorkInjury.VisibilityToggled += HandleVisibilityToggled;
            scrollContainer.AddSubview(claimWorkInjury.View);

            claimMedicalItem = new ClaimMedicalItemView(_model);
            claimMedicalItem.VisibilityToggled += HandleVisibilityToggled;
            scrollContainer.AddSubview(claimMedicalItem.View);

            massageNotice = new UILabel();
            massageNotice.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            massageNotice.TextColor = Colors.DARK_GREY_COLOR;
            massageNotice.TextAlignment = UITextAlignment.Center;
            massageNotice.BackgroundColor = Colors.Clear;
            massageNotice.LineBreakMode = UILineBreakMode.WordWrap;
            massageNotice.Lines = 0;
            massageNotice.Text = "massageNoticeText".tr();
            scrollContainer.AddSubview(massageNotice);

            treatmentDetailsButton = new GSButton();
            treatmentDetailsButton.SetTitle("claimDetailsFor".tr(), UIControlState.Normal);
            scrollContainer.AddSubview(treatmentDetailsButton);

            DismmssVCTableViewSource tableSource = new DismmssVCTableViewSource(
                claimMedicalItem.TypeOfMedicalProfessionalSelect.popoverController,
                claimMedicalItem.TypeOfMedicalProfessionalSelect.listController.tableView,
                "ClaimOptionTableCell",
                typeof(ClaimOptionTableCell));

            _model.OnInvalidClaim += HandleOnInvalidClaim;//need to add

            var set = this.CreateBindingSet<ClaimDetailsView1, Core.ViewModels.ClaimDetailsViewModel>();
            set.Bind(providerTitle).To(vm => vm.ClaimSubmissionType.Name).WithConversion("ClaimServiceProvider");
            set.Bind(providerName).To(vm => vm.ServiceProvider.BusinessName);
            set.Bind(providerAddress).To(vm => vm.ServiceProvider.Address);
            set.Bind(providerCity).To(vm => vm.ServiceProvider.FormattedAddress);
            set.Bind(providerPhone).To(vm => vm.ServiceProvider.Phone).WithConversion("PhonePrefix");
            set.Bind(registrationNumber).To(vm => vm.ServiceProvider.RegistrationNumber).WithConversion("RegistrationNumPrefix");
            set.Bind(planParticpantLabel).To(vm => vm.Participant.FullName).WithConversion("ClaimDetailsForPrefix");

            //OTHER BENEFITS
            set.Bind(claimOtherBenefits.toggleSwitch).To(vm => vm.ClaimOtherBenefitsViewModel.CoverageUnderAnotherBenefitsPlan);
            set.Bind(claimOtherBenefits.IsOtherCoverageGSCToggle.toggleSwitch).To(vm => vm.ClaimOtherBenefitsViewModel.IsOtherCoverageWithGSC);
            set.Bind(claimOtherBenefits.HasClaimBeenSubmittedToOtherBenefitPlanToggle.toggleSwitch).To(vm => vm.ClaimOtherBenefitsViewModel.HasClaimBeenSubmittedToOtherBenefitPlan);
            set.Bind(claimOtherBenefits.PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.toggleSwitch).To(vm => vm.ClaimOtherBenefitsViewModel.PayAnyUnpaidBalanceThroughOtherGSCPlan);
            set.Bind(claimOtherBenefits.OtherGscNumberField.textField).To(vm => vm.ClaimOtherBenefitsViewModel.OtherGSCNumber);

            //OTHER HCSA
            set.Bind(claimOtherBenefitsHCSA.toggleSwitch).To(vm => vm.ClaimOtherBenefitsViewModel.PayUnderHCSA);
            set.Bind(claimOtherBenefitsHCSA.View).For(v => v.Hidden).To(vm => vm.ClaimOtherBenefitsViewModel.PayUnderHCSAVisible).WithConversion("BoolOpposite");
            set.Bind(claimOtherBenefitsHCSALabel).For(l => l.Hidden).To(vm => vm.ClaimOtherBenefitsViewModel.PayUnderHCSAVisible).WithConversion("BoolOpposite");

            //MOTOR VEHICLE
            set.Bind(claimMotorVehicle.toggleSwitch).To(vm => vm.ClaimMotorVehicleViewModel.IsTreatmentDueToAMotorVehicleAccident);
            set.Bind(claimMotorVehicle.DateOfMotorVehicleAccidentSelect.dateController.datePicker).To(vm => vm.ClaimMotorVehicleViewModel.DateOfMotorVehicleAccident);
            set.Bind(claimMotorVehicle.DateOfMotorVehicleAccidentSelect.detailsLabel).To(vm => vm.ClaimMotorVehicleViewModel.DateOfMotorVehicleAccident).WithConversion("DateToString").OneWay();

            //WORK INJURY
            set.Bind(claimWorkInjury.toggleSwitch).To(vm => vm.ClaimWorkInjuryViewModel.IsTreatmentDueToAWorkRelatedInjury);
            set.Bind(claimWorkInjury.WorkRelatedInjuryCaseNumberField.textField).To(vm => vm.ClaimWorkInjuryViewModel.WorkRelatedInjuryCaseNumber);
            set.Bind(claimWorkInjury.DateOfWorkRelatedInjurySelect.dateController.datePicker).To(vm => vm.ClaimWorkInjuryViewModel.DateOfWorkRelatedInjury);            
            set.Bind(claimWorkInjury.DateOfWorkRelatedInjurySelect.detailsLabel).To(vm => vm.ClaimWorkInjuryViewModel.DateOfWorkRelatedInjury).WithConversion("DateToString").OneWay();

            //MEDICAL ITEM
            //set.Bind (this).For (v => v.TypesOfMedicalProfessional).To (vm => vm.ClaimMedicalItemViewModel.TypesOfMedicalProfessional);
            set.Bind(claimMedicalItem.DateOfReferral.dateController.datePicker).To(vm => vm.ClaimMedicalItemViewModel.DateOfReferral);
            set.Bind(claimMedicalItem.DateOfReferral.detailsLabel).To(vm => vm.ClaimMedicalItemViewModel.DateOfReferral).WithConversion("DateToString").OneWay();
            set.Bind(claimMedicalItem.TypeOfMedicalProfessionalSelect.detailsLabel).To(vm => vm.ClaimMedicalItemViewModel.TypeOfMedicalProfessional.Name);

            //tableview
            set.Bind(tableSource).To(vm => vm.ClaimMedicalItemViewModel.TypesOfMedicalProfessional);
            set.Bind(tableSource).For(s => s.SelectedItem).To(vm => vm.ClaimMedicalItemViewModel.TypeOfMedicalProfessional);

            //GST
            set.Bind(claimOtherBenefitsGST.toggleSwitch).To(vm => vm.ClaimMedicalItemViewModel.IsGSTHSTIncluded);
            set.Bind(claimOtherBenefitsGST.View).For(v => v.Hidden).To(vm => vm.ClaimMedicalItemViewModel.Question16Visible).WithConversion("BoolOpposite");

            if (_model.ClaimMedicalItemViewModel.Questions13Through15Visible)
            {
                medicalItemVisible = true;
                set.Bind(claimMedicalItem.toggleSwitch).To(vm => vm.ClaimMedicalItemViewModel.HasReferralBeenPreviouslySubmitted);
            }
            else
            {
                if (_model.ClaimMedicalItemViewModel.Question12Visible)
                {
                    claimMedicalItem.setQuestion12();
                    medicalItemVisible = true;
                    set.Bind(claimMedicalItem.toggleSwitch).To(vm => vm.ClaimMedicalItemViewModel.IsMedicalItemForSportsOnly);
                }
                else
                {
                    if (claimMedicalItem != null)
                    {
                        claimMedicalItem.View.RemoveFromSuperview();
                    }
                    medicalItemVisible = false;
                }
            }

            //MAIN VIEW
            set.Bind(massageNotice).For(v => v.Hidden).To(vm => vm.ClaimMedicalItemViewModel.MassageOntarioMessageVisible).WithConversion("BoolOpposite");
            set.Bind(this.treatmentDetailsButton).To(vm => vm.TreatmentDetailsClickCommand);
            set.Bind(this).For(v => v.Participant).To(vm => vm.Participant);
            set.Apply();

            claimMedicalItem.TypeOfMedicalProfessionalSelect.listController.tableView.Source = tableSource;
            claimMedicalItem.TypeOfMedicalProfessionalSelect.listController.tableView.ReloadData();

            if (_model.ClaimMedicalItemViewModel.IsDateOfReferralSetByUser)
            {
                claimMedicalItem.DateOfReferral.ShowDate();
            }

            if (_model.ClaimMedicalItemViewModel.TypeOfMedicalProfessional != null)
            {
                claimMedicalItem.TypeOfMedicalProfessionalSelect.ShowDetails();
            }

            if (_model.Participant == null)
            {
                View.AddSubview(participantBlockOverlay);
            }

            (this.ViewModel as ViewModelBase).CreatedFromRehydration = false;
            rehydrationservice.Rehydrating = false;
        }

        private void HandleViewTapped(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        private void dismissKeyboard()
        {
            this.View.EndEditing(true);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _messenger.Publish<OnClaimDetailsViewModelMessage>(new OnClaimDetailsViewModelMessage(this));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            dismissKeyboard();

            claimOtherBenefits.OtherGscNumberField.hideError();
            claimMotorVehicle.DateOfMotorVehicleAccidentSelect.hideError();
            claimWorkInjury.DateOfWorkRelatedInjurySelect.hideError();
            claimWorkInjury.WorkRelatedInjuryCaseNumberField.hideError();
            claimMedicalItem.TypeOfMedicalProfessionalSelect.hideError();
        }

        void HandleOnInvalidClaim(object sender, EventArgs e)
        {
            Console.WriteLine("invalid claim");

            if (_model.EmptyGSCNumber || _model.InvalidGSCNumber)
            {
                claimOtherBenefits.OtherGscNumberField.showError(_model.InvalidGSCNumber ? "invalidGSCNumber".FormatWithBrandKeywords(LocalizableBrand.GSC) : "otherBenefitsGSCNumberError".FormatWithBrandKeywords(LocalizableBrand.GSC));
            }
            else
            {
                claimOtherBenefits.OtherGscNumberField.hideError();
            }

            if (_model.InvalidDateOfAccident || _model.MissingDateOfAccident)
            {
                claimMotorVehicle.DateOfMotorVehicleAccidentSelect.showError("specifyDateOfInjury".tr());
            }
            else
            {
                claimMotorVehicle.DateOfMotorVehicleAccidentSelect.hideError();
            }

            if (_model.DateOfReferralError || _model.DateOfReferralTooOld)
            {
                claimMedicalItem.DateOfReferral.showError("dateofPrescription12Months".tr());
            }
            else
            {
                claimMedicalItem.DateOfReferral.hideError();
            }

            if (_model.InvalidDateOfInjury || _model.MissingDateOfInjury)
            {
                claimWorkInjury.DateOfWorkRelatedInjurySelect.showError("specifyDateOfInjury".tr());
            }
            else
            {
                claimWorkInjury.DateOfWorkRelatedInjurySelect.hideError();
            }

            if (_model.MissingCaseNumber)
            {
                claimWorkInjury.WorkRelatedInjuryCaseNumberField.showError("workInjuryCaseNumberError".tr());
            }
            else
            {
                claimWorkInjury.WorkRelatedInjuryCaseNumberField.hideError();
            }

            if (_model.EmptyTypeOfMedicalProfessional)
            {
                claimMedicalItem.TypeOfMedicalProfessionalSelect.showError("medicalProfessionalError".tr());
            }
            else
            {
                claimMedicalItem.TypeOfMedicalProfessionalSelect.hideError();
            }
        }

        private void HandleVisibilityToggled(object sender, EventArgs e)
        {
            CGRect claimOtherFrame = claimOtherBenefits.View.Frame;
            CGRect claimHCSAFrame = claimOtherBenefitsHCSA.View.Frame;
            CGRect claimMotorVehicleFrame = claimMotorVehicle.View.Frame;
            CGRect claimWorkInjuryFrame = claimWorkInjury.View.Frame;
            CGRect claimMedicalItemFrame = CGRect.Empty;
            CGRect claimGSTFrame = claimOtherBenefitsGST.View.Frame;
            CGRect massageNoticeFrame = massageNotice.Frame;

            if (medicalItemVisible)
            {
                claimMedicalItemFrame = (CGRect)claimMedicalItem.View.Frame;
            }

            CGRect treatmentDetailsButtonFrame = treatmentDetailsButton.Frame;

            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float yPos = 0;
            yPos = (float)claimOtherFrame.Y + claimOtherBenefits.CurrentHeight + itemPadding;

            if (!claimOtherBenefitsHCSA.View.Hidden)
            {
                claimHCSAFrame.Y = (float)(yPos + claimOtherBenefitsHCSALabel.Frame.Height + itemPadding);
                yPos += (float)claimHCSAFrame.Height + itemPadding;
            }

            claimMotorVehicleFrame.Y = yPos;
            claimWorkInjuryFrame.Y = claimMotorVehicleFrame.Y + claimMotorVehicle.CurrentHeight + itemPadding;

            yPos = (float)claimWorkInjuryFrame.Y + claimWorkInjury.CurrentHeight + itemPadding;
            if (medicalItemVisible)
            {
                claimMedicalItemFrame.Y = yPos;
                yPos += claimMedicalItem.CurrentHeight + itemPadding;
            }

            if (!claimOtherBenefitsGST.View.Hidden)
            {
                claimGSTFrame.Y = yPos;
                yPos += (float)claimGSTFrame.Height + itemPadding;
            }

            if (!massageNotice.Hidden)
            {
                massageNoticeFrame.Y = yPos;
                yPos += (float)massageNoticeFrame.Height + itemPadding;
            }

            treatmentDetailsButtonFrame.Y = yPos;

            UIView.Animate(Constants.TOGGLE_ANIMATION_DURATION, 0, UIViewAnimationOptions.CurveEaseInOut,
                () =>
                {
                    if (!claimOtherBenefitsHCSA.View.Hidden)
                    {
                        claimOtherBenefitsHCSA.View.Frame = claimHCSAFrame;
                    }

                    claimMotorVehicle.View.Frame = claimMotorVehicleFrame;
                    claimWorkInjury.View.Frame = claimWorkInjuryFrame;
                    if (medicalItemVisible)
                    {
                        claimMedicalItem.View.Frame = claimMedicalItemFrame;
                    }
                    if (!claimOtherBenefitsGST.View.Hidden)
                    {
                        claimOtherBenefitsGST.View.Frame = claimGSTFrame;
                    }
                    if (!massageNotice.Hidden)
                    {
                        massageNotice.Frame = massageNoticeFrame;
                    }
                    treatmentDetailsButton.Frame = treatmentDetailsButtonFrame;
                    float ViewContainerWidth = (float)base.View.Frame.Width;
                    scrollContainer.ContentSize = new CGSize(ViewContainerWidth, treatmentDetailsButton.Frame.Y + treatmentDetailsButton.Frame.Height + itemPadding);
                },
                () =>
                {
                    //COMPLETION
                    View.SetNeedsLayout();

                }
            );
        }

        int redrawCount = 0;
        float medicalItemHeight = 0;
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float startY = ViewContentYPositionPadding;
            float extraPos = Constants.IOS_7_TOP_PADDING;
            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float yPos = startY;

            scrollContainer.Frame = participantBlockOverlay.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);

            float providerYPos = 0;

            providerTitle.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)providerTitle.Frame.Height);
            providerTitle.SizeToFit();

            yPos += (float)providerTitle.Frame.Height;

            providerName.Frame = new CGRect(0, providerYPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            providerYPos += (float)providerName.Frame.Height;
            providerAddress.Frame = new CGRect(0, providerYPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            providerYPos += (float)providerAddress.Frame.Height;
            providerCity.Frame = new CGRect(0, providerYPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            providerYPos += (float)providerCity.Frame.Height;
            providerPhone.Frame = new CGRect(0, providerYPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            providerYPos += (float)providerPhone.Frame.Height;
            registrationNumber.Frame = new CGRect(0, providerYPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            providerYPos += (float)registrationNumber.Frame.Height;

            providerContainer.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, providerYPos);
            yPos += (float)providerContainer.Frame.Height + Constants.DRUG_LOOKUP_TOP_PADDING;

            planParticpantLabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)planParticpantLabel.Frame.Height);
            planParticpantLabel.SizeToFit();

            float componentYPos = 0;

            claimOtherBenefits.View.Frame = new CGRect(0, (float)planParticpantLabel.Frame.Y + (float)planParticpantLabel.Frame.Height + itemPadding, ViewContainerWidth, claimOtherBenefits.CurrentHeight);

            componentYPos += (float)claimOtherBenefits.View.Frame.Y + claimOtherBenefits.CurrentHeight + itemPadding;

            if (!claimOtherBenefitsHCSA.View.Hidden)
            {
                claimOtherBenefitsHCSALabel.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, componentYPos, ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2, (float)claimOtherBenefitsHCSALabel.Frame.Height);
                claimOtherBenefitsHCSALabel.SizeToFit();

                componentYPos += (float)(claimOtherBenefitsHCSALabel.Frame.Height + itemPadding);
                claimOtherBenefitsHCSA.View.Frame = new CGRect(0, componentYPos, ViewContainerWidth, claimOtherBenefitsHCSA.CurrentHeight);
                componentYPos += claimOtherBenefitsHCSA.CurrentHeight + itemPadding;
            }

            claimMotorVehicle.View.Frame = new CGRect(0, componentYPos, ViewContainerWidth, claimMotorVehicle.CurrentHeight);
            claimWorkInjury.View.Frame = new CGRect(0, (float)claimMotorVehicle.View.Frame.Y + claimMotorVehicle.CurrentHeight + itemPadding, ViewContainerWidth, claimWorkInjury.CurrentHeight);

            componentYPos = (float)claimWorkInjury.View.Frame.Y + claimWorkInjury.CurrentHeight + itemPadding;

            if (medicalItemVisible)
            {
                claimMedicalItem.View.Frame = new CGRect(0, componentYPos, ViewContainerWidth, claimMedicalItem.CurrentHeight);
                componentYPos += claimMedicalItem.CurrentHeight + itemPadding;
                if (medicalItemHeight != claimMedicalItem.CurrentHeight)
                {
                    medicalItemHeight = claimMedicalItem.CurrentHeight;
                    redrawCount = 0;
                }
            }

            if (!claimOtherBenefitsGST.View.Hidden)
            {
                claimOtherBenefitsGST.View.Frame = new CGRect(0, componentYPos, ViewContainerWidth, claimOtherBenefitsGST.CurrentHeight);
                componentYPos += claimOtherBenefitsGST.CurrentHeight + itemPadding;
            }

            if (!massageNotice.Hidden)
            {
                massageNotice.Frame = new CGRect(sidePadding, componentYPos, ViewContainerWidth - sidePadding * 2, (float)massageNotice.Frame.Height);
                massageNotice.SizeToFit();
                massageNotice.Frame = new CGRect(sidePadding, componentYPos, ViewContainerWidth - sidePadding * 2, (float)massageNotice.Frame.Height);
                componentYPos += (float)massageNotice.Frame.Height + itemPadding;
            }

            treatmentDetailsButton.Frame = new CGRect(ViewContainerWidth / 2 - BUTTON_WIDTH / 2, componentYPos, BUTTON_WIDTH, BUTTON_HEIGHT);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, treatmentDetailsButton.Frame.Y + treatmentDetailsButton.Frame.Height + itemPadding + GetBottomPadding());

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
                return (float)base.View.Frame.Width; ;
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
                return Constants.IOS_7_TOP_PADDING;
            }

        }

        private List<ClaimOption> _typesOfMedicalProfessional;
        public List<ClaimOption> TypesOfMedicalProfessional
        {
            get
            {
                return _typesOfMedicalProfessional;
            }
            set
            {
                _typesOfMedicalProfessional = value;
                if (claimMedicalItem != null)
                {
                    claimMedicalItem.TypeOfMedicalProfessionalSelect.listController.tableView.ReloadData();
                }
            }
        }

        private bool _dateIsSet;
        public bool DateIsSet
        {
            get
            {
                return _dateIsSet;
            }
            set
            {
                _dateIsSet = value;
            }
        }

        private Participant _participant;
        public Participant Participant
        {
            get
            {
                return _participant;
            }
            set
            {
                _participant = value;
                if (value != null)
                {
                    participantBlockOverlay.RemoveFromSuperview();
                    treatmentDetailsButton.SetTitle("claimDetailsFor".tr() + _participant.FullName.ToUpper(), UIControlState.Normal);
                }
                else
                {
                    View.AddSubview(participantBlockOverlay);
                    treatmentDetailsButton.SetTitle("claimDetailsFor".tr(), UIControlState.Normal);
                }
            }
        }
    }
}