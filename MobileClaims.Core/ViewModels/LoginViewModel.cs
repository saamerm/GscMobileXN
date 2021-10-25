using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModelParameters;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using MobileClaims.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using Plugin.Fingerprint.Abstractions;

namespace MobileClaims.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private const string OBFUSCATED_USER_ID = "********";

        public event EventHandler UpdateRequired;
        public event EventHandler ShowBiometricLoginEvent;

        private readonly IDataService _dataservice;
        private readonly IAppUpgradeService _appUpgradeService;

        // This is not used in this viewmodel, however shome how it is reinitialized if not resolved here, before dashboard viewmodel load.
        // TODO: Fix this order in which the participant service need to be resolved. 
        private readonly IParticipantService _participantService;

        private bool _didDisplayUpdateDialog;
        private bool newUser;
        private readonly bool HaveCard;
        private readonly bool HaveMember;

        private bool _showForceUpgradeDialog;
        private bool _showGenericErrorMessage;
        private bool _showLoginFields = true;
        private bool _hideTouchLogin = true;
        private bool _rememberme;
        private bool _loggedInBefore;
        private bool _loggingin;

        private string _clearUserName;
        private string _probablyWrongPassword;
        private string _loginFailedMessage;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken => _cancellationTokenSource.Token;
#if DEBUG
        private string _username= "leohammond";
        private string _password = "Aa12345678";
#else
        private string _password;
        private string _username;
