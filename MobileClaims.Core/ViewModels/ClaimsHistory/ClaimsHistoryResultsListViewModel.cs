using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.HCSA;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.ClaimsHistory
{
    public class ClaimsHistoryResultsListViewModel : HCSAViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimsHistoryService _claimshistoryservice;
        private readonly IUserDialogs _dialogservice;
        private readonly MvxSubscriptionToken _closeThis;
        private readonly MvxSubscriptionToken _rightSideGreyedOut;

        public ClaimsHistoryResultsListViewModel(IMvxMessenger messenger, IClaimsHistoryService claimshistoryservice)
        {
            _messenger = messenger;
            _claimshistoryservice = claimshistoryservice;
            _dialogservice = Mvx.IoCProvider.Resolve<IUserDialogs>();

            SearchResults = new ObservableCollection<ClaimState>();

            _closeThis = _messenger.Subscribe<RequestCloseClaimsHistoryResultsListViewModel>((message) =>
            {
                _messenger.Unsubscribe<RequestCloseClaimsHistoryResultsListViewModel>(_closeThis);
                _messenger.Unsubscribe<IsRightSideGreyedOutUpdated>(_rightSideGreyedOut);
                RaiseCloseClaimsHistoryResultsListVM(new EventArgs());
                Close(this);
            });

            _rightSideGreyedOut = _messenger.Subscribe<IsRightSideGreyedOutUpdated>((message) =>
            {
                RaisePropertyChanged(() => IsRightSideGreyedOut);
            });
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_claimshistoryservice.SelectedSearchResultType != null)
            {
                SearchResultType = _claimshistoryservice.SelectedSearchResultType;
                SearchResults = _claimshistoryservice.GetSearchResultsByBenefitID(_claimshistoryservice.SelectedSearchResultType.BenefitID);
            }
            else
            {
                ErrorMessage = "You've managed to arrive at this View Model without selecting a Search Result Type. This should be impossible.";
            }
        }

        private ClaimHistorySearchResultSummary _searchResultType;
        public ClaimHistorySearchResultSummary SearchResultType
        {
            get { return _searchResultType; }
            set
            {
                if (_searchResultType != value)
                {
                    _searchResultType = value;
                    RaisePropertyChanged(() => SearchResultType);
                }
            }
        }

        private ObservableCollection<ClaimState> _searchResults;
        public ObservableCollection<ClaimState> SearchResults
        {
            get { return _searchResults; }
            set
            {
                if (_searchResults != value)
                {
                    _searchResults = value;
                    SetTotalAmounts();
                    RaisePropertyChanged(() => SearchResults);
                }
            }
        }

        private double _totalClaimedAmount;
        public double TotalClaimedAmount
        {
            get { return _totalClaimedAmount; }
            set
            {
                if (_totalClaimedAmount != value)
                {
                    _totalClaimedAmount = value;
                    RaisePropertyChanged(() => TotalClaimedAmount);
                }
            }
        }

        private double _totalOtherPaidAmount;
        public double TotalOtherPaidAmount
        {
            get { return _totalOtherPaidAmount; }
            set
            {
                if (_totalOtherPaidAmount != value)
                {
                    _totalOtherPaidAmount = value;
                    RaisePropertyChanged(() => TotalOtherPaidAmount);
                }
            }
        }

        private double _totalPaidAmount;
        public double TotalPaidAmount
        {
            get { return _totalPaidAmount; }
            set
            {
                if (_totalPaidAmount != value)
                {
                    _totalPaidAmount = value;
                    RaisePropertyChanged(() => TotalPaidAmount);
                }
            }
        }

        private double _totalCopay;
        public double TotalCopay
        {
            get { return _totalCopay; }
            set
            {
                if (_totalCopay != value)
                {
                    _totalCopay = value;
                    RaisePropertyChanged(() => TotalCopay);
                }
            }
        }

        public Participant SelectedParticipant
        {
            get { return _claimshistoryservice.SelectedParticipant; }
        }

        public string Period
        {
            get { return _claimshistoryservice.Period; }
        }

        public string DateOfInquiry
        {
            get { return string.Format("{0} {1}", Resource.DateOfInquiry, _claimshistoryservice.DateOfInquiry.ToString(GSCHelper.DATE_OF_INQUIRY_FORMAT)); }
        }

        public string LinesOfBusiness
        {
            get { return _claimshistoryservice.LinesOfBusiness; }
        }

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

        public bool IsRightSideGreyedOut
        {
            get { return _claimshistoryservice.IsRightSideGreyedOutMaster; }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
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
            get { return _errorCode; }
            set
            {
                if (_errorCode != value)
                {
                    _errorCode = value;
                    RaisePropertyChanged(() => ErrorCode);
                }
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged(() => IsLoading);
                }
            }
        }

        public string SearchCriteria
        {
            get { return Resource.SearchCriteria; }
        }

        public string Claims
        {
            get { return Resource.Claims; }
        }

        public string SearchedForLabel
        {
            get { return Resource.SearchedFor; }
        }

        public string NumberOfClaimsLabel
        {
            get { return string.Format("{0} {1}", SearchResults.Count, Resource.Claims); }
        }

        public string TotalsLabel
        {
            get { return Resource.TotalsCaps; }
        }

        public string ClaimedAmountLabel
        {
            get { return Resource.ClaimedAmount; }
        }

        public string OtherPaidAmountLabel
        {
            get { return Resource.OtherPaidAmount; }
        }

        public string PaidAmountLabel
        {
            get { return Resource.PaidAmount; }
        }

        public string CopayDeductibleLabel
        {
            get { return Resource.CopayDeductible; }
        }

        public string ParticipantLabel
        {
            get { return Resource.ParticipantWithColon; }
        }

        public string LineOfBusinessLabel
        {
            get { return Resource.LineOfBusiness; }
        }

        public string PeriodLabel
        {
            get { return Resource.Period; }
        }

        public string ClaimTypeLabel
        {
            get { return Resource.ClaimType; }
        }

        MvxCommand<ClaimState> _selectSearchResultCommand;
        public ICommand SelectSearchResultCommand
        {
            get
            {
                _selectSearchResultCommand = _selectSearchResultCommand ?? new MvxCommand<ClaimState>((selectedResult) =>
                {
                    _claimshistoryservice.SelectedSearchResult = selectedResult;
                    ShowViewModel<ClaimsHistoryResultDetailViewModel>();
                },
                (selectedResult) =>
                {
                    return true;
                });
                return _selectSearchResultCommand;
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

        MvxCommand _showLoadingCommand;
        public ICommand ShowLoadingCommand
        {
            get
            {
                _showLoadingCommand = _showLoadingCommand ?? new MvxCommand(() =>
                {
                    _dialogservice.ShowLoading(Resource.Loading);
                },
                () =>
                {
                    return !IsLoading;
                });
                return _showLoadingCommand;
            }
        }

        MvxCommand _hideLoadingCommand;
        public ICommand HideLoadingCommand
        {
            get
            {
                _hideLoadingCommand = _hideLoadingCommand ?? new MvxCommand(() =>
                {
                    _dialogservice.HideLoading();
                },
                () =>
                {
                    return true;
                });
                return _hideLoadingCommand;
            }
        }

        public event EventHandler CloseClaimsHistoryResultsListVM;
        protected virtual void RaiseCloseClaimsHistoryResultsListVM(EventArgs e)
        {
            if (this.CloseClaimsHistoryResultsListVM != null)
            {
                CloseClaimsHistoryResultsListVM(this, e);
            }
        }

        private void SetTotalAmounts()
        {
            double totalClaimedAmount = 0d;
            double totalOtherPaidAmount = 0d;
            double totalPaidAmount = 0d;
            double totalCopay = 0d;
            if (SearchResults != null && SearchResults.Count > 0)
            {
                foreach (ClaimState cs in SearchResults)
                {
                    totalClaimedAmount += cs.ClaimedAmount;
                    totalOtherPaidAmount += cs.OtherPaidAmount;

                    if (cs.PaidAmount != null)
                        totalPaidAmount += double.Parse(cs.PaidAmount.ToString());

                    if (cs.CopayAmount != null)
                        totalCopay += double.Parse(cs.CopayAmount.ToString());
                }
            }
            TotalClaimedAmount = totalClaimedAmount;
            TotalOtherPaidAmount = totalOtherPaidAmount;
            TotalPaidAmount = totalPaidAmount;
            TotalCopay = totalCopay;
        }
    }
}
