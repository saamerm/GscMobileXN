using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimTreatmentDetailsEntryPGViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IParticipantService _participantservice;
        private readonly MvxSubscriptionToken _claimsubmissionbenefitsretrieved;
        private readonly MvxSubscriptionToken _lenstypesretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly MvxSubscriptionToken _visionspheresretrieved;
        private readonly MvxSubscriptionToken _visioncylindersretrieved;
        private readonly MvxSubscriptionToken _visionaxesretrieved;
        private readonly MvxSubscriptionToken _visionprismsretrieved;
        private readonly MvxSubscriptionToken _visionbifocalsretrieved;

        public string AmountPaidByPPorGPText => Resource.TreatmentDetailsAmountPaidByPpOrGpIfApplicable;
        public string MissingAmountPaidByPpOrGpErrorMessage => Resource.MissingAmountPaidByPpOrGpValidation;
        public string InvalidAmountPaidByPpOrGpErrorMessage => Resource.InvalidAmountPaidByPpOrGpValidation;

        public ClaimTreatmentDetailsEntryPGViewModel(IMvxMessenger messenger, IClaimService claimservice, IParticipantService participantservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _participantservice = participantservice;

            if (_claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
            {
                AmountPaidByAlternateCarrierVisible = true;
            }

#if CCQ || FPPM
             IsAmountPaidByPPorGPVisible = false;
#else
            if ((string.Equals(ClaimSubmissionType.ID, "GLASSES", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ClaimSubmissionType.ID, "CONTACTS", StringComparison.OrdinalIgnoreCase))
                && _claimservice.Claim.Participant.IsOrUnderAgeOf18()
                && _claimservice.Claim.Participant.IsResidentOfQuebecProvince())
            {
                IsAmountPaidByPPorGPVisible = true;
            }
            else
            {
                IsAmountPaidByPPorGPVisible = false;
            }
#endif

            TypesOfEyewear = _claimservice.ClaimSubmissionBenefits;

            SetTypesOfLens();

            if (_participantservice.PlanMember != null && !string.IsNullOrEmpty(_participantservice.PlanMember.PlanMemberID))
            {
                SetFieldVisibility(_participantservice.PlanMember.PlanConditions);
            }

            _claimsubmissionbenefitsretrieved = _messenger.Subscribe<GetClaimSubmissionBenefitsComplete>((message) =>
            {
                TypesOfEyewear = _claimservice.ClaimSubmissionBenefits;
            });

            _lenstypesretrieved = _messenger.Subscribe<GetLensTypesComplete>((message) =>
            {
                SetTypesOfLens();
            });

            _visionspheresretrieved = _messenger.Subscribe<GetVisionSpheresComplete>((message) =>
            {
                VisionSpheres = _claimservice.VisionSpheres;
            });

            _visioncylindersretrieved = _messenger.Subscribe<GetVisionCylindersComplete>((message) =>
            {
                VisionCylinders = _claimservice.VisionCylinders;
            });

            _visionaxesretrieved = _messenger.Subscribe<GetVisionAxesComplete>((message) =>
            {
                VisionAxes = _claimservice.VisionAxes;
            });

            _visionprismsretrieved = _messenger.Subscribe<GetVisionPrismsComplete>((message) =>
            {
                VisionPrisms = _claimservice.VisionPrisms;
            });

            _visionbifocalsretrieved = _messenger.Subscribe<GetVisionBifocalsComplete>((message) =>
            {
                VisionBifocals = _claimservice.VisionBifocals;
            });

            VisionSpheres = _claimservice.VisionSpheres;
            VisionCylinders = _claimservice.VisionCylinders;
            VisionAxes = _claimservice.VisionAxes;
            VisionPrisms = _claimservice.VisionPrisms;
            VisionBifocals = _claimservice.VisionBifocals;

            if (_claimservice.SelectedTreatmentDetailID != null && _claimservice.SelectedTreatmentDetailID != Guid.Empty)
            {
                EditMode = true;
                EditID = _claimservice.SelectedTreatmentDetailID;
                _claimservice.SelectedTreatmentDetailID = Guid.Empty;
                PopulateFields();
            }
            else
            {
                EditMode = false;
                EditID = Guid.Empty;
            }

            _shouldcloseself = _messenger.Subscribe<ClearClaimTreatmentDetailsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimTreatmentDetailsViewRequested>(_shouldcloseself);
                _messenger.Unsubscribe<GetClaimSubmissionBenefitsComplete>(_claimsubmissionbenefitsretrieved);
                _messenger.Unsubscribe<GetLensTypesComplete>(_lenstypesretrieved);
                _messenger.Unsubscribe<GetClaimSubmissionBenefitsComplete>(_claimsubmissionbenefitsretrieved);
                _messenger.Unsubscribe<GetVisionSpheresComplete>(_visionspheresretrieved);
                _messenger.Unsubscribe<GetVisionCylindersComplete>(_visioncylindersretrieved);
                _messenger.Unsubscribe<GetVisionAxesComplete>(_visionaxesretrieved);
                _messenger.Unsubscribe<GetVisionPrismsComplete>(_visionprismsretrieved);
                _messenger.Unsubscribe<GetVisionBifocalsComplete>(_visionbifocalsretrieved);
                Close(this);
            });
        }

        public ClaimSubmissionType ClaimSubmissionType => _claimservice.SelectedClaimSubmissionType;

        public int TreatmentDetailsCount => _claimservice.Claim.TreatmentDetails.Count;

        private DateTime _dateOfPurchase;
        public DateTime DateOfPurchase
        {
            get => (this._dateOfPurchase == DateTime.MinValue) ? DateTime.Now : this._dateOfPurchase;
            set
            {
                _dateOfPurchase = value;
                RaisePropertyChanged(() => DateOfPurchase);
            }
        }

        private ClaimSubmissionBenefit _typeOfEyewear;
        public ClaimSubmissionBenefit TypeOfEyewear
        {
            get => _typeOfEyewear;
            set
            {
                _typeOfEyewear = value;
                UpdateAmountEnabledFields();
                UpdatePrescriptionEnabledFields();
                RaisePropertyChanged(() => TypeOfEyewear);
            }
        }

        private LensType _typeOfLens;
        public LensType TypeOfLens
        {
            get => _typeOfLens;
            set
            {
                _typeOfLens = value;
                UpdatePrescriptionEnabledFields();
                RaisePropertyChanged(() => TypeOfLens);
            }
        }

        private bool _acknowledgeEyewareIsCorrective;
        public bool AcknowledgeEyewareIsCorrective
        {
            get => _acknowledgeEyewareIsCorrective;
            set
            {
                _acknowledgeEyewareIsCorrective = value;
                RaisePropertyChanged(() => AcknowledgeEyewareIsCorrective);
            }
        }

        private bool _isAcknowledgeEyewearIsCorrectiveVisible;
        public bool IsAcknowledgeEyewearIsCorrectiveVisible
        {
            get => _isAcknowledgeEyewearIsCorrectiveVisible;
            set
            {
                _isAcknowledgeEyewearIsCorrectiveVisible = value;
                if (_isAcknowledgeEyewearIsCorrectiveVisible == false) AcknowledgeEyewareIsCorrective = true;
                RaisePropertyChanged(() => IsAcknowledgeEyewearIsCorrectiveVisible);
            }
        }

        private string _totalAmountCharged;
        public string TotalAmountCharged
        {
            get => (string.IsNullOrEmpty(this._totalAmountCharged)) ? string.Empty : this._totalAmountCharged;
            set
            {
                _totalAmountCharged = value;
                RaisePropertyChanged(() => TotalAmountCharged);
            }
        }

        private bool _isTotalAmountChargedVisible;
        public bool IsTotalAmountChargedVisible
        {
            get => _isTotalAmountChargedVisible;
            set
            {
                _isTotalAmountChargedVisible = value;
                RaisePropertyChanged(() => IsTotalAmountChargedVisible);
            }
        }

        private string _frameAmount;
        public string FrameAmount
        {
            get => (String.IsNullOrEmpty(this._frameAmount)) ? "" : this._frameAmount;
            set
            {
                _frameAmount = value;
                RaisePropertyChanged(() => FrameAmount);
            }
        }

        private bool _isFrameAmountEnabled;
        public bool IsFrameAmountEnabled
        {
            get => _isFrameAmountEnabled;
            set
            {
                if (value != _isFrameAmountEnabled)
                {
                    _isFrameAmountEnabled = value;
                    if (!_isFrameAmountEnabled) FrameAmount = string.Empty; // clear value if being disabled
                    RaisePropertyChanged(() => IsFrameAmountEnabled);
                }
            }
        }

        private string _eyeglassLensesAmount;
        public string EyeglassLensesAmount
        {
            get => (String.IsNullOrEmpty(this._eyeglassLensesAmount)) ? "" : this._eyeglassLensesAmount;
            set
            {
                _eyeglassLensesAmount = value;
                RaisePropertyChanged(() => EyeglassLensesAmount);
            }
        }

        private bool _isEyeglassLensesAmountEnabled;
        public bool IsEyeglassLensesAmountEnabled
        {
            get => _isEyeglassLensesAmountEnabled;
            set
            {
                if (value != _isEyeglassLensesAmountEnabled)
                {
                    _isEyeglassLensesAmountEnabled = value;
                    if (!_isEyeglassLensesAmountEnabled) EyeglassLensesAmount = string.Empty; // clear value if being disabled
                    RaisePropertyChanged(() => IsEyeglassLensesAmountEnabled);
                }
            }
        }

        private string _feeAmount;
        public string FeeAmount
        {
            get => (String.IsNullOrEmpty(this._feeAmount)) ? "" : this._feeAmount;
            set
            {
                _feeAmount = value;
                RaisePropertyChanged(() => FeeAmount);
            }
        }

        private bool _isFrameLensAndFeeEntryVisible;
        public bool IsFrameLensAndFeeEntryVisible
        {
            get => _isFrameLensAndFeeEntryVisible;
            set
            {
                _isFrameLensAndFeeEntryVisible = value;
                RaisePropertyChanged(() => IsFrameLensAndFeeEntryVisible);
            }
        }

        private string _amountPaidByAlternateCarrier;
        public string AmountPaidByAlternateCarrier
        {
            get => (String.IsNullOrEmpty(this._amountPaidByAlternateCarrier)) ? "" : this._amountPaidByAlternateCarrier;
            set
            {
                _amountPaidByAlternateCarrier = value;
                RaisePropertyChanged(() => AmountPaidByAlternateCarrier);
            }
        }

        private bool _amountPaidByAlternateCarrierVisible = false;
        public bool AmountPaidByAlternateCarrierVisible
        {
            get => _amountPaidByAlternateCarrierVisible;
            set
            {
                _amountPaidByAlternateCarrierVisible = value;
                RaisePropertyChanged(() => AmountPaidByAlternateCarrierVisible);
            }
        }

        public double AmountPaidByPPorGPDollarValue => GSCHelper.GetDollarAmount(this.AmountPaidByPPorGP);

        private string _amountPaidByPPorGP;
        public string AmountPaidByPPorGP
        {
            get => _amountPaidByPPorGP;
            set => SetProperty(ref _amountPaidByPPorGP, value);
        }

        private bool _isAmountPaidByPPorGPVisible;
        public bool IsAmountPaidByPPorGPVisible
        {
            get => _isAmountPaidByPPorGPVisible;
            set => SetProperty(ref _isAmountPaidByPPorGPVisible, value);
        }

        private List<ClaimSubmissionBenefit> _typesOfEyewear;
        public List<ClaimSubmissionBenefit> TypesOfEyewear
        {
            get => _typesOfEyewear;
            set
            {
                _typesOfEyewear = value;
                if (_typesOfEyewear != null && _typesOfEyewear.Count == 1)
                    TypeOfEyewear = _typesOfEyewear.FirstOrDefault();
                RaisePropertyChanged(() => TypesOfEyewear);
            }
        }

        private List<LensType> _typesOfLens;
        public List<LensType> TypesOfLens
        {
            get => _typesOfLens;
            set
            {
                _typesOfLens = value;
                RaisePropertyChanged(() => TypesOfLens);
            }
        }

        private bool _editMode = false;
        public bool EditMode
        {
            get => _editMode;
            set
            {
                _editMode = value;
                RaisePropertyChanged(() => EditMode);
            }
        }

        private Guid EditID { get; set; }

        public double TotalAmountChargedDollarValue
        {
            get
            {
                double tac = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                return tac;
            }
        }

        public double FrameDollarValue
        {
            get
            {
                double fa = GSCHelper.GetDollarAmount(this.FrameAmount);
                return fa;
            }
        }

        public double EyeglassLensesDollarValue
        {
            get
            {
                double ela = GSCHelper.GetDollarAmount(this.EyeglassLensesAmount);
                return ela;
            }
        }

        public double FeeDollarValue
        {
            get
            {
                double feea = GSCHelper.GetDollarAmount(this.FeeAmount);
                return feea;
            }
        }

        public double TotalDollarValue
        {
            get
            {
                if (IsTotalAmountChargedVisible)
                {
                    double tac = GSCHelper.GetDollarAmount(this.TotalAmountCharged);

                    return tac;
                }
                else
                {
                    double fa = GSCHelper.GetDollarAmount(this.FrameAmount);
                    double ela = GSCHelper.GetDollarAmount(this.EyeglassLensesAmount);
                    double feea = GSCHelper.GetDollarAmount(this.FeeAmount);

                    return fa + ela + feea;
                }
            }
        }

        public double ACDollarValue
        {
            get
            {
                double ac = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);

                return ac;
            }
        }

        private bool _isPrescriptionDetailsVisible;
        public bool IsPrescriptionDetailsVisible
        {
            get => _isPrescriptionDetailsVisible;
            set
            {
                _isPrescriptionDetailsVisible = value;
                RaisePropertyChanged(() => IsPrescriptionDetailsVisible);
            }
        }

        private bool _isRightSphereEnabled;
        public bool IsRightSphereEnabled
        {
            get => _isRightSphereEnabled;
            set
            {
                if (value != _isRightSphereEnabled)
                {
                    _isRightSphereEnabled = value;

                    if (!_isRightSphereEnabled)
                        RightSphere = null;
                    else
                    {
                        if (VisionSpheres != null && VisionSpheres.Count > 0)
                            RightSphere = VisionSpheres.FirstOrDefault(s => s.ID == "0.00");
                        else
                            RightSphere = null;
                    }

                    RaisePropertyChanged(() => IsRightSphereEnabled);
                }
            }
        }

        private ClaimOption _rightSphere;
        public ClaimOption RightSphere
        {
            get => _rightSphere;
            set
            {
                _rightSphere = value;
                RaisePropertyChanged(() => RightSphere);
            }
        }

        private bool _isRightCylinderEnabled;
        public bool IsRightCylinderEnabled
        {
            get => _isRightCylinderEnabled;
            set
            {
                if (value != _isRightCylinderEnabled)
                {
                    _isRightCylinderEnabled = value;

                    if (!_isRightCylinderEnabled)
                        RightCylinder = null;
                    else
                    {
                        if (VisionCylinders != null && VisionCylinders.Count > 0)
                            RightCylinder = VisionCylinders.FirstOrDefault(s => s.ID == "");
                        else
                            RightCylinder = null;
                    }

                    RaisePropertyChanged(() => IsRightCylinderEnabled);
                }
            }
        }

        private ClaimOption _rightCylinder;
        public ClaimOption RightCylinder
        {
            get => _rightCylinder;
            set
            {
                _rightCylinder = value;
                RaisePropertyChanged(() => RightCylinder);
            }
        }

        private bool _isRightAxisEnabled;
        public bool IsRightAxisEnabled
        {
            get => _isRightAxisEnabled;
            set
            {
                if (value != _isRightAxisEnabled)
                {
                    _isRightAxisEnabled = value;

                    if (!_isRightAxisEnabled)
                        RightAxis = null;
                    else
                    {
                        if (VisionAxes != null && VisionAxes.Count > 0)
                            RightAxis = VisionAxes.FirstOrDefault(s => s.ID == "");
                        else
                            RightAxis = null;
                    }

                    RaisePropertyChanged(() => IsRightAxisEnabled);
                }
            }
        }

        private ClaimOption _rightAxis;
        public ClaimOption RightAxis
        {
            get => _rightAxis;
            set
            {
                _rightAxis = value;
                RaisePropertyChanged(() => RightAxis);
            }
        }

        private bool _isRightPrismEnabled;
        public bool IsRightPrismEnabled
        {
            get => _isRightPrismEnabled;
            set
            {
                if (value != _isRightPrismEnabled)
                {
                    _isRightPrismEnabled = value;

                    if (!_isRightPrismEnabled)
                        RightPrism = null;
                    else
                    {
                        if (VisionPrisms != null && VisionPrisms.Count > 0)
                            RightPrism = VisionPrisms.FirstOrDefault(s => s.ID == "");
                        else
                            RightPrism = null;
                    }

                    RaisePropertyChanged(() => IsRightPrismEnabled);
                }
            }
        }

        private ClaimOption _rightPrism;
        public ClaimOption RightPrism
        {
            get => _rightPrism;
            set
            {
                _rightPrism = value;
                RaisePropertyChanged(() => RightPrism);
            }
        }

        private bool _isRightBifocalEnabled;
        public bool IsRightBifocalEnabled
        {
            get => _isRightBifocalEnabled;
            set
            {
                if (value != _isRightBifocalEnabled)
                {
                    _isRightBifocalEnabled = value;

                    RightBifocal = null; // default is none, should reset to null every time this property changes

                    RaisePropertyChanged(() => IsRightBifocalEnabled);
                }
            }
        }

        private ClaimOption _rightBifocal;
        public ClaimOption RightBifocal
        {
            get => _rightBifocal;
            set
            {
                _rightBifocal = value;
                RaisePropertyChanged(() => RightBifocal);
            }
        }

        private bool _isLeftSphereEnabled;
        public bool IsLeftSphereEnabled
        {
            get => _isLeftSphereEnabled;
            set
            {
                if (value != _isLeftSphereEnabled)
                {
                    _isLeftSphereEnabled = value;

                    if (!_isLeftSphereEnabled)
                        LeftSphere = null;
                    else
                    {
                        if (VisionSpheres != null && VisionSpheres.Count > 0)
                            LeftSphere = VisionSpheres.FirstOrDefault(s => s.ID == "0.00");
                        else
                            LeftSphere = null;
                    }

                    RaisePropertyChanged(() => IsLeftSphereEnabled);
                }
            }
        }

        private ClaimOption _leftSphere;
        public ClaimOption LeftSphere
        {
            get => _leftSphere;
            set
            {
                _leftSphere = value;
                RaisePropertyChanged(() => LeftSphere);
            }
        }

        private bool _isLeftCylinderEnabled;
        public bool IsLeftCylinderEnabled
        {
            get => _isLeftCylinderEnabled;
            set
            {
                if (value != _isLeftCylinderEnabled)
                {
                    _isLeftCylinderEnabled = value;

                    if (!_isLeftCylinderEnabled)
                        LeftCylinder = null;
                    else
                    {
                        if (VisionCylinders != null && VisionCylinders.Count > 0)
                            LeftCylinder = VisionCylinders.FirstOrDefault(s => s.ID == "");
                        else
                            LeftCylinder = null;
                    }

                    RaisePropertyChanged(() => IsLeftCylinderEnabled);
                }
            }
        }

        private ClaimOption _leftCylinder;
        public ClaimOption LeftCylinder
        {
            get => _leftCylinder;
            set
            {
                _leftCylinder = value;
                RaisePropertyChanged(() => LeftCylinder);
            }
        }

        private bool _isLeftAxisEnabled;
        public bool IsLeftAxisEnabled
        {
            get => _isLeftAxisEnabled;
            set
            {
                if (value != _isLeftAxisEnabled)
                {
                    _isLeftAxisEnabled = value;

                    if (!_isLeftAxisEnabled)
                        LeftAxis = null;
                    else
                    {
                        if (VisionAxes != null && VisionAxes.Count > 0)
                            LeftAxis = VisionAxes.FirstOrDefault(s => s.ID == "");
                        else
                            LeftAxis = null;
                    }

                    RaisePropertyChanged(() => IsLeftAxisEnabled);
                }
            }
        }

        private ClaimOption _leftAxis;
        public ClaimOption LeftAxis
        {
            get => _leftAxis;
            set
            {
                _leftAxis = value;
                RaisePropertyChanged(() => LeftAxis);
            }
        }

        private bool _isLeftPrismEnabled;
        public bool IsLeftPrismEnabled
        {
            get => _isLeftPrismEnabled;
            set
            {
                if (value != _isLeftPrismEnabled)
                {
                    _isLeftPrismEnabled = value;

                    if (!_isLeftPrismEnabled)
                        LeftPrism = null;
                    else
                    {
                        if (VisionPrisms != null && VisionPrisms.Count > 0)
                            LeftPrism = VisionPrisms.FirstOrDefault(s => s.ID == "");
                        else
                            LeftPrism = null;
                    }

                    RaisePropertyChanged(() => IsLeftPrismEnabled);
                }
            }
        }

        private ClaimOption _leftPrism;
        public ClaimOption LeftPrism
        {
            get => _leftPrism;
            set
            {
                _leftPrism = value;
                RaisePropertyChanged(() => LeftPrism);
            }
        }

        private bool _isLeftBifocalEnabled;
        public bool IsLeftBifocalEnabled
        {
            get => _isLeftBifocalEnabled;
            set
            {
                if (value != _isLeftBifocalEnabled)
                {
                    _isLeftBifocalEnabled = value;

                    LeftBifocal = null; // default is none, should reset to null every time this property changes

                    RaisePropertyChanged(() => IsLeftBifocalEnabled);
                }
            }
        }

        private ClaimOption _leftBifocal;
        public ClaimOption LeftBifocal
        {
            get => _leftBifocal;
            set
            {
                _leftBifocal = value;
                RaisePropertyChanged(() => LeftBifocal);
            }
        }

        private List<ClaimOption> _visionSpheres;
        public List<ClaimOption> VisionSpheres
        {
            get => _visionSpheres;
            set
            {
                _visionSpheres = value;
                RaisePropertyChanged(() => VisionSpheres);
            }
        }

        private List<ClaimOption> _visionCylinders;
        public List<ClaimOption> VisionCylinders
        {
            get => _visionCylinders;
            set
            {
                _visionCylinders = value;
                RaisePropertyChanged(() => VisionCylinders);
            }
        }

        private List<ClaimOption> _visionAxes;
        public List<ClaimOption> VisionAxes
        {
            get => _visionAxes;
            set
            {
                _visionAxes = value;
                RaisePropertyChanged(() => VisionAxes);
            }
        }

        private List<ClaimOption> _visionPrisms;
        public List<ClaimOption> VisionPrisms
        {
            get => _visionPrisms;
            set
            {
                _visionPrisms = value;
                RaisePropertyChanged(() => VisionPrisms);
            }
        }

        private List<ClaimOption> _visionBifocals;
        public List<ClaimOption> VisionBifocals
        {
            get => _visionBifocals;
            set
            {
                _visionBifocals = value;
                RaisePropertyChanged(() => VisionBifocals);
            }
        }

        public ICommand SubmitEntryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        double tac = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                        double fa = GSCHelper.GetDollarAmount(this.FrameAmount);
                        double ela = GSCHelper.GetDollarAmount(this.EyeglassLensesAmount);
                        double fee = GSCHelper.GetDollarAmount(this.FeeAmount);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail td = new TreatmentDetail()
                        {
                            ID = Guid.NewGuid(),
                            ClaimSubmissionType = this.ClaimSubmissionType,
                            DateOfPurchase = this.DateOfPurchase,
                            TypeOfEyewear = this.TypeOfEyewear,
                            TypeOfLens = this.TypeOfLens,
                            TreatmentAmount = this.TotalDollarValue,
                            TotalAmountCharged = tac,
                            IsTotalAmountChargedVisible = this.IsTotalAmountChargedVisible,
                            FrameAmount = fa,
                            EyeglassLensesAmount = ela,
                            FeeAmount = fee,
                            IsFeeAmountVisible = this.IsFrameLensAndFeeEntryVisible,
                            IsEyeglassLensesAmountVisible = this.IsFrameLensAndFeeEntryVisible,
                            IsFrameAmountVisible = this.IsFrameLensAndFeeEntryVisible,
                            IsAcknowledgeEyewearIsCorrectiveVisible = this.IsAcknowledgeEyewearIsCorrectiveVisible,
                            AcknowledgeEyewearIsCorrective = this.AcknowledgeEyewareIsCorrective,
                            AlternateCarrierPayment = acp,
                            AmountPaidByPPorGP = AmountPaidByPPorGPDollarValue,
                            IsAlternateCarrierPaymentVisible = this.AmountPaidByAlternateCarrierVisible,
                            IsAmountPaidByPPorGPVisible = this.IsAmountPaidByPPorGPVisible
                        };

                        // Prescription details
                        td.IsPrescriptionDetailsVisible = this.IsPrescriptionDetailsVisible;
                        if (IsPrescriptionDetailsVisible)
                        {
                            td.RightSphere = this.RightSphere;
                            td.RightCylinder = this.RightCylinder;
                            td.RightAxis = this.RightAxis;
                            td.RightPrism = this.RightPrism;
                            td.RightBifocal = this.RightBifocal;
                            td.LeftSphere = this.LeftSphere;
                            td.LeftCylinder = this.LeftCylinder;
                            td.LeftAxis = this.LeftAxis;
                            td.LeftPrism = this.LeftPrism;
                            td.LeftBifocal = this.LeftBifocal;
                            td.IsRightSphereEnabled = this.IsRightSphereEnabled;
                            td.IsRightCylinderEnabled = this.IsRightCylinderEnabled;
                            td.IsRightAxisEnabled = this.IsRightAxisEnabled;
                            td.IsRightPrismEnabled = this.IsRightPrismEnabled;
                            td.IsRightBifocalEnabled = this.IsRightBifocalEnabled;
                            td.IsLeftSphereEnabled = this.IsLeftSphereEnabled;
                            td.IsLeftCylinderEnabled = this.IsLeftCylinderEnabled;
                            td.IsLeftAxisEnabled = this.IsLeftAxisEnabled;
                            td.IsLeftPrismEnabled = this.IsLeftPrismEnabled;
                            td.IsLeftBifocalEnabled = this.IsLeftBifocalEnabled;
                        }

                        _claimservice.Claim.TreatmentDetails.Add(td);
                        _claimservice.PersistClaim();
                        _messenger.Publish<ClaimTreatmentDetailsListUpdated>(new ClaimTreatmentDetailsListUpdated(this) { TreatmentDetail = td });
                        RaiseClaimTreatmentEntrySuccess(new EventArgs());
                        Close(this);
                        if (TreatmentDetailsCount >= 1 && !_claimservice.IsTreatmentDetailsListInNavStack)
                            this.ShowViewModel<ClaimTreatmentDetailsListViewModel>();
                    }
                },
                () => { return !EditMode; });
            }
        }

        public ICommand SaveEntryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        double tac = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                        double fa = GSCHelper.GetDollarAmount(this.FrameAmount);
                        double ela = GSCHelper.GetDollarAmount(this.EyeglassLensesAmount);
                        double fee = GSCHelper.GetDollarAmount(this.FeeAmount);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail updated = null;
                        foreach (TreatmentDetail td in _claimservice.Claim.TreatmentDetails)
                        {
                            if (td.ID == EditID)
                            {
                                td.ClaimSubmissionType = this.ClaimSubmissionType;
                                td.DateOfPurchase = this.DateOfPurchase;
                                td.TypeOfEyewear = this.TypeOfEyewear;
                                td.TypeOfLens = this.TypeOfLens;
                                td.TreatmentAmount = this.TotalDollarValue;
                                td.TotalAmountCharged = tac;
                                td.IsTotalAmountChargedVisible = this.IsTotalAmountChargedVisible;
                                td.FrameAmount = fa;
                                td.EyeglassLensesAmount = ela;
                                td.FeeAmount = fee;
                                td.IsFeeAmountVisible = this.IsFrameLensAndFeeEntryVisible;
                                td.IsEyeglassLensesAmountVisible = this.IsFrameLensAndFeeEntryVisible;
                                td.IsFrameAmountVisible = this.IsFrameLensAndFeeEntryVisible;
                                td.IsAcknowledgeEyewearIsCorrectiveVisible = this.IsAcknowledgeEyewearIsCorrectiveVisible;
                                td.AcknowledgeEyewearIsCorrective = this.AcknowledgeEyewareIsCorrective;
                                td.AlternateCarrierPayment = acp;
                                td.AmountPaidByPPorGP = AmountPaidByPPorGPDollarValue;
                                td.IsAlternateCarrierPaymentVisible = this.AmountPaidByAlternateCarrierVisible;
                                td.IsAmountPaidByPPorGPVisible = this.IsAmountPaidByPPorGPVisible;

                                // Prescription Details
                                td.IsPrescriptionDetailsVisible = this.IsPrescriptionDetailsVisible;
                                if (this.IsPrescriptionDetailsVisible)
                                {
                                    td.RightSphere = this.RightSphere;
                                    td.RightCylinder = this.RightCylinder;
                                    td.RightAxis = this.RightAxis;
                                    td.RightPrism = this.RightPrism;
                                    td.RightBifocal = this.RightBifocal;
                                    td.LeftSphere = this.LeftSphere;
                                    td.LeftCylinder = this.LeftCylinder;
                                    td.LeftAxis = this.LeftAxis;
                                    td.LeftPrism = this.LeftPrism;
                                    td.LeftBifocal = this.LeftBifocal;
                                    td.IsRightSphereEnabled = this.IsRightSphereEnabled;
                                    td.IsRightCylinderEnabled = this.IsRightCylinderEnabled;
                                    td.IsRightAxisEnabled = this.IsRightAxisEnabled;
                                    td.IsRightPrismEnabled = this.IsRightPrismEnabled;
                                    td.IsRightBifocalEnabled = this.IsRightBifocalEnabled;
                                    td.IsLeftSphereEnabled = this.IsLeftSphereEnabled;
                                    td.IsLeftCylinderEnabled = this.IsLeftCylinderEnabled;
                                    td.IsLeftAxisEnabled = this.IsLeftAxisEnabled;
                                    td.IsLeftPrismEnabled = this.IsLeftPrismEnabled;
                                    td.IsLeftBifocalEnabled = this.IsLeftBifocalEnabled;
                                }
                                else // should all be null
                                {
                                    td.RightSphere = null;
                                    td.RightCylinder = null;
                                    td.RightAxis = null;
                                    td.RightPrism = null;
                                    td.RightBifocal = null;
                                    td.LeftSphere = null;
                                    td.LeftCylinder = null;
                                    td.LeftAxis = null;
                                    td.LeftPrism = null;
                                    td.LeftBifocal = null;
                                    td.IsRightSphereEnabled = false;
                                    td.IsRightCylinderEnabled = false;
                                    td.IsRightAxisEnabled = false;
                                    td.IsRightPrismEnabled = false;
                                    td.IsRightBifocalEnabled = false;
                                    td.IsLeftSphereEnabled = false;
                                    td.IsLeftCylinderEnabled = false;
                                    td.IsLeftAxisEnabled = false;
                                    td.IsLeftPrismEnabled = false;
                                    td.IsLeftBifocalEnabled = false;
                                }

                                updated = td;
                            }
                        }
                        _claimservice.SelectedTreatmentDetailID = Guid.Empty;
                        _claimservice.PersistClaim();
                        _messenger.Publish<ClaimTreatmentDetailsListUpdated>(new ClaimTreatmentDetailsListUpdated(this) { TreatmentDetail = updated });
                        RaiseClaimTreatmentEntrySuccess(new EventArgs());
                        Close(this);
                    }
                },
                () => { return EditMode; });
            }
        }

        public ICommand DeleteEntryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    TreatmentDetail tdToRemove = _claimservice.Claim.GetTreatmentDetailByID(EditID);
                    _claimservice.Claim.TreatmentDetails.Remove(tdToRemove);
                    _claimservice.SelectedTreatmentDetailID = Guid.Empty;
                    _claimservice.PersistClaim();
                    _messenger.Publish<ClaimTreatmentDetailsListUpdated>(new ClaimTreatmentDetailsListUpdated(this) { TreatmentDetail = tdToRemove });
                    RaiseClaimTreatmentEntrySuccess(new EventArgs());
                    Close(this);
                },
                () => { return EditMode; });
            }
        }

        private bool _missingTypeOfEyewear = false;
        public bool MissingTypeOfEyewear
        {
            get => _missingTypeOfEyewear;
            set
            {
                _missingTypeOfEyewear = value;
                RaisePropertyChanged(() => MissingTypeOfEyewear);
            }
        }
        private bool _missingTypeOfLens = false;
        public bool MissingTypeOfLens
        {
            get => _missingTypeOfLens;
            set
            {
                _missingTypeOfLens = value;
                RaisePropertyChanged(() => MissingTypeOfLens);
            }
        }
        private bool _missingDateOfPurchase = false;
        public bool MissingDateOfPurchase
        {
            get => _missingDateOfPurchase;
            set
            {
                _missingDateOfPurchase = value;
                RaisePropertyChanged(() => MissingDateOfPurchase);
            }
        }
        private bool _dateOfPurchaseError = false;
        public bool DateOfPurchaseError
        {
            get => _dateOfPurchaseError;
            set
            {
                _dateOfPurchaseError = value;
                RaisePropertyChanged(() => DateOfPurchaseError);
            }
        }
        private bool _invalidDateOfPurchase = false;
        public bool InvalidDateOfPurchase
        {
            get => _invalidDateOfPurchase;
            set
            {
                _invalidDateOfPurchase = value;
                RaisePropertyChanged(() => InvalidDateOfPurchase);
            }
        }
        private bool _missingAC = false;
        public bool MissingAC
        {
            get => _missingAC;
            set
            {
                _missingAC = value;
                RaisePropertyChanged(() => MissingAC);
            }
        }
        private bool _invalidAC = false;
        public bool InvalidAC
        {
            get => _invalidAC;
            set
            {

                if (!MissingAC) //Only return invalid when AC isn't missing                
                {
                    _invalidAC = value;
                    RaisePropertyChanged(() => InvalidAC);
                }
            }
        }

        private bool _ACError = false;
        public bool ACError
        {
            get => _ACError;
            set
            {
                _ACError = value;
                RaisePropertyChanged(() => ACError);
            }
        }

        private bool _missingAmountPaidByPPorGP;
        public bool MissingAmountPaidByPPorGP
        {
            get => _missingAmountPaidByPPorGP;
            set => SetProperty(ref _missingAmountPaidByPPorGP, value);
        }

        private bool _invalidAmountPaidByPPorGP;
        public bool InvalidAmountPaidByPPorGP
        {
            get => _invalidAmountPaidByPPorGP;
            set
            {
                if (!MissingAmountPaidByPPorGP)
                {
                    SetProperty(ref _invalidAmountPaidByPPorGP, value);
                }
            }
        }

        private bool _badAmountPaidByPPorGP;
        public bool BadAmountPaidByPPorGP
        {
            get => _badAmountPaidByPPorGP;
            set => SetProperty(ref _badAmountPaidByPPorGP, value);
        }

        private bool _amountPaidByPPorGPError;
        public bool AmountPaidByPPorGPError
        {
            get => _amountPaidByPPorGPError;
            set => SetProperty(ref _amountPaidByPPorGPError, value);
        }

        private bool _missingEyeglassLensesAmount = false;
        public bool MissingEyeglassLensesAmount
        {
            get => _missingEyeglassLensesAmount;
            set
            {
                _missingEyeglassLensesAmount = value;
                RaisePropertyChanged(() => MissingEyeglassLensesAmount);
            }
        }
        private bool _invalidEyeglassLensesAmount = false;
        public bool InvalidEyeglassLensesAmount
        {
            get => _invalidEyeglassLensesAmount;
            set
            {
                if (!MissingEyeglassLensesAmount) //Only return invalid when quanity isn't missing                
                {
                    _invalidEyeglassLensesAmount = value;
                    RaisePropertyChanged(() => InvalidEyeglassLensesAmount);
                }
            }
        }

        private bool _eyeglassLensesAmountError = false;
        public bool EyeglassLensesAmountError
        {
            get => _eyeglassLensesAmountError;
            set
            {
                _eyeglassLensesAmountError = value;
                RaisePropertyChanged(() => EyeglassLensesAmountError);
            }
        }
        private bool _missingFrameAmount = false;
        public bool MissingFrameAmount
        {
            get => _missingFrameAmount;
            set
            {
                _missingFrameAmount = value;
                RaisePropertyChanged(() => MissingFrameAmount);
            }
        }
        private bool _invalidFrameAmount = false;
        public bool InvalidFrameAmount
        {
            get => _invalidFrameAmount;
            set
            {
                if (!MissingFrameAmount) //Only return invalid when quanity isn't missing                
                {
                    _invalidFrameAmount = value;
                    RaisePropertyChanged(() => InvalidFrameAmount);
                }
            }
        }

        private bool _frameAmountError = false;
        public bool FrameAmountError
        {
            get => _frameAmountError;
            set
            {
                _frameAmountError = value;
                RaisePropertyChanged(() => FrameAmountError);
            }
        }

        private bool _invalidFeeAmount = false;
        public bool InvalidFeeAmount
        {
            get => _invalidFeeAmount;
            set
            {
                _invalidFeeAmount = value;
                RaisePropertyChanged(() => InvalidFeeAmount);
            }
        }

        private bool _acknowledgedEyewareIsCorrective = false;
        public bool AcknowledgedEyewareIsCorrective
        {
            get => _acknowledgedEyewareIsCorrective;
            set
            {
                _acknowledgedEyewareIsCorrective = value;
                RaisePropertyChanged(() => AcknowledgedEyewareIsCorrective);
            }
        }

        private bool _badvalueAC = false;
        public bool BadValueAC
        {
            get => _badvalueAC;
            set
            {
                _badvalueAC = value;
                RaisePropertyChanged(() => BadValueAC);
            }
        }

        private bool _dateTooOld = false;
        public bool DateTooOld
        {
            get => _dateTooOld;
            set
            {
                _dateTooOld = value;
                RaisePropertyChanged(() => DateTooOld);
            }
        }

        private bool _missingRightSphere = false;
        public bool MissingRightSphere
        {
            get => _missingRightSphere;
            set
            {
                _missingRightSphere = value;
                RaisePropertyChanged(() => MissingRightSphere);
            }
        }

        private bool _missingRightCylinder = false;
        public bool MissingRightCylinder
        {
            get => _missingRightCylinder;
            set
            {
                _missingRightCylinder = value;
                RaisePropertyChanged(() => MissingRightCylinder);
            }
        }

        private bool _missingRightAxis = false;
        public bool MissingRightAxis
        {
            get => _missingRightAxis;
            set
            {
                _missingRightAxis = value;
                RaisePropertyChanged(() => MissingRightAxis);
            }
        }

        private bool _missingRightPrism = false;
        public bool MissingRightPrism
        {
            get => _missingRightPrism;
            set
            {
                _missingRightPrism = value;
                RaisePropertyChanged(() => MissingRightPrism);
            }
        }

        private bool _missingRightBifocal = false;
        public bool MissingRightBifocal
        {
            get => _missingRightBifocal;
            set
            {
                _missingRightBifocal = value;
                RaisePropertyChanged(() => MissingRightBifocal);
            }
        }

        private bool _missingLeftSphere = false;
        public bool MissingLeftSphere
        {
            get => _missingLeftSphere;
            set
            {
                _missingLeftSphere = value;
                RaisePropertyChanged(() => MissingLeftSphere);
            }
        }

        private bool _missingLeftCylinder = false;
        public bool MissingLeftCylinder
        {
            get => _missingLeftCylinder;
            set
            {
                _missingLeftCylinder = value;
                RaisePropertyChanged(() => MissingLeftCylinder);
            }
        }

        private bool _missingLeftAxis = false;
        public bool MissingLeftAxis
        {
            get => _missingLeftAxis;
            set
            {
                _missingLeftAxis = value;
                RaisePropertyChanged(() => MissingLeftAxis);
            }
        }

        private bool _missingLeftPrism = false;
        public bool MissingLeftPrism
        {
            get => _missingLeftPrism;
            set
            {
                _missingLeftPrism = value;
                RaisePropertyChanged(() => MissingLeftPrism);
            }
        }

        private bool _missingLeftBifocal = false;
        public bool MissingLeftBifocal
        {
            get => _missingLeftBifocal;
            set
            {
                _missingLeftBifocal = value;
                RaisePropertyChanged(() => MissingLeftBifocal);
            }
        }

        private bool _missingTotalAmountCharged = false;
        public bool MissingTotalAmountCharged
        {
            get => _missingTotalAmountCharged;
            set
            {
                _missingTotalAmountCharged = value;
                RaisePropertyChanged(() => MissingTotalAmountCharged);
            }
        }

        private bool _invalidTotalAmountCharged = false;
        public bool InvalidTotalAmountCharged
        {
            get => _invalidTotalAmountCharged;
            set
            {
                _invalidTotalAmountCharged = value;
                RaisePropertyChanged(() => InvalidTotalAmountCharged);
            }
        }

        private bool _totalAmountChargedError = false;
        public bool TotalAmountChargedError
        {
            get => _totalAmountChargedError;
            set
            {
                _totalAmountChargedError = value;
                RaisePropertyChanged(() => TotalAmountChargedError);
            }
        }

        private bool _prescriptionDetailsError = false;
        public bool PrescriptionDetailsError
        {
            get => _prescriptionDetailsError;
            set
            {
                _prescriptionDetailsError = value;
                RaisePropertyChanged(() => PrescriptionDetailsError);
            }
        }

        public event EventHandler ClaimTreatmentEntrySuccess;
        protected virtual void RaiseClaimTreatmentEntrySuccess(EventArgs e)
        {
            if (this.ClaimTreatmentEntrySuccess != null)
            {
                ClaimTreatmentEntrySuccess(this, e);
            }
        }

        public event EventHandler OnInvalidClaimDetails;
        protected virtual void RaiseInvalidClaimDetails(EventArgs e)
        {
            if (this.OnInvalidClaimDetails != null)
            {
                OnInvalidClaimDetails(this, e);
            }
        }

        private bool IsValid()
        {
            // Reset for validation
            MissingTypeOfEyewear = false;
            MissingDateOfPurchase = false;
            InvalidDateOfPurchase = false;
            DateOfPurchaseError = false;
            MissingEyeglassLensesAmount = false;
            InvalidEyeglassLensesAmount = false;
            EyeglassLensesAmountError = false;
            MissingFrameAmount = false;
            InvalidFrameAmount = false;
            FrameAmountError = false;
            InvalidFeeAmount = false;
            MissingTypeOfLens = false;
            MissingAC = false;
            InvalidAC = false;
            ACError = false;
            MissingAmountPaidByPPorGP = false;
            InvalidAmountPaidByPPorGP = false;
            BadAmountPaidByPPorGP = false;
            AmountPaidByPPorGPError = false;
            BadValueAC = false;
            DateTooOld = false;
            AcknowledgedEyewareIsCorrective = !AcknowledgeEyewareIsCorrective;
            MissingRightSphere = false;
            MissingRightCylinder = false;
            MissingRightAxis = false;
            MissingRightPrism = false;
            MissingRightBifocal = false;
            MissingLeftSphere = false;
            MissingLeftCylinder = false;
            MissingLeftAxis = false;
            MissingLeftPrism = false;
            MissingLeftBifocal = false;
            MissingTotalAmountCharged = false;
            InvalidTotalAmountCharged = false;
            PrescriptionDetailsError = false;
            TreatmentDetailEntryPGValidator validator = new TreatmentDetailEntryPGValidator();

            ValidationResult result = null;
            string validationRuleSet = "default";
            if (AmountPaidByAlternateCarrierVisible)
            {
                validationRuleSet += ",AlternateCarrier";
            }

            if (IsAmountPaidByPPorGPVisible)
            {
                validationRuleSet += ",AmountPaidByPPorGP";
            }

            if (IsPrescriptionDetailsVisible && TypeOfEyewear != null)
            {
                if (TypeOfEyewear.ID == 70146) // Left
                {
                    validationRuleSet += ",Left";
                    if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                        validationRuleSet += ",LeftBifocal";
                }
                else if (TypeOfEyewear.ID == 70147) // Right
                {
                    validationRuleSet += ",Right";
                    if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                        validationRuleSet += ",RightBifocal";
                }
                else // Pair, Frame and Lenses, Frame Only
                {
                    validationRuleSet += ",Pairs";
                    if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                        validationRuleSet += ",LeftBifocal,RightBifocal";
                }
            }

            result = validator.Validate<ClaimTreatmentDetailsEntryPGViewModel>(this, ruleSet: validationRuleSet);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Type":
                            MissingTypeOfEyewear = true;
                            break;
                        case "Empty Date":
                            MissingDateOfPurchase = true;
                            break;
                        case "Future Date":
                            InvalidDateOfPurchase = true;
                            break;
                        case "Empty Lenses Amount":
                            MissingEyeglassLensesAmount = true;
                            break;
                        case "Invalid Lenses Amount":
                            InvalidEyeglassLensesAmount = true;
                            break;
                        case "Empty Frame Amount":
                            MissingFrameAmount = true;
                            break;
                        case "Invalid Frame Amount":
                            InvalidFrameAmount = true;
                            break;
                        case "Invalid Fee Amount":
                            InvalidFeeAmount = true;
                            break;
                        case "Empty Lens":
                            MissingTypeOfLens = true;
                            break;
                        case "Empty AC":
                            MissingAC = true;
                            break;
                        case "Invalid AC":
                            InvalidAC = true;
                            break;
                        case "BadValue AC":
                            BadValueAC = true;
                            break;
                        case "Empty AmountPaidByPPorGP":
                            MissingAmountPaidByPPorGP = true;
                            break;
                        case "Invalid AmountPaidByPPorGP":
                            InvalidAmountPaidByPPorGP = true;
                            break;
                        case "BadValue AmountPaidByPPorGP":
                            BadAmountPaidByPPorGP = true;
                            break;
                        case "Date TooOld":
                            DateTooOld = true;
                            break;
                        case "Empty RightSphere":
                            MissingRightSphere = true;
                            break;
                        case "Empty RightCylinder":
                            MissingRightCylinder = true;
                            break;
                        case "Empty RightAxis":
                            MissingRightAxis = true;
                            break;
                        case "Empty RightPrism":
                            MissingRightPrism = true;
                            break;
                        case "Empty RightBifocal":
                            MissingRightBifocal = true;
                            break;
                        case "Empty LeftSphere":
                            MissingLeftSphere = true;
                            break;
                        case "Empty LeftCylinder":
                            MissingLeftCylinder = true;
                            break;
                        case "Empty LeftAxis":
                            MissingLeftAxis = true;
                            break;
                        case "Empty LeftPrism":
                            MissingLeftPrism = true;
                            break;
                        case "Empty LeftBifocal":
                            MissingLeftBifocal = true;
                            break;
                        case "Empty TotalAmountCharged":
                            MissingTotalAmountCharged = true;
                            break;
                        case "Invalid TotalAmountCharged":
                            InvalidTotalAmountCharged = true;
                            break;
                    }
                }
                //needed for a special WP scenario
                if (!MissingTypeOfEyewear)
                {
                    MissingTypeOfEyewear = string.IsNullOrEmpty(TypeOfEyewear.Name);
                }
                DateOfPurchaseError = (MissingDateOfPurchase || InvalidDateOfPurchase || DateTooOld);
                EyeglassLensesAmountError = (MissingEyeglassLensesAmount || InvalidEyeglassLensesAmount);
                FrameAmountError = (MissingFrameAmount || InvalidFrameAmount);
                ACError = (MissingAC || InvalidAC || BadValueAC);
                AmountPaidByPPorGPError = MissingAmountPaidByPPorGP || InvalidAmountPaidByPPorGP || BadAmountPaidByPPorGP;
                TotalAmountChargedError = (MissingTotalAmountCharged || InvalidTotalAmountCharged);
                PrescriptionDetailsError = (MissingLeftSphere || MissingLeftCylinder || MissingLeftAxis || MissingLeftPrism || MissingLeftBifocal ||
                                            MissingRightSphere || MissingRightCylinder || MissingRightAxis || MissingRightPrism || MissingRightBifocal);
            }

            List<bool> errors = new List<bool>();
            errors.Add(AcknowledgedEyewareIsCorrective);
            errors.Add(MissingTypeOfEyewear);
            errors.Add(MissingDateOfPurchase);
            errors.Add(InvalidDateOfPurchase);
            errors.Add(MissingEyeglassLensesAmount);
            errors.Add(InvalidEyeglassLensesAmount);
            errors.Add(MissingFrameAmount);
            errors.Add(InvalidFrameAmount);
            errors.Add(InvalidFeeAmount);
            errors.Add(MissingTypeOfLens);
            errors.Add(MissingAC);
            errors.Add(InvalidAC);
            errors.Add(BadValueAC);
            errors.Add(MissingAmountPaidByPPorGP);
            errors.Add(InvalidAmountPaidByPPorGP);
            errors.Add(BadAmountPaidByPPorGP);
            errors.Add(DateTooOld);
            errors.Add(MissingRightSphere);
            errors.Add(MissingRightCylinder);
            errors.Add(MissingRightAxis);
            errors.Add(MissingRightPrism);
            errors.Add(MissingRightBifocal);
            errors.Add(MissingLeftSphere);
            errors.Add(MissingLeftCylinder);
            errors.Add(MissingLeftAxis);
            errors.Add(MissingLeftPrism);
            errors.Add(MissingLeftBifocal);
            errors.Add(MissingTotalAmountCharged);
            errors.Add(InvalidTotalAmountCharged);
            if (errors.Any(b => b == true))
            {
                RaiseInvalidClaimDetails(new EventArgs());
                return false;
            }
            return true;
        }

        private void PopulateFields()
        {
            TreatmentDetail td = _claimservice.Claim.GetTreatmentDetailByID(EditID);

            if (td != null)
            {
                this.DateOfPurchase = td.DateOfPurchase;
                this.TypeOfEyewear = td.TypeOfEyewear;
                this.TypeOfLens = td.TypeOfLens;
                this.TotalAmountCharged = td.TotalAmountCharged.ToString();
                this.FrameAmount = td.FrameAmount.ToString();
                this.EyeglassLensesAmount = td.EyeglassLensesAmount.ToString();
                this.FeeAmount = td.FeeAmount.ToString();
                this.AcknowledgeEyewareIsCorrective = td.AcknowledgeEyewearIsCorrective;
                if (AmountPaidByAlternateCarrierVisible)
                {
                    this.AmountPaidByAlternateCarrier = td.AlternateCarrierPayment.ToString();
                }

                if (td.IsAmountPaidByPPorGPVisible)
                {
                    this.AmountPaidByPPorGP = td.AmountPaidByPPorGP.ToString();
                }

                if (IsPrescriptionDetailsVisible)
                {
                    if (TypeOfEyewear.ID == 70146) //Prescription Lens, Left Only - 70146
                    {
                        if (VisionSpheres != null && td.LeftSphere != null)
                            this.LeftSphere = VisionSpheres.Where(s => s.ID == td.LeftSphere.ID).FirstOrDefault();

                        if (VisionCylinders != null && td.LeftCylinder != null)
                            this.LeftCylinder = VisionCylinders.Where(c => c.ID == td.LeftCylinder.ID).FirstOrDefault();

                        if (VisionAxes != null && td.LeftAxis != null)
                            this.LeftAxis = VisionAxes.Where(a => a.ID == td.LeftAxis.ID).FirstOrDefault();

                        if (VisionPrisms != null && td.LeftPrism != null)
                            this.LeftPrism = VisionPrisms.Where(p => p.ID == td.LeftPrism.ID).FirstOrDefault();

                        if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                        {
                            if (VisionBifocals != null && td.LeftBifocal != null)
                                this.LeftBifocal = VisionBifocals.Where(b => b.ID == td.LeftBifocal.ID).FirstOrDefault();
                        }
                    }
                    else if (TypeOfEyewear.ID == 70147) //Prescription Lens, Right Only - 70147
                    {
                        if (VisionSpheres != null && td.RightSphere != null)
                            this.RightSphere = VisionSpheres.Where(s => s.ID == td.RightSphere.ID).FirstOrDefault();

                        if (VisionCylinders != null && td.RightCylinder != null)
                            this.RightCylinder = VisionCylinders.Where(c => c.ID == td.RightCylinder.ID).FirstOrDefault();

                        if (VisionAxes != null && td.RightAxis != null)
                            this.RightAxis = VisionAxes.Where(a => a.ID == td.RightAxis.ID).FirstOrDefault();

                        if (VisionPrisms != null && td.RightPrism != null)
                            this.RightPrism = VisionPrisms.Where(p => p.ID == td.RightPrism.ID).FirstOrDefault();

                        if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                        {
                            if (VisionBifocals != null && td.RightBifocal != null)
                                this.RightBifocal = VisionBifocals.Where(b => b.ID == td.RightBifocal.ID).FirstOrDefault();
                        }
                    }
                    else //Prescription Lenses, Pair - 70148, Prescription Glasses, Frame and Lenses - 70144, Prescription Glasses, Frame Only - 70142
                    {
                        if (VisionSpheres != null && td.RightSphere != null)
                            this.RightSphere = VisionSpheres.Where(s => s.ID == td.RightSphere.ID).FirstOrDefault();

                        if (VisionCylinders != null && td.RightCylinder != null)
                            this.RightCylinder = VisionCylinders.Where(c => c.ID == td.RightCylinder.ID).FirstOrDefault();

                        if (VisionAxes != null && td.RightAxis != null)
                            this.RightAxis = VisionAxes.Where(a => a.ID == td.RightAxis.ID).FirstOrDefault();

                        if (VisionPrisms != null && td.RightPrism != null)
                            this.RightPrism = VisionPrisms.Where(p => p.ID == td.RightPrism.ID).FirstOrDefault();

                        if (VisionSpheres != null && td.LeftSphere != null)
                            this.LeftSphere = VisionSpheres.Where(s => s.ID == td.LeftSphere.ID).FirstOrDefault();

                        if (VisionCylinders != null && td.LeftCylinder != null)
                            this.LeftCylinder = VisionCylinders.Where(c => c.ID == td.LeftCylinder.ID).FirstOrDefault();

                        if (VisionAxes != null && td.LeftAxis != null)
                            this.LeftAxis = VisionAxes.Where(a => a.ID == td.LeftAxis.ID).FirstOrDefault();

                        if (VisionPrisms != null && td.LeftPrism != null)
                            this.LeftPrism = VisionPrisms.Where(p => p.ID == td.LeftPrism.ID).FirstOrDefault();

                        if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                        {
                            if (VisionBifocals != null && td.RightBifocal != null)
                                this.RightBifocal = VisionBifocals.Where(b => b.ID == td.RightBifocal.ID).FirstOrDefault();

                            if (VisionBifocals != null && td.LeftBifocal != null)
                                this.LeftBifocal = VisionBifocals.Where(b => b.ID == td.LeftBifocal.ID).FirstOrDefault();
                        }
                    }
                }
            }
        }

        private void SetTypesOfLens()
        {
            if (_claimservice.LensTypes != null)
            {
                TypesOfLens = _claimservice.LensTypes;

                if (TypesOfLens.Count > 0)
                    TypeOfLens = TypesOfLens.Where(lt => lt.ID == "SV").FirstOrDefault(); //default to Single Vision
            }
        }

        private void SetFieldVisibility(PlanConditions pc)
        {
            if (pc == null)
            {
                IsPrescriptionDetailsVisible = false;
            }
            else
            {
                if (pc.VisionPlanType == 0) // Max Dollar plan
                {
                    IsTotalAmountChargedVisible = true;
                    IsFrameLensAndFeeEntryVisible = false;
                    if (pc.VisionRequiresPrescription)
                    {
                        IsPrescriptionDetailsVisible = true;
                        IsAcknowledgeEyewearIsCorrectiveVisible = false;
                    }
                    else
                    {
                        IsPrescriptionDetailsVisible = false;
                        IsAcknowledgeEyewearIsCorrectiveVisible = true;
                    }
                }
                else if (pc.VisionPlanType == 1) // Plan Sponsor Schedule plan
                {
                    IsTotalAmountChargedVisible = false;
                    IsFrameLensAndFeeEntryVisible = true;
                    IsAcknowledgeEyewearIsCorrectiveVisible = false;
                    if (pc.VisionRequiresPrescription)
                    {
                        IsPrescriptionDetailsVisible = true;
                    }
                    else
                    {
                        IsPrescriptionDetailsVisible = false;
                    }
                }
                else if (pc.VisionPlanType == 2) // Provider Schedule plan
                {
                    IsTotalAmountChargedVisible = false;
                    IsFrameLensAndFeeEntryVisible = true;
                    IsPrescriptionDetailsVisible = true;
                    IsAcknowledgeEyewearIsCorrectiveVisible = false;
                }
            }
        }

        private void UpdateAmountEnabledFields()
        {
            if (TypeOfEyewear == null)
            {
                IsFrameAmountEnabled = false;
                IsEyeglassLensesAmountEnabled = false;
            }
            else
            {
                if (TypeOfEyewear.ID == 70146 || TypeOfEyewear.ID == 70147 || TypeOfEyewear.ID == 70148) // lenses only
                {
                    IsFrameAmountEnabled = false;
                    IsEyeglassLensesAmountEnabled = true;
                }
                else if (TypeOfEyewear.ID == 70142) // frame only
                {
                    IsFrameAmountEnabled = true;
                    IsEyeglassLensesAmountEnabled = false;
                }
                else if (TypeOfEyewear.ID == 70144) // frame and lenses
                {
                    IsFrameAmountEnabled = true;
                    IsEyeglassLensesAmountEnabled = true;
                }
                else // probably shouldn't ever get here
                {
                    IsFrameAmountEnabled = false;
                    IsEyeglassLensesAmountEnabled = false;
                }
            }
        }

        private void UpdatePrescriptionEnabledFields()
        {
            if (!IsPrescriptionDetailsVisible || TypeOfEyewear == null)
                return;

            if (TypeOfEyewear.ID == 70146) //Prescription Lens, Left Only - 70146
            {
                IsRightSphereEnabled = false;
                IsRightCylinderEnabled = false;
                IsRightAxisEnabled = false;
                IsRightPrismEnabled = false;
                IsRightBifocalEnabled = false;
                IsLeftSphereEnabled = true;
                IsLeftCylinderEnabled = true;
                IsLeftAxisEnabled = true;
                IsLeftPrismEnabled = true;

                if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                    IsLeftBifocalEnabled = true;
                else
                    IsLeftBifocalEnabled = false;
            }
            else if (TypeOfEyewear.ID == 70147) //Prescription Lens, Right Only - 70147
            {
                IsRightSphereEnabled = true;
                IsRightCylinderEnabled = true;
                IsRightAxisEnabled = true;
                IsRightPrismEnabled = true;
                IsLeftSphereEnabled = false;
                IsLeftCylinderEnabled = false;
                IsLeftAxisEnabled = false;
                IsLeftPrismEnabled = false;
                IsLeftBifocalEnabled = false;

                if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                    IsRightBifocalEnabled = true;
                else
                    IsRightBifocalEnabled = false;
            }
            else //Prescription Lenses, Pair - 70148, Prescription Glasses, Frame and Lenses - 70144, Prescription Glasses, Frame Only - 70142
            {
                IsRightSphereEnabled = true;
                IsRightCylinderEnabled = true;
                IsRightAxisEnabled = true;
                IsRightPrismEnabled = true;
                IsLeftSphereEnabled = true;
                IsLeftCylinderEnabled = true;
                IsLeftAxisEnabled = true;
                IsLeftPrismEnabled = true;

                if (TypeOfLens != null && (TypeOfLens.ID != "SV"))
                {
                    IsLeftBifocalEnabled = true;
                    IsRightBifocalEnabled = true;
                }
                else
                {
                    IsLeftBifocalEnabled = false;
                    IsRightBifocalEnabled = false;
                }
            }
        }
    }
}