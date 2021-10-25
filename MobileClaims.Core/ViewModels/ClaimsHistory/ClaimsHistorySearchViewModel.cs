using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.HCSA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.ClaimsHistory
{
    public class ClaimsHistorySearchViewModel : HCSAViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimsHistoryService _claimshistoryservice;
        private readonly IParticipantService _participantservice;
        private readonly ILoginService _loginservice;
        private MvxSubscriptionToken _participantschanged;
        private const string ALL_BENEFIT_ID = GSCHelper.ClaimHistoryBenefitIDForAll;
        private readonly IUserDialogs _dialogservice;

        public ClaimsHistorySearchViewModel(IMvxMessenger messenger, IClaimsHistoryService claimshistoryservice, IParticipantService participantservice, ILoginService loginservice)
        {
            _messenger = messenger;
            _claimshistoryservice = claimshistoryservice;
            _participantservice = participantservice;
            _loginservice = loginservice;
            _dialogservice = Mvx.IoCProvider.Resolve<IUserDialogs>();

            ClaimHistoryTypes = new ObservableCollection<ClaimHistoryType>();
            ClaimHistoryPayees = new ObservableCollection<ClaimHistoryPayee>();
            ClaimHistoryBenefits = new ObservableCollection<ClaimHistoryBenefit>();
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_claimshistoryservice.ClaimHistoryTypes != null && _claimshistoryservice.ClaimHistoryTypes.Count > 0)
            {
                ClaimHistoryTypes = new ObservableCollection<ClaimHistoryType>(_claimshistoryservice.ClaimHistoryTypes);
                SetSelectedClaimHistoryType();
            }
            else
            {
                IsClaimHistoryTypesBusy = true;
                _claimshistoryservice.GetClaimHistoryTypes(() =>
                    {
                        ClaimHistoryTypes = new ObservableCollection<ClaimHistoryType>(_claimshistoryservice.ClaimHistoryTypes);
                        SetSelectedClaimHistoryType();
                        IsClaimHistoryTypesBusy = false;
                    }, (message, code) =>
                    {
                        ErrorMessage = message;
                        ErrorCode = code;
                        IsClaimHistoryTypesBusy = false;
                    });
            }

            if (_claimshistoryservice.ClaimHistoryPayees != null && _claimshistoryservice.ClaimHistoryPayees.Count > 0)
            {
                ClaimHistoryPayees = new ObservableCollection<ClaimHistoryPayee>(_claimshistoryservice.ClaimHistoryPayees);
                SetSelectedClaimHistoryPayees();
            }
            else
            {
                IsClaimHistoryPayessBusy = true;
                _claimshistoryservice.GetClaimHistoryTypes(() =>
                    {
                        ClaimHistoryPayees = new ObservableCollection<ClaimHistoryPayee>(_claimshistoryservice.ClaimHistoryPayees);
                        SetAllPayeesIsSelected(true);
                        IsClaimHistoryPayessBusy = false;
                    }, (message, code) =>
                    {
                        ErrorMessage = message;
                        ErrorCode = code;
                        IsClaimHistoryTypesBusy = false;
                    });
            }

            if (_claimshistoryservice.SelectedParticipant == null) //default selected participant to current plan member
            {
                _claimshistoryservice.SelectedParticipant = (Participant)_participantservice.PlanMember;
            }

            if (_claimshistoryservice.ClaimHistoryBenefits != null && _claimshistoryservice.ClaimHistoryBenefits.Count > 0)
            {
                ClaimHistoryBenefits = new ObservableCollection<ClaimHistoryBenefit>(_claimshistoryservice.ClaimHistoryBenefits);
            }
            else
            {
                IsClaimHistoryBenefitsBusy = true;
                _claimshistoryservice.GetClaimHistoryBenefits(_loginservice.CurrentPlanMemberID, () =>
                     {
                         ClaimHistoryBenefits = new ObservableCollection<ClaimHistoryBenefit>(_claimshistoryservice.ClaimHistoryBenefits);
                         IsClaimHistoryBenefitsBusy = false;
                     }, (message, code) =>
                     {
                         ErrorMessage = message;
                         ErrorCode = code;
                         IsClaimHistoryBenefitsBusy = false;
                     });
            }

            if (_claimshistoryservice.SelectedDisplayBy.Key == string.Empty)
            {
                _claimshistoryservice.SelectedDisplayBy = new KeyValuePair<string, string>(GSCHelper.DisplayByYearToDateKey, Resource.ClaimsHistorySearchViewModel_DisplayByYearToDate);
            }

            IsRightSideGreyedOut = true;
        }

        private ObservableCollection<ClaimHistoryType> _claimHistoryTypes;
        public ObservableCollection<ClaimHistoryType> ClaimHistoryTypes
        {
            get => _claimHistoryTypes;
            set
            {
                if (_claimHistoryTypes != value)
                {
                    _claimHistoryTypes = value;
                    RaisePropertyChanged(() => ClaimHistoryTypes);
                }
            }
        }

        private ObservableCollection<ClaimHistoryPayee> _claimHistoryPayees;
        public ObservableCollection<ClaimHistoryPayee> ClaimHistoryPayees
        {
            get => _claimHistoryPayees;
            set
            {
                if (_claimHistoryPayees != value)
                {
                    _claimHistoryPayees = RemoveAllFromClaimHistoryPayees(value);                    
                    RaisePropertyChanged(() => ClaimHistoryPayees);
                }
            }
        }

        private ClaimHistoryPayee AllClaimHistoryPayee { get; set; }

        private ObservableCollection<ClaimHistoryBenefit> _claimHistoryBenefits;
        private ObservableCollection<ClaimHistoryBenefit> ClaimHistoryBenefits
        {
            get => _claimHistoryBenefits;
            set
            {
                if (_claimHistoryBenefits != value)
                {
                    _claimHistoryBenefits = value;
                    SetOtherClaimHistoryBenefitsProperties();
                    RaisePropertyChanged(() => ClaimHistoryBenefits);
                }
            }
        }

        private ClaimHistoryBenefit _allClaimHistoryBenefit;
        public ClaimHistoryBenefit AllClaimHistoryBenefit
        {
            get => _allClaimHistoryBenefit;
            set
            {
                if (_allClaimHistoryBenefit != value)
                {
                    _allClaimHistoryBenefit = value;
                    RaisePropertyChanged(() => AllClaimHistoryBenefit);
                }
            }
        }

        public bool AllClaimHistoryBenefitIsSelected
        {
            get => AllClaimHistoryBenefit.IsSelected;
            set
            {
                if (AllClaimHistoryBenefit.IsSelected != value)
                {
                    AllClaimHistoryBenefit.IsSelected = value;
                    RaisePropertyChanged(() => AllClaimHistoryBenefitIsSelected);
                    RaisePropertyChanged(() => AllClaimHistoryBenefit);
                    SetAllBenefits(AllClaimHistoryBenefitIsSelected);
                    IsFullBenefitsListVisible = !AllClaimHistoryBenefit.IsSelected; //shows when all benefits is "No"
                }
            }
        }

        private ObservableCollection<ClaimHistoryBenefit> _claimHistoryBenefitsWithoutAll;
        public ObservableCollection<ClaimHistoryBenefit> ClaimHistoryBenefitsWithoutAll
        {
            get => _claimHistoryBenefitsWithoutAll;
            set
            {
                if (_claimHistoryBenefitsWithoutAll != value)
                {
                    _claimHistoryBenefitsWithoutAll = value;
                    RaisePropertyChanged(() => ClaimHistoryBenefitsWithoutAll);
                }
            }
        }

        private bool _isFullBenefitsListVisible;
        public bool IsFullBenefitsListVisible
        {
            get => _isFullBenefitsListVisible;
            set
            {
                if (_isFullBenefitsListVisible != value)
                {
                    _isFullBenefitsListVisible = value;
                    RaisePropertyChanged(() => IsFullBenefitsListVisible);
                }
            }
        }

        private ClaimHistoryType _selectedClaimHistoryType;
        public ClaimHistoryType SelectedClaimHistoryType
        {
            get => _selectedClaimHistoryType;
            set
            {
                if (_selectedClaimHistoryType != value)
                {
                    _selectedClaimHistoryType = value;
                    _claimshistoryservice.SelectedClaimHistoryType = _selectedClaimHistoryType;
                    RaisePropertyChanged(() => SelectedClaimHistoryType);
                    CheckClaimHistoryBenefitLogic();
                    if (_selectedClaimHistoryType.ID == GSCHelper.DefaultClaimsHistoryTypeID)
                        AllClaimHistoryBenefitIsSelected = true;
                }
            }
        }

        public ClaimHistoryPayee SelectedClaimHistoryPayee => GetSelectedClaimHistoryPayee();

        public Participant SelectedParticipant => _claimshistoryservice.SelectedParticipant;

        public ObservableCollection<ClaimHistoryBenefit> SelectedClaimHistoryBenefits => GetSelectedClaimHistoryBenefits();

        private bool _isClaimHistoryBenefitEnabled = true;
        public bool IsClaimHistoryBenefitEnabled
        {
            get => _isClaimHistoryBenefitEnabled;
            set
            {
                if (_isClaimHistoryBenefitEnabled != value)
                {
                    _isClaimHistoryBenefitEnabled = value;
                    RaisePropertyChanged(() => IsClaimHistoryBenefitEnabled);
                }
            }
        }

        public KeyValuePair<string, string> SelectedDisplayBy => _claimshistoryservice.SelectedDisplayBy;

        public DateTime SelectedStartDate => _claimshistoryservice.SelectedStartDate;

        public DateTime SelectedEndDate => _claimshistoryservice.SelectedEndDate;

        public int SelectedYear => _claimshistoryservice.SelectedYear;

        private bool _isRightSideGreyedOut;
        public bool IsRightSideGreyedOut
        {
            get => _isRightSideGreyedOut;
            set
            {
                if (_isRightSideGreyedOut != value)
                {
                    _isRightSideGreyedOut = value;
                    _claimshistoryservice.IsSearchRightSideGreyedOut = value;
                    RaisePropertyChanged(() => IsRightSideGreyedOut);
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged(() => ErrorMessage);
                }
            }
        }

        private int _errorCode;
        public int ErrorCode
        {
            get => _errorCode;
            set
            {
                if (_errorCode != value)
                {
                    _errorCode = value;
                    RaisePropertyChanged(() => ErrorCode);
                }
            }
        }

        private bool _isClaimHistoryTypesBusy = false;
        public bool IsClaimHistoryTypesBusy
        {
            get => _isClaimHistoryTypesBusy;
            set
            {
                _isClaimHistoryTypesBusy = value;
                UpdateOverallIsBusyIndicator();
            }
        }

        private bool _isClaimHistoryPayeesBusy = false;
        public bool IsClaimHistoryPayessBusy
        {
            get => _isClaimHistoryPayeesBusy;
            set
            {
                _isClaimHistoryPayeesBusy = value;
                UpdateOverallIsBusyIndicator();
            }
        }

        private bool _isClaimHistoryBenefitsBusy = false;
        public bool IsClaimHistoryBenefitsBusy
        {
            get => _isClaimHistoryBenefitsBusy;
            set
            {
                _isClaimHistoryBenefitsBusy = value;
                UpdateOverallIsBusyIndicator();
            }
        }

        private bool _isSearching = false;
        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                _isSearching = value;
                UpdateOverallIsBusyIndicator();
            }
        }

        public string SearchCriteria => Resource.SearchCriteria;

        public string ClaimType => Resource.ClaimType;

        public string ClaimsPaidTo => Resource.ClaimsPaidTo;

        public string PlanMember => Resource.PlanMember;

        public string Benefit => Resource.Benefit;

        public string DisplayByLabel => Resource.DisplayByBasedOnServiceDateOfClaim;

        public string Update => Resource.Update;

        private bool _noPayeeSelected = false;
        public bool NoPayeeSelected
        {
            get => _noPayeeSelected;
            set
            {
                if (_noPayeeSelected != value)
                {
                    _noPayeeSelected = value;
                    RaisePropertyChanged(() => NoPayeeSelected);
                }
            }
        }

        private bool _noBenefitSelected = false;
        public bool NoBenefitSelected
        {
            get => _noBenefitSelected;
            set
            {
                if (_noBenefitSelected != value)
                {
                    _noBenefitSelected = value;
                    RaisePropertyChanged(() => NoBenefitSelected);
                }
            }
        }

        private string _payeeValidationMessage;
        public string PayeeValidationMessage
        {
            get => _payeeValidationMessage;
            set
            {
                if (_payeeValidationMessage != value)
                {
                    _payeeValidationMessage = value;
                    RaisePropertyChanged(() => PayeeValidationMessage);
                }
            }
        }

        private string _benefitValidationMessage;
        public string BenefitValidationMessage
        {
            get => _benefitValidationMessage;
            set
            {
                if (_benefitValidationMessage != value)
                {
                    _benefitValidationMessage = value;
                    RaisePropertyChanged(() => BenefitValidationMessage);
                }
            }
        }

        MvxCommand _selectParticipantCommand;
        public ICommand SelectParticipantCommand
        {
            get
            {
                _selectParticipantCommand = _selectParticipantCommand ?? new MvxCommand(() =>
                {
                    ShowViewModel<ClaimsHistoryParticipantsViewModel>();
                },
                () =>
                {
                    return true;
                });
                return _selectParticipantCommand;
            }
        }


        MvxCommand _selectDisplayByCommand;
        public ICommand SelectDisplayByCommand
        {
            get
            {
                _selectDisplayByCommand = _selectDisplayByCommand ?? new MvxCommand(() =>
                {
                    ShowViewModel<ClaimsHistoryDisplayByViewModel>();
                },
                    () =>
                {
                    return true;
                });
                return _selectDisplayByCommand;
            }
        }

        MvxCommand _searchClaimsHistoryCommand;
        public ICommand SearchClaimsHistoryCommand
        {
            get
            {
                _searchClaimsHistoryCommand = _searchClaimsHistoryCommand ?? new MvxCommand(() =>
                {
                    if (IsValid())
                    {
                        string selectedClaimBenefits = GetFormattedSelectedClaimHistoryBenefits();
                        DateTime startDate = GetStartDate();
                        DateTime endDate = GetEndDate();
                        _claimshistoryservice.SelectedClaimHistoryBenefits = SelectedClaimHistoryBenefits.ToList();
                        _claimshistoryservice.SelectedClaimHistoryPayee = GetSelectedClaimHistoryPayee();
                        _claimshistoryservice.IsAllBenefitsSelected = AllClaimHistoryBenefitIsSelected;
                        _claimshistoryservice.SelectedStartDate = startDate;
                        _claimshistoryservice.SelectedEndDate = endDate;
                        IsSearching = true;
                        _dialogservice.ShowLoading(Resource.Loading);
                        _claimshistoryservice.SearchClaimsHistory(SelectedParticipant.PlanMemberID, SelectedClaimHistoryType.ID,
                            SelectedClaimHistoryPayee.ID, selectedClaimBenefits, startDate, endDate, () =>
                            {
                                IsSearching = false;
                                IsRightSideGreyedOut = false;
                                _dialogservice.HideLoading();
                                _messenger.Publish<RequestCloseClaimsHistoryResultsListViewModel>(new RequestCloseClaimsHistoryResultsListViewModel(this));
                                RaiseCloseClaimsHistorySearchVM(new EventArgs());
                                Close(this);
                            }, (message, code) =>
                            {
                                ErrorMessage = message;
                                ErrorCode = code;
                                IsSearching = false;
                                _dialogservice.HideLoading();
                                _dialogservice.Alert(ErrorMessage, null, Resource.ok);
                            });
                    }
                },
                () =>
                {
                    return true;
                });
                return _searchClaimsHistoryCommand;
            }
        }

        public event EventHandler CloseClaimsHistorySearchVM;
        protected virtual void RaiseCloseClaimsHistorySearchVM(EventArgs e)
        {
            if (this.CloseClaimsHistorySearchVM != null)
            {
                CloseClaimsHistorySearchVM(this, e);
            }
        }

        private void UpdateOverallIsBusyIndicator()
        {
            IsBusy = (IsClaimHistoryTypesBusy || IsClaimHistoryPayessBusy || IsClaimHistoryBenefitsBusy || IsSearching);
        }

        private void CheckClaimHistoryBenefitLogic()
        {
            if (SelectedClaimHistoryType != null && SelectedClaimHistoryType.ID == GSCHelper.ClaimHistoryDentalPredeterminationID)
            {
                ResetSelectedClaimHistoryBenefits(GSCHelper.ClaimHistoryBenefitIDForDental);
                IsClaimHistoryBenefitEnabled = false;
                
                // reset display by
                if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
                {
                    ResetDisplayByToYearToDate();
                }
            }
            else if (SelectedClaimHistoryType != null && SelectedClaimHistoryType.ID == GSCHelper.ClaimHistoryMedicalItemID)
            {
                ResetSelectedClaimHistoryBenefits(GSCHelper.ClaimHistoryBenefitIDForEHS);
                IsClaimHistoryBenefitEnabled = false;

                // reset display by
                if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
                {
                    ResetDisplayByToYearToDate();
                }
            }
            else
            {
                IsClaimHistoryBenefitEnabled = true;
            }
        }

        private string GetFormattedSelectedClaimHistoryBenefits()
        {
            string benefits = string.Join("-", SelectedClaimHistoryBenefits.Select(x => x.ID));
            benefits = benefits.Trim('-');
            return benefits;
        }

        private DateTime GetStartDate()
        {
            if (SelectedDisplayBy.Key == GSCHelper.DisplayByDateRangeKey)
            {
                return SelectedStartDate;
            }
            else if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
            {
                return new DateTime(SelectedYear, 1, 1);
            }
            else //(SelectedDisplayBy.Key == GSCHelper.DisplayByYearToDateKey)
            {
                return GSCHelper.StartDateOfCurrentYear;
            }
        }

        private DateTime GetEndDate()
        {
            if (SelectedDisplayBy.Key == GSCHelper.DisplayByDateRangeKey)
            {
                return SelectedEndDate;
            }
            else if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
            {
                return new DateTime(SelectedYear, 12, 31);
            }
            else //(SelectedDisplayBy.Key == GSCHelper.DisplayByYearToDateKey)
            {
                return DateTime.Now;
            }
        }

        private ObservableCollection<ClaimHistoryPayee> RemoveAllFromClaimHistoryPayees(ObservableCollection<ClaimHistoryPayee> value)
        {
            ObservableCollection<ClaimHistoryPayee> newCollection = new ObservableCollection<ClaimHistoryPayee>();
            foreach (ClaimHistoryPayee chp in value)
            {
                if (chp.ID != GSCHelper.ClaimHistoryPayeeAllID)
                {
                    newCollection.Add(chp);
                }
                else
                {
                    AllClaimHistoryPayee = chp;
                }
            }
            return newCollection;
        }

        //NOTE: This only works when the collection of Payees is [AL, SU, PR]. If anymore are added, this breaks.
        private ClaimHistoryPayee GetSelectedClaimHistoryPayee()
        {
            ClaimHistoryPayee selected = null;
            foreach (ClaimHistoryPayee chp in ClaimHistoryPayees)
            {
                if (chp.IsSelected)
                {
                    if (selected != null) //means both SU and PR are selected, so AL should be sent back
                    {
                        selected = AllClaimHistoryPayee;
                    }
                    else
                    {
                        selected = chp;
                    }
                }
            }
            return selected;
        }

        private void SetOtherClaimHistoryBenefitsProperties()
        {
            ObservableCollection<ClaimHistoryBenefit> woAll = new ObservableCollection<ClaimHistoryBenefit>();
            foreach (ClaimHistoryBenefit chb in ClaimHistoryBenefits)
            {
                if (chb.ID == ALL_BENEFIT_ID)
                {
                    AllClaimHistoryBenefit = chb;
                    AllClaimHistoryBenefitIsSelected = true;
                }
                else
                {
                    chb.IsSelected = true;
                    woAll.Add(chb);
                }
            }

            if (AllClaimHistoryBenefit == null)
            {
                AllClaimHistoryBenefit = new ClaimHistoryBenefit { ID = ALL_BENEFIT_ID, Name = Resource.ClaimHistorySearch_AllBenefits };
                AllClaimHistoryBenefitIsSelected = true;
            }

            ClaimHistoryBenefitsWithoutAll = woAll;

            //properly set IsSelected based on previous search
            if (_claimshistoryservice.SelectedClaimHistoryBenefits != null && _claimshistoryservice.SelectedClaimHistoryBenefits.Count > 0)
            {
                if (_claimshistoryservice.SelectedClaimHistoryBenefits.Count == ClaimHistoryBenefitsWithoutAll.Count)
                {
                    AllClaimHistoryBenefitIsSelected = true;
                }
                else
                {
                    AllClaimHistoryBenefitIsSelected = false;
                    foreach (ClaimHistoryBenefit schb in _claimshistoryservice.SelectedClaimHistoryBenefits)
                    {
                        ClaimHistoryBenefit chbwo = ClaimHistoryBenefitsWithoutAll.FirstOrDefault(c => c.ID == schb.ID);
                        if (chbwo != null) chbwo.IsSelected = true;
                    }
                }
            }
            CheckClaimHistoryBenefitLogic();
        }

        private ObservableCollection<ClaimHistoryBenefit> GetSelectedClaimHistoryBenefits()
        {
            ObservableCollection<ClaimHistoryBenefit> selected = new ObservableCollection<ClaimHistoryBenefit>();

            foreach (ClaimHistoryBenefit chb in ClaimHistoryBenefitsWithoutAll)
            {
                if (chb.IsSelected)
                    selected.Add(chb);
            }

            return selected;
        }

        private void ResetSelectedClaimHistoryBenefits(string selectedID)
        {
            AllClaimHistoryBenefitIsSelected = false;

            foreach (ClaimHistoryBenefit chb in ClaimHistoryBenefitsWithoutAll)
            {
                if (!string.IsNullOrEmpty(selectedID) && chb.ID == selectedID)
                    chb.IsSelected = true;
                else
                    chb.IsSelected = false;
            }
        }

        private void SetAllBenefits(bool val)
        {
            foreach (ClaimHistoryBenefit chb in ClaimHistoryBenefitsWithoutAll)
            {
                chb.IsSelected = val;
            }
        }

        private void SetAllPayeesIsSelected(bool val)
        {
            foreach (ClaimHistoryPayee chp in ClaimHistoryPayees)
            {
                chp.IsSelected = val;
            }
        }

        private void ResetDisplayByToYearToDate()
        {
            _claimshistoryservice.SelectedDisplayBy = new KeyValuePair<string, string>(GSCHelper.DisplayByYearToDateKey, Resource.ClaimsHistorySearchViewModel_DisplayByYearToDate);
            _claimshistoryservice.SelectedStartDate = GetStartDate();
            _claimshistoryservice.SelectedEndDate = GetEndDate();
            RaisePropertyChanged(() => SelectedDisplayBy);
        }

        private void SetSelectedClaimHistoryType()
        {
            if (ClaimHistoryTypes != null && ClaimHistoryTypes.Count > 0)
            {
                if (_claimshistoryservice.SelectedClaimHistoryType != null)
                {
                    SelectedClaimHistoryType = _claimshistoryservice.SelectedClaimHistoryType;
                }
                else
                {
                    SelectedClaimHistoryType = ClaimHistoryTypes.FirstOrDefault(cht => cht.ID == GSCHelper.DefaultClaimsHistoryTypeID);
                }
            }
        }

        private void SetSelectedClaimHistoryPayees()
        {
            if (_claimshistoryservice.SelectedClaimHistoryPayee != null && ClaimHistoryPayees != null && ClaimHistoryPayees.Count > 0)
            {
                SetAllPayeesIsSelected(false);
                if (_claimshistoryservice.SelectedClaimHistoryPayee.ID == GSCHelper.ClaimHistoryPayeeAllID)
                {
                    SetAllPayeesIsSelected(true);
                }
                else //set only the one payee as selected
                {
                    ClaimHistoryPayee chp = ClaimHistoryPayees.FirstOrDefault(c => c.ID == _claimshistoryservice.SelectedClaimHistoryPayee.ID);
                    if (chp != null) chp.IsSelected = true;
                }
            }
        }

        public class ClaimsHistorySearchValidator : AbstractValidator<ClaimsHistorySearchViewModel>
        {
            public ClaimsHistorySearchValidator()
            {
                RuleFor(vm => vm.SelectedClaimHistoryPayee).Must(HaveAtLeastOnePayeeSelected).WithMessage(Resource.PleaseSelectPayee);
                RuleFor(vm => vm.SelectedClaimHistoryBenefits).Must(HaveAtLeastOneBenefitSelected).WithMessage(Resource.PleaseSelectBenefit);
            }
            public bool HaveAtLeastOnePayeeSelected(ClaimHistoryPayee payee)
            {
                return payee != null;
            }
            public bool HaveAtLeastOneBenefitSelected(ObservableCollection<ClaimHistoryBenefit> benefits)
            {
                return benefits.Count > 0;
            }
        }

        private bool IsValid()
        {
            // Reset for validation
            NoPayeeSelected = false;
            NoBenefitSelected = false;
            PayeeValidationMessage = string.Empty;
            BenefitValidationMessage = string.Empty;

            ClaimsHistorySearchValidator validator = new ClaimsHistorySearchValidator();
            validator.CascadeMode = CascadeMode.Continue;
            ValidationResult result = new ValidationResult();
            result = validator.Validate<ClaimsHistorySearchViewModel>(this, "SelectedClaimHistoryPayee", "SelectedClaimHistoryBenefits");

            foreach (var failure in result.Errors)
            {
                switch (failure.PropertyName)
                {
                    case "SelectedClaimHistoryPayee":
                    {
                        NoPayeeSelected = true;
                        PayeeValidationMessage += failure.ErrorMessage;

                        break;
                    }
                    case "SelectedClaimHistoryBenefits":
                    {
                        NoBenefitSelected = true;
                        BenefitValidationMessage += failure.ErrorMessage;

                        break;
                    }
                }
            }
            return result.IsValid;
        }
    }
}
