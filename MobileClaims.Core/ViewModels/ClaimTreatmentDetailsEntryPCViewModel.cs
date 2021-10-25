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
    public class ClaimTreatmentDetailsEntryPCViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IParticipantService _participantservice;
        private readonly MvxSubscriptionToken _claimsubmissionbenefitsretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly MvxSubscriptionToken _visionspheresretrieved;
        private readonly MvxSubscriptionToken _visioncylindersretrieved;
        private readonly MvxSubscriptionToken _visionaxesretrieved;
        private readonly MvxSubscriptionToken _visionprismsretrieved;
        private readonly MvxSubscriptionToken _visionbifocalsretrieved;

        public string AmountPaidByPPorGPText => Resource.TreatmentDetailsAmountPaidByPpOrGpIfApplicable;
        public string MissingAmountPaidByPpOrGpErrorMessage => Resource.MissingAmountPaidByPpOrGpValidation;
        public string InvalidAmountPaidByPpOrGpErrorMessage => Resource.InvalidAmountPaidByPpOrGpValidation;

        public ClaimTreatmentDetailsEntryPCViewModel(IMvxMessenger messenger, IClaimService claimservice, IParticipantService participantservice)
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

            if (_participantservice.PlanMember != null && !string.IsNullOrEmpty(_participantservice.PlanMember.PlanMemberID))
            {
                if (_participantservice.PlanMember.PlanConditions != null)
                {
                    bool vrp = _participantservice.PlanMember.PlanConditions.VisionRequiresPrescription;
                    IsPrescriptionDetailsVisible = vrp;
                    IsAcknowledgeEyewearIsCorrectiveVisible = !vrp;
                }
            }

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

            _claimsubmissionbenefitsretrieved = _messenger.Subscribe<GetClaimSubmissionBenefitsComplete>((message) =>
            {
                TypesOfEyewear = _claimservice.ClaimSubmissionBenefits;
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
                UpdatePrescriptionEnabledFields();
                RaisePropertyChanged(() => TypeOfEyewear);
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
            get => (String.IsNullOrEmpty(this._totalAmountCharged)) ? "" : this._totalAmountCharged;
            set
            {
                _totalAmountCharged = value;
                RaisePropertyChanged(() => TotalAmountCharged);
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

        public double TotalDollarValue
        {
            get
            {
                double tac = GSCHelper.GetDollarAmount(this.TotalAmountCharged);

                return tac;
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
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail td = new TreatmentDetail()
                        {
                            ID = Guid.NewGuid(),
                            ClaimSubmissionType = this.ClaimSubmissionType,
                            DateOfPurchase = this.DateOfPurchase,
                            TypeOfEyewear = this.TypeOfEyewear,
                            IsAcknowledgeEyewearIsCorrectiveVisible = this.IsAcknowledgeEyewearIsCorrectiveVisible,
                            AcknowledgeEyewearIsCorrective = this.AcknowledgeEyewareIsCorrective,
                            TreatmentAmount = amt,
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
                            td.IsRightBifocalEnabled = true;
                            td.IsLeftSphereEnabled = this.IsLeftSphereEnabled;
                            td.IsLeftCylinderEnabled = this.IsLeftCylinderEnabled;
                            td.IsLeftAxisEnabled = this.IsLeftAxisEnabled;
                            td.IsLeftPrismEnabled = this.IsLeftPrismEnabled;
                            td.IsLeftBifocalEnabled = true;
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
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail updated = null;
                        foreach (TreatmentDetail td in _claimservice.Claim.TreatmentDetails)
                        {
                            if (td.ID == EditID)
                            {
                                td.ClaimSubmissionType = this.ClaimSubmissionType;
                                td.DateOfPurchase = this.DateOfPurchase;
                                td.TypeOfEyewear = this.TypeOfEyewear;
                                td.IsAcknowledgeEyewearIsCorrectiveVisible = this.IsAcknowledgeEyewearIsCorrectiveVisible;
                                td.AcknowledgeEyewearIsCorrective = this.AcknowledgeEyewareIsCorrective;
                                td.TreatmentAmount = amt;
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
                                    td.IsRightBifocalEnabled = true;
                                    td.IsLeftSphereEnabled = this.IsLeftSphereEnabled;
                                    td.IsLeftCylinderEnabled = this.IsLeftCylinderEnabled;
                                    td.IsLeftAxisEnabled = this.IsLeftAxisEnabled;
                                    td.IsLeftPrismEnabled = this.IsLeftPrismEnabled;
                                    td.IsLeftBifocalEnabled = true;
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
                if (!MissingTotalAmountCharged) //Only return invalid when quanity isn't missing                
                {
                    _invalidTotalAmountCharged = value;
                    RaisePropertyChanged(() => InvalidTotalAmountCharged);
                }
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

        // Added so that icon will not be present until user saves
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
            MissingTotalAmountCharged = false;
            InvalidTotalAmountCharged = false;
            TotalAmountChargedError = false;
            MissingAC = false;
            InvalidAC = false;
            ACError = false;
            BadValueAC = false;
            MissingAmountPaidByPPorGP = false;
            InvalidAmountPaidByPPorGP = false;
            BadAmountPaidByPPorGP = false;
            AmountPaidByPPorGPError = false;
            DateTooOld = false;
            AcknowledgedEyewareIsCorrective = !AcknowledgeEyewareIsCorrective;
            MissingRightSphere = false;
            MissingRightCylinder = false;
            MissingRightAxis = false;
            MissingRightPrism = false;
            MissingLeftSphere = false;
            MissingLeftCylinder = false;
            MissingLeftAxis = false;
            MissingLeftPrism = false;
            PrescriptionDetailsError = false;
            TreatmentDetailEntryPCValidator validator = new TreatmentDetailEntryPCValidator();
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
                if (TypeOfEyewear.ID == 70101) // Left
                    validationRuleSet += ",Left";
                else if (TypeOfEyewear.ID == 70103) // Right
                    validationRuleSet += ",Right";
                else // Pairs
                    validationRuleSet += ",Pairs";
            }

            result = validator.Validate<ClaimTreatmentDetailsEntryPCViewModel>(this, ruleSet: validationRuleSet);

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
                        case "Empty Amount":
                            MissingTotalAmountCharged = true;
                            break;
                        case "Invalid Amount":
                            InvalidTotalAmountCharged = true;
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
                    }
                }
                DateOfPurchaseError = (MissingDateOfPurchase || InvalidDateOfPurchase || DateTooOld);
                TotalAmountChargedError = (MissingTotalAmountCharged || InvalidTotalAmountCharged);
                ACError = (MissingAC || InvalidAC || BadValueAC);
                AmountPaidByPPorGPError = MissingAmountPaidByPPorGP || InvalidAmountPaidByPPorGP || BadAmountPaidByPPorGP;
                PrescriptionDetailsError = (MissingLeftSphere || MissingLeftCylinder || MissingLeftAxis || MissingLeftPrism ||
                                            MissingRightSphere || MissingRightCylinder || MissingRightAxis || MissingRightPrism);
            }

            List<bool> errors = new List<bool>();
            errors.Add(AcknowledgedEyewareIsCorrective);
            errors.Add(MissingTypeOfEyewear);
            errors.Add(MissingDateOfPurchase);
            errors.Add(InvalidDateOfPurchase);
            errors.Add(MissingTotalAmountCharged);
            errors.Add(InvalidTotalAmountCharged);
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
            errors.Add(MissingLeftSphere);
            errors.Add(MissingLeftCylinder);
            errors.Add(MissingLeftAxis);
            errors.Add(MissingLeftPrism);
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
                this.AcknowledgeEyewareIsCorrective = td.AcknowledgeEyewearIsCorrective;
                this.TotalAmountCharged = td.TreatmentAmount.ToString();
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
                    if (VisionBifocals != null && td.RightBifocal != null)
                        this.RightBifocal = VisionBifocals.FirstOrDefault(b => b.ID == td.RightBifocal.ID);

                    if (VisionBifocals != null && td.LeftBifocal != null)
                        this.LeftBifocal = VisionBifocals.FirstOrDefault(b => b.ID == td.LeftBifocal.ID);

                    if (TypeOfEyewear.ID == 70101) //Prescription Contact(s), Left Only - 70101
                    {
                        if (VisionSpheres != null && td.LeftSphere != null)
                            this.LeftSphere = VisionSpheres.FirstOrDefault(s => s.ID == td.LeftSphere.ID);

                        if (VisionCylinders != null && td.LeftCylinder != null)
                            this.LeftCylinder = VisionCylinders.FirstOrDefault(c => c.ID == td.LeftCylinder.ID);

                        if (VisionAxes != null && td.LeftAxis != null)
                            this.LeftAxis = VisionAxes.FirstOrDefault(a => a.ID == td.LeftAxis.ID);

                        if (VisionPrisms != null && td.LeftPrism != null)
                            this.LeftPrism = VisionPrisms.FirstOrDefault(p => p.ID == td.LeftPrism.ID);
                    }
                    else if (TypeOfEyewear.ID == 70103) //Prescription Contact(s), Right Only - 70103
                    {
                        if (VisionSpheres != null && td.RightSphere != null)
                            this.RightSphere = VisionSpheres.FirstOrDefault(s => s.ID == td.RightSphere.ID);

                        if (VisionCylinders != null && td.RightCylinder != null)
                            this.RightCylinder = VisionCylinders.FirstOrDefault(c => c.ID == td.RightCylinder.ID);

                        if (VisionAxes != null && td.RightAxis != null)
                            this.RightAxis = VisionAxes.FirstOrDefault(a => a.ID == td.RightAxis.ID);

                        if (VisionPrisms != null && td.RightPrism != null)
                            this.RightPrism = VisionPrisms.FirstOrDefault(p => p.ID == td.RightPrism.ID);
                    }
                    else //Prescription Contact(s), Pairs - 70102
                    {
                        if (VisionSpheres != null && td.RightSphere != null)
                            this.RightSphere = VisionSpheres.FirstOrDefault(s => s.ID == td.RightSphere.ID);

                        if (VisionCylinders != null && td.RightCylinder != null)
                            this.RightCylinder = VisionCylinders.FirstOrDefault(c => c.ID == td.RightCylinder.ID);

                        if (VisionAxes != null && td.RightAxis != null)
                            this.RightAxis = VisionAxes.FirstOrDefault(a => a.ID == td.RightAxis.ID);

                        if (VisionPrisms != null && td.RightPrism != null)
                            this.RightPrism = VisionPrisms.FirstOrDefault(p => p.ID == td.RightPrism.ID);

                        if (VisionSpheres != null && td.LeftSphere != null)
                            this.LeftSphere = VisionSpheres.FirstOrDefault(s => s.ID == td.LeftSphere.ID);

                        if (VisionCylinders != null && td.LeftCylinder != null)
                            this.LeftCylinder = VisionCylinders.FirstOrDefault(c => c.ID == td.LeftCylinder.ID);

                        if (VisionAxes != null && td.LeftAxis != null)
                            this.LeftAxis = VisionAxes.FirstOrDefault(a => a.ID == td.LeftAxis.ID);

                        if (VisionPrisms != null && td.LeftPrism != null)
                            this.LeftPrism = VisionPrisms.FirstOrDefault(p => p.ID == td.LeftPrism.ID);
                    }
                }
            }
        }

        private void UpdatePrescriptionEnabledFields()
        {
            if (!IsPrescriptionDetailsVisible)
                return;

            if (TypeOfEyewear.ID == 70101) //Prescription Contact(s), Left Only - 70101
            {
                IsRightSphereEnabled = false;
                IsRightCylinderEnabled = false;
                IsRightAxisEnabled = false;
                IsRightPrismEnabled = false;
                IsLeftSphereEnabled = true;
                IsLeftCylinderEnabled = true;
                IsLeftAxisEnabled = true;
                IsLeftPrismEnabled = true;
            }
            else if (TypeOfEyewear.ID == 70103) //Prescription Contact(s), Right Only - 70103
            {
                IsRightSphereEnabled = true;
                IsRightCylinderEnabled = true;
                IsRightAxisEnabled = true;
                IsRightPrismEnabled = true;
                IsLeftSphereEnabled = false;
                IsLeftCylinderEnabled = false;
                IsLeftAxisEnabled = false;
                IsLeftPrismEnabled = false;
            }
            else //Prescription Contact(s), Pairs - 70102
            {
                IsRightSphereEnabled = true;
                IsRightCylinderEnabled = true;
                IsRightAxisEnabled = true;
                IsRightPrismEnabled = true;
                IsLeftSphereEnabled = true;
                IsLeftCylinderEnabled = true;
                IsLeftAxisEnabled = true;
                IsLeftPrismEnabled = true;
            }
        }
    }
}