using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using MobileClaims.Core.Validators;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class EligibilityCheckCPViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IEligibilityService _eligibilityservice;
        private readonly IParticipantService _participantservice;//added by vivian on June 16 2014
        private readonly MvxSubscriptionToken _eligibilitybenefitsretrieved;
        private readonly MvxSubscriptionToken _eligibilityprovincesretrieved;
        private MvxSubscriptionToken _eligibilitycheckcomplete;
        private MvxSubscriptionToken _eligibilityCheckFailure;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public EligibilityCheckCPViewModel(IMvxMessenger messenger, IEligibilityService eligibilityservice, IParticipantService participantservice)
        {
            _messenger = messenger;
            _eligibilityservice = eligibilityservice;
            _participantservice = participantservice;

            TypesOfTreatment = _eligibilityservice.EligibilityBenefits;     
            
            SetServiceProvinces();
          
            _eligibilitybenefitsretrieved = _messenger.Subscribe<GetEligibilityBenefitsComplete>((message) =>
            {
                TypesOfTreatment = _eligibilityservice.EligibilityBenefits;
            });

            _eligibilityprovincesretrieved = _messenger.Subscribe<GetEligibilityProvincesComplete>((message) =>
            {
                SetServiceProvinces();
            });

            _shouldcloseself = _messenger.Subscribe<ClearEligibilityCheckCPRequest>((message) =>
            {
                _messenger.Unsubscribe<ClearEligibilityCheckCPRequest>(_shouldcloseself);
                Close(this);
            });
        }

        public EligibilityCheckType EligibilityCheckType => _eligibilityservice.SelectedEligibilityCheckType;

        public Participant SelectedParticipant => _eligibilityservice.EligibilitySelectedParticipant;

        private EligibilityBenefit _typeOfTreatment;
        public EligibilityBenefit TypeOfTreatment
        {
            get => _typeOfTreatment;
            set
            {
                _typeOfTreatment = value;
                _eligibilityservice.EligibilityCheck.TypeOfTreatment = _typeOfTreatment;
                if (_typeOfTreatment != null)
                    _eligibilityservice.EligibilityCheck.BenefitCategoryID = _typeOfTreatment.ID;
                else
                    _eligibilityservice.EligibilityCheck.BenefitCategoryID = string.Empty;

                RaisePropertyChanged(() => TypeOfTreatment);
            }
        }

        private DateTime _dateOfTreatment;
        public DateTime DateOfTreatment
        {
            get => _dateOfTreatment;
            set
            {
                _dateOfTreatment = value;
                _eligibilityservice.EligibilityCheck.TreatmentDate = _dateOfTreatment.ToString(GSCHelper.GSC_DATE_FORMAT);
                RaisePropertyChanged(() => DateOfTreatment);
            }
        }

        private string _totalAmountOfVisit;
        public string TotalAmountOfVisit
        {
            get => _totalAmountOfVisit;
            set
            {
                _totalAmountOfVisit = value;

                double amt = GSCHelper.GetDollarAmount(_totalAmountOfVisit);
                _eligibilityservice.EligibilityCheck.ClaimAmount = amt;

                RaisePropertyChanged(() => TotalAmountOfVisit);
            }
        }

        private EligibilityProvince _provinceOfService;
        public EligibilityProvince ProvinceOfService
        {
            get => _provinceOfService;
            set
            {
                _provinceOfService = value;
                _eligibilityservice.EligibilityCheck.Province = _provinceOfService;
                if (_provinceOfService != null)
                    _eligibilityservice.EligibilityCheck.ProvinceCode = _provinceOfService.ID;
                else
                    _eligibilityservice.EligibilityCheck.ProvinceCode = string.Empty;

                RaisePropertyChanged(() => ProvinceOfService);
            }
        }

        private List<EligibilityBenefit> _typesOfTreatment;
        public List<EligibilityBenefit> TypesOfTreatment
        {
            get => _typesOfTreatment;
            set
            {
                _typesOfTreatment = value;
                SetDefaultTypeOfTreatment();
                RaisePropertyChanged(() => TypesOfTreatment);
            }
        }

        private List<EligibilityProvince> _serviceProvinces;
        public List<EligibilityProvince> ServiceProvinces
        {
            get => _serviceProvinces;
            set
            {
                _serviceProvinces = value;
                RaisePropertyChanged(() => ServiceProvinces);
            }
        }

        private bool _busy = false;
        public bool Busy
        {
            get => _busy;
            set
            {
                if (_busy != value)
                {
                    _busy = value;
                    _messenger.Publish<BusyIndicator>(new BusyIndicator(this)
                    {
                        Busy = _busy
                    });
                    RaisePropertyChanged(() => Busy);
                }
            }
        }

        public ICommand SubmitEligibilityCheckCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                        {
                            Mvx.IoCProvider.Resolve<IUserDialogs>().ShowLoading();
                        }
                        Busy = true;
                        Unsubscribe();
                        _eligibilityservice.CheckEligibility();

                        _eligibilitycheckcomplete = _messenger.Subscribe<EligibilityCheckSubmissionComplete>((message) =>
                        {
                            Busy = false;
                            if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                            {
                                Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                            }
                            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
                            _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailure);

                            this.ShowViewModel<EligibilityResultsViewModel>();
                        });
                        _eligibilityCheckFailure = _messenger.Subscribe<EligibilityCheckSubmissionError>((message) =>
                        {
                            Busy = false;
                            _messenger.Unsubscribe<EligibilityCheckSubmissionComplete>(_eligibilitycheckcomplete);
                            _messenger.Unsubscribe<EligibilityCheckSubmissionError>(_eligibilityCheckFailure);

                            Dialogs.Alert(Resource.GenericErrorDialogMessage);
                        });
                    }
                });
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<GetEligibilityBenefitsComplete>(_eligibilitybenefitsretrieved);
            _messenger.Unsubscribe<GetEligibilityProvincesComplete>(_eligibilityprovincesretrieved);
        }

        private bool IsValid()
        {
            // Reset for validation       
            MissingTotalAmount = false;
            InvalidTotalAmount = false;
            TotalAmountError = false;
            MissingTypeOfTreament = false;
            InvalidDateOfTreatment = false;
            FutureDateOfTreatment = false;
            EligibilityCheckCPValidator validator = new EligibilityCheckCPValidator();
            ValidationResult result = null;
			result = validator.Validate<EligibilityCheckCPViewModel>(this, "DateOfTreatment", "TotalAmountOfVisit", "TypeOfTreatment");
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.ErrorMessage)
                    {
                        case "Empty Amount":
                            MissingTotalAmount = true;
                            break;
                        case "Invalid Amount":
                            InvalidTotalAmount = true;
                            break;
                        case "Empty TypeOfTreatment":
                            MissingTypeOfTreament = true;
                            break;
                        case "Date TooOld":
                            InvalidDateOfTreatment = true;
                            break;
                        case "Future Date":
                            FutureDateOfTreatment = true;
                            break;
                    }
                }
                TotalAmountError = (MissingTotalAmount || InvalidTotalAmount);
            }

            List<bool> errors = new List<bool>();
            errors.Add(MissingTotalAmount);
            errors.Add(InvalidTotalAmount);
            errors.Add(MissingTypeOfTreament);
            errors.Add(InvalidDateOfTreatment);
            errors.Add(FutureDateOfTreatment);
            if (errors.Any(b => b == true))
            {
                RaiseOnInvalidEligibilityCheck(new EventArgs());
                return false;
            }
            return true;
        }

        private void SetServiceProvinces()
        {
            if (_eligibilityservice.EligibilityProvinces != null && _eligibilityservice.EligibilityProvinces.Count > 0)
            {
                Busy = false;

                ServiceProvinces = _eligibilityservice.EligibilityProvinces;
                foreach (EligibilityProvince ep in ServiceProvinces)
                {
                    if (ep.IsDefault)
                    {
                        this.ProvinceOfService = ep;
                        break;
                    }
                }
            }
            else
            {
                Busy = true;
            }
        }

        private void SetDefaultTypeOfTreatment()
        {
            if (TypesOfTreatment != null && TypesOfTreatment.Count > 0)
            {
                if (EligibilityCheckType.ID == "CHIRO")
                    TypeOfTreatment = TypesOfTreatment.Where(t => t.ID == "70080").FirstOrDefault();
                else if (EligibilityCheckType.ID == "PHYSIO")
                    TypeOfTreatment = TypesOfTreatment.Where(t => t.ID == "70216").FirstOrDefault();
                else if (TypesOfTreatment.Count == 1)
                    TypeOfTreatment = TypesOfTreatment.FirstOrDefault();
                else
                    TypeOfTreatment = null;
            }
            else
                TypeOfTreatment = null;
        }

        public event EventHandler OnInvalidEligibilityCheck;
        protected virtual void RaiseOnInvalidEligibilityCheck(EventArgs e)
        {
            if (this.OnInvalidEligibilityCheck != null)
            {
                OnInvalidEligibilityCheck(this, e);
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

        private bool _missingTypeOfTreatment = false;
        public bool MissingTypeOfTreament
        {
            get => _missingTypeOfTreatment;
            set
            {
                _missingTypeOfTreatment = value;
                RaisePropertyChanged(() => MissingTypeOfTreament);
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

        private bool _futureDateOfTreatment = false;
        public bool FutureDateOfTreatment
        {
            get => _futureDateOfTreatment;
            set
            {
                _futureDateOfTreatment = value;
                RaisePropertyChanged(() => FutureDateOfTreatment);
            }
        }
    }
}
