using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Services;
using System;
using System.Threading.Tasks;
using MobileClaims.Core.Validators;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimWorkInjuryViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;

        public ClaimWorkInjuryViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _claimservice = claimservice;
            _messenger = messenger;

            IsTreatmentDueToAWorkRelatedInjury = _claimservice.Claim.IsTreatmentDueToAWorkRelatedInjury;
            DateOfWorkRelatedInjury = _claimservice.Claim.DateOfWorkRelatedInjury;
            if (_claimservice.Claim.WorkRelatedInjuryCaseNumber != 0)
                WorkRelatedInjuryCaseNumber = _claimservice.Claim.WorkRelatedInjuryCaseNumber.ToString();
        }

        private bool _isTreatmentDueToAWorkRelatedInjury = false;
        public bool IsTreatmentDueToAWorkRelatedInjury
        {
            get
            {
                return _isTreatmentDueToAWorkRelatedInjury;
            }
            set
            {
                _isTreatmentDueToAWorkRelatedInjury = value;
                _claimservice.Claim.IsTreatmentDueToAWorkRelatedInjury = _isTreatmentDueToAWorkRelatedInjury;
                RaisePropertyChanged(() => IsTreatmentDueToAWorkRelatedInjury);
            }
        }

        private DateTime _dateOfWorkRelatedInjury;
        public DateTime DateOfWorkRelatedInjury
        {
            get
            {
                return (this._dateOfWorkRelatedInjury == DateTime.MinValue) ? DateTime.Now : this._dateOfWorkRelatedInjury;
            }
            set
            {
                _dateOfWorkRelatedInjury = value;
                _claimservice.Claim.DateOfWorkRelatedInjury = _dateOfWorkRelatedInjury;
                RaisePropertyChanged(() => DateOfWorkRelatedInjury);
            }
        }

        private string _workRelatedInjuryCaseNumber;
        public string WorkRelatedInjuryCaseNumber
        {
            get
            {
                return _workRelatedInjuryCaseNumber;
            }
            set
            {
                _workRelatedInjuryCaseNumber = value;
                int caseNum;
                if (int.TryParse(_workRelatedInjuryCaseNumber, out caseNum))
                    _claimservice.Claim.WorkRelatedInjuryCaseNumber = caseNum;
                RaisePropertyChanged(() => WorkRelatedInjuryCaseNumber);
            }
        }

        public override Task RaisePropertyChanged(System.ComponentModel.PropertyChangedEventArgs changedArgs)
        {
            _claimservice.PersistClaim();
            return base.RaisePropertyChanged(changedArgs);
        }

        public override Task RaiseAllPropertiesChanged()
        {
            _claimservice.PersistClaim();
            return base.RaiseAllPropertiesChanged();
        }

        public ValidationResult Validate()
        {
            WorkInjuryValidator validator = new WorkInjuryValidator();
            ValidationResult results = new ValidationResult();
            if (IsTreatmentDueToAWorkRelatedInjury)
				results = validator.Validate<ClaimWorkInjuryViewModel>(this, "DateOfWorkRelatedInjury", "WorkRelatedInjuryCaseNumber");
            return results;
        }

        public void ClearData()
        {
            this.WorkRelatedInjuryCaseNumber = string.Empty;
            this.DateOfWorkRelatedInjury = DateTime.Now;
            this.IsTreatmentDueToAWorkRelatedInjury = false;
        }
    }
}
