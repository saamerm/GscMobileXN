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
    public class ClaimTreatmentDetailsEntryOMFViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly MvxSubscriptionToken _claimsubmissionbenefitsretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimTreatmentDetailsEntryOMFViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;

            _claimsubmissionbenefitsretrieved = _messenger.Subscribe<GetClaimSubmissionBenefitsComplete>((message) =>
            {
                TypeOfTreatment = _claimservice.ClaimSubmissionBenefits.FirstOrDefault();
            });

            if (_claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
            {
                AmountPaidByAlternateCarrierVisible = true;
            }

            if (_claimservice.ClaimSubmissionBenefits != null && _claimservice.ClaimSubmissionBenefits.Count > 0)
                TypeOfTreatment = _claimservice.ClaimSubmissionBenefits.FirstOrDefault();

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

        private ClaimSubmissionBenefit _typeOfTreatment;
        public ClaimSubmissionBenefit TypeOfTreatment
        {
            get
            {
                return _typeOfTreatment;
            }
            set
            {
                _typeOfTreatment = value;
                RaisePropertyChanged(() => TypeOfTreatment);
            }
        }

        private DateTime _dateOfMonthlyTreatment;
        public DateTime DateOfMonthlyTreatment
        {
            get
            {
                return (this._dateOfMonthlyTreatment == DateTime.MinValue) ? DateTime.Now : this._dateOfMonthlyTreatment;
            }
            set
            {
                _dateOfMonthlyTreatment = value;
                RaisePropertyChanged(() => DateOfMonthlyTreatment);
            }
        }

        private string _orthodonticMonthlyFee;
        public string OrthodonticMonthlyFee
        {
            get
            {
                return (String.IsNullOrEmpty(this._orthodonticMonthlyFee)) ? "" : this._orthodonticMonthlyFee;
            }
            set
            {
                _orthodonticMonthlyFee = value;
                RaisePropertyChanged(() => OrthodonticMonthlyFee);
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
                double omf = GSCHelper.GetDollarAmount(this.OrthodonticMonthlyFee);

                return omf;
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

        private bool _missingDateOfMonthlyTreatment = false;
        public bool MissingDateOfMonthlyTreatment
        {
            get
            {
                return _missingDateOfMonthlyTreatment;
            }
            set
            {
                _missingDateOfMonthlyTreatment = value;
                RaisePropertyChanged(() => MissingDateOfMonthlyTreatment);
            }
        }
        private bool _dateOfMonthlyTreatmentError = false;
        public bool DateOfMonthlyTreatmentError
        {
            get
            {
                return _dateOfMonthlyTreatmentError;
            }
            set
            {
                _dateOfMonthlyTreatmentError = value;
                RaisePropertyChanged(() => DateOfMonthlyTreatmentError);
            }
        }
        private bool _invalidDateOfMonthlyTreatment = false;
        public bool InvalidDateOfMonthlyTreatment
        {
            get
            {
                return _invalidDateOfMonthlyTreatment;
            }
            set
            {                
                _invalidDateOfMonthlyTreatment = value;
                RaisePropertyChanged(() => InvalidDateOfMonthlyTreatment);                
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
        private bool _missingOrthodonticMonthlyFee = false;
        public bool MissingOrthodonticMonthlyFee
        {
            get
            {
                return _missingOrthodonticMonthlyFee;
            }
            set
            {
                _missingOrthodonticMonthlyFee = value;
                RaisePropertyChanged(() => MissingOrthodonticMonthlyFee);
            }
        }
        private bool _invalidOrthodonticMonthlyFee = false;
        public bool InvalidOrthodonticMonthlyFee
        {
            get
            {
                return _invalidOrthodonticMonthlyFee;
            }
            set
            {
                if (!MissingOrthodonticMonthlyFee) //Only return invalid when quanity isn't missing                
                {
                    _invalidOrthodonticMonthlyFee = value;
                    RaisePropertyChanged(() => InvalidOrthodonticMonthlyFee);
                }
            }
        }

        private bool _orthodonticMonthlyFeeError = false;
        public bool OrthodonticMonthlyFeeError
        {
            get
            {
                return _orthodonticMonthlyFeeError;
            }
            set
            {
                _orthodonticMonthlyFeeError = value;
                RaisePropertyChanged(() => OrthodonticMonthlyFeeError);
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
                        double omf = GSCHelper.GetDollarAmount(this.OrthodonticMonthlyFee);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail td = new TreatmentDetail()
                        {
                            ID = Guid.NewGuid(),
                            ClaimSubmissionType = this.ClaimSubmissionType,
                            TypeOfTreatment = this.TypeOfTreatment,
                            DateOfMonthlyTreatment = this.DateOfMonthlyTreatment,
                            OrthodonticMonthlyFee = omf,
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
                        double omf = GSCHelper.GetDollarAmount(this.OrthodonticMonthlyFee);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail updated = null;
                        foreach (TreatmentDetail td in _claimservice.Claim.TreatmentDetails)
                        {
                            if (td.ID == EditID)
                            {
                                td.ClaimSubmissionType = this.ClaimSubmissionType;
                                td.TypeOfTreatment = this.TypeOfTreatment;
                                td.DateOfMonthlyTreatment = this.DateOfMonthlyTreatment;
                                td.OrthodonticMonthlyFee = omf;
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
            MissingDateOfMonthlyTreatment = false;
            InvalidDateOfMonthlyTreatment = false;
            DateOfMonthlyTreatmentError = false;
            MissingOrthodonticMonthlyFee = false;
            InvalidOrthodonticMonthlyFee = false;
            OrthodonticMonthlyFeeError = false;
            MissingAC = false;
            InvalidAC = false;
            ACError = false;
            BadValueAC = false;
            DateTooOld = false;
            TreatmentDetailEntryOMFValidator validator = new TreatmentDetailEntryOMFValidator();
            ValidationResult result = null;
            if (AmountPaidByAlternateCarrierVisible)
                result = validator.Validate<ClaimTreatmentDetailsEntryOMFViewModel>(this, ruleSet: "default,AlternateCarrier");
            else
                result = validator.Validate<ClaimTreatmentDetailsEntryOMFViewModel>(this, ruleSet: "default");

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Date":
                            MissingDateOfMonthlyTreatment = true;
                            break;
                        case "Future Date":
                            InvalidDateOfMonthlyTreatment = true;
                            break;
                        case "Empty Amount":
                            MissingOrthodonticMonthlyFee = true;
                            break;
                        case "Invalid Amount":
                            InvalidOrthodonticMonthlyFee = true;
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
                DateOfMonthlyTreatmentError = (MissingDateOfMonthlyTreatment || InvalidDateOfMonthlyTreatment || DateTooOld);
                OrthodonticMonthlyFeeError = (MissingOrthodonticMonthlyFee || InvalidOrthodonticMonthlyFee);
                ACError = (MissingAC || InvalidAC || BadValueAC);
            }

            List<bool> errors = new List<bool>();
            errors.Add(MissingDateOfMonthlyTreatment);
            errors.Add(InvalidDateOfMonthlyTreatment);
            errors.Add(MissingOrthodonticMonthlyFee);
            errors.Add(InvalidOrthodonticMonthlyFee);
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
                this.DateOfMonthlyTreatment = td.DateOfMonthlyTreatment;
                this.OrthodonticMonthlyFee = td.OrthodonticMonthlyFee.ToString();
                if (AmountPaidByAlternateCarrierVisible) this.AmountPaidByAlternateCarrier = td.AlternateCarrierPayment.ToString();
            }
        }
    }
}
