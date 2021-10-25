using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models.Upload.Specialized.PerType;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.Constants;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.Models.Upload;
using Acr.UserDialogs;
using MobileClaims.Core.Extensions;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimDetailsViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IParticipantService _participantservice;
        private readonly IClaimService _claimservice;
        private readonly IDeviceService _deviceservice;
        private readonly IRehydrationService _rehydrationService;
        private readonly IUserDialogs _userDialog;
        private readonly MvxSubscriptionToken _participantselected;
        private readonly MvxSubscriptionToken _shouldcloseself;

        private IClaimExService _claimExService;
        private Participant _participant;
        private ClaimOtherBenefitsViewModel _claimOtherBenefitsViewModel;
        private ReasonOfAccidentViewModel _isClaimDueToAccidentViewModel;
        private ClaimMotorVehicleViewModel _claimMotorVehicleViewModel;
        private ClaimWorkInjuryViewModel _claimWorkInjuryViewModel;
        private ClaimMedicalItemViewModel _claimMedicalItemViewModel;
        private ClaimParticipantsViewModel _claimParticipantViewMode;
        private DentalClaimDetailViewModel _claimDetnalViewModel;

        private bool _missingTreatmentDetails;
        private bool _emptyGSCNumber;
        private bool _invalidGSCNumber;
        private bool _GSCNumberError;
        private bool _missingDateOfAccident;
        private bool _missingDateOfInjury;
        private bool _invalidDateOfAccident;
        private bool _invalidDateOfReferral;
        private bool _invalidDateOfInjury;
        private bool _dateOfAccidentError;
        private bool _dateOfInjuryError;
        private bool _missingCaseNumber;
        private bool _emptyTypeOfMedicalProfessional;
        private bool _dateOfReferralTooOld;
        private bool _dateOfReferralError;
        private bool _serviceProviderVisible;
        private string _claimDetailsForButtonText;
        private string _gscNumberValidationErrorText;
        private string _accidentDateValidationErrorText;
        private string _workInjuryDateValidationErrorText;
        private string _workInjuryCaseNumberValidationErrorText;
        private string _medicalProfessionValidationErrorText;
        private string _medicalReferralDateValidationErrorText;

        public event EventHandler OnInvalidClaim;

        public string Title => Resource.ClaimDetails.ToUpperInvariant();
        public string ClaimDetailsOtherBenefitsTitle { get; private set; } = Resource.ClaimDetailsOtherBenefitsTitle;
        public string ClaimDetailsOtherBenefitsWithGsc => string.Format(Resource.ClaimDetailsOtherBenefitsWithGsc, BrandResource.BrandAcronym);
        public string ClaimDetailsOtherBenefitsSubmitted => Resource.ClaimDetailsOtherBenefitsSubmitted;
        public string ClaimDetailsOtherBenefitsPayBalanceThroughOtherGSC => string.Format(Resource.ClaimDetailsOtherBenefitsPayBalanceThroughOtherGSC, BrandResource.BrandAcronym);
        public string ClaimDetailsOtherBenefitsEnterGSC => string.Format(Resource.ClaimDetailsOtherBenefitsEnterGSC, BrandResource.BrandAcronym);
        public string ClaimDetailsOtherBenefitsHSCA => Resource.ClaimDetailsOtherBenefitsHSCA;
        public string ClaimDetailsReasonOfAccident => Resource.ClaimDetailsReasonOfAccident;
        public string ClaimDetailsMotorVehicleTitle => Resource.ClaimDetailsMotorVehicleTitle;
        public string ClaimDetailsMotorVehicleDate => Resource.ClaimDetailsMotorVehicleDate;
        public string ClaimDetailsWorkInjuryTitle => Resource.ClaimDetailsWorkInjuryTitle;
        public string ClaimDetailsWorkInjuryDate => Resource.ClaimDetailsWorkInjuryDate;
        public string ClaimDetailsWorkInjuryCaseNumber => Resource.ClaimDetailsWorkInjuryCaseNumber;
        public string ClaimDetailsOtherTypeOfAccident => Resource.ClaimDetailsOtherTypeOfAccident;
        public string ClaimDetailsMedicalItemTitle => Resource.ClaimDetailsMedicalItemTitle;
        public string ClaimDetailsMedicalItemReferralDate => Resource.ClaimDetailsMedicalItemReferralDate;
        public string ClaimDetailsMedicalItemProfessional => Resource.ClaimDetailsMedicalItemProfessional;
        public string ClaimDetailsGstIncludedQ => Resource.ClaimDetailsGstIncludedQ;
        public string ClaimDetailsMassageNoticeText => Resource.ClaimDetailsMassageNoticeText;

        public string ClaimDetailsForButtonText
        {
            get => _claimDetailsForButtonText;
            set => SetProperty(ref _claimDetailsForButtonText, value);
        }

        public string GSCNumberValidationErrorText
        {
            get => _gscNumberValidationErrorText;
            set => SetProperty(ref _gscNumberValidationErrorText, value);
        }

        public string WorkInjuryCaseNumberValidationErrorText
        {
            get => _workInjuryCaseNumberValidationErrorText;
            set => SetProperty(ref _workInjuryCaseNumberValidationErrorText, value);
        }

        public string AccidentDateValidationErrorText
        {
            get => _accidentDateValidationErrorText;
            set => SetProperty(ref _accidentDateValidationErrorText, value);
        }

        public string WorkInjuryDateValidationErrorText
        {
            get => _workInjuryDateValidationErrorText;
            set => SetProperty(ref _workInjuryDateValidationErrorText, value);
        }

        public string MedicalProfessionValidationErrorText
        {
            get => _medicalProfessionValidationErrorText;
            set => SetProperty(ref _medicalProfessionValidationErrorText, value);
        }

        public string MedicalReferralDateValidationErrorText
        {
            get => _medicalReferralDateValidationErrorText;
            set => SetProperty(ref _medicalReferralDateValidationErrorText, value);
        }

        public ClaimSubmissionType ClaimSubmissionType => _claimservice.SelectedClaimSubmissionType;

        public ServiceProvider ServiceProvider => _claimservice.Claim?.Provider;

        public Participant Participant
        {
            get => _participant;
            set
            {
                SetProperty(ref _participant, value);
#if CCQ || FPPM
                var isMinorFromQuebecProvice = _participant.IsResidentOfQuebecProvince() && _participant.IsOrUnderAgeOf18();
                if (isMinorFromQuebecProvice && ClaimSubmissionType.ID.IsVisionEnhancement())
                {
                    ClaimDetailsOtherBenefitsTitle = BrandResource.ClaimDetailsOtherBenefitsTitle;
                }
                else
                {
                    ClaimDetailsOtherBenefitsTitle = Resource.ClaimDetailsOtherBenefitsTitle;
                }
#else
                ClaimDetailsOtherBenefitsTitle = Resource.ClaimDetailsOtherBenefitsTitle;
#endif
                ClaimDetailsForButtonText = $"{Resource.ClaimDetailsFor} {_participant?.FullName?.ToUpperInvariant()}";
            }
        }

        public ObservableCollection<TreatmentDetail> TreatmentDetails => _claimservice.Claim?.TreatmentDetails;

        public ClaimOtherBenefitsViewModel ClaimOtherBenefitsViewModel
        {
            get => _claimOtherBenefitsViewModel;
            set => SetProperty(ref _claimOtherBenefitsViewModel, value);
        }

        public ReasonOfAccidentViewModel IsClaimDueToAccidentViewModel
        {
            get => _isClaimDueToAccidentViewModel;
            set => SetProperty(ref _isClaimDueToAccidentViewModel, value);
        }

        public ClaimMotorVehicleViewModel ClaimMotorVehicleViewModel
        {
            get => _claimMotorVehicleViewModel;
            set => SetProperty(ref _claimMotorVehicleViewModel, value);
        }

        public ClaimWorkInjuryViewModel ClaimWorkInjuryViewModel
        {
            get => _claimWorkInjuryViewModel;
            set => SetProperty(ref _claimWorkInjuryViewModel, value);
        }

        public ClaimMedicalItemViewModel ClaimMedicalItemViewModel
        {
            get => _claimMedicalItemViewModel;
            set => SetProperty(ref _claimMedicalItemViewModel, value);
        }

        public ClaimParticipantsViewModel ClaimParticipantsViewModel
        {
            get => _claimParticipantViewMode;
            set => SetProperty(ref _claimParticipantViewMode, value);
        }

        public DentalClaimDetailViewModel ClaimDentalViewModel
        {
            get => _claimDetnalViewModel;
            set => SetProperty(ref _claimDetnalViewModel, value);
        }

        public bool MissingTreatmentDetails
        {
            get => _missingTreatmentDetails;
            set => SetProperty(ref _missingTreatmentDetails, value);
        }

        public bool EmptyGSCNumber
        {
            get => _emptyGSCNumber;
            set => SetProperty(ref _emptyGSCNumber, value);
        }

        public bool InvalidGSCNumber
        {
            get => _invalidGSCNumber;
            set => SetProperty(ref _invalidGSCNumber, value);
        }

        public bool GSCNumberError
        {
            get => _GSCNumberError;
            set => SetProperty(ref _GSCNumberError, value);
        }

        public bool MissingDateOfAccident
        {
            get => _missingDateOfAccident;
            set => SetProperty(ref _missingDateOfAccident, value);
        }

        public bool MissingDateOfInjury
        {
            get => _missingDateOfInjury;
            set => SetProperty(ref _missingDateOfInjury, value);
        }

        public bool InvalidDateOfAccident
        {
            get => _invalidDateOfAccident;
            set => SetProperty(ref _invalidDateOfAccident, value);
        }

        public bool InvalidDateOfReferral
        {
            get => _invalidDateOfReferral;
            set => SetProperty(ref _invalidDateOfReferral, value);
        }

        public bool InvalidDateOfInjury
        {
            get => _invalidDateOfInjury;
            set => SetProperty(ref _invalidDateOfInjury, value);
        }

        public bool DateOfAccidentError
        {
            get => _dateOfAccidentError;
            set => SetProperty(ref _dateOfAccidentError, value);
        }

        public bool DateOfInjuryError
        {
            get => _dateOfInjuryError;
            set => SetProperty(ref _dateOfInjuryError, value);
        }

        public bool MissingCaseNumber
        {
            get => _missingCaseNumber;
            set => SetProperty(ref _missingCaseNumber, value);
        }

        public bool EmptyTypeOfMedicalProfessional
        {
            get => _emptyTypeOfMedicalProfessional;
            set => SetProperty(ref _emptyTypeOfMedicalProfessional, value);
        }

        public bool DateOfReferralTooOld
        {
            get => _dateOfReferralTooOld;
            set => SetProperty(ref _dateOfReferralTooOld, value);
        }

        public bool DateOfReferralError
        {
            get => _dateOfReferralError;
            set => SetProperty(ref _dateOfReferralError, value);
        }

        public bool ServiceProviderVisible
        {
            get => _serviceProviderVisible;
            set => SetProperty(ref _serviceProviderVisible, value);
        }

        public IMvxCommand TreatmentDetailsClickCommand { get; }

        public ClaimDetailsViewModel(IMvxMessenger messenger,
            IParticipantService participantservice,
            IClaimService claimservice,
            IDeviceService deviceservice,
            IRehydrationService rehydrationService,
            IUserDialogs userDialogs)
        {
            _messenger = messenger;
            _participantservice = participantservice;
            _claimservice = claimservice;
            _deviceservice = deviceservice;
            _rehydrationService = rehydrationService;
            _userDialog = userDialogs;

            ClaimOtherBenefitsViewModel = new ClaimOtherBenefitsViewModel(messenger, claimservice, participantservice);
            IsClaimDueToAccidentViewModel = new ReasonOfAccidentViewModel(claimservice);
            ClaimMotorVehicleViewModel = new ClaimMotorVehicleViewModel(messenger, claimservice);
            ClaimWorkInjuryViewModel = new ClaimWorkInjuryViewModel(messenger, claimservice);
            ClaimMedicalItemViewModel = new ClaimMedicalItemViewModel(messenger, claimservice);
            ClaimDentalViewModel = new DentalClaimDetailViewModel(claimservice);

            TreatmentDetailsClickCommand = new MvxCommand(ExecuteTreatmentDetailsClickCommand);

            if (_claimservice.Claim != null)
            {
                Participant = _claimservice.Claim.Participant;
            }
            else if (_participantservice.SelectedParticipant != null)
            {
                Participant = _participantservice.SelectedParticipant;
            }
            else
            {
                Participant = null;
            }

            _participantselected = _messenger.Subscribe<ClaimParticipantSelected>(message =>
            {
                var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
                if (!rehydrationservice.Rehydrating && Participant != null) //participant is being changed
                {
                    ClearClaimData();
                    _messenger.Publish(new ClearClaimDetailsViewRequested(this));
                }
                else
                {
                    rehydrationservice.Rehydrating = false;
                }
                Participant = _participantservice.SelectedParticipant;
            });

            _shouldcloseself = _messenger.Subscribe<ClearClaimDetailsViewRequested>(message =>
            {
                _messenger.Unsubscribe<ClearClaimDetailsViewRequested>(_shouldcloseself);
                _messenger.Unsubscribe<ClaimParticipantSelected>(_participantselected);
                Close(this);
            });
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_deviceservice.CurrentDevice == GSCHelper.OS.Droid)
            {
                _rehydrationService.ProcessEntryPoint = nameof(ClaimDetailsViewModel);
                _rehydrationService.Save();
            }

            ServiceProviderVisible = !ClaimSubmissionType.ID.Equals("DRUG", StringComparison.OrdinalIgnoreCase);
        }

        protected virtual void RaiseInvalidClaim(EventArgs e)
        {
            OnInvalidClaim?.Invoke(this, e);
        }

        private async void ExecuteTreatmentDetailsClickCommand()
        {
            EmptyGSCNumber = false;
            InvalidGSCNumber = false;
            GSCNumberError = false;
            MissingDateOfAccident = false;
            MissingDateOfInjury = false;
            InvalidDateOfAccident = false;
            InvalidDateOfInjury = false;
            DateOfAccidentError = false;
            DateOfInjuryError = false;
            MissingCaseNumber = false;
            InvalidDateOfReferral = false;
            EmptyTypeOfMedicalProfessional = false;
            DateOfReferralTooOld = false;
            DateOfReferralError = false;

            var validatOtherBenefits = ClaimOtherBenefitsViewModel.Validate();
            if (ClaimOtherBenefitsViewModel.OtherGSCNumberEnabled)
            {
                if (!validatOtherBenefits.IsValid)
                {
                    foreach (var failure in validatOtherBenefits.Errors)
                    {
                        var shouldExitForLoop = false;
                        switch (failure.ErrorMessage)
                        {
                            case PlanMemberIdValidationMessages.EmptyString:
                                EmptyGSCNumber = true;
                                GSCNumberValidationErrorText = string.Format(Resource.OtherBenefitsGSCNumberError, BrandResource.BrandAcronym);
                                shouldExitForLoop = true;
                                break;
                            case PlanMemberIdValidationMessages.NonAlphaNumbericGSCPlanMemberId:
                            case PlanMemberIdValidationMessages.InvalidGSCPlanMemberId:
                                InvalidGSCNumber = true;
                                GSCNumberValidationErrorText = string.Format(Resource.InvalidGscNumber, BrandResource.BrandAcronym);
                                break;
                        }
                        if (shouldExitForLoop)
                        {
                            break;
                        }
                    }
                    GSCNumberError = true;
                }
                else
                {
                    _claimExService = Mvx.IoCProvider.Resolve<IClaimExService>();
                    _userDialog.ShowLoading(Resource.Loading);
                    try
                    {
                        var selectedPlanMemberId = string.Empty;
                        var selectedParticipantNumber = string.Empty;
                        var planMemberIdArray = _claimservice.Claim?.Participant?.PlanMemberID?.Split('-');

                        if (planMemberIdArray.Any())
                        {
                            selectedPlanMemberId = planMemberIdArray[0];
                            if (planMemberIdArray.Count() > 1)
                            {
                                selectedParticipantNumber = planMemberIdArray[1];
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Couldn't get required information");
                        }

                        var validationResponse = await _claimExService.ValidateSecondaryPlanMemeberId
                            (selectedPlanMemberId, selectedParticipantNumber, _claimservice.Claim.OtherGSCNumber);

                        _userDialog.HideLoading();
                        if (validationResponse.ValidationstatusCode != 0)
                        {
                            GSCNumberError = true;
                            InvalidGSCNumber = true;
                            GSCNumberValidationErrorText = string.Format(Resource.InvalidGscNumber, BrandResource.BrandAcronym);
                            _userDialog.Alert(string.Format(Resource.InvalidGscNumber, BrandResource.BrandAcronym));
                        }
                    }
                    catch (Exception ex)
                    {
                        GSCNumberError = true;
                        InvalidGSCNumber = true;
                        _userDialog.HideLoading();
                        _userDialog.Alert(Resource.GenericErrorDialogMessage);
                    }
                    finally
                    {
                        _userDialog.HideLoading();
                    }
                }
            }

            bool? isReasonOfAccidentSelected = null;
            if (IsClaimDueToAccidentViewModel.IsClaimDueToAccident)
            {
                isReasonOfAccidentSelected = (ClaimMotorVehicleViewModel.IsTreatmentDueToAMotorVehicleAccident
                || ClaimWorkInjuryViewModel.IsTreatmentDueToAWorkRelatedInjury
                || (ClaimDentalViewModel.IsOtherTypeOfAccidentVisible && ClaimDentalViewModel.IsOtherTypeOfAccident));
            }

            if (isReasonOfAccidentSelected.HasValue && !isReasonOfAccidentSelected.Value)
            {
                _userDialog.Alert(Resource.ClaimDetailsMissingAccidentType);
            }

            var validateMotorVehicle = ClaimMotorVehicleViewModel.Validate();
            if (!validateMotorVehicle.IsValid)
            {
                foreach (var failure in validateMotorVehicle.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Date":
                            MissingDateOfAccident = true;
                            AccidentDateValidationErrorText = Resource.SpecifyDateOfInjury;
                            break;
                        case "Future Date":
                            InvalidDateOfAccident = true;
                            AccidentDateValidationErrorText = Resource.DateofPrescription12Months;
                            break;
                    }
                }
                DateOfAccidentError = true;
            }

            var validateWorkInjury = ClaimWorkInjuryViewModel.Validate();
            if (!validateWorkInjury.IsValid)
            {
                foreach (var failure in validateWorkInjury.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Date":
                            MissingDateOfInjury = true;
                            WorkInjuryDateValidationErrorText = Resource.SpecifyDateOfInjury;
                            break;
                        case "Future Date":
                            InvalidDateOfInjury = true;
                            WorkInjuryDateValidationErrorText = Resource.DateofPrescription12Months;
                            break;
                        case "Empty Case Number":
                            MissingCaseNumber = true;
                            WorkInjuryCaseNumberValidationErrorText = Resource.WorkInjuryCaseNumberError;
                            break;
                    }
                }
            }
            DateOfInjuryError = (MissingDateOfInjury || InvalidDateOfInjury);

            var validateMI = ClaimMedicalItemViewModel.Validate();
            if (!validateMI.IsValid)
            {
                foreach (var failure in validateMI.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Future Date":
                            InvalidDateOfReferral = true;
                            MedicalReferralDateValidationErrorText = Resource.DateNotWithinLast12Months;
                            break;
                        case "Empty TypeOfMedicalProfessional":
                            EmptyTypeOfMedicalProfessional = true;
                            MedicalProfessionValidationErrorText = Resource.MedicalProfessionalError;
                            break;
                        case "DateOfReferral TooOld":
                            DateOfReferralTooOld = true;
                            MedicalReferralDateValidationErrorText = Resource.DateNotWithinLast12Months;
                            break;
                    }
                }
            }
            DateOfReferralError = (DateOfReferralTooOld || InvalidDateOfReferral);

            var errors = new List<bool>
                    {
                        EmptyGSCNumber,
                        InvalidGSCNumber,
                        MissingDateOfAccident,
                        MissingDateOfInjury,
                        InvalidDateOfAccident,
                        InvalidDateOfInjury,
                        MissingCaseNumber,
                        InvalidDateOfReferral,
                        EmptyTypeOfMedicalProfessional,
                        DateOfReferralTooOld,
                        isReasonOfAccidentSelected.HasValue && !isReasonOfAccidentSelected.Value
                    };
            if (errors.Any(b => b))
            {
                RaiseInvalidClaim(new EventArgs());
                return;
            }
            PublishMessages();

            if (_claimservice.Claim.TreatmentDetails.Count == 0)
            {
                ShowAppropriateTDEntryViewModel();
            }
            else
            {
                ShowViewModel<ClaimTreatmentDetailsListViewModel>();
            }
        }

        private void ClearClaimData()
        {
            ClaimOtherBenefitsViewModel.ClearData();
            ClaimMotorVehicleViewModel.ClearData();
            ClaimWorkInjuryViewModel.ClearData();
            ClaimMedicalItemViewModel.ClearData();
            ClaimDentalViewModel.ClearData();
            Claim newClaim = new Claim();
            newClaim.Type = _claimservice.Claim.Type;
            newClaim.ClaimSubmissionTypes = _claimservice.ClaimSubmissionTypes;
            newClaim.Provider = _claimservice.Claim.Provider;
            newClaim.Participant = _claimservice.Claim.Participant;
            _claimservice.Claim = newClaim;
        }

        private void ShowAppropriateTDEntryViewModel()
        {
            switch (ClaimSubmissionType.ID)
            {
                case "ACUPUNCTURE":
                case "CHIROPODY":
                case "CHIRO":
                case "PHYSIO":
                case "PODIATRY":
                    ShowViewModel<ClaimTreatmentDetailsEntry1ViewModel>();
                    break;
                case "PSYCHOLOGY":
                case "MASSAGE":
                case "NATUROPATHY":
                case "SPEECH":
                    ShowViewModel<ClaimTreatmentDetailsEntry2ViewModel>();
                    break;
                case "MI":
                    ShowViewModel<ClaimTreatmentDetailsEntryMIViewModel>();
                    break;
                case "ORTHODONTIC":
                    ShowViewModel<ClaimTreatmentDetailsEntryOMFViewModel>();
                    break;
                case "CONTACTS":
                    ShowViewModel<ClaimTreatmentDetailsEntryPCViewModel>();
                    break;
                case "GLASSES":
                    ShowViewModel<ClaimTreatmentDetailsEntryPGViewModel>();
                    break;
                case "EYEEXAM":
                    ShowViewModel<ClaimTreatmentDetailsEntryREEViewModel>();
                    break;
                case "DRUG":
                    INonRealTimeClaimUploadProperties uploadable = (INonRealTimeClaimUploadProperties)NonRealTimeUploadFactory.Create(NonRealTimeClaimType.Drug, nameof(ClaimDocumentsUploadViewModel));
                    ShowViewModel<ClaimDocumentsUploadViewModel, ClaimDocumentsUploadViewModelParameters>(new ClaimDocumentsUploadViewModelParameters(uploadable));
                    break;
                case "DENTAL":
                    ShowViewModel<ClaimTreatmentDetailsEntryDentalViewModel>();
                    break;
            }
        }

        private void PublishMessages()
        {
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }
    }
}