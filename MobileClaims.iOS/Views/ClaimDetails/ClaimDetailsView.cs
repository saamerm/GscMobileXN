using System;
using System.Threading.Tasks;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimDetails
{
    public partial class ClaimDetailsView : GSCBaseViewController, IRehydrating
    {
        private const int _animationDelay = 1000;
        private Participant _participant;
        private ClaimDetailsViewModel _viewModel;

        public bool Rehydrating { get; set; }

        public bool FinishedRehydrating { get; set; }

        public Participant Participant
        {
            get => _participant;
            set => _participant = value;
        }

        public ClaimDetailsView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);

            UnsubscribeEventHandlers();
            SubscribeEventHandlers();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            View.EndEditing(animated);

            OtherGSCNumberTextField.ShouldShowError = false;
            AccidentDatePicker.ShouldShowError = false;
            WorkInjuryDatePicker.ShouldShowError = false;
            CaseNumberTextField.ShouldShowError = false;
            MedicalProfessionTypeTableView.ShouldShowError = false;

            UnsubscribeEventHandlers();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (Rehydrating)
            {
                rehydrationservice.Rehydrating = Rehydrating;
            }

            _viewModel = (ClaimDetailsViewModel)ViewModel;

            UnsubscribeEventHandlers();
            SubscribeEventHandlers();

            HealthProviderTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimDetailsSectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            HealthProviderNameLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetaulsHealthProviderDetailsFontSize, Colors.Black);
            HealthProviderAddressLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetaulsHealthProviderDetailsFontSize, Colors.Black);
            HealthProviderCityLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetaulsHealthProviderDetailsFontSize, Colors.Black);
            HealthProviderPhoneNumberLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetaulsHealthProviderDetailsFontSize, Colors.Black);
            HealthProviderRegNumberLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetaulsHealthProviderDetailsFontSize, Colors.Black);

            ClaimDetailsHeaderLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimDetailsSectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            OtherBenefitsHCSALabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimDetailsSectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            NoticeLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.ClaimDetaulsHealthProviderDetailsFontSize, Colors.DARK_GREY_COLOR);

            OtherBenefitsToggle.IsUsedAsGroupHeader = true;
            OtherGSCNumberTextField.SetEnabled(false, false);
            OtherBenefitsHCSAToggle.IsUsedAsGroupHeader = true;

            ReasonOfAccidentToggle.IsUsedAsGroupHeader = true;

            MotorVehicleAccidentToggle.IsUsedAsGroupHeader = false;
            AccidentDatePicker.CanClearDate = false;

            WorkRelatedInjuryToggle.IsUsedAsGroupHeader = false;
            WorkInjuryDatePicker.CanClearDate = false;
            WorkInjuryDatePicker.ParentController = this;

            OtherTypeOfAccidentToggle.IsUsedAsGroupHeader = false;

            IsMedicalPrescriptionAvailableToggle.IsUsedAsGroupHeader = true;
            MedPrescriptionReferralDatePicker.CanClearDate = true;
            GstHstToggle.IsUsedAsGroupHeader = true;

            SetBindings();

            (this.ViewModel as ViewModelBase).CreatedFromRehydration = false;
            rehydrationservice.Rehydrating = false;

            this.View.SetNeedsLayout();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            OtherBenefitsToggleHeightConstraint.Constant = OtherBenefitsToggle.EstimatedHeight;
            OtherBenefitsHCSAToggleHeightConstraint.Constant = OtherBenefitsHCSAToggle.EstimatedHeight;
            MotorVehicleAccidentToggleHeightConstraint.Constant = MotorVehicleAccidentToggle.EstimatedHeight;
            ReasonOfAccidentToggleHeightConstraint.Constant = ReasonOfAccidentToggle.EstimatedHeight;
            WorkRelatedInjuryToggleHeightConstraint.Constant = WorkRelatedInjuryToggle.EstimatedHeight;
            IsMedicalPrescriptionAvailableToggleHeightConstraint.Constant = IsMedicalPrescriptionAvailableToggle.EstimatedHeight;
            MedProfessionTypeTableViewHeightConstraint.Constant = MedicalProfessionTypeTableView.EstimatedHeight;
            GstHstToggleHeightConstraint.Constant = GstHstToggle.EstimatedHeight;
            OtherTypeOfAccidentToggleHightConstraint.Constant = OtherTypeOfAccidentToggle.EstimatedHeight;
        }

        private void SubscribeEventHandlers()
        {
            OtherBenefitsToggle.SwitchValueChanged += OtherBenefitsSwitch_ValueChanged;
            SubmittedWithOtherBenefitsToggle.SwitchValueChanged += SubmittedWithOtherBenefitsToggle_SwitchValueChanged;
            PayUnpaidAmountToggle.SwitchValueChanged += PayUnpaidAmountToggle_SwitchValueChanged;

            ReasonOfAccidentToggle.SwitchValueChanged += ReasonOfAccidentToggle_SwitchValueChanged;

            MotorVehicleAccidentToggle.SwitchValueChanged += MotorVehicleAccident_ValueChanged;
            WorkRelatedInjuryToggle.SwitchValueChanged += WorkRelatedInjury_ValueChanged;
            OtherTypeOfAccidentToggle.SwitchValueChanged += OtherTypeOfAccidentToggle_SwitchValueChanged;

            IsMedicalPrescriptionAvailableToggle.SwitchValueChanged += IsMedicalPrescriptionAvailable_ValueChanged;

            MedicalProfessionTypeTableView.ItemSelectionRemoved += MedicalProfessionTypeTableView_ItemSelectionRemoved;

            MedPrescriptionReferralDatePicker.DateSelected += MedPrescriptionReferralDatePicker_DateSelected;
            MedPrescriptionReferralDatePicker.DateSelectionRemoved += MedPrescriptionReferralDatePicker_DateSelectionRemoved;
        }

        private void UnsubscribeEventHandlers()
        {
            OtherBenefitsToggle.SwitchValueChanged -= OtherBenefitsSwitch_ValueChanged;
            SubmittedWithOtherBenefitsToggle.SwitchValueChanged -= SubmittedWithOtherBenefitsToggle_SwitchValueChanged;
            PayUnpaidAmountToggle.SwitchValueChanged -= PayUnpaidAmountToggle_SwitchValueChanged;

            ReasonOfAccidentToggle.SwitchValueChanged -= ReasonOfAccidentToggle_SwitchValueChanged;

            MotorVehicleAccidentToggle.SwitchValueChanged -= MotorVehicleAccident_ValueChanged;
            WorkRelatedInjuryToggle.SwitchValueChanged -= WorkRelatedInjury_ValueChanged;
            OtherTypeOfAccidentToggle.SwitchValueChanged -= OtherTypeOfAccidentToggle_SwitchValueChanged;

            IsMedicalPrescriptionAvailableToggle.SwitchValueChanged -= IsMedicalPrescriptionAvailable_ValueChanged;

            MedicalProfessionTypeTableView.ItemSelectionRemoved -= MedicalProfessionTypeTableView_ItemSelectionRemoved;

            MedPrescriptionReferralDatePicker.DateSelected -= MedPrescriptionReferralDatePicker_DateSelected;
            MedPrescriptionReferralDatePicker.DateSelectionRemoved -= MedPrescriptionReferralDatePicker_DateSelectionRemoved;
        }

        private void SetBindings()
        {
            DismmssVCTableViewSource tableSource = new DismmssVCTableViewSource(
                MedicalProfessionTypeTableView.PopoverController,
                MedicalProfessionTypeTableView.TableViewController.tableView,
                "ClaimOptionTableCell", typeof(ClaimOptionTableCell));
            MedicalProfessionTypeTableView.TableViewController.tableView.Source = tableSource;

            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var nullObjectToBoolConverter = new NullObjectToBoolValueConverter<Participant>();

            var set = this.CreateBindingSet<ClaimDetailsView, ClaimDetailsViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(HealthProviderTitleLabel).For(x => x.Hidden).To(vm => vm.ServiceProviderVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(HealthProviderInfoStackView).For(x => x.Hidden).To(vm => vm.ServiceProviderVisible).WithConversion(boolOppositeValueConverter, null);

            set.Bind(HealthProviderTitleLabel).To(vm => vm.ClaimSubmissionType.Name).WithConversion("ClaimServiceProvider");
            set.Bind(HealthProviderNameLabel).To(vm => vm.ServiceProvider.BusinessName);
            set.Bind(HealthProviderAddressLabel).To(vm => vm.ServiceProvider.Address);
            set.Bind(HealthProviderCityLabel).To(vm => vm.ServiceProvider.FormattedAddress);
            set.Bind(HealthProviderPhoneNumberLabel).To(vm => vm.ServiceProvider.Phone).WithConversion("PhonePrefix");
            set.Bind(HealthProviderRegNumberLabel).To(vm => vm.ServiceProvider.RegistrationNumber).WithConversion("RegistrationNumPrefix");
            set.Bind(ClaimDetailsHeaderLabel).To(vm => vm.Participant.FullName).WithConversion("ClaimDetailsForPrefix");

            set.Bind(OtherBenefitsToggle).For(x => x.SwitchValue).To(vm => vm.ClaimOtherBenefitsViewModel.CoverageUnderAnotherBenefitsPlan);
            set.Bind(OtherBenefitsToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherBenefitsTitle);

            set.Bind(GSCCoverageToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherBenefitsWithGsc);
            set.Bind(GSCCoverageToggle).For(x => x.SwitchValue).To(vm => vm.ClaimOtherBenefitsViewModel.IsOtherCoverageWithGSC);

            set.Bind(SubmittedWithOtherBenefitsToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherBenefitsSubmitted);
            set.Bind(SubmittedWithOtherBenefitsToggle).For(x => x.SwitchValue).To(vm => vm.ClaimOtherBenefitsViewModel.HasClaimBeenSubmittedToOtherBenefitPlan);

            set.Bind(PayUnpaidAmountToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherBenefitsPayBalanceThroughOtherGSC);
            set.Bind(PayUnpaidAmountToggle).For(x => x.SwitchValue).To(vm => vm.ClaimOtherBenefitsViewModel.PayAnyUnpaidBalanceThroughOtherGSCPlan);

            set.Bind(OtherGSCNumberTextField.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherBenefitsEnterGSC);
            set.Bind(OtherGSCNumberTextField.TextField).To(vm => vm.ClaimOtherBenefitsViewModel.OtherGSCNumber);
            set.Bind(OtherGSCNumberTextField).For(x => x.ErrorText).To(vm => vm.GSCNumberValidationErrorText);
            set.Bind(OtherGSCNumberTextField).For(x => x.ShouldShowError).To(vm => vm.GSCNumberError);

            set.Bind(OtherBenefitsHCSAToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherBenefitsHSCA);
            set.Bind(OtherBenefitsHCSAToggle).For(x => x.SwitchValue).To(vm => vm.ClaimOtherBenefitsViewModel.PayUnderHCSA);
            set.Bind(OtherBenefitsHCSAToggle).For(x => x.Hidden).To(vm => vm.ClaimOtherBenefitsViewModel.PayUnderHCSAVisible).WithConversion("BoolOpposite");
            set.Bind(OtherBenefitsHCSALabel).To(vm => vm.ClaimOtherBenefitsViewModel.AlternateHCSASubtitle);
            set.Bind(OtherBenefitsHCSALabel).For(x => x.Hidden).To(vm => vm.ClaimOtherBenefitsViewModel.PayUnderHCSAVisible).WithConversion("BoolOpposite");

            set.Bind(ReasonOfAccidentToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsReasonOfAccident);
            set.Bind(ReasonOfAccidentToggle).For(x => x.SwitchValue).To(vm => vm.IsClaimDueToAccidentViewModel.IsClaimDueToAccident);

            set.Bind(MotorVehicleAccidentToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsMotorVehicleTitle);
            set.Bind(MotorVehicleAccidentToggle).For(x => x.SwitchValue).To(vm => vm.ClaimMotorVehicleViewModel.IsTreatmentDueToAMotorVehicleAccident);

            set.Bind(AccidentDatePicker.Label).For(x => x.Text).To(vm => vm.ClaimDetailsMotorVehicleDate);
            set.Bind(AccidentDatePicker.DatePickerController.datePicker).To(vm => vm.ClaimMotorVehicleViewModel.DateOfMotorVehicleAccident);
            set.Bind(AccidentDatePicker.DetailsLabel).To(vm => vm.ClaimMotorVehicleViewModel.DateOfMotorVehicleAccident).WithConversion("DateToString").OneWay();
            set.Bind(AccidentDatePicker).For(x => x.ErrorText).To(vm => vm.AccidentDateValidationErrorText);
            set.Bind(AccidentDatePicker).For(x => x.ShouldShowError).To(vm => vm.DateOfAccidentError);

            set.Bind(WorkRelatedInjuryToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsWorkInjuryTitle);
            set.Bind(WorkRelatedInjuryToggle).For(x => x.SwitchValue).To(vm => vm.ClaimWorkInjuryViewModel.IsTreatmentDueToAWorkRelatedInjury);

            set.Bind(WorkInjuryDatePicker.Label).For(x => x.Text).To(vm => vm.ClaimDetailsWorkInjuryDate);
            set.Bind(WorkInjuryDatePicker.DetailsLabel).To(vm => vm.ClaimWorkInjuryViewModel.DateOfWorkRelatedInjury).WithConversion("DateToString").OneWay();
            set.Bind(WorkInjuryDatePicker.DatePickerController.datePicker).To(vm => vm.ClaimWorkInjuryViewModel.DateOfWorkRelatedInjury);
            set.Bind(WorkInjuryDatePicker).For(x => x.ErrorText).To(vm => vm.WorkInjuryDateValidationErrorText);
            set.Bind(WorkInjuryDatePicker).For(x => x.ShouldShowError).To(vm => vm.DateOfInjuryError);

            set.Bind(CaseNumberTextField.Label).For(x => x.Text).To(vm => vm.ClaimDetailsWorkInjuryCaseNumber);
            set.Bind(CaseNumberTextField.TextField).To(vm => vm.ClaimWorkInjuryViewModel.WorkRelatedInjuryCaseNumber);
            set.Bind(CaseNumberTextField).For(x => x.ErrorText).To(vm => vm.WorkInjuryCaseNumberValidationErrorText);
            set.Bind(CaseNumberTextField).For(x => x.ShouldShowError).To(vm => vm.MissingCaseNumber);

            set.Bind(IsMedicalPrescriptionAvailableToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsMedicalItemTitle);
            if (_viewModel.ClaimMedicalItemViewModel.Questions13Through15Visible)
            {
                set.Bind(IsMedicalPrescriptionAvailableToggle).For(x => x.SwitchValue).To(vm => vm.ClaimMedicalItemViewModel.HasReferralBeenPreviouslySubmitted);
            }
            else
            {
                if (_viewModel.ClaimMedicalItemViewModel.Question12Visible)
                {
                    set.Bind(IsMedicalPrescriptionAvailableToggle).For(x => x.SwitchValue).To(vm => vm.ClaimMedicalItemViewModel.IsMedicalItemForSportsOnly);
                }
            }
            set.Bind(IsMedicalPrescriptionAvailableToggle).For(x => x.Hidden).To(vm => vm.ClaimMedicalItemViewModel.Questions13Through15Visible).WithConversion("BoolOpposite");

            set.Bind(MedPrescriptionReferralDatePicker.Label).For(x => x.Text).To(vm => vm.ClaimDetailsMedicalItemReferralDate);
            set.Bind(MedPrescriptionReferralDatePicker.DatePickerController.datePicker).To(vm => vm.ClaimMedicalItemViewModel.DateOfReferral);
            set.Bind(MedPrescriptionReferralDatePicker.DetailsLabel).To(vm => vm.ClaimMedicalItemViewModel.DateOfReferral).WithConversion("DateToString").OneWay();
            set.Bind(MedPrescriptionReferralDatePicker).For(x => x.ErrorText).To(vm => vm.MedicalReferralDateValidationErrorText);
            set.Bind(MedPrescriptionReferralDatePicker).For(x => x.ShouldShowError).To(vm => vm.DateOfReferralError);
            set.Bind(MedPrescriptionReferralDatePicker).For(x => x.ShouldUseCurrentDefaultDate).To(vm => vm.ClaimMedicalItemViewModel.IsDateOfReferralSetByUser).OneWay();

            set.Bind(MedicalProfessionTypeTableView.Label).For(x => x.Text).To(vm => vm.ClaimDetailsMedicalItemProfessional);
            set.Bind(MedicalProfessionTypeTableView.DetailsLabel).To(vm => vm.ClaimMedicalItemViewModel.TypeOfMedicalProfessional.Name);
            set.Bind(MedicalProfessionTypeTableView).For(x => x.ErrorText).To(vm => vm.MedicalProfessionValidationErrorText);
            set.Bind(MedicalProfessionTypeTableView).For(x => x.ShouldShowError).To(vm => vm.EmptyTypeOfMedicalProfessional);

            set.Bind(tableSource).To(vm => vm.ClaimMedicalItemViewModel.TypesOfMedicalProfessional);
            set.Bind(tableSource).For(s => s.SelectedItem).To(vm => vm.ClaimMedicalItemViewModel.TypeOfMedicalProfessional);

            set.Bind(GstHstToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsGstIncludedQ);
            set.Bind(GstHstToggle).For(x => x.SwitchValue).To(vm => vm.ClaimMedicalItemViewModel.IsGSTHSTIncluded);
            set.Bind(GstHstToggle).For(x => x.Hidden).To(vm => vm.ClaimMedicalItemViewModel.Question16Visible).WithConversion("BoolOpposite");

            set.Bind(OtherTypeOfAccidentToggle.Label).For(x => x.Text).To(vm => vm.ClaimDetailsOtherTypeOfAccident);
            set.Bind(OtherTypeOfAccidentToggle).For(x => x.SwitchValue).To(vm => vm.ClaimDentalViewModel.IsOtherTypeOfAccident);
            set.Bind(OtherTypeOfAccidentToggle).For(x => x.Hidden).To(vm => vm.ClaimDentalViewModel.IsOtherTypeOfAccidentVisible).WithConversion(boolOppositeValueConverter, null);

            set.Bind(NoticeLabel).To(vm => vm.ClaimDetailsMassageNoticeText);
            set.Bind(NoticeLabel).For(x => x.Hidden).To(vm => vm.ClaimMedicalItemViewModel.MassageOntarioMessageVisible).WithConversion("BoolOpposite");

            set.Bind(SubmitButton).To(vm => vm.TreatmentDetailsClickCommand);
            set.Bind(SubmitButton).For("Title").To(vm => vm.ClaimDetailsForButtonText);

            set.Bind(ScrollView).For(x => x.UserInteractionEnabled).To(vm => vm.Participant).WithConversion(nullObjectToBoolConverter, true);
            set.Bind(ContentView).For(x => x.UserInteractionEnabled).To(vm => vm.Participant).WithConversion(nullObjectToBoolConverter, true);
            set.Bind(ContentOverlayView).For(x => x.Hidden).To(vm => vm.Participant).WithConversion(nullObjectToBoolConverter, true);
            set.Apply();

            if (_viewModel.ClaimMedicalItemViewModel.TypeOfMedicalProfessional != null)
            {
                MedicalProfessionTypeTableView.ShowDetails();
            }
        }

        private void MedPrescriptionReferralDatePicker_DateSelected(object sender, EventArgs e)
        {
            _viewModel.ClaimMedicalItemViewModel.DateOfReferral = DateTime.Now;
        }

        private void MedPrescriptionReferralDatePicker_DateSelectionRemoved(object sender, EventArgs e)
        {
            _viewModel.ClaimMedicalItemViewModel.DateOfReferral = DateTime.MinValue;
        }

        private void MedicalProfessionTypeTableView_ItemSelectionRemoved(object sender, EventArgs e)
        {
            _viewModel.ClaimMedicalItemViewModel.TypeOfMedicalProfessional = null;
        }

        private void PayUnpaidAmountToggle_SwitchValueChanged(object sender, EventArgs e)
        {
            OtherGSCNumberTextField.ShouldShowError = false;
            OtherGSCNumberTextField.SetEnabled(PayUnpaidAmountToggle.SwitchValue, true);
        }

        private void SubmittedWithOtherBenefitsToggle_SwitchValueChanged(object sender, EventArgs e)
        {
            //PayUnpaidAmountToggle.SwitchValue = false;
            OtherGSCNumberTextField.Value = string.Empty;
            if (SubmittedWithOtherBenefitsToggle.SwitchValue)
            {
                DisableOtherGSCNumberFields();
            }
            else
            {
                PayUnpaidAmountToggle.SetEnabled(true, true);
            }
        }

        private void DisableOtherGSCNumberFields()
        {
            PayUnpaidAmountToggle.SetEnabled(false, true);
            OtherGSCNumberTextField.SetEnabled(false, false);
        }

        private void OtherBenefitsSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (OtherBenefitsToggle.SwitchValue)
            {
                AnimateStackView(OtherBenefitsStackView, new UIView[] { GSCCoverageToggle, SubmittedWithOtherBenefitsToggle, PayUnpaidAmountToggle }, false);
            }
            else
            {
                AnimateStackView(OtherBenefitsStackView,
                    new UIView[] { GSCCoverageToggle, SubmittedWithOtherBenefitsToggle, PayUnpaidAmountToggle },
                    true);
            }
        }

        private void ReasonOfAccidentToggle_SwitchValueChanged(object sender, EventArgs e)
        {
            if (ReasonOfAccidentToggle.SwitchValue)
            {
                AnimateStackView(ReasonOfAccidentStackView,
                    new UIView[] { MotorVehicleAccidentToggle, WorkRelatedInjuryToggle, OtherTypeOfAccidentToggle },
                    false);
            }
            else
            {
                if (MotorVehicleAccidentToggle.SwitchValue)
                {
                    MotorVehicleAccidentToggle.SwitchValue = false;
                }

                if (WorkRelatedInjuryToggle.SwitchValue)
                {
                    WorkRelatedInjuryToggle.SwitchValue = false;
                }

                if (OtherTypeOfAccidentToggle.SwitchValue)
                {
                    OtherTypeOfAccidentToggle.SwitchValue = false;
                }

                AnimateStackView(ReasonOfAccidentStackView,
                  new UIView[] { MotorVehicleAccidentToggle, WorkRelatedInjuryToggle, OtherTypeOfAccidentToggle },
                  true);
            }
        }

        private async void MotorVehicleAccident_ValueChanged(object sender, EventArgs e)
        {
            if (MotorVehicleAccidentToggle.SwitchValue)
            {
                AnimateStackView(MotorVehicleAccidentStackView,
                    new UIView[] { AccidentDatePicker },
                    false);

                if (WorkRelatedInjuryToggle.SwitchValue)
                {
                    await Task.Delay(_animationDelay);
                    WorkRelatedInjuryToggle.SwitchValue = false;
                }

                if (OtherTypeOfAccidentToggle.SwitchValue)
                {
                    OtherTypeOfAccidentToggle.SwitchValue = false;
                }
            }
            else
            {
                AnimateStackView(MotorVehicleAccidentStackView,
                    new UIView[] { AccidentDatePicker },
                    true);
            }
        }

        private async void WorkRelatedInjury_ValueChanged(object sender, EventArgs e)
        {
            if (WorkRelatedInjuryToggle.SwitchValue)
            {
                AnimateStackView(WorkRelatedInjuryStackView,
                    new UIView[] { WorkInjuryDatePicker, CaseNumberTextField },
                    false);

                if (MotorVehicleAccidentToggle.SwitchValue)
                {
                    await Task.Delay(_animationDelay);
                    MotorVehicleAccidentToggle.SwitchValue = false;
                }

                if (OtherTypeOfAccidentToggle.SwitchValue)
                {
                    OtherTypeOfAccidentToggle.SwitchValue = false;
                }
            }
            else
            {
                AnimateStackView(WorkRelatedInjuryStackView,
                    new UIView[] { WorkInjuryDatePicker, CaseNumberTextField },
                    true);
            }
        }

        private void OtherTypeOfAccidentToggle_SwitchValueChanged(object sender, EventArgs e)
        {
            if (OtherTypeOfAccidentToggle.SwitchValue)
            {
                if (MotorVehicleAccidentToggle.SwitchValue)
                {
                    MotorVehicleAccidentToggle.SwitchValue = false;
                }

                if (WorkRelatedInjuryToggle.SwitchValue)
                {
                    WorkRelatedInjuryToggle.SwitchValue = false;
                }
            }
        }

        private void IsMedicalPrescriptionAvailable_ValueChanged(object sender, EventArgs e)
        {
            if (IsMedicalPrescriptionAvailableToggle.SwitchValue)
            {
                AnimateStackView(IsMedicalPrescriptionAvailableStackView,
                    new UIView[] { MedPrescriptionReferralDatePicker },
                    false);
            }
            else
            {
                AnimateStackView(IsMedicalPrescriptionAvailableStackView,
                    new UIView[] { MedPrescriptionReferralDatePicker },
                    true);
            }
        }

        private void AnimateStackView(UIView parentView, UIView[] subViews, bool hide)
        {
            UIView.AnimateKeyframes(0.5, 0, UIViewKeyframeAnimationOptions.CalculationModeLinear,
            () =>
            {
                if (!hide)
                {
                    UIView.AddKeyframeWithRelativeStartTime(0.5, 0.9,
                    () =>
                    {
                        parentView.Alpha = 1f;
                    });

                    UIView.AddKeyframeWithRelativeStartTime(0.5, 0.1,
                    () =>
                    {
                        parentView.Hidden = hide;
                    });
                }
                else
                {
                    UIView.AddKeyframeWithRelativeStartTime(0, 0.4,
                     () =>
                     {
                         parentView.Alpha = 0f;
                     });

                    UIView.AddKeyframeWithRelativeStartTime(0.5, 0.9,
                    () =>
                    {
                        parentView.Hidden = hide;
                    });
                }
            },
            (bool finished) =>
            {
                parentView.LayoutIfNeeded();
            });
        }
    }
}