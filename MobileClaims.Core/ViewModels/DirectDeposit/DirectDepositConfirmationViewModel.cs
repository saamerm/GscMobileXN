using System;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DirectDepositConfirmationViewModel : ViewModelBase<DirectDepositViewModelParameters>
    {
        public string Title { get; set; }
        public string AndroidTitle => Resource.DirectDeposit;
        public string SubTitle => Resource.DDConfirmUpdateLabel;

        public string ConfirmDDUpdateLabel => Resource.DDConfirmUpdateLabel;
        public string AuthoriseDepositFundLabel => BrandResource.DDConfirmAuthoriseDepositFundLabel;

        public string BankInfoTitle => Resource.DDValidationErrBankInfo;
        public string TransitNumberTitle => Resource.DirectDepositTransitNumber;
        public string BankNumberTitle => Resource.DirectDepositBankNumber;
        public string AccountNumberTitle => Resource.DirectDepositAccountNumber;

        public string NoteConfirmation1Label => Resource.DDClickLabel;
        public string NoteConfirmation2Label => Resource.Submit.ToUpper();
        public string NoteConfirmation3Label => Resource.DDNoteConfirmation;
        public string NoteConfirmation4Label => Resource.DDChangeInformationLabel;

        public string NoteConfirmationLabel => Resource.DDConfirmNoteConfirmationLabel;

        public string ContinueTitle => Resource.Submit.ToUpperInvariant();
        public string ChangeInformationTitle => Resource.ChangeInformationTitle.ToUpperInvariant();

        public IMvxAsyncCommand ContinueCommand { get; set; }
        public IMvxAsyncCommand ChangeInformationCommand { get; set; }
        private readonly IDirectDepositService _directDepositService;
        private readonly IDeviceService _deviceService;

        private bool _isAuthorisedDepositFund;
        public bool IsAuthorisedDepositFund
        {
            get => _isAuthorisedDepositFund;
            set => SetProperty(ref _isAuthorisedDepositFund, value);
        }

        public DirectDepositConfirmationViewModel(IDirectDepositService directDepositService, IDeviceService deviceService)
        {
            SelectAuthoriseDepositFundCommand = new MvxAsyncCommand(ExecuteSelectAuthoriseDepositFundCommand);
            ContinueCommand = new MvxAsyncCommand(ExecuteContinueCommand);
            ChangeInformationCommand = new MvxAsyncCommand(ExecuteChangeInformationCommand);
            _deviceService = deviceService;
            _directDepositService = directDepositService;
        }
       
        public string AccountNumber { get; set; }
        public string TransitNumber { get; set; }
        public string BankNumberWithName { get; set; }
        
        public DirectDepositInfo directDepositInfo { get; set; }
        public IMvxAsyncCommand SelectAuthoriseDepositFundCommand { get; }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        private async Task ExecuteSelectAuthoriseDepositFundCommand()
        {
            IsAuthorisedDepositFund = !IsAuthorisedDepositFund;
        }

        public override void Prepare(DirectDepositViewModelParameters parameter)
        {
            if (parameter != null && parameter.DirectDepositInfo != null)
            {
                directDepositInfo = parameter.DirectDepositInfo;

                AccountNumber = parameter.DirectDepositInfo.AccountNumber;
                TransitNumber = parameter.DirectDepositInfo.TransitNumber;

                if (!string.IsNullOrEmpty(parameter.DirectDepositInfo.BankNumber) && !string.IsNullOrEmpty(parameter.DirectDepositInfo.BankName))
                {
                    BankNumberWithName = string.Join(" - ", parameter.DirectDepositInfo.BankNumber, parameter.DirectDepositInfo.BankName);
                }
                else if (!string.IsNullOrEmpty(parameter.DirectDepositInfo.BankNumber))
                {
                    //BankNumberWithName would be only Bank Number in the case when this navigate from DD Error Page
                    BankNumberWithName = parameter.DirectDepositInfo.BankNumber;
                }

               
            }
        }

        private async Task ExecuteContinueCommand()
        {
            //TODo  Navigation on Result Page.
            //TODo  Implement Code when there is an error response.
            if (IsAuthorisedDepositFund)
            {
                try
                {
                    Dialogs.ShowLoading(Resource.Loading);

                    var directDepositBankDetailResponse =
                        await _directDepositService.SubmitDirectDepositDetails(_loginservice.GroupPlanNumber,                        //_directDepositInfo);
                                directDepositInfo);
                    if (directDepositBankDetailResponse != null)
                    {
                        await ShowViewModel<DirectDepositResultsViewModel>();
                    }
                    else
                    {
                      await Dialogs.AlertAsync(Resource.GenericErrorDialogMessage);
                    }
                }
                catch (Exception ex)
                {
                    Dialogs.HideLoading();
                    Console.WriteLine(ex.Message);
                    await Dialogs.AlertAsync(ex.Message);
                }
                finally
                {
                    Dialogs.HideLoading();
                }
            }
        }

        private async Task ExecuteChangeInformationCommand()
        {
            // If you use iOS, it goes back to the same instance of Direct Deposit View Model, but if you use
            // Android, it creates a new DirectDeposit ViewModel and Fragment, and so it doesn't retain the 
            // Direct deposit values, so we can only go back as there is no easy way to close two ViewModels at once.
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
                //Navigate to Direct Deposit Feature Page.
                await ShowViewModel<DirectDepositViewModel>();
            else
                GoBack();
        }
    }
}
