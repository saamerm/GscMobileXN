using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Validators;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DirectDepositViewModel : ViewModelBase
    {
        private readonly IDirectDepositService _directDepositService;
        private readonly IDeviceService _deviceService;
        private readonly DirectDepositTransitNumberValidator _directDepositTransitNumberValidator;
        private readonly DirectDepositBankNumberValidator _directDepositBankNumberValidator;
        private readonly DirectDepositAccountNumberValidator _directDepositAccountNumberValidator;
        private DirectDepositInfo _directDepositInfo;

        private string _transitNumber;
        private string _bankNumber;
        private string _accountNumber;
        private bool _isDirectDepositAuthorized;
        private bool _isEnrolledForEmailNotification;
        private bool _isDirectDepositThroughEngine;
        private bool? _isStep1Completed;
        private bool? _isStep2Completed;
        private bool? _isStep3Completed;
        private bool? _isBankDetailsValid;
        private readonly IUserDialogs _userDialogs;

        public string Title => Resource.DirectDeposit;
        public string AndroidTitle => Resource.DirectDeposit;
        public string SubTitle => Resource.DirectDepositNavbarSubtitle1;
        public string SubTitle2 => Resource.DirectDepositNavbarSubtitle2;

        public string Step => Resource.Step.ToUpperInvariant();
        public string Step1 => $"{Step} 1:";
        public string Step1Title => Resource.Authorization;
        public string OptInForDirectDeposit => BrandResource.DirectDepositOptInMessage;
        public string OptOutOfDirectDeposit => Resource.DirectDepositOptOutMessage;

        public string Step2 => $"{Step} 2:";
        public string Step2Title => Resource.DirectDepositBankingInfoTitle;
        public string EnterBankingInfoMessage => Resource.DirectDepositBankingInfoMessage;
        public string TransitNumberTitle => Resource.DirectDepositTransitNumber;
        public string BankNumberTitle => Resource.DirectDepositBankNumber;
        public string AccountNumberTitle => Resource.DirectDepositAccountNumber;
        public string SaveAndContinueTitle => Resource.SaveAndContinue;

        public string Step3 => $"{Step} 3:";
        public string Step3Title => Resource.DirectDepositEmailPreferencesTitle;
        public string OptInForEmailNotification => Resource.DirectDepositReceiveEmailNotification;
        public string OptOutOfEmailNotification => Resource.DirectDepositDoNotReceiveEmailNotification;

        public string DisclaimerTitle => Resource.DirectDepositFinePrints;
        public string DiscalimerNote1 => Resource.DirectDepositDisclaimer1;
        public string DiscalimerNote2 => Resource.DirectDepositDisclaimer2;
        public string DiscalimerNote3 => Resource.DirectDepositDisclaimer3;
        public string DiscalimerNote3B => Resource.DirectDepositDisclaimer3B;

        public string ContinueButtonTitle => Resource.Continue.ToUpperInvariant();
        /// <summary>
        /// Used by Android to determine whether the user has already completed their Direct deposit before
        /// in order to expand all sections
        /// </summary>
        private bool _shouldExpandAllSections;
        public bool ShouldExpandAllSections
        {
            get => _shouldExpandAllSections;
            set => SetProperty(ref _shouldExpandAllSections, value);
        }

        public bool? IsStep1Completed
        {
            get => _isStep1Completed;
            set => SetProperty(ref _isStep1Completed, value);
        }

        public bool? IsStep2Completed
        {
            get => _isStep2Completed;
            set => SetProperty(ref _isStep2Completed, value);
        }

        public bool? IsBankDetailsValid
        {
            get => _isBankDetailsValid;
            set => SetProperty(ref _isBankDetailsValid, value);
        }

        public bool? IsStep3Completed
        {
            get => _isStep3Completed;
            set => SetProperty(ref _isStep3Completed, value);
        }

        // Used by Android
        public bool IsDirectDepositAuthorized
        {
            get => _isDirectDepositAuthorized;
            set
            {
                SetProperty(ref _isDirectDepositAuthorized, value);
                IsStep1Completed = _isDirectDepositAuthorized;
            }
        }

        public bool IsEnrolledForEmailNotification
        {
            get => _isEnrolledForEmailNotification;
            set
            {
                SetProperty(ref _isEnrolledForEmailNotification, value);
                IsStep3Completed = _isEnrolledForEmailNotification;
            }
        }

        public bool IsDirectDepositThroughEngine
        {
            get => _isDirectDepositThroughEngine;
            set => SetProperty(ref _isDirectDepositThroughEngine, value);
        }

        public string TransitNumber
        {
            get => _transitNumber;
            set
            {
                SetProperty(ref _transitNumber, value);

                var validationResult = _directDepositTransitNumberValidator.Validate(_transitNumber);
                IsTransitNumberValid = validationResult.IsValid;
                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidTransitNumberValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidTransitNumberValidation));

                TransitNumberErrorText = validationErros?.ErrorMessage;
            }
        }

        public string BankNumber
        {
            get => _bankNumber;
            set
            {
                SetProperty(ref _bankNumber, value);

                var validationResult = _directDepositBankNumberValidator.Validate(_bankNumber);
                IsBankNumberValid = validationResult.IsValid;
                // TODO: Fix AC for validation PBI for direct deposit. We need two messages 1) empty field 2) invalid value
                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidBankNumberValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidBankNumberValidation));

                BankNumberErrorText = validationErros?.ErrorMessage;
            }
        }

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                SetProperty(ref _accountNumber, value);
                var validationResult = _directDepositAccountNumberValidator.Validate(_accountNumber);
                IsAccountNumberValid = validationResult.IsValid;

                var validationErros = validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAccountNumberValidation))
                    ?? validationResult.Errors.FirstOrDefault(x => string.Equals(x.ErrorMessage, Resource.InvalidAccountNumberValidation));
                AccountNumberErrorText = validationErros?.ErrorMessage;
            }
        }

        private bool? _isTransitNumberValid;
        public bool? IsTransitNumberValid
        {
            get => _isTransitNumberValid;
            set => SetProperty(ref _isTransitNumberValid, value);
        }

        private bool? _isBankNumberValid;
        public bool? IsBankNumberValid
        {
            get => _isBankNumberValid;
            set => SetProperty(ref _isBankNumberValid, value);
        }

        private bool? _isAccountNumberValid;
        public bool? IsAccountNumberValid
        {
            get => _isAccountNumberValid;
            set => SetProperty(ref _isAccountNumberValid, value);
        }

        private string _transitNumberErrorText;
        public string TransitNumberErrorText
        {
            get => _transitNumberErrorText;
            set => SetProperty(ref _transitNumberErrorText, value);
        }

        private string _bankNumberErrorText;
        public string BankNumberErrorText
        {
            get => _bankNumberErrorText;
            set => SetProperty(ref _bankNumberErrorText, value);
        }

        public string _accountNumberErrorText;
        public string AccountNumberErrorText
        {
            get => _accountNumberErrorText;
            set => SetProperty(ref _accountNumberErrorText, value);
        }

        public string BankName { get; set; }

        public DirectDepositStep1Model Step1Model { get; set; }

        private DirectDepositStep2Model _step2Model;
        public DirectDepositStep2Model Step2Model
        {

            get => _step2Model;
            set
            {
                SetProperty(ref _step2Model, value);
            }
        }


        public DirectDepositStep3Model Step3Model { get; set; }

        public ObservableCollection<DirectDepositStep> Steps { get; set; }

        public IMvxAsyncCommand SelectAuthorizeDirectDepositCommand { get; }

        public IMvxAsyncCommand ValidateAndSaveBankingInfoCommand { get; }

        public IMvxAsyncCommand SelectReceiveNotificationCommand { get; }

        public IMvxAsyncCommand ContinueCommand { get; set; }

        public DirectDepositViewModel(IDirectDepositService directDepositService, IUserDialogs userDialogs, IDeviceService deviceService)
        {
            _deviceService = deviceService;
            _directDepositService = directDepositService;
            _userDialogs = userDialogs;
            _directDepositBankNumberValidator = new DirectDepositBankNumberValidator();
            _directDepositAccountNumberValidator = new DirectDepositAccountNumberValidator();
            _directDepositTransitNumberValidator = new DirectDepositTransitNumberValidator();

            Steps = new ObservableCollection<DirectDepositStep>()
            {
                new DirectDepositStep() { IsStepCompleted = false, StepName = Step1Title, StepNumber = Step1 },
                new DirectDepositStep() { IsStepCompleted = false, StepName = Step2Title, StepNumber = Step2 },
                new DirectDepositStep() { IsStepCompleted = false, StepName = Step3Title, StepNumber = Step3 },
            };

            Step1Model = new DirectDepositStep1Model
            {
                OptInForDirectDeposit = OptInForDirectDeposit
            };

            Step2Model = new DirectDepositStep2Model()
            {
                EnterBankingInfoMessage = EnterBankingInfoMessage,
                TransitNumberTitle = TransitNumberTitle,
                BankNumberTitle = BankNumberTitle,
                AccountNumberTitle = AccountNumberTitle,
                SaveAndContinueTitle = SaveAndContinueTitle
            };

            Step3Model = new DirectDepositStep3Model()
            {
                OptInForEmailNotification = OptInForEmailNotification,
                OptOutOfEmailNotification = OptOutOfEmailNotification
            };

            SelectAuthorizeDirectDepositCommand = new MvxAsyncCommand(ExecuteSelectAuthorizeDirectDepositCommand);
            ValidateAndSaveBankingInfoCommand = new MvxAsyncCommand(ExecuteValidateAndSaveBankingInfoCommand);
            SelectReceiveNotificationCommand = new MvxAsyncCommand(ExecuteSelectReceiveNotificationCommand);
            ContinueCommand = new MvxAsyncCommand(ExecuteContinueCommand);
        }


        public override async Task Initialize()
        {
            await base.Initialize();

            Dialogs.ShowLoading(Resource.Loading);

            try
            {
                _directDepositInfo = await _directDepositService.GetDirectDepositInfoAsync(_loginservice.GroupPlanNumber);
                IsDirectDepositAuthorized = _directDepositInfo.IsDirectDepositAuthorized;
                IsEnrolledForEmailNotification = _directDepositInfo.IsEnrolledForEmailNotification;
                IsDirectDepositThroughEngine = _directDepositInfo.IsDirectDepositThroughEngine;
                BankNumber = _directDepositInfo.BankNumber;
                TransitNumber = _directDepositInfo.TransitNumber;
                AccountNumber = _directDepositInfo.AccountNumber;

                Step1Model.IsDirectDepositAuthorized = IsDirectDepositAuthorized;
                if (!string.IsNullOrEmpty(AccountNumber))
                {
                    Step2Model.AccountNumber = AccountNumber;
                }
                else
                {
                    Step2Model.AccountNumberErrorText = " ";
                }
                if (!string.IsNullOrEmpty(BankNumber))
                {
                    Step2Model.BankNumber = BankNumber;
                }
                else
                {
                    Step2Model.BankNumberErrorText = " ";
                }
                if (!string.IsNullOrEmpty(TransitNumber))
                {
                    Step2Model.TransitNumber = TransitNumber;
                }
                else
                {
                    Step2Model.TransitNumberErrorText = " ";
                }
                Step3Model.IsOptedForNotification = IsEnrolledForEmailNotification;

                Steps[1].IsStepCompleted = !string.IsNullOrWhiteSpace(AccountNumber)
                    && !string.IsNullOrWhiteSpace(BankNumber)
                    && !string.IsNullOrWhiteSpace(TransitNumber);
                if (_deviceService.CurrentDevice == GSCHelper.OS.Droid)
                {
                    Steps[1].IsStepCompleted = Steps[1].IsStepCompleted && IsAccountNumberValid.Value && IsBankNumberValid.Value && IsTransitNumberValid.Value;
                }
                Steps[0].IsStepCompleted = Step1Model.IsDirectDepositAuthorized && Steps[1].IsStepCompleted;
                Steps[2].IsStepCompleted = Step3Model.IsOptedForNotification && Steps[1].IsStepCompleted;
                if (_deviceService.CurrentDevice == GSCHelper.OS.Droid)
                {
                    Steps[2].IsStepCompleted = Steps[1].IsStepCompleted;
                }

                if (_directDepositInfo != null && IsTransitNumberValid.Value && IsBankNumberValid.Value && IsAccountNumberValid.Value)
                {
                    Steps[0].InvokeShouldExpandSectionEvent(true, 0);
                    Steps[1].InvokeShouldExpandSectionEvent(true, 1);
                    Steps[2].InvokeShouldExpandSectionEvent(true, 2);
                    ShouldExpandAllSections = true;
                }
                else
                {
                    Steps[0].InvokeShouldExpandSectionEvent(true, 0);
                }

                if (IsDirectDepositAuthorized)
                {
                    IsStep1Completed = IsDirectDepositAuthorized;
                    IsStep2Completed = IsDirectDepositAuthorized;
                    IsStep3Completed = IsDirectDepositAuthorized;
                    Steps[0].IsStepCompleted = IsDirectDepositAuthorized;
                    Steps[1].IsStepCompleted = IsDirectDepositAuthorized;
                    Steps[2].IsStepCompleted = IsDirectDepositAuthorized;
                }
                else
                {
                    IsStep1Completed = null;
                    IsStep2Completed = null;
                    IsStep3Completed = null;
                }
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await LogoutWithAlertMessageAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Dialogs.HideLoading();
            }
            finally
            {
                Dialogs.HideLoading();
            }
        }

        private async Task ExecuteSelectAuthorizeDirectDepositCommand()
        {
            Step1Model.IsDirectDepositAuthorized = !Step1Model.IsDirectDepositAuthorized || true;
            Steps[0].IsStepCompleted = true;
            IsDirectDepositAuthorized = Step1Model.IsDirectDepositAuthorized;
            await SetStepAsCompleted(0);
        }

        private async Task ExecuteValidateAndSaveBankingInfoCommand()
        {
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                IsTransitNumberValid = Step2Model.TransitNumberValid;
                IsBankNumberValid = Step2Model.BankNumberValid;
                IsAccountNumberValid = Step2Model.AccountNumberValid;
            }
            if (IsTransitNumberValid.Value && IsBankNumberValid.Value && IsAccountNumberValid.Value)
            {
                // Only Client Side Validation after discuss with Team.                          
                Steps[1].IsStepCompleted = true;
                Steps[2].IsStepCompleted = true;
                await SetStepAsCompleted(1);
            }
        }

        private async Task ExecuteSelectReceiveNotificationCommand()
        {
            Step3Model.IsOptedForNotification = !Step3Model.IsOptedForNotification;
            Steps[2].IsStepCompleted = true;
        }

        private async Task ExecuteContinueCommand()
        {
            Dialogs.ShowLoading(Resource.Loading);
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                IsTransitNumberValid = Step2Model.TransitNumberValid;
                IsBankNumberValid = Step2Model.BankNumberValid;
                IsAccountNumberValid = Step2Model.AccountNumberValid;
            }
            if (Steps[0].IsStepCompleted && Steps[1].IsStepCompleted && Steps[2].IsStepCompleted &&
                IsTransitNumberValid.Value && IsBankNumberValid.Value &&  IsAccountNumberValid.Value)
            {
                try
                {
                   
                    if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
                    {
                        TransitNumber = Step2Model.TransitNumber;
                        BankNumber = Step2Model.BankNumber;
                        AccountNumber = Step2Model.AccountNumber;
                    }
                    var directDepositBankDetails =
                        await _directDepositService.ValidateUserBankDetails(_loginservice.GroupPlanNumber,                        //_directDepositInfo);
                            new BankValidationRequest(long.Parse(_loginservice.GroupPlanNumber), BankNumber, TransitNumber));
                    IsBankDetailsValid = directDepositBankDetails?.IsValidBankInfo;

                    if (IsBankDetailsValid is true)
                    {
                        BankName = directDepositBankDetails.BankName;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                
                //Prepare Object to pass through Parameter
                DirectDepositInfo _ddinfo = new DirectDepositInfo();
                _ddinfo.AccountNumber = AccountNumber;
                _ddinfo.BankNumber = BankNumber;
                _ddinfo.TransitNumber = TransitNumber;
                _ddinfo.IsDirectDepositAuthorized = IsDirectDepositAuthorized;
                _ddinfo.IsEnrolledForEmailNotification = Step3Model.IsOptedForNotification;

                if (IsBankDetailsValid is false)
                {
                    //Load Failure View - Bank Number and Transit Number is not correct according to validation api
                    //Construct ViewModel for the failure view and pass DirectDepositInfo as parameter
                    await ShowViewModel<DDValidationErrorViewModel, DirectDepositViewModelParameters>(new DirectDepositViewModelParameters(_ddinfo));

                }
                else
                {
                    _ddinfo.BankName = BankName;
                    // Load Success View - Bank Number and Transit Number is correct according to validation api
                    // Construct ViewModel for the Success view  pass DirectDepositInfo as parameter               
                    await ShowViewModel<DirectDepositConfirmationViewModel, DirectDepositViewModelParameters>(new DirectDepositViewModelParameters(_ddinfo));
                }
            }
            Dialogs.HideLoading();
        }

        private async Task SetStepAsCompleted(int index)
        {
            Dialogs.ShowLoading(Resource.Loading);

            if (index == 0)
            {
                Steps[0].InvokeShouldExpandSectionEvent(false, 0);
                Steps[1].InvokeShouldExpandSectionEvent(true, 1);
                Steps[2].InvokeShouldExpandSectionEvent(false, 2);
            }
            else if (index == 1)
            {
                Steps[0].InvokeShouldExpandSectionEvent(false, 0);
                Steps[1].InvokeShouldExpandSectionEvent(false, 1);
                Steps[2].InvokeShouldExpandSectionEvent(true, 2);
            }
            else if (index == 2)
            {
                Steps[0].InvokeShouldExpandSectionEvent(false, 0);
                Steps[1].InvokeShouldExpandSectionEvent(false, 1);
                Steps[2].InvokeShouldExpandSectionEvent(true, 2);
            }
            Dialogs.HideLoading();
        }
    }
}