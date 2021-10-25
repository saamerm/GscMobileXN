using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using System;
using System.Threading.Tasks;
using MobileClaims.Core.Validators;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimMotorVehicleViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;

        public ClaimMotorVehicleViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _claimservice = claimservice;
            _messenger = messenger;

            IsTreatmentDueToAMotorVehicleAccident = _claimservice.Claim.IsTreatmentDueToAMotorVehicleAccident;
            DateOfMotorVehicleAccident = _claimservice.Claim.DateOfMotorVehicleAccident;
        }

        public ClaimSubmissionType ClaimSubmissionType => _claimservice.SelectedClaimSubmissionType;

        private bool _isTreatmentDueToAMotorVehicleAccident = false;
        public bool IsTreatmentDueToAMotorVehicleAccident
        {
            get => _isTreatmentDueToAMotorVehicleAccident;
            set
            {
                _isTreatmentDueToAMotorVehicleAccident = value;
                _claimservice.Claim.IsTreatmentDueToAMotorVehicleAccident = _isTreatmentDueToAMotorVehicleAccident;
                RaisePropertyChanged(() => IsTreatmentDueToAMotorVehicleAccident);
            }
        }

        private DateTime _dateOfMotorVehicleAccident;
        public DateTime DateOfMotorVehicleAccident
        {
            get => (this._dateOfMotorVehicleAccident == DateTime.MinValue) ? DateTime.Now : this._dateOfMotorVehicleAccident;
            set
            {
                _dateOfMotorVehicleAccident = value;
                _claimservice.Claim.DateOfMotorVehicleAccident = _dateOfMotorVehicleAccident;
                RaisePropertyChanged(() => DateOfMotorVehicleAccident);
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
            MotorVehicleValidator validator = new MotorVehicleValidator();
            ValidationResult results = new ValidationResult();
            if (IsTreatmentDueToAMotorVehicleAccident)
            {
                results = validator.Validate(this, "DateOfMotorVehicleAccident");
            }
            return results;
        }

        public void ClearData()
        {
            this.DateOfMotorVehicleAccident = DateTime.Now;
            this.IsTreatmentDueToAMotorVehicleAccident = false;
        }
    }
}
