using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MobileClaims.Core.ViewModels
{
    public class ViewModelBase<T> : ViewModelBase, IMvxViewModel<T>
    {
        private T _viewModelParameter;
        public T ViewModelParameter
        {
            get => _viewModelParameter;
            internal set => SetProperty(ref _viewModelParameter, value);
        }

        public virtual void Prepare(T parameter)
        {
            ViewModelParameter = parameter;
        }
    }

    public class ViewModelBase : MvxViewModel, IRemove
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IRehydrationService _rehydrationService;
        private readonly IMvxMessenger _messenger;
        private readonly IUserDialogs _userDialog;
        protected ILoginService _loginservice;
        protected IMvxLog Logger;

        protected MvxSubscriptionToken _loggedouttoken;
        private readonly MvxSubscriptionToken _requestnavtodruglookup;
        private readonly MvxSubscriptionToken _requestnavtolocateserviceprovider;
        private readonly MvxSubscriptionToken _requestnavtospendingaccounts;
        private MvxSubscriptionToken _androidpresenterset;

        private readonly MvxBundle _requestedByBundle = new MvxBundle(new Dictionary<string, string>
        {
            { NavigationRequestTypes.RequestedBy, NavigationRequestTypes.Rehydration }
        });

        private readonly object _sync = new object();

        ///<summary>
        /// Adding the property to track the rehydration status on Android.  Android is particularly stubborn in this scenario
        /// </summary>
        public bool CreatedFromRehydration { get; set; }

        public IUserDialogs Dialogs { get; }

        public ViewModelBase()
        {
            _rehydrationService = Mvx.IoCProvider.Resolve<IRehydrationService>();
            _loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            Logger = Mvx.IoCProvider.Resolve<IMvxLog>();
            Dialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();

            _loggedouttoken = _messenger.Subscribe<LoggedOutMessage>(message =>
            {
                _messenger.Unsubscribe<LoggedOutMessage>(_loggedouttoken);
                ShowViewModel<LoginViewModel>();
            });

            var viewFinder = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            Type viewType = null;
            try
            {
                viewType = viewFinder.GetViewType(GetType());
            }
            catch (Exception ex)
            {
                Logger.TraceException($"Could not find view for viewmodel type {GetType().Name}", ex);
            }

            if (_rehydrationService.Rehydrating)
            {
                CreatedFromRehydration = true;
                return;
            }

            if (GetType().Name.StartsWith("Claim")
                && !GetType().Name.StartsWith("ClaimsHistory", StringComparison.OrdinalIgnoreCase)
                && !GetType().Name.StartsWith("ClaimSummary", StringComparison.OrdinalIgnoreCase)
                && viewType != null)
            {
                if (GetType().Name.Contains("ClaimSubmissionResultViewModel"))
                {
                    _rehydrationService.ClearData();
                    return;
                }

                if (GetType().Name.Contains("ClaimSubmissionTypeViewModel"))
                {
                    _rehydrationService.BusinessProcess.Clear();
                    _rehydrationService.ProcessEntryPoint = GetType().Name;
                }

                if (!GetType().Name.ToLower().StartsWith("claimtreatmentdetailsentry")
                    && !_rehydrationService.BusinessProcess.Contains(GetType())
                    && !GetType().Name.ToLower().Contains("termsandconditions")
                    && !GetType().Name.ToLower().Contains("ClaimServiceProviderProvideInformationViewModel".ToLower()))
                {
                    Logger.Trace(GetType().Name);
                    _rehydrationService.BusinessProcess.Add(GetType());
                    _rehydrationService.Save();
                }
            }
        }

        public ViewModelBase(IMvxMessenger messenger, ILoginService loginservice)
        {
            _loginservice = loginservice;
            _messenger = messenger;
            _userDialog = Mvx.IoCProvider.Resolve<IUserDialogs>();

            Logger = Mvx.IoCProvider.Resolve<IMvxLog>();
            _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

            _loggedouttoken = _messenger.Subscribe<LoggedOutMessage>(message =>
            {
                Unsubscribe();
                ShowViewModel<LoginViewModel>();
            });

            _requestnavtodruglookup = _messenger.Subscribe<RequestNavToDrugLookup>(message =>
            {
                Unsubscribe();
                HandleNavigateAwayRequest();
            });

            _requestnavtolocateserviceprovider = _messenger.Subscribe<RequestNavToLocateServiceProvider>(message =>
            {
                Unsubscribe();
                HandleNavigateAwayRequest();
            });


            _requestnavtospendingaccounts = _messenger.Subscribe<RequestNavToSpendingAccounts>(message =>
            {
                Unsubscribe();
                HandleNavigateAwayRequest();
            });
        }

        public ViewModelBase(IMvxMessenger messenger, ILoginService loginservice, IUserDialogs userDialogs)
        {
            _loginservice = loginservice;
            _messenger = messenger;
            Dialogs = userDialogs;

            Logger = Mvx.IoCProvider.Resolve<IMvxLog>();
            _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

            _loggedouttoken = _messenger.Subscribe<LoggedOutMessage>(message =>
            {
                Unsubscribe();
                ShowViewModel<LoginViewModel>();
            });

            _requestnavtodruglookup = _messenger.Subscribe<RequestNavToDrugLookup>(message =>
            {
                Unsubscribe();
                HandleNavigateAwayRequest();
            });

            _requestnavtolocateserviceprovider = _messenger.Subscribe<RequestNavToLocateServiceProvider>(message =>
            {
                Unsubscribe();
                HandleNavigateAwayRequest();
            });

            _requestnavtospendingaccounts = _messenger.Subscribe<RequestNavToSpendingAccounts>(message =>
            {
                Unsubscribe();
                HandleNavigateAwayRequest();
            });
        }

        public void Init(bool rehydrating)
        {
            if (rehydrating)
            {
            }
        }

        /// <summary>
        /// Pops all the elements from the existing stack in the Rehydration service back onto the nav stack.  Assumes the dependent view models either rely on a Service for state management (like the claims
        /// process) or they have logic for rehydrating their state from local storage some other way
        /// </summary>
        public async Task RehydrateProcess()
        {
            //Rebuild the nav stack for the last business process that was in process
            // ShowViewModel(typeof(MainNavigationViewModel), new Dictionary<string, string>(), null, new MvxRequestedBy(MvxRequestedByType.Other, "Rehydration"));

            await ShowViewModel<MainNavigationViewModel>(_requestedByBundle);

            for (int i = 0; i < _rehydrationService.BusinessProcess.Count(); i++)
            {
                if (_rehydrationService.BusinessProcess[i] != typeof(ClaimOtherBenefitsViewModel)
                    && _rehydrationService.BusinessProcess[i] != typeof(ClaimMotorVehicleViewModel)
                    && _rehydrationService.BusinessProcess[i] != typeof(ClaimWorkInjuryViewModel)
                    && _rehydrationService.BusinessProcess[i] != typeof(ClaimMedicalItemViewModel))
                    {
                    // Fix For - ClaimType getting NotDefined while user Resume the Drug Claim Submission and process hangs at end
                        if (_rehydrationService.BusinessProcess[i] == typeof(ClaimDocumentsUploadViewModel))
                        {
                            INonRealTimeClaimUploadProperties uploadable = (INonRealTimeClaimUploadProperties)NonRealTimeUploadFactory.Create(NonRealTimeClaimType.Drug, nameof(ClaimDocumentsUploadViewModel));
                            await ShowViewModel<ClaimDocumentsUploadViewModel, ClaimDocumentsUploadViewModelParameters>(new ClaimDocumentsUploadViewModelParameters(uploadable));
                        }
                        else
                        {
                            await ShowViewModel(_rehydrationService.BusinessProcess[i]);
                        }
                        // In case of non-real time claim submission, drug claim for example, we dont want to show anything after doc upload screen when claim
                        // is resumed.
                        if (_rehydrationService.BusinessProcess[i] == typeof(ClaimDocumentsUploadViewModel))
                        {
                            break;
                        }
                    
                }
            }
        }

        public async Task RehydrateDroidProcess()
        {
            var _loopsync = new object();

            var userDialog = Mvx.IoCProvider.Resolve<IUserDialogs>();
            userDialog.ShowLoading(Resource.Loading);

            //Rebuild the nav stack for the last business process that was in process
            for (int i = 0; i < _rehydrationService.BusinessProcess.Count; i++)
            {
                lock (_loopsync)
                {
                    //bool vmSet = false;
                    if (_rehydrationService.BusinessProcess[i] != typeof(ClaimOtherBenefitsViewModel)
                    && _rehydrationService.BusinessProcess[i] != typeof(ClaimMotorVehicleViewModel)
                    && _rehydrationService.BusinessProcess[i] != typeof(ClaimWorkInjuryViewModel)
                    && _rehydrationService.BusinessProcess[i] != typeof(ClaimMedicalItemViewModel))
                    {
                        // Fix For - ClaimType getting NotDefined while user Resume the Drug Claim Submission and process hangs at end
                        if (_rehydrationService.BusinessProcess[i] == typeof(ClaimDocumentsUploadViewModel))
                        {
                            INonRealTimeClaimUploadProperties uploadable = (INonRealTimeClaimUploadProperties)NonRealTimeUploadFactory.Create(NonRealTimeClaimType.Drug, nameof(ClaimDocumentsUploadViewModel));
                            ShowViewModel<ClaimDocumentsUploadViewModel, ClaimDocumentsUploadViewModelParameters>(new ClaimDocumentsUploadViewModelParameters(uploadable));
                        }
                        else
                        {
                            ShowViewModel(_rehydrationService.BusinessProcess[i]);
                        }
                        // In case of non-real time claim submission, drug claim for example, we dont want to show anything after doc upload screen when claim
                        // is resumed.
                        if (_rehydrationService.BusinessProcess[i] == typeof(ClaimDocumentsUploadViewModel))
                        {
                            break;
                        }
                    }
                }

                //hack to let fragmentactivity be created
                if (i == 0)
                {
                    await Task.Delay(500);
                }
            }

            userDialog.HideLoading();
        }

        protected MvxCommand<int> m_RemoveCommand;
        public virtual ICommand RemoveCommand => null;

        private MvxCommand _goBackCommand;
        public virtual ICommand GoBackCommand
        {
            get
            {
                _goBackCommand = _goBackCommand ?? new MvxCommand(GoBack);
                return _goBackCommand;
            }
        }

        protected async virtual void GoBack()
        {
            await Close(this);
        }

        private MvxAsyncCommand _goHomeCommand;

        protected virtual async Task Logout()
        {
            // This is necessary to ensure the dashboard is closed on logout
            // If this is not done, a second instance will appear and the menu fragment will get sandwiched between them
            if (DashboardViewModel.CurrentDashboardInstance != null)
            {
               await DashboardViewModel.CurrentDashboardInstance.Close(DashboardViewModel.CurrentDashboardInstance);
            }
            _loginservice.Logout();
            await ShowViewModel<LoginViewModel>();
        }

        protected virtual async Task LogoutWithAlertMessageAsync(bool showAlertMessageFirst = false)
        {
            _loginservice.Logout();
            if (showAlertMessageFirst)
            {
                await _userDialog.AlertAsync("[EN] Your session time out. Please login into continue");
                if (this.GetType() != typeof(LoginViewModel))
                {
                    await ShowViewModel<LoginViewModel>();
                }
            }
            else
            {
                if (this.GetType() != typeof(LoginViewModel))
                {
                    await ShowViewModel<LoginViewModel>();
                }
                if (_userDialog != null)
                {
                    _userDialog.Alert(Resource.SessionTimedOutMessage);
                }
            }
        }

        public ICommand GoHomeCommand
        {
            get
            {
                _goHomeCommand ??= new MvxAsyncCommand(GoHome);
                return _goHomeCommand;
            }
        }

        public Task<bool> ShowViewModel<T>()
            where T : IMvxViewModel
        {
            var result = _navigationService.Navigate<T>();
            return result;
        }

        public Task<bool> ShowViewModel<T>(IMvxBundle bundle)
            where T : IMvxViewModel
        {
            var result = _navigationService.Navigate<T>(bundle);
            return result;
        }

        public Task<bool> ShowViewModel<TViewModel, TParameter>(TParameter parameter)
            where TViewModel : IMvxViewModel<TParameter>
        {
            return _navigationService.Navigate<TViewModel, TParameter>(parameter);
        }

        public Task<bool> ShowViewModel<TViewModel, TParameter>(TParameter parameter, MvxBundle presentationBundle)
            where TViewModel : IMvxViewModel<TParameter>
        {
            return _navigationService.Navigate<TViewModel, TParameter>(parameter, presentationBundle);
        }

        public Task<bool> ShowViewModel(Type type, MvxBundle presentationBundle=null)
        {
            if(presentationBundle == null) {
                return _navigationService.Navigate(type);
			}

            return _navigationService.Navigate(type, presentationBundle);
        }

        public Task<bool> ShowViewModel(Type type, IViewModelParameters parameter)
        {
            if (parameter == null)
            {
                return _navigationService.Navigate(type);
            }

            return _navigationService.Navigate(type, parameter);
        }

        public async Task<bool> Close<T>(T viewModel)
            where T : IMvxViewModel
        {
            return await _navigationService.Close(viewModel);
        }

        private async Task GoHome()
        {
            await Close(this);
            _messenger.Publish(new RequestNavToRoot(this));
            await ShowViewModel<DashboardViewModel>(new MvxBundle(new Dictionary<string, string>
            {
                { NavigationRequestTypes.RequestedBy, NavigationRequestTypes.BottomNavigationBar }
            }));
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<LoggedOutMessage>(_loggedouttoken);
            _messenger.Unsubscribe<RequestNavToDrugLookup>(_requestnavtodruglookup);
            _messenger.Unsubscribe<RequestNavToLocateServiceProvider>(_requestnavtolocateserviceprovider);
            _messenger.Unsubscribe<RequestNavToSpendingAccounts>(_requestnavtospendingaccounts);
        }

        private async void HandleNavigateAwayRequest()
        {
            await Close(this);
        }
    }
}