using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Util;
using MobileClaims.Core.Validators;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimMedicalItemViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly MvxSubscriptionToken _typesofmedicalprofessionalretrieved;
        private readonly IRehydrationService _rehydrationservice;
        private Timer saveTimeout;

        public ClaimMedicalItemViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            saveTimeout = new Timer(s => {
                _claimservice.PersistClaim();
            }, 1000);
            _claimservice = claimservice;
            _messenger = messenger;
            _rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();

            if (_rehydrationservice.Rehydrating)
            {
                _isMedicalItemForSportsOnly = _claimservice.Claim.IsMedicalItemForSportsOnly;
                _dateOfReferral = _claimservice.Claim.DateOfReferral;                
                if (_dateOfReferral > DateTime.MinValue)
                {
                    IsDateOfReferralSetByUser = true;
                }
                _typeOfMedicalProfessional = _claimservice.Claim.TypeOfMedicalProfessional;
                _hasReferralBeenPreviouslySubmitted = _claimservice.Claim.HasReferralBeenPreviouslySubmitted;
                Question16Visible = _claimservice.Claim.IsIsGSTHSTIncludedVisible;
                _isGstHstIncluded = _claimservice.Claim.IsGSTHSTIncluded;

                RaiseAllPropertiesChanged();
            }

            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "MI")
            {
                Question12Visible = true;
                IsMedicalItemForSportsOnly = _claimservice.Claim.IsMedicalItemForSportsOnly;
                Questions13Through15Visible = false;
                Question16Visible = false;
            }
            else
            {
                Question12Visible = false;
                if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "ORTHODONTIC")
                {
                    Questions13Through15Visible = false;
                    Question16Visible = true;
                }
                else
                {

                    Questions13Through15Visible = true;
                    Question16Visible = false;
                    DateOfReferral = _claimservice.Claim.DateOfReferral;
                    TypeOfMedicalProfessional = _claimservice.Claim.TypeOfMedicalProfessional;
                    HasReferralBeenPreviouslySubmitted = _claimservice.Claim.HasReferralBeenPreviouslySubmitted;
                }
            }

            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "MASSAGE")
            {
                Question16Visible = true;
                MassageOntarioMessageVisible = true;
            }

            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "DRUG")
            {
                Questions13Through15Visible = false;
            }

            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "DENTAL")
            {
                Questions13Through15Visible = false;
            }

            TypesOfMedicalProfessional = _claimservice.TypesOfMedicalProfessional;

            _typesofmedicalprofessionalretrieved = _messenger.Subscribe<GetTypesOfMedicalProfessionalComplete>((message) =>
            {
                _messenger.Unsubscribe<GetTypesOfMedicalProfessionalComplete>(_typesofmedicalprofessionalretrieved);
                TypesOfMedicalProfessional = _claimservice.TypesOfMedicalProfessional;
            });
            IsGSTHSTIncluded = _claimservice.Claim.IsGSTHSTIncluded;
            PersistClaim();
        }

        public ClaimSubmissionType ClaimSubmissionType => _claimservice.SelectedClaimSubmissionType;

        private bool _isMedicalItemForSportsOnly = false;
        public bool IsMedicalItemForSportsOnly //Q12
        {
            get => _isMedicalItemForSportsOnly;
            set
            {
                _isMedicalItemForSportsOnly = value;
                if (!_rehydrationservice.Rehydrating)
                {
                    _claimservice.Claim.IsMedicalItemForSportsOnly = _isMedicalItemForSportsOnly;
                    PersistClaim();
                }
                RaisePropertyChanged(() => IsMedicalItemForSportsOnly);

            }
        }

        private bool _question12Visible = false;
        public bool Question12Visible
        {
            get => _question12Visible;
            set
            {
                _question12Visible = value;
                RaisePropertyChanged(() => Question12Visible);
            }
        }

        private bool _hasReferralBeenPreviouslySubmitted = false;
        public bool HasReferralBeenPreviouslySubmitted //Q13
        {
            get => _hasReferralBeenPreviouslySubmitted;
            set
            {
                _hasReferralBeenPreviouslySubmitted = value;
                if (!_rehydrationservice.Rehydrating)
                {
                    _claimservice.Claim.HasReferralBeenPreviouslySubmitted = _hasReferralBeenPreviouslySubmitted;
                    if (_hasReferralBeenPreviouslySubmitted)
                    {
                        Questions14And15Enabled = true;
                    }
                    else
                    {
                        Questions14And15Enabled = false;
                    }
                    PersistClaim();
                }
                Questions14And15Visible = value;
                RaisePropertyChanged(() => HasReferralBeenPreviouslySubmitted);
            }
        }

        private DateTime _dateOfReferral;
        public DateTime DateOfReferral //Q14
        {
            get => (this._dateOfReferral == DateTime.MinValue) ? DateTime.Now : this._dateOfReferral;
            set
            {
                _dateOfReferral = value;
                if (!_rehydrationservice.Rehydrating)
                {
                    _claimservice.Claim.DateOfReferral = _dateOfReferral;
                    if (_dateOfReferral > DateTime.MinValue)
                        IsDateOfReferralSetByUser = true;
                    else
                        IsDateOfReferralSetByUser = false;
                    PersistClaim();
                }
                RaisePropertyChanged(() => DateOfReferral);
            }
        }

        private ClaimOption _typeOfMedicalProfessional;
        public ClaimOption TypeOfMedicalProfessional //Q15
        {
            get => _typeOfMedicalProfessional;
            set
            {
                _typeOfMedicalProfessional = value;
                if (!_rehydrationservice.Rehydrating)
                {
                    _claimservice.Claim.TypeOfMedicalProfessional = _typeOfMedicalProfessional;
                    PersistClaim();
                }
                RaisePropertyChanged(() => TypeOfMedicalProfessional);

            }
        }

        private bool _questions13through15Visible;
        public bool Questions13Through15Visible
        {
            get => _questions13through15Visible;
            set
            {
                _questions13through15Visible = value;
                RaisePropertyChanged(() => Questions13Through15Visible);
            }
        }

        private bool _questions14And15Enabled = false;
        public bool Questions14And15Enabled
        {
            get => _questions14And15Enabled;
            set
            {
                _questions14And15Enabled = value;
                RaisePropertyChanged(() => Questions14And15Enabled);
            }
        }

        private bool _questions14And15Visible;
        public bool Questions14And15Visible
        {
            get => _questions14And15Visible;
            set
            {
                _questions14And15Visible = value;              
                RaisePropertyChanged(() => Questions14And15Visible);
            }
        }

        private List<ClaimOption> _typesOfMedicalProfessional;
        public List<ClaimOption> TypesOfMedicalProfessional
        {
            get => _typesOfMedicalProfessional;
            set
            {
                _typesOfMedicalProfessional = value;
                if (!_rehydrationservice.Rehydrating)
                {
                    _claimservice.Claim.TypesOfMedicalProfessional = _typesOfMedicalProfessional;
                    PersistClaim();
                }
                RaisePropertyChanged(() => TypesOfMedicalProfessional);

            }
        }

        private bool _isGstHstIncluded;
        public bool IsGSTHSTIncluded //Q16
        {
            get => _isGstHstIncluded;
            set
            {
                _isGstHstIncluded = value;
                if (!_rehydrationservice.Rehydrating)
                {
                    _claimservice.Claim.IsGSTHSTIncluded = _isGstHstIncluded;
                    PersistClaim();
                }
                RaisePropertyChanged(() => IsGSTHSTIncluded);

            }
        }

        private bool _question16Visible = false;
        public bool Question16Visible
        {
            get => _question16Visible;
            set
            {
                _question16Visible = value;
                if (!_question16Visible) IsGSTHSTIncluded = false;

                RaisePropertyChanged(() => Question16Visible);
            }
        }

        private bool _massageOntarioMessageVisible = false;
        public bool MassageOntarioMessageVisible
        {
            get => _massageOntarioMessageVisible;
            set
            {
                _massageOntarioMessageVisible = value;
                RaisePropertyChanged(() => MassageOntarioMessageVisible);
            }
        }

        private bool _isDateOfReferralSetByUser = false;
        public bool IsDateOfReferralSetByUser
        {
            get => _isDateOfReferralSetByUser;
            set
            {
                _isDateOfReferralSetByUser = value;
                RaisePropertyChanged(() => IsDateOfReferralSetByUser);
            }
        }

        public override Task RaisePropertyChanged(System.ComponentModel.PropertyChangedEventArgs changedArgs)
        {
            PersistClaim();
            return base.RaisePropertyChanged(changedArgs);
        }

        public override Task RaiseAllPropertiesChanged()
        {
            PersistClaim();
            return base.RaiseAllPropertiesChanged();
        }

        public ValidationResult Validate()
        {
            MedicalItemValidator validator = new MedicalItemValidator();
            ValidationResult results = new ValidationResult();
           
            if (_dateOfReferral > DateTime.MinValue)
                results = validator.Validate<ClaimMedicalItemViewModel>(this, ruleSet: "default,DateOfReferral");
            else
                results = validator.Validate<ClaimMedicalItemViewModel>(this, ruleSet: "default");
            
            return results;
        }

        public void ClearData()
        {
            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "MI")
            {
                Question12Visible = true;
                IsMedicalItemForSportsOnly = false;
                Questions13Through15Visible = false;
                Question16Visible = false;
                MassageOntarioMessageVisible = false;
            }
            else
            {
                if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "ORTHODONTIC")
                {
                    Questions13Through15Visible = false;
                    Question16Visible = true;
                    IsGSTHSTIncluded = false;
                }
                else
                {
                    Questions13Through15Visible = true;
                    Question16Visible = false;
                    DateOfReferral = DateTime.MinValue;
                    TypeOfMedicalProfessional = null;
                    HasReferralBeenPreviouslySubmitted = false;
                }
                Question12Visible = false;
                MassageOntarioMessageVisible = false;
            }

            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "MASSAGE")
            {
                Question16Visible = true;
                MassageOntarioMessageVisible = true;
                IsGSTHSTIncluded = false;
            }

            if (_claimservice.SelectedClaimSubmissionType != null && _claimservice.SelectedClaimSubmissionType.ID == "DENTALs")
            {
                Questions13Through15Visible = false;
            }
        }

        private void PersistClaim()
        {
            saveTimeout.Reset();
        }
    }
}