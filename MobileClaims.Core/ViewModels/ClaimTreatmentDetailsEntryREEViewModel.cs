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
    public class ClaimTreatmentDetailsEntryREEViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly MvxSubscriptionToken _claimsubmissionbenefitsretrieved;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimTreatmentDetailsEntryREEViewModel(IMvxMessenger messenger, IClaimService claimservice)
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

        private DateTime _dateOfExamination;
        public DateTime DateOfExamination
        {
            get
            {
                return (this._dateOfExamination == DateTime.MinValue) ? DateTime.Now : this._dateOfExamination;
            }
            set
            {
                _dateOfExamination = value;
                RaisePropertyChanged(() => DateOfExamination);
            }
        }

        private string _totalAmountOfExamination;
        public string TotalAmountOfExamination
        {
            get
            {
                return (String.IsNullOrEmpty(this._totalAmountOfExamination)) ? "" : this._totalAmountOfExamination;
            }
            set
            {
                _totalAmountOfExamination = value;
                RaisePropertyChanged(() => TotalAmountOfExamination);
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
                double tae = GSCHelper.GetDollarAmount(this.TotalAmountOfExamination);

                return tae;
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
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountOfExamination);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail td = new TreatmentDetail()
                        {
                            ID = Guid.NewGuid(),
                            ClaimSubmissionType = this.ClaimSubmissionType,
                            TypeOfTreatment = this.TypeOfTreatment,
                            DateOfExamination = this.DateOfExamination,
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
                        double amt = GSCHelper.GetDollarAmount(this.TotalAmountOfExamination);
                        double acp = GSCHelper.GetDollarAmount(this.AmountPaidByAlternateCarrier);
                        TreatmentDetail updated = null;
                        foreach (TreatmentDetail td in _claimservice.Claim.TreatmentDetails)
                        {
                            if (td.ID == EditID)
                            {
                                td.ClaimSubmissionType = this.ClaimSubmissionType;
                                td.TypeOfTreatment = this.TypeOfTreatment;
                                td.DateOfExamination = this.DateOfExamination;
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

        private bool _missingDateOfExamination = false;
        public bool MissingDateOfExamination
        {
            get
            {
                return _missingDateOfExamination;
            }
            set
            {
                _missingDateOfExamination = value;
                RaisePropertyChanged(() => MissingDateOfExamination);
            }
        }
        private bool _dateOfExaminationError = false;
        public bool DateOfExaminationError
        {
            get
            {
                return _dateOfExaminationError;
            }
            set
            {
                _dateOfExaminationError = value;
                RaisePropertyChanged(() => DateOfExaminationError);
            }
        }
        private bool _invalidDateOfExamination = false;
        public bool InvalidDateOfExamination
        {
            get
            {
                return _invalidDateOfExamination;
            }
            set
            {
                _invalidDateOfExamination = value;
                RaisePropertyChanged(() => InvalidDateOfExamination);
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
        private bool _missingTotalAmountOfExamination = false;
        public bool MissingTotalAmountOfExamination
        {
            get
            {
                return _missingTotalAmountOfExamination;
            }
            set
            {
                _missingTotalAmountOfExamination = value;
                RaisePropertyChanged(() => MissingTotalAmountOfExamination);
            }
        }
        private bool _invalidTotalAmountOfExamination = false;
        public bool InvalidTotalAmountOfExamination
        {
            get
            {
                return _invalidTotalAmountOfExamination;
            }
            set
            {
                if (!MissingTotalAmountOfExamination) //Only return invalid when quanity isn't missing                
                {
                    _invalidTotalAmountOfExamination = value;
                    RaisePropertyChanged(() => InvalidTotalAmountOfExamination);
                }
            }
        }

        private bool _totalAmountOfExaminationError = false;
        public bool TotalAmountOfExaminationError
        {
            get
            {
                return _totalAmountOfExaminationError;
            }
            set
            {
                _totalAmountOfExaminationError = value;
                RaisePropertyChanged(() => TotalAmountOfExaminationError);
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

        private bool IsValid()
        {
            // Reset for validation
            MissingDateOfExamination = false;
            InvalidDateOfExamination = false;
            DateOfExaminationError = false;
            MissingTotalAmountOfExamination = false;
            InvalidTotalAmountOfExamination = false;
            TotalAmountOfExaminationError = false;
            MissingAC = false;
            InvalidAC = false;
            ACError = false;
            BadValueAC = false;
            DateTooOld = false;
            TreatmentDetailEntryREEValidator validator = new TreatmentDetailEntryREEValidator();
            ValidationResult result = null;
            if (AmountPaidByAlternateCarrierVisible)
                result = validator.Validate<ClaimTreatmentDetailsEntryREEViewModel>(this, ruleSet: "default,AlternateCarrier");
            else
                result = validator.Validate<ClaimTreatmentDetailsEntryREEViewModel>(this, ruleSet: "default");

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Date":
                            MissingDateOfExamination = true;
                            break;
                        case "Future Date":
                            InvalidDateOfExamination = true;
                            break;
                        case "Empty Amount":
                            MissingTotalAmountOfExamination = true;
                            break;
                        case "Invalid Amount":
                        case "Amount TooHigh":
                            InvalidTotalAmountOfExamination = true;
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
                DateOfExaminationError = (MissingDateOfExamination || InvalidDateOfExamination || DateTooOld);
                TotalAmountOfExaminationError = (MissingTotalAmountOfExamination || InvalidTotalAmountOfExamination);
                ACError = (MissingAC || InvalidAC || BadValueAC);
            }

            List<bool> errors = new List<bool>();
            errors.Add(MissingDateOfExamination);
            errors.Add(InvalidDateOfExamination);
            errors.Add(MissingTotalAmountOfExamination);
            errors.Add(InvalidTotalAmountOfExamination);
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
                this.DateOfExamination = td.DateOfExamination;
                this.TotalAmountOfExamination = td.TreatmentAmount.ToString();
                if (AmountPaidByAlternateCarrierVisible) this.AmountPaidByAlternateCarrier = td.AlternateCarrierPayment.ToString();
            }
        }
    }
}
