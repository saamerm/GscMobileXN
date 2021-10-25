using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModels.HCSA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.ViewModelParameters;

namespace MobileClaims.Core.ViewModels.ClaimsHistory
{
    public class ClaimsHistoryResultDetailViewModel : HCSAViewModelBase
    {
        private readonly IParticipantService _participantService;
        private readonly IMvxMessenger _messenger;
        private readonly IClaimsHistoryService _claimshistoryservice;
        private readonly IClaimService _claimservice;
        private readonly IUserDialogs _userDialogs;

        private bool _isWebClaimAgreementAccepted;

        public ClaimsHistoryResultDetailViewModel(IParticipantService participantService, IMvxMessenger messenger, 
            IClaimsHistoryService claimshistoryservice, IClaimService claimservice, IUserDialogs userDialogs)
        {
            _messenger = messenger;
            _participantService = participantService;
            _claimshistoryservice = claimshistoryservice;
            _claimservice = claimservice;
            _userDialogs = userDialogs;

            ShowPaymentInfoCommand = new MvxCommand(ShowPaymentInfo);

            SelectedSearchResult = null;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_claimshistoryservice.SelectedSearchResult != null)
            {
                SelectedSearchResult = _claimshistoryservice.SelectedSearchResult;
            }
            else
            {
                ErrorMessage = "You've managed to arrive at this View Model without selecting a Search Result. This should be impossible.";
            }

            if (_claimshistoryservice.SelectedParticipant != null)
            {
                SelectedParticipant = _claimshistoryservice.SelectedParticipant;
            }

            if (_claimshistoryservice.SelectedSearchResultType != null)
            {
                SearchResults = _claimshistoryservice.GetSearchResultsByBenefitID(_claimshistoryservice.SelectedSearchResultType.BenefitID);
            }

            foreach (var searchResult in SearchResults)
            {
                searchResult.ExecuteOpenConfirmationOfPayment = OpenConfirmationOfPayment;
                searchResult.ExecuteOpenPaymentInfo = ShowPaymentInfo;
            }   
        }

        public void RefreshSelectedSearchResult()
        {
            if (_claimshistoryservice.SelectedSearchResultType != null)
            {
                SearchResults = _claimshistoryservice.GetSearchResultsByBenefitID(_claimshistoryservice.SelectedSearchResultType.BenefitID);
            }
            _selectedSearchResult = _claimshistoryservice.SelectedSearchResult;

        }

        private ClaimState _selectedSearchResult;
        public ClaimState SelectedSearchResult
        {
            get => _selectedSearchResult;
            set
            {
                if (_selectedSearchResult != value)
                {
                    _selectedSearchResult = value;
                    _claimshistoryservice.SelectedSearchResult = value;
                    IsPaymentButtonVisible = (_selectedSearchResult.Payment != null);
                    SelectedSearchResult.IsPaymentButtonVisible = IsPaymentButtonVisible;
                    RaisePropertyChanged(() => SearchResults);
                    RaisePropertyChanged(() => SelectedSearchResult);
                }
            }
        }

