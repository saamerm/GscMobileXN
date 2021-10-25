using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MobileClaims.Core.Validators;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimTreatmentDetailsEntryMIViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly MvxSubscriptionToken _claimsubmissionbenefitsretrieved;
        private readonly MvxSubscriptionToken _typesofmedicalprofessionalretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimTreatmentDetailsEntryMIViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;

            if (_claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
            {
                AmountPaidByAlternateCarrierVisible = true;
            }

            ItemDescriptions = _claimservice.ClaimSubmissionBenefits;

            TypesOfMedicalProfessional = _claimservice.TypesOfMedicalProfessional;

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

            _claimsubmissionbenefitsretrieved = _messenger.Subscribe<GetClaimSubmissionBenefitsComplete>((message) =>
            {
                ItemDescriptions = _claimservice.ClaimSubmissionBenefits;
            });

            _typesofmedicalprofessionalretrieved = _messenger.Subscribe<GetTypesOfMedicalProfessionalComplete>((message) =>
            {
                TypesOfMedicalProfessional = _claimservice.TypesOfMedicalProfessional;
            });

            _shouldcloseself = _messenger.Subscribe<ClearClaimTreatmentDetailsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimTreatmentDetailsViewRequested>(_shouldcloseself);
                Close(this);
            });
        }

        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimservice.SelectedClaimSubmissionType;
            }
        }

        public int TreatmentDetailsCount
        {
            get
            {
                return _claimservice.Claim.TreatmentDetails.Count;
            }
        }

        private ClaimSubmissionBenefit _itemDescription;
        public ClaimSubmissionBenefit ItemDescription
        {
            get
            {
                return _itemDescription;
            }
            set
            {
                _itemDescription = value;
                RaisePropertyChanged(() => ItemDescription);
            }
        }

        private DateTime _pickupDate;
        public DateTime PickupDate
        {
            get
            {
                return (this._pickupDate == DateTime.MinValue) ? DateTime.Now : this._pickupDate;
            }
            set
            {
                _pickupDate = value;
                RaisePropertyChanged(() => PickupDate);
            }
        }

        private string _quantity;
        public string Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                RaisePropertyChanged(() => Quantity);
            }
        }

        private string _totalAmountCharged;
        public string TotalAmountCharged
        {
            get
            {
                return (String.IsNullOrEmpty(this._totalAmountCharged)) ? "" : this._totalAmountCharged;
            }
            set
            {
                _totalAmountCharged = value;
                RaisePropertyChanged(() => TotalAmountCharged);
            }
        }

        private string _amountPaidByAlternateCarrier;
        public string AmountPaidByAlternateCarrier
        {
            get
            {
                return (String.IsNullOrEmpty(this._amountPaidByAlternateCarrier)) ? "" : this._amountPaidByAlternateCarrier;
            }
            set
            {
                _amountPaidByAlternateCarrier = value;
                RaisePropertyChanged(() => AmountPaidByAlternateCarrier);
            }
        }

        private bool _gstHstIncludedInTotal = false;
        public bool GSTHSTIncludedInTotal
        {
            get
            {
                return _gstHstIncludedInTotal;
            }
            set
            {
                _gstHstIncludedInTotal = value;
                RaisePropertyChanged(() => GSTHSTIncludedInTotal);
            }
        }

        private bool _pstIncludedInTotal = false;
        public bool PSTIncludedInTotal
        {
            get
            {
                return _pstIncludedInTotal;
            }
            set
            {
                _pstIncludedInTotal = value;
                RaisePropertyChanged(() => PSTIncludedInTotal);
            }
        }

        private bool _hasReferralBeenPreviouslySubmitted = false;
        public bool HasReferralBeenPreviouslySubmitted
        {
            get
            {
                return _hasReferralBeenPreviouslySubmitted;
            }
            set
            {
                _hasReferralBeenPreviouslySubmitted = value;
                if (_hasReferralBeenPreviouslySubmitted)
                {
                    Questions14And15Enabled = true;                    
                }
                else
                {
                    Questions14And15Enabled = false;                    
                }
                Questions14And15Visible = value;
                RaisePropertyChanged(() => HasReferralBeenPreviouslySubmitted);
            }
        }

        private DateTime _dateOfReferral;
        public DateTime DateOfReferral
        {
            get
            {
                return (this._dateOfReferral == DateTime.MinValue) ? DateTime.Now : this._dateOfReferral;
            }
            set
            {
                _dateOfReferral = value;
                if (_dateOfReferral > DateTime.MinValue)
                    IsDateOfReferralSetByUser = true;
                else
                    IsDateOfReferralSetByUser = false;
                RaisePropertyChanged(() => DateOfReferral);
            }
        }

        private ClaimOption _typeOfMedicalProfessional;
        public ClaimOption TypeOfMedicalProfessional
        {
            get
            {
                return _typeOfMedicalProfessional;
            }
            set
            {
                _typeOfMedicalProfessional = value;
                RaisePropertyChanged(() => TypeOfMedicalProfessional);
            }
        }

        private bool _questions14And15Enabled = false;
        public bool Questions14And15Enabled
        {
            get
            {
                return _questions14And15Enabled;
            }
            set
            {
                _questions14And15Enabled = value;
                RaisePropertyChanged(() => Questions14And15Enabled);
            }
        }

        private bool _questions14And15Visible = false;
        public bool Questions14And15Visible
        {
            get
            {
                return _questions14And15Visible;
            }
            set
            {
                _questions14And15Visible = value;
                RaisePropertyChanged(() => Questions14And15Visible);
            }
        }

        private bool _amountPaidByAlternateCarrierVisible = false;
        public bool AmountPaidByAlternateCarrierVisible
        {
            get
            {
                return _amountPaidByAlternateCarrierVisible;
            }
            set
            {
                _amountPaidByAlternateCarrierVisible = value;
                RaisePropertyChanged(() => AmountPaidByAlternateCarrierVisible);
            }
        }

        private List<ClaimSubmissionBenefit> _itemDescriptions;
        public List<ClaimSubmissionBenefit> ItemDescriptions
        {
            get
            {
                return _itemDescriptions;
            }
            set
            {
                _itemDescriptions = value;
                if (_itemDescriptions != null && _itemDescriptions.Count == 1)
                    ItemDescription = _itemDescriptions.FirstOrDefault();
                RaisePropertyChanged(() => ItemDescriptions);
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
                RaisePropertyChanged(() => TypesOfMedicalProfessional);
            }
        }

        private bool _editMode = false;
        public bool EditMode
        {
            get
            {
                return _editMode;
            }
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

        private bool _isDateOfReferralSetByUser = false;
        public bool IsDateOfReferralSetByUser
        {
            get
            {
                return _isDateOfReferralSetByUser;
            }
            set
            {
                _isDateOfReferralSetByUser = value;
                RaisePropertyChanged(() => IsDateOfReferralSetByUser);
            }
        }

        private bool _missingItemDescription = false;
        public bool MissingItemDescription
        {
            get
            {
                return _missingItemDescription;
            }
            set
            {
                _missingItemDescription = value;
                RaisePropertyChanged(() => MissingItemDescription);
            }
        }
        private bool _missingPickupDate = false;
        public bool MissingPickupDate
        {
            get
            {
                return _missingPickupDate;
            }
            set
            {
                _missingPickupDate = value;
                RaisePropertyChanged(() => MissingPickupDate);
            }
        }
        private bool _pickupDateError = false;
        public bool PickupDateError
        {
            get
            {
                return _pickupDateError;
            }
            set
            {
                _pickupDateError = value;
                RaisePropertyChanged(() => PickupDateError);
            }
        }
        private bool _invalidPickupDate = false;
        public bool InvalidPickupDate
        {
            get
            {
                return _invalidPickupDate;
            }
            set
            {
                _invalidPickupDate = value;
                RaisePropertyChanged(() => InvalidPickupDate);
            }
        }
        private bool _missingTotalAmount = false;
        public bool MissingTotalAmount
        {
            get
            {
                return _missingTotalAmount;
            }
            set
            {
                _missingTotalAmount = value;
                RaisePropertyChanged(() => MissingTotalAmount);
            }
        }
        private bool _invalidTotalAmount = false;
        public bool InvalidTotalAmount
        {
            get
            {
                return _invalidTotalAmount;
            }
            set
            {
                if (!MissingTotalAmount) //Only return invalid when total isn't missing                
                {
                    _invalidTotalAmount = value;
                    RaisePropertyChanged(() => InvalidTotalAmount);
                }
            }
        }

        private bool _totalAmountError = false;
        public bool TotalAmountError
        {
            get
            {
                return _totalAmountError;
            }
            set
            {
                _totalAmountError = value;
                RaisePropertyChanged(() => TotalAmountError);
            }
        }
        private bool _missingAC = false;
        public bool MissingAC
        {
            get
            {
                return _missingAC;
            }
            set
            {
                _missingAC = value;
                RaisePropertyChanged(() => MissingAC);
            }
        }
        private bool _invalidAC = false;
        public bool InvalidAC
        {
            get
            {
                return _invalidAC;
            }
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
            get
            {
                return _ACError;
            }
            set
            {
                _ACError = value;
                RaisePropertyChanged(() => ACError);
            }
        }
        private bool _missingQuantity = false;
        public bool MissingQuantity
        {
            get
            {
                return _missingQuantity;
            }
            set
            {
                _missingQuantity = value;
                RaisePropertyChanged(() => MissingQuantity);
            }
        }
        private bool _invalidQuantity = false;
        public bool InvalidQuantity
        {
            get
            {
                return _invalidQuantity;
            }
            set
            {
                if (!MissingQuantity) //Only return invalid when quanity isn't missing                
                {
                    _invalidQuantity = value;
                    RaisePropertyChanged(() => InvalidQuantity);
                }
            }
        }

        private bool _quantityError = false;
        public bool QuantityError
        {
            get
            {
                return _quantityError;
            }
            set
            {
                _quantityError = value;
                RaisePropertyChanged(() => QuantityError);
            }
        }

        private bool _emptyTypeOfMedicalProfessional = false;
        public bool EmptyTypeOfMedicalProfessional
        {
            get
            {
                return _emptyTypeOfMedicalProfessional;
            }
            set
            {
                _emptyTypeOfMedicalProfessional = value;
                RaisePropertyChanged(() => EmptyTypeOfMedicalProfessional);
            }
        }

        private bool _badvalueAC = false;
        public bool BadValueAC
        {
            get
            {
                return _badvalueAC;
            }
            set
            {
                _badvalueAC = value;
                RaisePropertyChanged(() => BadValueAC);
            }
        }

        private bool _dateTooOld = false;
        public bool DateTooOld
        {
            get
            {
                return _dateTooOld;
            }
            set
            {
                _dateTooOld = value;
                RaisePropertyChanged(() => DateTooOld);
            }
        }

        private bool _dateOfReferralTooOld = false;
        public bool DateOfReferralTooOld
        {
            get
            {
                return _dateOfReferralTooOld;
            }
            set
            {
                _dateOfReferralTooOld = value;
                RaisePropertyChanged(() => DateOfReferralTooOld);
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

        public ICommand SubmitEntryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        Unsubscribe();
                        int q = 0;
                        int.TryParse(this.Quantity, out q);
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail td = new TreatmentDetail()
                        {
                            ID = Guid.NewGuid(),
                            ClaimSubmissionType = this.ClaimSubmissionType,
                            ItemDescription = this.ItemDescription,
                            PickupDate = this.PickupDate,
                            Quantity = q,
                            TreatmentAmount = amt,
                            AlternateCarrierPayment = acp,
                            GSTHSTIncludedInTotal = this.GSTHSTIncludedInTotal,
                            PSTIncludedInTotal = this.PSTIncludedInTotal,
                            HasReferralBeenPreviouslySubmitted = this.HasReferralBeenPreviouslySubmitted,
                            TypeOfMedicalProfessional = this.TypeOfMedicalProfessional,
                            DateOfReferral = this._dateOfReferral,
                            IsAlternateCarrierPaymentVisible = this.AmountPaidByAlternateCarrierVisible
                        };
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
                        Unsubscribe();
                        int q = 0;
                        int.TryParse(this.Quantity, out q);
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountCharged);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail updated = null;
                        foreach (TreatmentDetail td in _claimservice.Claim.TreatmentDetails)
                        {
                            if (td.ID == EditID)
                            {
                                td.ClaimSubmissionType = this.ClaimSubmissionType;
                                td.ItemDescription = this.ItemDescription;
                                td.PickupDate = this.PickupDate;
                                td.Quantity = q;
                                td.TreatmentAmount = amt;
                                td.AlternateCarrierPayment = acp;
                                td.GSTHSTIncludedInTotal = this.GSTHSTIncludedInTotal;
                                td.PSTIncludedInTotal = this.PSTIncludedInTotal;
                                td.HasReferralBeenPreviouslySubmitted = this.HasReferralBeenPreviouslySubmitted;
                                td.TypeOfMedicalProfessional = this.TypeOfMedicalProfessional;
                                td.DateOfReferral = this._dateOfReferral;
                                td.IsAlternateCarrierPaymentVisible = this.AmountPaidByAlternateCarrierVisible;
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
                    Unsubscribe();
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

        private bool IsValid()
        {
            // Reset for validation
            MissingItemDescription = false;
            MissingPickupDate = false;
            InvalidPickupDate = false;
            PickupDateError = false;
            MissingTotalAmount = false;
            InvalidTotalAmount = false;
            TotalAmountError = false;
            MissingAC = false;
            InvalidAC = false;
            ACError = false;
            MissingQuantity = false;
            InvalidQuantity = false;
            QuantityError = false;
            EmptyTypeOfMedicalProfessional = false;
            BadValueAC = false;
            DateTooOld = false;
            DateOfReferralTooOld = false;
            TreatmentDetailEntryMIValidator validator = new TreatmentDetailEntryMIValidator();
            ValidationResult result = null;
            string validationRuleSet = "default";
            if (AmountPaidByAlternateCarrierVisible) validationRuleSet += ",AlternateCarrier";
            if (_dateOfReferral > DateTime.MinValue) validationRuleSet += ",DateOfReferral";

            result = validator.Validate<ClaimTreatmentDetailsEntryMIViewModel>(this, ruleSet: validationRuleSet);

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Item":
                            MissingItemDescription = true;
                            break;
                        case "Empty Date":
                            MissingPickupDate = true;
                            break;
                        case "Future Date":
                            InvalidPickupDate = true;
                            break;
                        case "Empty Amount":
                            MissingTotalAmount = true;
                            break;
                        case "Invalid Amount":
                        case "Amount TooHigh":
                            InvalidTotalAmount = true;
                            break;
                        case "Empty Quantity":
                            MissingQuantity = true;
                            break;
                        case "Invalid Quantity":
                            InvalidQuantity = true;
                            break;
                        case "Empty AC":
                            MissingAC = true;
                            break;
                        case "Invalid AC":
                            InvalidAC = true;
                            break;
                        case "Empty TypeOfMedicalProfessional":
                            EmptyTypeOfMedicalProfessional = true;
                            break;
                        case "BadValue AC":
                            BadValueAC = true;
                            break;
                        case "Date TooOld":
                            DateTooOld = true;
                            break;
                        case "DateOfReferral TooOld":
                            DateOfReferralTooOld = true;
                            break;
                    }
                }
                PickupDateError = (MissingPickupDate || InvalidPickupDate || DateTooOld);
                TotalAmountError = (MissingTotalAmount || InvalidTotalAmount);
                QuantityError = (MissingQuantity || InvalidQuantity);
                ACError = (MissingAC || InvalidAC || BadValueAC);
            }

            List<bool> errors = new List<bool>();
            errors.Add(MissingItemDescription);
            errors.Add(MissingPickupDate);
            errors.Add(InvalidPickupDate);
            errors.Add(MissingTotalAmount);
            errors.Add(InvalidTotalAmount);
            errors.Add(MissingAC);
            errors.Add(InvalidAC);
            errors.Add(MissingQuantity);
            errors.Add(InvalidQuantity);
            errors.Add(EmptyTypeOfMedicalProfessional);
            errors.Add(BadValueAC);
            errors.Add(DateTooOld);
            errors.Add(DateOfReferralTooOld);
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
                this.ItemDescription = td.ItemDescription;
                this.PickupDate = td.PickupDate;
                this.Quantity = td.Quantity.ToString();
                this.TotalAmountCharged = td.TreatmentAmount.ToString();
                if (AmountPaidByAlternateCarrierVisible) this.AmountPaidByAlternateCarrier = td.AlternateCarrierPayment.ToString();
                this.GSTHSTIncludedInTotal = td.GSTHSTIncludedInTotal;
                this.PSTIncludedInTotal = td.PSTIncludedInTotal;
                this.HasReferralBeenPreviouslySubmitted = td.HasReferralBeenPreviouslySubmitted;
                this.TypeOfMedicalProfessional = td.TypeOfMedicalProfessional;
                this.DateOfReferral = td.DateOfReferral;
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<GetClaimSubmissionBenefitsComplete>(_claimsubmissionbenefitsretrieved);
            _messenger.Unsubscribe<GetTypesOfMedicalProfessionalComplete>(_typesofmedicalprofessionalretrieved);
        }
    }

}