#endif

        private MvxSubscriptionToken _loggedintoken;
        private MvxSubscriptionToken _authenticationerrortoken;
        private MvxSubscriptionToken _connectionissuestoken;

        public string UserNamePlaceholderText => BrandResource.Username;
        public string PasswordPlaceholderText => BrandResource.Password;
        public string MyIdCardText => BrandResource.MyIdCardText;
        public string LoginText => BrandResource.Login;
        public string RememberMeText => BrandResource.RememberMe;
        public string Ok => Resource.ok;
        public string Cancel => Resource.Cancel;
        public string ForceUpdateTitle => Resource.ForceUpdateTitle;
        public string UpdateAvailableMessage => Resource.UpdateAvailableMessage;
        public string UpdateAvailableMessage2 => Resource.UpdateAvailableMessage2;
        public string ConnectionError => Resource.ConnectionError;
        public string LoginErrorText => Resource.LoginError;
        public string PasswordChangedErrorTouchId => Resource.PasswordChangedErrorTouchId;
        public string PasswordChangedErrorFaceId => Resource.PasswordChangedErrorFaceId;
        public string PasswordChangedErrorFingerprint => Resource.PasswordChangedErrorFingerprint;

        public bool ShowForceUpgradeDialog
        {
            get => _showForceUpgradeDialog;
            set => SetProperty(ref _showForceUpgradeDialog, value);
        }

        public bool ShowGenericErrorMessage
        {
            get => _showGenericErrorMessage;
            set => SetProperty(ref _showGenericErrorMessage, value);
        }

        public bool ShowLoginFields
        {
            get => _showLoginFields;
            set
            {
                SetProperty(ref _showLoginFields, value);
                RaisePropertyChanged(() => ShowTouchLogin);
            }
        }

        public bool ShowTouchLogin => ShowLoginFields && !HideTouchLogin;

        public bool HideTouchLogin
        {
            get => _hideTouchLogin;
            set
            {
                SetProperty(ref _hideTouchLogin, value);
                RaisePropertyChanged(() => ShowTouchLogin);
            }
        }

        public bool ShouldExit { get; set; }

        public bool RememberMe
        {
            get => _rememberme;
            set => SetProperty(ref _rememberme, value);
        }

        public bool LoggedInBefore
        {
            get => _loggedInBefore;
            set => SetProperty(ref _loggedInBefore, value);
        }

        public bool UsingBiometricLogin { get; set; }

        public bool CanShowCard => HaveMember && HaveCard;

        public string UserName
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                if (_username != OBFUSCATED_USER_ID) _clearUserName = _username;
                LoginCommand.RaiseCanExecuteChanged();
                LoginForDroidCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
                LoginForDroidCommand.RaiseCanExecuteChanged();
            }
        }

        public string LoginFailedMessage
        {
            get => _loginFailedMessage;
            set => SetProperty(ref _loginFailedMessage, value);
        }

        public ClientValidationStatus ClientValidationStatus { get; set; }

        public IMvxCommand ShowIDCard { get; }
        public IMvxAsyncCommand LoginCommand { get; }
        public IMvxAsyncCommand LoginForDroidCommand { get; }
        public IMvxAsyncCommand BiometricLoginCommand { get; }

        public LoginViewModel(IMvxMessenger messenger,
            ILoginService loginService,
            IDataService dataService,
            IAppUpgradeService appUpgradeService,
            IUserDialogs userDialogs,
            IParticipantService participantService)
            : base(messenger, loginService, userDialogs)
        {
            _dataservice = dataService;
            _appUpgradeService = appUpgradeService;
            _participantService = participantService;
            _cancellationTokenSource = new CancellationTokenSource();

            ShouldExit = _loginservice.ShouldExit;

            var persistedUsername = _dataservice.GetUserName();
            if (!string.IsNullOrEmpty(persistedUsername))
            {
                _rememberme = true;
                _username = OBFUSCATED_USER_ID;
                _clearUserName = persistedUsername;
            }

            var idCard = _dataservice.GetIDCard();

            HaveMember = _dataservice.GetCardPlanMember() != null && !string.IsNullOrWhiteSpace(_dataservice.GetCardPlanMember().FullName);

            HaveCard = idCard != null
                       && !string.IsNullOrEmpty(idCard.FrontImageFilePath)
                       && !string.IsNullOrEmpty(idCard.BackImageFilePath)
                       && idCard.BackImageByteArray != null
                       && idCard.FrontImageByteArray != null;

            //Check for persisted card data here - if it exists, set LoggedInBefore to true
            if (idCard != null && !string.IsNullOrEmpty(idCard.PlanMemberFullName))
            {
                LoggedInBefore = true;
            }

            LoginCommand = new MvxAsyncCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            LoginForDroidCommand = new MvxAsyncCommand(ExecuteLoginForDroidCommand, CanExecuteLoginCommand);
            BiometricLoginCommand = new MvxAsyncCommand(ExecuteBiometricLogin, CanExecuteBiometricLogin);
            ShowIDCard = new MvxCommand(ExecuteShowIdCard, CanExecuteShowIdCard);
        }

        public override async void Start()
        {
            base.Start();
            try
            {
                if (!await CheckIfUpdateRequiredAsync())
                {
                    ShouldExit = true;
                    return;
                }

                if (RememberMe && await Mvx.IoCProvider.Resolve<IBiometricsService>().CanLoginWithBiometrics())
                {
                    HideTouchLogin = false;
                    ShowBiometricLoginEvent?.Invoke(this, EventArgs.Empty);
                }

                ShouldExit = false;
            }
            catch (Exception ex)
            {
                Dialogs.Alert(Resource.GenericErrorDialogMessage);
#if DEBUG
                Debug.WriteLine($"LoginViewModel Exception: {ex}");
                Crashes.TrackError(ex);
#endif
            }
        }

        private async Task<bool> CheckIfUpdateRequiredAsync()
        {
            bool canProceedWithLogin;
            try
            {
                // TODO: according to docs Acr.UserDialog.ShowLoading shouldn't be used before ViewDidLoad on iOS, else it'd throw an exception
                // Dialogs.ShowLoading(Resource.Loading);
                ClientValidationStatus = await _appUpgradeService.CheckIfUpdateRequiredAsync();
                switch (ClientValidationStatus)
                {
                    case ClientValidationStatus.UpdateRequired:
                        {
                            Dialogs.HideLoading();

                            ShowForceUpgradeDialog = true;
                            ShowGenericErrorMessage = false;
                            ShowLoginFields = false;

                            if (!_didDisplayUpdateDialog)
                            {
                                var okSelected = await Dialogs.ConfirmAsync(UpdateAvailableMessage, ForceUpdateTitle, Ok, Cancel);
                                _didDisplayUpdateDialog = true;

                                if (okSelected)
                                {
                                    UpdateRequired?.Invoke(this, new EventArgs());
                                }
                            }

                            canProceedWithLogin = false;
                            break;
                        }
                    case ClientValidationStatus.Error:
                    case ClientValidationStatus.InvalidClientError:
                    case ClientValidationStatus.NetworkError:
                        Dialogs.HideLoading();

                        ShowForceUpgradeDialog = false;
                        ShowGenericErrorMessage = true;
                        ShowLoginFields = false;
                        await Dialogs.AlertAsync(ConnectionError, string.Empty, Ok);

                        canProceedWithLogin = false;
                        break;
                    case ClientValidationStatus.UpdateNotRequired:
                    default:
                        ShowForceUpgradeDialog = false;
                        ShowGenericErrorMessage = false;
                        ShowLoginFields = true;
                        canProceedWithLogin = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                canProceedWithLogin = false;
#if DEBUG
                Debug.WriteLine($"LoginViewModel Exception: {ex}");
                Crashes.TrackError(ex);
#endif
            }

            return canProceedWithLogin;
        }

        private bool CanExecuteLoginCommand()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        private bool CanExecuteBiometricLogin()
        {
            if (ShouldExit)
            {
                return false;
            }
            var bioService = Mvx.IoCProvider.Resolve<IBiometricsService>();
            var canUseBiometrics = bioService.CanLoginWithBiometrics();
            canUseBiometrics.Wait(_cancellationToken);
            return canUseBiometrics.Result;
        }

        private bool CanExecuteShowIdCard()
        {
            return LoggedInBefore;
        }

        private async Task ExecuteLoginCommand()
        {
            await DoExecuteLoginCommand(SuccessfulLoginHandlerAsync);
        }

        private async Task ExecuteLoginForDroidCommand()
        {
            await DoExecuteLoginCommand(DroidSuccessfulLoginHandlerAsync);
        }

        private async Task DoExecuteLoginCommand(Func<Task> successHandler)
        {
            try
            {
                newUser = !string.Equals(_dataservice.GetLastUserToLogin(), _clearUserName);
                if (newUser)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                }

                Dialogs.ShowLoading(Resource.Loading);
                await _loginservice.AuthorizeCredentialsAsync(_clearUserName, Password);
                await successHandler.Invoke();
            }
            catch (UnauthorizedAccessException ex)
            {
                AuthenticationFailedHandler(ex);
            }
            catch (Exception ex) when (ex.InnerException is WebException || ex is WebException)
            {
                ConnectionFailedHandler();
            }
            catch (Exception)
            {
                // To maintain backward compatibility
                AuthenticationFailedHandler();
            }
            finally
            {
                Dialogs.HideLoading();
            }
        }

        private async Task ExecuteBiometricLogin()
        {
            try
            {
                var bioService = Mvx.IoCProvider.Resolve<IBiometricsService>();
                var password = await bioService.GetPasswordFromKeychain(_cancellationToken);

                Dialogs.ShowLoading(Resource.Loading);
                if (!string.IsNullOrEmpty(password))
                {
                    Password = password;
                    UsingBiometricLogin = true;
                    var currentDevice = Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice;
                    if (currentDevice == GSCHelper.OS.iOS)
                    {
                        LoginCommand.Execute(null);
                    }
                    else if (currentDevice == GSCHelper.OS.Droid)
                    {
                        LoginForDroidCommand.Execute(null);
                    }
                }
            }
            finally
            {
                Dialogs.HideLoading();
            }
        }

        private void ExecuteShowIdCard()
        {
            ShowViewModel<CardViewModel, CardViewModelParameter>(new CardViewModelParameter(true));
        }

        private async Task SuccessfulLoginHandlerAsync()
        {
            try
            {
                Dialogs.ShowLoading(Resource.Loading);

                bool? confirm = null;
                bool? iosSuccess = null;

                if (RememberMe)
                {
                    //save the username
                    _dataservice.PersistUserName(_clearUserName);
                    var bioService = Mvx.IoCProvider.Resolve<IBiometricsService>();

                    if (!string.IsNullOrWhiteSpace(_probablyWrongPassword) && !_probablyWrongPassword.Equals(Password))
                    {
                        UsingBiometricLogin = false;
                    }

                    // The biometrics setting will be null if the user has not chosen to use or deny touchID/faceID
                    if (!UsingBiometricLogin
                        && (!_dataservice.GetUseBiometricsSetting().HasValue
                            || _dataservice.GetUseBiometricsSetting() == true)
                        && await bioService.BiometricsAvailable())
                    {
                        // Confirm with the user to use biometrics to save
                        var confirmConfig = new ConfirmConfig
                        {
                            Message = bioService.GetConfirmMessage(),
                            OkText = Resource.Use,
                            CancelText = Resource.DontUse
                        };
                        Dialogs.HideLoading();
                        confirm = await Dialogs.ConfirmAsync(confirmConfig);
                        if (confirm.HasValue && confirm.Value)
                        {
                            iosSuccess = await bioService.SavePasswordToKeychain(Password, _cancellationToken);
                            Dialogs.ShowLoading(Resource.Loading);
                            _dataservice.PersistUseBiometricsSetting(iosSuccess.Value);
                        }
                        else
                        {
                            // Change the setting so that we don't ask again
                            _dataservice.PersistUseBiometricsSetting(false);
                        }
                    }
                }
                else
                {
                    //remove persisted username
                    _dataservice.PersistUserName(string.Empty);
                    Mvx.IoCProvider.Resolve<IBiometricsService>().RemoveStoredCredentials();
                }

                UsingBiometricLogin = false;

                if (iosSuccess.HasValue && iosSuccess.Value || !iosSuccess.HasValue)
                {
                    await ShowViewModel<MainNavigationViewModel>();
                    await ShowViewModel<DashboardViewModel>();
                }
            }
            catch (Exception)
            {
                Dialogs.HideLoading();
                Dialogs.Alert(Resource.GenericErrorDialogMessage);
            }
            finally
            {
                Dialogs.HideLoading();
            }
        }

        private async Task DroidSuccessfulLoginHandlerAsync()
        {
            try
            {
                Dialogs.ShowLoading(Resource.Loading);

                bool? success = null;
                if (RememberMe)
                {
                    //save the username
                    _dataservice.PersistUserName(_clearUserName);
                    var bioService = Mvx.IoCProvider.Resolve<IBiometricsService>();

                    // Reset _usingBiometricLogin when _probablyWrongPassword and Password are not equal
                    // TODO: Remove the following condition
                    // Following condition might not ever gets executed because _probablyWrongPassword is
                    // only set by AuthenticationFailedHandler.
                    // In this method _probablyWrongPassword and Password are always going to be unequal 
                    if (!string.IsNullOrWhiteSpace(_probablyWrongPassword) && !_probablyWrongPassword.Equals(Password))
                    {
                        UsingBiometricLogin = false;
                    }

                    if (!UsingBiometricLogin && _dataservice.GetUseBiometricsSetting() != false
                                             && await bioService.BiometricsAvailable())
                    {
                        success = await bioService.SavePasswordToKeychain(Password, _cancellationToken);
                        _dataservice.PersistUseBiometricsSetting(success.Value);
                    }
                }
                else
                {
                    //remove persisted username
                    _dataservice.PersistUserName(string.Empty);
                    Mvx.IoCProvider.Resolve<IBiometricsService>().RemoveStoredCredentials();
                }

                UsingBiometricLogin = false;

                if (success.HasValue && success.Value || success.HasValue == false)
                {
                    GoBackCommand.Execute(null);
                    await ShowViewModel<DashboardViewModel>();
                }
            }
            catch (Exception ex)
            {
                Dialogs.HideLoading();
                Dialogs.Alert(Resource.GenericErrorDialogMessage);
            }
            finally
            {
                Dialogs.HideLoading();
            }
        }

        private void ConnectionFailedHandler()
        {
            Dialogs.HideLoading();
            Dialogs.Alert(ConnectionError, null, Ok);
        }

        private void AuthenticationFailedHandler(UnauthorizedAccessException ex = null)
        {
            _probablyWrongPassword = this.Password;

            Dialogs.HideLoading();

            if (!isInBackground)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                {
                    Dialogs.Alert(ex.Message, string.Empty, Ok);
                }
                else
                {
                    Dialogs.Alert(LoginFailedMessage, string.Empty, Ok);
                }
            }
        }

        public bool isInBackground { get; set; }
    }
}