        private ObservableCollection<ClaimState> _searchResults;
        public ObservableCollection<ClaimState> SearchResults
        {
            get => _searchResults;
            set
            {
                if (_searchResults != value)
                {
                    _searchResults = value;
                    RaisePropertyChanged(() => SearchResults);
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

        private bool _isPaymentButtonVisible;
        public bool IsPaymentButtonVisible
        {
            get => _isPaymentButtonVisible;
            set
            {
                if (_isPaymentButtonVisible != value)
                {
                    _isPaymentButtonVisible = value;
                    RaisePropertyChanged(() => IsPaymentButtonVisible);
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

        public string ClaimDetails => Resource.ClaimDetails;

        public string Of => Resource.Of;

        public string Claims => Resource.Claims;

        public string ServiceDate => Resource.ServiceDate;

        public string ClaimFormNumber => Resource.ClaimFormNumber;

        public string ServiceDescription => Resource.ServiceDescription;

        public string ClaimedAmount => Resource.ClaimedAmount;

        public string OtherPaidAmount => Resource.OtherPaidAmount;

        public string PaidAmount => Resource.PaidAmount;

        public string CopayDeductible => Resource.CopayDeductible;

        public string PaymentDate => Resource.PaymentDate;

        public string PaidTo => Resource.PaidTo;

        public string Quantity => Resource.Quantity;

        public string PaymentInformationLabel => Resource.PaymentInformation;

        public string EOBLabel => Resource.ExplanationOfBenefits;

        MvxCommand _gobackcommand;
        public override ICommand GoBackCommand
        {
            get
            {
                _gobackcommand = _gobackcommand ?? new MvxCommand(() =>
                {

                    _messenger.Publish(new RequestCloseClaimsHistoryResultsListViewModel(this));
                    base.GoBackCommand.Execute(null);
                });
                return _gobackcommand;
            }
        }

        public IMvxCommand ShowPaymentInfoCommand { get; }

        private void ShowPaymentInfo()
        {
            ShowViewModel<ClaimsHistoryPaymentInfoViewModel>();
        }

        private async void OpenConfirmationOfPayment()
        {
            try
            {
                var parameter = new MvxBundle();

                var topCardViewData = await GetTopCardViewData();

                var uploadDocumentsFormData = new UploadDocumentsFormData
                {
                    ClaimFormId = SelectedSearchResult.ClaimFormID,
                    ParticipantNumber = topCardViewData.ParticipantNumber
                };

                topCardViewData.ClaimActionState = SearchResults
                    .Where(x => x.ClaimFormID.ToString() == topCardViewData.ClaimForm).Select(x => x.ClaimActionStatus)
                    .FirstOrDefault();

                IClaimUploadProperties uploadable = (IClaimUploadProperties)UploadFactory.Create(topCardViewData.ClaimActionState, nameof(ConfirmationOfPaymentUploadViewModel));

                ConfirmationOfPaymentUploadViewModelParameters copParamaters =
                    new ConfirmationOfPaymentUploadViewModelParameters(topCardViewData, uploadDocumentsFormData, uploadable);

                await _participantService.GetUserAgreementWCS();

                if (_participantService.UserAgreement == null)
                {
                    throw new NullResponseException();
                }

                _isWebClaimAgreementAccepted = _participantService.UserAgreement.IsAccepted;

                if (_isWebClaimAgreementAccepted)
                {
                    await ShowViewModel<ConfirmationOfPaymentUploadViewModel, IViewModelParameters>(copParamaters);
                }
                else
                {
                    var catalog = new NavigationCatalog
                    {
                        NavigateTo = typeof(ConfirmationOfPaymentUploadViewModel).FullName,
                        NavigateToParameter = copParamaters, //parameter,
                        NavigateFrom = typeof(ClaimsHistoryResultDetailViewModel).FullName
                    };

                    await ShowViewModel<WebAgreementViewModel, WebAgreementViewModelParameters>(new WebAgreementViewModelParameters(catalog));
                }
            }
            catch (NullResponseException e)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
            catch (Exception e)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
        }

        private async Task<TopCardViewData> GetTopCardViewData()
        {
            var claimFormId = SelectedSearchResult.ClaimFormID.ToString();

            var claim = await GetCopClaimSummaryAsync(claimFormId);

            var topCardViewData = new TopCardViewData
            {
                ClaimForm = claim.ClaimFormID.ToString(),
                ClaimedAmount = claim.TotalCdRendAmt?.AddDolarSignBasedOnCulture(),
                ServiceDescription = claim.BenefitTypeDescr,
                ServiceDate = claim.MinServeDate.Date.ToString("d") == claim.MaxServeDate.Date.ToString("d")
                    ? claim.MinServeDate.Date.ToString("d")
                    : $"{claim.MinServeDate.Date:d} - {claim.MaxServeDate.Date:d}",
                UserName = claim.ParticipantName,
                ParticipantNumber = claim.ParticipantNumber,
                EobMessages = string.Join(Environment.NewLine + Environment.NewLine, 
                    claim.EOBMessages?.Select(x => x.Message).Distinct().ToList() ?? new List<string>()),
            };

            return topCardViewData;
        }

        private async Task<ClaimSummary> GetCopClaimSummaryAsync(string claimFormId)
        {
            IEnumerable<ClaimSummary> claimSummary;
            try
            {
                claimSummary = await _claimservice.GetCOPClaimSummaryAsync(claimFormId);
                if (claimSummary == null)
                {
                    throw new NullResponseException();
                }
            }
            catch (NullResponseException e)
            {
                return null;
            }
            return claimSummary.FirstOrDefault();
        }
    }
}
