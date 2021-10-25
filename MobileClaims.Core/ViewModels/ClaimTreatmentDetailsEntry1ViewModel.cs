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
    public class ClaimTreatmentDetailsEntry1ViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly MvxSubscriptionToken _claimsubmissionbenefitsretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimTreatmentDetailsEntry1ViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;

            if (_claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
            {
                AmountPaidByAlternateCarrierVisible = true;
            }

            TypesOfTreatment = _claimservice.ClaimSubmissionBenefits;
            
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
                TypesOfTreatment = _claimservice.ClaimSubmissionBenefits;
            });

            _shouldcloseself = _messenger.Subscribe<ClearClaimTreatmentDetailsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimTreatmentDetailsViewRequested>(_shouldcloseself);
                Close(this);
            });
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

        public ClaimSubmissionType ClaimSubmissionType => _claimservice.SelectedClaimSubmissionType;

        public int TreatmentDetailsCount => _claimservice.Claim.TreatmentDetails.Count;

        private ClaimSubmissionBenefit _typeOfTreatment;
        public ClaimSubmissionBenefit TypeOfTreatment
        {
            get => _typeOfTreatment;
            set
            {
                _typeOfTreatment = value;
                RaisePropertyChanged(() => TypeOfTreatment);
            }
        }

        private DateTime _dateOfTreatment;
        public DateTime DateOfTreatment
        {
            get => (this._dateOfTreatment == DateTime.MinValue) ? DateTime.Now : this._dateOfTreatment;
            set
            {
                _dateOfTreatment = value;
                RaisePropertyChanged(() => DateOfTreatment);
            }
        }

        private string _totalAmountOfVisit;
        public string TotalAmountOfVisit
        {
            get => (String.IsNullOrEmpty(this._totalAmountOfVisit)) ? "" : this._totalAmountOfVisit;
            set
            {
                _totalAmountOfVisit = value;
                    RaisePropertyChanged(() => TotalAmountOfVisit);
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

        private List<ClaimSubmissionBenefit> _typesOfTreatment;
        public List<ClaimSubmissionBenefit> TypesOfTreatment
        {
            get => _typesOfTreatment;
            set
            {
                _typesOfTreatment = value;
                if (_typesOfTreatment != null && _typesOfTreatment.Count == 1)
                    TypeOfTreatment = _typesOfTreatment.FirstOrDefault();
                RaisePropertyChanged(() => TypesOfTreatment);
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
                double tav = GSCHelper.GetDollarAmount(this.TotalAmountOfVisit);

                return tav;
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

        public ICommand SubmitEntryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        Unsubscribe();
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountOfVisit);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail td = new TreatmentDetail()
                        {
                            ID = Guid.NewGuid(),
                            ClaimSubmissionType = this.ClaimSubmissionType,
                            TypeOfTreatment = this.TypeOfTreatment,
                            TreatmentDate = this.DateOfTreatment,
                            TreatmentAmount = amt,
                            AlternateCarrierPayment = acp,
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
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountOfVisit);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail updated = null;
                        foreach (TreatmentDetail td in _claimservice.Claim.TreatmentDetails)
                        {
                            if (td.ID == EditID)
                            {
                                td.ClaimSubmissionType = this.ClaimSubmissionType;
                                td.TypeOfTreatment = this.TypeOfTreatment;
                                td.TreatmentDate = this.DateOfTreatment;
                                td.TreatmentAmount = amt;
                                td.AlternateCarrierPayment = acp;
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

        private bool _missingTypeOfTreatment = false;
        public bool MissingTypeOfTreatment
        {
            get => _missingTypeOfTreatment;
            set
            {
                _missingTypeOfTreatment = value;
                RaisePropertyChanged(() => MissingTypeOfTreatment);
            }
        }
        private bool _missingDateOfTreatment = false;
        public bool MissingDateOfTreatment
        {
            get => _missingDateOfTreatment;
            set
            {
                _missingDateOfTreatment = value;
                RaisePropertyChanged(() => MissingDateOfTreatment);
            }
        }
        private bool _invalidDateOfTreatment = false;
        public bool InvalidDateOfTreatment
        {
            get => _invalidDateOfTreatment;
            set
            {
                _invalidDateOfTreatment = value;
                RaisePropertyChanged(() => InvalidDateOfTreatment);
            }
        }
        private bool _dateOfTreatmentError = false;
        public bool DateOfTreatmentError
        {
            get => _dateOfTreatmentError;
            set
            {
                _dateOfTreatmentError = value;
                RaisePropertyChanged(() => DateOfTreatmentError);
            }
        }
        private bool _missingTotalAmount = false;
        public bool MissingTotalAmount
        {
            get => _missingTotalAmount;
            set
            {
                _missingTotalAmount = value;
                RaisePropertyChanged(() => MissingTotalAmount);
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
        private bool _invalidTotalAmount = false;
        public bool InvalidTotalAmount
        {
            get => _invalidTotalAmount;
            set
            {
                if (!MissingTotalAmount) //Only return invalid when Total isn't missing
                {
                    _invalidTotalAmount = value;
                    RaisePropertyChanged(() => InvalidTotalAmount);
                }                
            }
        }

        private bool _totalAmountError = false;
        public bool TotalAmountError
        {
            get => _totalAmountError;
            set
            {
                _totalAmountError = value;
                RaisePropertyChanged(() => TotalAmountError);
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

        private bool IsValid()
        {
            // Reset for validation
            MissingTypeOfTreatment = false;
            MissingDateOfTreatment = false;
            InvalidDateOfTreatment = false;
            DateOfTreatmentError = false;
            MissingTotalAmount = false;
            InvalidTotalAmount = false;
            MissingAC = false;
            InvalidAC = false;
            ACError = false;
            TotalAmountError = false;
            BadValueAC = false;
            DateTooOld = false;
            TreatmentDetailEntry1Validator validator = new TreatmentDetailEntry1Validator();
            ValidationResult result = null;
            if (AmountPaidByAlternateCarrierVisible)
                result = validator.Validate<ClaimTreatmentDetailsEntry1ViewModel>(this, ruleSet: "default,AlternateCarrier");
            else
                result = validator.Validate<ClaimTreatmentDetailsEntry1ViewModel>(this, ruleSet: "default");
            
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Type":
                            MissingTypeOfTreatment = true;
                            break;
                        case "Empty Date":
                            MissingDateOfTreatment = true;
                            break;
                        case "Future Date":
                            InvalidDateOfTreatment = true;
                            break;
                        case "Empty Amount":
                            MissingTotalAmount = true;
                            break;
                        case "Invalid Amount":
                            InvalidTotalAmount = true;
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
                        case "Date TooOld":
                            DateTooOld = true;
                            break;
                    }
                }
                TotalAmountError = (MissingTotalAmount || InvalidTotalAmount);
                DateOfTreatmentError = (MissingDateOfTreatment || InvalidDateOfTreatment || DateTooOld);
                ACError = (MissingAC || InvalidAC || BadValueAC);
            }

            List<bool> errors = new List<bool>();
            errors.Add(MissingTypeOfTreatment);
            errors.Add(MissingDateOfTreatment);
            errors.Add(InvalidDateOfTreatment);
            errors.Add(MissingTotalAmount);
            errors.Add(InvalidTotalAmount);
            errors.Add(MissingAC);
            errors.Add(InvalidAC);
            errors.Add(BadValueAC);
            errors.Add(DateTooOld);
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
                this.TypeOfTreatment = td.TypeOfTreatment;
                this.DateOfTreatment = td.TreatmentDate;
                this.TotalAmountOfVisit = td.TreatmentAmount.ToString();
                if (AmountPaidByAlternateCarrierVisible) this.AmountPaidByAlternateCarrier = td.AlternateCarrierPayment.ToString();
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<GetClaimSubmissionBenefitsComplete>(_claimsubmissionbenefitsretrieved);
        }
    }
}
