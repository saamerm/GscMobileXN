using System;
using System.Threading.Tasks;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DDValidationErrorViewModel : ViewModelBase<DirectDepositViewModelParameters>
    {
        private readonly IDeviceService _deviceService;
        private DirectDepositViewModelParameters _parameter;
        public string Title { get; set; }
        public string AndroidTitle => Resource.DirectDeposit;
        public string SubTitle => Resource.DDValidationErrWarning;
        public string Warningmsg => Resource.DDValidationErrWarning;
        public string ErrorTitle => Resource.DDValidationError;
        public string ErrNote => Resource.DDValidationErrNote;
        public string ErrNoteContact => Resource.DDValidationErrNoteContact;
        public string ErrNoteContinue => Resource.DDValidationErrNoteContinue;
        public string ErrExplaination => Resource.DDValidationErrExplaination;
        public string ErrBankInfo => Resource.DDValidationErrBankInfo;
        public string TransitNumberTitle => Resource.DirectDepositTransitNumber;
        public string BankNumberTitle => Resource.DirectDepositBankNumber;
        public string AccountNumberTitle => Resource.DirectDepositAccountNumber;
        public string ContinueTitle => Resource.Continue.ToUpperInvariant();
        public string ChangeInformationTitle => Resource.ChangeInformationTitle.ToUpperInvariant();

        public string ErrorTitle1 => Resource.DDValidationErrorPart1;
        public string ErrorTitle2 => Resource.DDValidationErrorPart2;
        public string ErrorTitle3 => Resource.DDValidationErrorPart3;
        public string ErrorTitle4 => Resource.DDValidationErrorPart4;
        public string ErrorTitle5 => Resource.DDValidationErrorPart5;

        public string ErrNoteContinue1 => Resource.DDClickLabel;
        public string ErrNoteContinue3 => Resource.DDNoteConfirmation;
        public string ErrNoteContinue4 => Resource.DDChangeInformationLabel; 

        /// <summary>
        /// For Android
        /// </summary>
        public string ErrorParagraph => ErrNote + "\n\n" + ErrNoteContact + "\n\n" + ErrNoteContinue;

        public IMvxAsyncCommand ContinueCommand { get; set; }
        public IMvxAsyncCommand ChangeInformationCommand { get; set; }

        public DDValidationErrorViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;

            ContinueCommand = new MvxAsyncCommand(ExecuteContinueCommand);
            ChangeInformationCommand = new MvxAsyncCommand(ExecuteChangeInformationCommand);
        }

        public string AccountNumber { get; set; }
        public string BankNumber { get; set; }
        public string TransitNumber { get; set; }


        public override void Prepare(DirectDepositViewModelParameters parameter)
        {
            _parameter = parameter;
            if (parameter != null &&  parameter.DirectDepositInfo != null)
            {
                AccountNumber = parameter.DirectDepositInfo.AccountNumber;
                BankNumber = parameter.DirectDepositInfo.BankNumber;
                TransitNumber = parameter.DirectDepositInfo.TransitNumber;
            }
        }

        private async Task ExecuteContinueCommand()
        {
            // Navigation to Confirmation Page.
            await ShowViewModel<DirectDepositConfirmationViewModel, DirectDepositViewModelParameters>(_parameter);
        }

        private async Task ExecuteChangeInformationCommand()
        {
            //Go back doesnt work with iOS, so we need to use ShowViewModel
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
                //Navigate to Direct Deposit Feature Page.
                await ShowViewModel<DirectDepositViewModel>();
            else
                GoBack();
        }
    }
}
