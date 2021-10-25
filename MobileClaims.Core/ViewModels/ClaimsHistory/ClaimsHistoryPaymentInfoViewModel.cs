using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.ClaimsHistory
{
    public class ClaimsHistoryPaymentInfoViewModel : HCSAViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimsHistoryService _claimshistoryservice;

        public ClaimsHistoryPaymentInfoViewModel(IMvxMessenger messenger, IClaimsHistoryService claimshistoryservice)
        {
            _messenger = messenger;
            _claimshistoryservice = claimshistoryservice;

            SearchResult = null;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (_claimshistoryservice.SelectedSearchResult != null)
            {
                SearchResult = _claimshistoryservice.SelectedSearchResult;
            }
            else
            {
                ErrorMessage = "You've managed to arrive at this View Model without selecting a Search Result. This should be impossible.";
            }

            if (_claimshistoryservice.SelectedParticipant != null)
            {
                SelectedParticipant = _claimshistoryservice.SelectedParticipant;
            }
        }

        private ClaimState _searchResult;
        public ClaimState SearchResult
        {
            get { return _searchResult; }
            set
            {
                if (_searchResult != value)
                {
                    _searchResult = value;
                    if (_searchResult != null)
                        Payment = _searchResult.Payment;
                    else
                        Payment = null;
                    RaisePropertyChanged(() => SearchResult);
                }
            }
        }

        private ClaimPayment _payment;
        public ClaimPayment Payment
        {
            get { return _payment; }
            set
            {
                if (_payment != value)
                {
                    _payment = value;
                    RaisePropertyChanged(() => Payment);
                }
            }
        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get { return _selectedParticipant; }
            set
            {
                if (_selectedParticipant != value)
                {
                    _selectedParticipant = value;
                    RaisePropertyChanged(() => SelectedParticipant);
                }
            }
        }

        public string DateOfInquiry
        {
            get { return _claimshistoryservice.DateOfInquiry.ToString(GSCHelper.DATE_OF_INQUIRY_FORMAT); }
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

        public string PaymentInformation
        {
            get { return Resource.PaymentInformation; }
        }

        public string GreenShieldIDNumber
        {
            get { return BrandResource.GreenShieldIDNumberNoColon; }
        }

        public string PaymentMethod
        {
            get { return Resource.PaymentMethodNoColon; }
        }

        public string PaymentAmount
        {
            get { return Resource.PaymentAmountNoColon; }
        }

        public string PaymentCurrency
        {
            get { return Resource.PaymentCurrencyNoColon; }
        }

        public string DateOfInquiryLabel
        {
            get { return Resource.DateOfInquiryNoColon; }
        }

        public string DirectDepositNumber
        {
            get { return Resource.DirectDepositNumberNoColon; }
        }

        public string StatementDate
        {
            get { return Resource.StatementDateNoColon; }
        }

        public string DepositDate
        {
            get { return Resource.DepositDateNoColon; }
        }

        public string ChequeNumberLabel
        {
            get { return Resource.ChequeNumberNoColon; }
        }

        public string ChequeStatusLabel
        {
            get { return Resource.ChequeStatusNoColon; }
        }

        public string PaymentDateLabel
        {
            get { return Resource.PaymentDateNoColon; }
        }

        public string CashedDateLabel
        {
            get { return Resource.CashedDateNoColon; }
        }
    }
}
