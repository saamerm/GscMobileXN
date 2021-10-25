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
    public class EligibilityCheckEyeViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IEligibilityService _eligibilityservice;
        private readonly MvxSubscriptionToken _eligibilityprovincesretrieved;
        private readonly MvxSubscriptionToken _eligibilitybenefitsretrieved;
        private readonly MvxSubscriptionToken _eligibilityoptionsretrieved;
        private MvxSubscriptionToken _eligibilitycheckcomplete;
        private MvxSubscriptionToken _eligibilityCheckFailure;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public EligibilityCheckEyeViewModel(IMvxMessenger messenger, IEligibilityService eligibilityservice)
        {
            _messenger = messenger;
            _eligibilityservice = eligibilityservice;

            if (EligibilityCheckType.ID == "GLASSES")
                IsLensTypeVisible = true;

            SetServiceProvinces();
            LensTypes = _eligibilityservice.EligibilityOptions;

            if (_eligibilityservice.EligibilityBenefits != null && _eligibilityservice.EligibilityBenefits.Count > 0)
                TypeOfTreatment = _eligibilityservice.EligibilityBenefits.FirstOrDefault();

            _eligibilitybenefitsretrieved = _messenger.Subscribe<GetEligibilityBenefitsComplete>((message) =>
            {
                if (_eligibilityservice.EligibilityBenefits != null && _eligibilityservice.EligibilityBenefits.Count > 0)
                    TypeOfTreatment = _eligibilityservice.EligibilityBenefits.FirstOrDefault();
            });

            _eligibilityprovincesretrieved = _messenger.Subscribe<GetEligibilityProvincesComplete>((message) =>
            {
                SetServiceProvinces();
            });

            _eligibilityoptionsretrieved = _messenger.Subscribe<GetEligibilityOptionsComplete>((message) =>
            {
                LensTypes = _eligibilityservice.EligibilityOptions;
            });

            _shouldcloseself = _messenger.Subscribe<ClearEligibilityCheckEyeRequest>((message) =>
            {
                _messenger.Unsubscribe<ClearEligibilityCheckEyeRequest>(_shouldcloseself);
                Close(this);
            });
        }

        public EligibilityCheckType EligibilityCheckType
        {
            get
            {
                return _eligibilityservice.SelectedEligibilityCheckType;
            }
        }

        public Participant SelectedParticipant
        {
            get
            {
                return _eligibilityservice.EligibilitySelectedParticipant;
            }
        }

        private EligibilityBenefit _typeOfTreatment;
        public EligibilityBenefit TypeOfTreatment
        {
            get
            {
                return _typeOfTreatment;
            }
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

        private DateTime _dateOfPurchaseOrService;
        public DateTime DateOfPurchaseOrService
        {
            get
            {
                return _dateOfPurchaseOrService;
            }
            set
            {
                _dateOfPurchaseOrService = value;
                _eligibilityservice.EligibilityCheck.TreatmentDate = _dateOfPurchaseOrService.ToString(GSCHelper.GSC_DATE_FORMAT);
                RaisePropertyChanged(() => DateOfPurchaseOrService);
            }
        }

        private string _totalCharge;
        public string TotalCharge
        {
            get
            {
                return _totalCharge;
            }
            set
            {
                _totalCharge = value;

                double amt = GSCHelper.GetDollarAmount(_totalCharge);
                _eligibilityservice.EligibilityCheck.ClaimAmount = amt;

                RaisePropertyChanged(() => TotalCharge);
            }
        }

        private EligibilityProvince _provinceOfService;
        public EligibilityProvince ProvinceOfService
        {
            get
            {
                return _provinceOfService;
            }
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

        private EligibilityOption _lensType;
        public EligibilityOption LensType
        {
            get
            {
                return _lensType;
            }
            set
            {
                _lensType = value;
                _eligibilityservice.EligibilityCheck.LensType = _lensType;
                if (_lensType != null)
                    _eligibilityservice.EligibilityCheck.LensTypeCode = _lensType.ID;
                else
                    _eligibilityservice.EligibilityCheck.LensTypeCode = string.Empty;

                RaisePropertyChanged(() => LensType);
            }
        }

        private bool _isLensTypeVisible = false;
        public bool IsLensTypeVisible
        {
            get
            {
                return _isLensTypeVisible;
            }
            set
            {
                _isLensTypeVisible = value;
                RaisePropertyChanged(() => IsLensTypeVisible);
            }
        }

        private List<EligibilityProvince> _serviceProvinces;
        public List<EligibilityProvince> ServiceProvinces
        {
            get
            {
                return _serviceProvinces;
            }
            set
            {
                _serviceProvinces = value;
                RaisePropertyChanged(() => ServiceProvinces);
            }
        }

        private List<EligibilityOption> _lensTypes;
        public List<EligibilityOption> LensTypes
        {
            get
            {
                return _lensTypes;
            }
            set
            {
                _lensTypes = value;
                SetDefaultLensType();
                RaisePropertyChanged(() => LensTypes);
            }
        }

        private bool _busy = false;
        public bool Busy
        {
            get
            {
                return _busy;
            }
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
                        Busy = true;
                        if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                        {
                            Mvx.IoCProvider.Resolve<IUserDialogs>().ShowLoading();
                        }
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

                            _messenger.Publish<ClearEligibilityResultsViewRequested>(new ClearEligibilityResultsViewRequested(this));//added by vivian on July 3 2014 for exception//ended of adding

                            this.ShowViewModel<EligibilityResultsViewModel>();
                        });
                        _eligibilityCheckFailure = _messenger.Subscribe<EligibilityCheckSubmissionError>((message) =>
                        {
                            Busy = false;
                            if (Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice == GSCHelper.OS.iOS)
                            {
                                Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                            }
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
            _messenger.Unsubscribe<GetEligibilityProvincesComplete>(_eligibilityprovincesretrieved);
            _messenger.Unsubscribe<GetEligibilityBenefitsComplete>(_eligibilitybenefitsretrieved);
            _messenger.Unsubscribe<GetEligibilityOptionsComplete>(_eligibilityoptionsretrieved);
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

        private void SetDefaultLensType()
        {
            if (LensTypes != null && LensTypes.Count > 0)
            {
                LensType = LensTypes.Where(lt => lt.ID == "SV").FirstOrDefault(); //default to Single Vision
            }
        }

        private bool IsValid()
        {
            // Reset for validation       
            MissingTotalAmount = false;
            InvalidTotalAmount = false;
            TotalAmountError = false;
            MissingTypeOfTreament = false;
            InvalidDateOfPurchaseOrService = false;
            FutureDateOfPurchaseOrService = false;
            EligibilityCheckEyeValidator validator = new EligibilityCheckEyeValidator();
            ValidationResult result = null;
			result = validator.Validate<EligibilityCheckEyeViewModel>(this, "TotalCharge", "TypeOfTreatment", "DateOfPurchaseOrService");
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
                            InvalidDateOfPurchaseOrService = true;
                            break;
                        case "Future Date":
                            FutureDateOfPurchaseOrService = true;
                            break;
                    }
                }
                TotalAmountError = (MissingTotalAmount || InvalidTotalAmount);
            }

            List<bool> errors = new List<bool>();
            errors.Add(MissingTotalAmount);
            errors.Add(InvalidTotalAmount);
            errors.Add(MissingTypeOfTreament);
            errors.Add(InvalidDateOfPurchaseOrService);
            errors.Add(FutureDateOfPurchaseOrService);
            if (errors.Any(b => b == true))
            {
                RaiseOnInvalidEligibilityCheck(new EventArgs());
                return false;
            }
            return true;
        }

        public event EventHandler OnInvalidEligibilityCheck;
        protected virtual void RaiseOnInvalidEligibilityCheck(EventArgs e)
        {
            if (this.OnInvalidEligibilityCheck != null)
            {
                OnInvalidEligibilityCheck(this, e);
            }
        }

        private Participant _participant;
        public Participant Participant
        {
            get { return _participant; }
            set
            {
                if (_participant != value)
                {
                    _participant = value;
                    RaisePropertyChanged(() => Participant);
                };
            }
        }

        //public void Init(NavigationHelper nav)
        //{
        //    if (nav.SelParticipant != null)
        //    {
        //        this.Participant = nav.SelParticipant;
        //    }

        //    if (!string.IsNullOrEmpty(nav.PlanMemberID))
        //    {
        //        if (_participantservice.PlanMember.PlanMemberID == nav.PlanMemberID)
        //        {
        //            this.Participant = _participantservice.PlanMember as Participant;
        //        }
        //        else
        //        {
        //            if (_participantservice.PlanMember.Dependents.Where(p => p.PlanMemberID == nav.PlanMemberID).FirstOrDefault() != null)
        //            {
        //                this.Participant = _participantservice.PlanMember.Dependents.Where(p => p.PlanMemberID == nav.PlanMemberID).FirstOrDefault();
        //            }
        //        }
        //    }
        //}

        //public class NavigationHelper
        //{
        //    public Participant SelParticipant { get; set; }
        //    public string PlanMemberID { get; set; }
        //    public NavigationHelper()
        //    {

        //    }
        //    public NavigationHelper(Participant participant)
        //    {
        //        SelParticipant = participant;
        //    }
        //    public NavigationHelper(string planmemberid)
        //    {
        //        this.PlanMemberID = planmemberid;
        //    }
        //}

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

        private bool _missingTypeOfTreatment = false;
        public bool MissingTypeOfTreament
        {
            get
            {
                return _missingTypeOfTreatment;
            }
            set
            {
                _missingTypeOfTreatment = value;
                RaisePropertyChanged(() => MissingTypeOfTreament);
            }
        }

        private bool _invalidDateOfPurchaseOrService = false;
        public bool InvalidDateOfPurchaseOrService
        {
            get
            {
                return _invalidDateOfPurchaseOrService;
            }
            set
            {
                _invalidDateOfPurchaseOrService = value;
                RaisePropertyChanged(() => InvalidDateOfPurchaseOrService);
            }
        }

        private bool _futureDateOfPurchaseOrService = false;
        public bool FutureDateOfPurchaseOrService
        {
            get
            {
                return _futureDateOfPurchaseOrService;
            }
            set
            {
                _futureDateOfPurchaseOrService = value;
                RaisePropertyChanged(() => FutureDateOfPurchaseOrService);
            }
        }
    }
}
