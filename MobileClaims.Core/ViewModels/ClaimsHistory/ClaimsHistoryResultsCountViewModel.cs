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
    public class ClaimsHistoryResultsCountViewModel : HCSAViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimsHistoryService _claimshistoryservice;
        private readonly ILoginService _loginservice;
        private readonly IParticipantService _participantservice;
        private readonly MvxSubscriptionToken _newSearchComplete;
        private readonly IUserDialogs _dialogservice;

        public ClaimsHistoryResultsCountViewModel(IMvxMessenger messenger, IClaimsHistoryService claimshistoryservice, ILoginService loginservice, IParticipantService participantservice)
        {
            _messenger = messenger;
            _claimshistoryservice = claimshistoryservice;
            _loginservice = loginservice;
            _participantservice = participantservice;
            _dialogservice = Mvx.IoCProvider.Resolve<IUserDialogs>();

            SearchResultsSummary = new ObservableCollection<ClaimHistorySearchResultSummary>();

            _newSearchComplete = _messenger.Subscribe<SearchClaimsHistoryComplete>((message) =>
            {
                SearchResultsSummary.Clear();
                foreach (var result in _claimshistoryservice.SearchResultsSummary)
                {
                    SearchResultsSummary.Add(result);
                }

                IsSearchResultsSummarySelected = false;
                SelectedParticipant = _claimshistoryservice.SelectedParticipant;  
                RaisePropertyChanged(() => DateOfInquiry);
                RaisePropertyChanged(() => Period);
                RaisePropertyChanged(() => LinesOfBusiness);
                RaisePropertyChanged(() => ClaimType);
            });
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_claimshistoryservice.ClaimHistoryTypes == null || _claimshistoryservice.ClaimHistoryTypes.Count == 0)
            {
                _claimshistoryservice.GetClaimHistoryTypes(() =>
                    {
                        // don't really need to do anything here, am only obtaining them to have ClaimsHistorySearchViewModel ready to go
                    }, (message, code) =>
                    {
                        // again, don't deal with the failure here, re-call the service when ClaimsHistorySearchViewModel is used
                    });
            }

            if (_claimshistoryservice.ClaimHistoryPayees == null || _claimshistoryservice.ClaimHistoryPayees.Count == 0)
            {
                _claimshistoryservice.GetClaimHistoryPayees(() =>
                    {
                        // don't really need to do anything here, am only obtaining them to have ClaimsHistorySearchViewModel ready to go
                    }, (message, code) =>
                     {
                         // again, don't deal with the failure here, re-call the service when ClaimsHistorySearchViewModel is used
                     });
            }

            if (_claimshistoryservice.SelectedParticipant != null)
            {
                SelectedParticipant = _claimshistoryservice.SelectedParticipant;
            }
            else //default to logged in plan member
            {
                _claimshistoryservice.SelectedParticipant = _participantservice.PlanMember;
                SelectedParticipant = _participantservice.PlanMember;
            }

            if (_claimshistoryservice.ClaimHistoryBenefits != null && _claimshistoryservice.ClaimHistoryBenefits.Count > 0)
            {
                // get default search results
                GetDefaultSearchResults();
            }
            else // get claim history benefits before getting default search results
            {
                this.IsBusy = true;
                _claimshistoryservice.GetClaimHistoryBenefits(_loginservice.CurrentPlanMemberID, () =>
                    {
                        // now get the default search results
                        GetDefaultSearchResults();
                    }, (message, code) =>
                    {
                        // unable to get benefits, means unable to get default search results -- set error properties
                        ErrorMessage = message;
                        ErrorCode = code;
                        this.IsBusy = false;
                    });
            }

            IsSearchResultsSummarySelected = false;
        }

        private ObservableCollection<ClaimHistorySearchResultSummary> _searchResultsSummary;
        public ObservableCollection<ClaimHistorySearchResultSummary> SearchResultsSummary
        {
            get => _searchResultsSummary;
            set
            {
                if (_searchResultsSummary != value)
                {
                    _searchResultsSummary = value;
                    RaisePropertyChanged(() => SearchResultsSummary);
                }
            }
        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                if (_selectedParticipant != value)
                {
                    _selectedParticipant = value;
                    RaisePropertyChanged(() => SelectedParticipant);
                }
            }
        }

        public string DateOfInquiry => string.Format("{0} {1}", Resource.DateOfInquiry, _claimshistoryservice.DateOfInquiry.ToString(GSCHelper.DATE_OF_INQUIRY_FORMAT));

        public string Period => _claimshistoryservice.Period;

        public string LinesOfBusiness => _claimshistoryservice.LinesOfBusiness;

        public string ClaimType
        {
            get
            {
                if (_claimshistoryservice.SelectedClaimHistoryType != null)
                    return _claimshistoryservice.SelectedClaimHistoryType.Name;
                else
                    return string.Empty;
            }
        }

        private bool _isSearchResultsSummarySelected;
        public bool IsSearchResultsSummarySelected
        {
            get => _isSearchResultsSummarySelected;
            set
            {
                if (_isSearchResultsSummarySelected != value)
                {
                    _isSearchResultsSummarySelected = value;
                    _claimshistoryservice.IsResultsCountSearchResultsSummarySelected = value;
                    RaisePropertyChanged(() => IsSearchResultsSummarySelected);
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

        public string ClaimsHistory => Resource.ClaimsHistory;

        public string SearchCriteria => Resource.SearchCriteria;

        public string ClaimsFor => Resource.ClaimsFor;

        public string ParticipantLabel => Resource.ParticipantWithColon;

        public string LineOfBusinessLabel => Resource.LineOfBusiness;

        public string PeriodLabel => Resource.Period;

        public string ClaimTypeLabel => Resource.ClaimType;

        MvxCommand<ClaimHistorySearchResultSummary> _selectSearchResultTypeCommand;
        public ICommand SelectSearchResultTypeCommand
        {
            get
            {
                _selectSearchResultTypeCommand = _selectSearchResultTypeCommand ?? new MvxCommand<ClaimHistorySearchResultSummary>((selectedSummaryItem) =>
                {
                    _claimshistoryservice.SelectedSearchResultType = selectedSummaryItem;
                    IsSearchResultsSummarySelected = true;
                    ShowViewModel<ClaimsHistoryResultsListViewModel>();
                },
                (selectedSummaryItem) =>
                {
                    return true;
                });
                return _selectSearchResultTypeCommand;
            }
        }

        MvxCommand _searchCriteriaCommand;
        public ICommand SearchCriteriaCommand
        {
            get
            {
                _searchCriteriaCommand = _searchCriteriaCommand ?? new MvxCommand(() =>
                {
                    ShowViewModel<ClaimsHistorySearchViewModel>();
                },
                () =>
                {
                    return true;
                });
                return _searchCriteriaCommand;
            }
        }

		private MvxCommand _showclaimscommand;
		public ICommand ShowChooseClaimOrHistoryCommand
		{
			get
			{
				return new MvxCommand(() =>
					{
						Unsubscribe();
						ShowViewModel<ViewModels.ChooseClaimOrHistoryViewModel>();
					});
			}
		}

        private void GetDefaultSearchResults()
        {
            this.IsBusy = true;
            _dialogservice.ShowLoading(Resource.Loading);

            if (_claimshistoryservice.ClaimHistoryTypes != null && _claimshistoryservice.ClaimHistoryTypes.Count > 0)
            {
                _claimshistoryservice.SelectedClaimHistoryType = _claimshistoryservice.ClaimHistoryTypes.FirstOrDefault(cht => cht.ID == GSCHelper.DefaultClaimsHistoryTypeID);
            }
            else
            {
                _claimshistoryservice.SelectedClaimHistoryType = new ClaimHistoryType { ID = GSCHelper.DefaultClaimsHistoryTypeID, Name = Resource.Claims, EarliestYear = GSCHelper.ClaimHistoryDefaultEarliestYear };
            }
            _claimshistoryservice.SelectedDisplayBy = new KeyValuePair<string, string>(GSCHelper.DisplayByYearToDateKey, Resource.ClaimsHistorySearchViewModel_DisplayByYearToDate);
            _claimshistoryservice.SelectedStartDate = GSCHelper.StartDateOfCurrentYear;
            _claimshistoryservice.SelectedEndDate = DateTime.Now;
            _claimshistoryservice.SelectedClaimHistoryBenefits = _claimshistoryservice.ClaimHistoryBenefits;
            _claimshistoryservice.IsAllBenefitsSelected = true;
            _claimshistoryservice.SelectedParticipant = _participantservice.PlanMember;
            _claimshistoryservice.SelectedClaimHistoryPayee = new ClaimHistoryPayee { ID = GSCHelper.ClaimHistoryPayeeAllID, Name = string.Empty };

            _claimshistoryservice.SearchClaimsHistory(_loginservice.CurrentPlanMemberID, GSCHelper.DefaultClaimsHistoryTypeID,
                GSCHelper.ClaimHistoryPayeeAllID, _claimshistoryservice.AllClaimHistoryBenefits, GSCHelper.StartDateOfCurrentYear,
                DateTime.Now, () =>
                {
                    SearchResultsSummary = new ObservableCollection<ClaimHistorySearchResultSummary>(_claimshistoryservice.SearchResultsSummary);
                    this.IsBusy = false;
                    _dialogservice.HideLoading();
                    RaisePropertyChanged(() => DateOfInquiry);
                    RaisePropertyChanged(() => Period);
                    RaisePropertyChanged(() => LinesOfBusiness);
                    RaisePropertyChanged(() => ClaimType);
                }, (message, code) =>
                {
                    SearchResultsSummary = new ObservableCollection<ClaimHistorySearchResultSummary>(_claimshistoryservice.SearchResultsSummary);
                    ErrorMessage = message;
                    ErrorCode = code;
                    this.IsBusy = false;
                    _dialogservice.HideLoading();
                    RaisePropertyChanged(() => DateOfInquiry);
                    RaisePropertyChanged(() => Period);
                    RaisePropertyChanged(() => LinesOfBusiness);
                    RaisePropertyChanged(() => ClaimType);
                });
        }
    }
}