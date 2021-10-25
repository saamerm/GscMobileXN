using System;
using System.Linq;
using MobileClaims.Core.Validators;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DirectDepositStep2Model : MvxNotifyPropertyChanged
    {
        private string _transitNumber;
        private string _bankNumber;
        private string _accountNumber;

        private bool _transitNumberValid;
        private bool _bankNumberValid;
        private bool _accountNumberValid;

        private string _transitNumberErrorText;
        private string _accountNumberErrorText;
        private string _bankNumberErrorText;

        private readonly DirectDepositTransitNumberValidator _directDepositTransitNumberValidator;
        private readonly DirectDepositBankNumberValidator _directDepositBankNumberValidator;
        private readonly DirectDepositAccountNumberValidator _directDepositAccountNumberValidator;

        public DirectDepositStep2Model()
        {
            _directDepositTransitNumberValidator = new DirectDepositTransitNumberValidator();
            _directDepositBankNumberValidator = new DirectDepositBankNumberValidator();
            _directDepositAccountNumberValidator = new DirectDepositAccountNumberValidator();
        }
        public string EnterBankingInfoMessage { get; set; }

        public string TransitNumberTitle { get; set; }

        public string BankNumberTitle { get; set; }

        public string AccountNumberTitle { get; set; }

        public string SaveAndContinueTitle { get; set; }

        public string TransitNumber
        {
            get => _transitNumber;
            set
            {
                SetProperty(ref _transitNumber, value);
                var validationResult = _directDepositTransitNumberValidator.Validate(_transitNumber);
                TransitNumberValid = validationResult.IsValid;

                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidTransitNumberValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidTransitNumberValidation));
                TransitNumberErrorText = validationErros?.ErrorMessage ?? " ";
            }
        }

        public bool TransitNumberValid
        {
            get => _transitNumberValid;
            set => SetProperty(ref _transitNumberValid, value);
        }

        public string TransitNumberErrorText
        {
            get => _transitNumberErrorText;
            set => SetProperty(ref _transitNumberErrorText, value);
        }

        public string BankNumber
        {
            get => _bankNumber;
            set
            {
                SetProperty(ref _bankNumber, value);

                var validationResult = _directDepositBankNumberValidator.Validate(_bankNumber);
                BankNumberValid = validationResult.IsValid;

                // TODO: Fix AC for validation PBI for direct deposit. We need two messages 1) empty field 2) invalid value
                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidBankNumberValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidBankNumberValidation));
                BankNumberErrorText = validationErros?.ErrorMessage ?? " ";

            }

        }

        public bool BankNumberValid
        {
            get => _bankNumberValid;
            set => SetProperty(ref _bankNumberValid, value);
        }

        public string BankNumberErrorText
        {
            get => _bankNumberErrorText;
            set => SetProperty(ref _bankNumberErrorText, value);
        }

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                SetProperty(ref _accountNumber, value);

                var validationResult = _directDepositAccountNumberValidator.Validate(_accountNumber);
                AccountNumberValid = validationResult.IsValid;

                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAccountNumberValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAccountNumberValidation));
                AccountNumberErrorText = validationErros?.ErrorMessage ?? " ";
            }
        }

        public bool AccountNumberValid
        {
            get => _accountNumberValid;
            set => SetProperty(ref _accountNumberValid, value);
        }
        public string AccountNumberErrorText
        {
            get => _accountNumberErrorText;
            set => SetProperty(ref _accountNumberErrorText, value);
        }
    }
}
