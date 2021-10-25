using System.Collections.Generic;
using MobileClaims.Core.Services;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Entities;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;


namespace MobileClaims.Core.ViewModels
{
    public class ProviderLookupProviderTypeViewModel : ViewModelBase
    {
        #region Member Variables
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _lookupservice;
        private readonly MvxSubscriptionToken _retrievedserviceproviders;
        private readonly MvxSubscriptionToken _navtoselfrequested;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly object _sync = new object();
        #endregion

        #region ctor
        public ProviderLookupProviderTypeViewModel(IMvxMessenger messenger, IProviderLookupService lookupservice)
        {
            lock (_sync)
            {
                _messenger = messenger;
                _lookupservice = lookupservice;

                _retrievedserviceproviders = _messenger.Subscribe<RetrievedServiceProviderTypes>((message) =>
                {
                    this.ServiceProviderTypes = _lookupservice.ServiceProviderTypes;
                    Busy = false;
                });

                if (_lookupservice.ServiceProviderTypes == null || _lookupservice.ServiceProviderTypes.Count == 0)
                {
                    Busy = true;
                    _lookupservice.GetServiceProviderTypes();
                }
                else
                {
                    this.ServiceProviderTypes = _lookupservice.ServiceProviderTypes;
                }

                _navtoselfrequested = _messenger.Subscribe<RequestNavToLocateServiceProvider>((message) =>
                {
                    _messenger.Unsubscribe<RequestNavToLocateServiceProvider>(_navtoselfrequested);
                    Close(this);
                });

                _shouldcloseself = _messenger.Subscribe<ClearProviderLookupProviderType>((message) =>
                {
                    _messenger.Unsubscribe<ClearProviderLookupProviderType>(_shouldcloseself);
                    SelectedServiceProviderType = null; // clear selection
                    Close(this);
                });
            }
        }
        #endregion
        
        #region Properties
        private bool _busy = false;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                if (_busy != value)
                {
                    _busy = value;
                    _messenger.Publish<BusyIndicator>(new BusyIndicator(this)
                    {
                        Busy = _busy
                    });
                    RaisePropertyChanged(() => Busy);
                }
            }
        }

        private ServiceProviderType _selectedserviceprovidertype;
        public ServiceProviderType SelectedServiceProviderType
        {
            get
            {
                return _selectedserviceprovidertype;
            }
            set
            {
                _selectedserviceprovidertype = value;
                RaisePropertyChanged(() => SelectedServiceProviderType);
            }
        }

        private List<ServiceProviderType> _serviceprovidertypes;
        public List<ServiceProviderType>ServiceProviderTypes
        {
            get
            {
                return _serviceprovidertypes;
            }
            private set
            {
                _serviceprovidertypes = value;
                RaisePropertyChanged(() => ServiceProviderTypes);
            }
        }
        #endregion

        #region Commands

        #region Android Helper Commands
        public ICommand ShowMyAccountCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    _messenger.Publish<RequestNavToCard>(new RequestNavToCard(this));
                    ShowViewModel<CardViewModel>();
                });
            }
        }

        public ICommand ShowBalancesCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    _messenger.Publish<RequestNavToSpendingAccounts>(new RequestNavToSpendingAccounts(this));
                    ShowViewModel<ChooseSpendingAccountTypeViewModel>();
                });
            }
        }

        public ICommand ShowFindServiceProviderCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    lock (_sync)
                    {
                        Unsubscribe();
                        _messenger.Publish<RequestNavToLocateServiceProvider>(new RequestNavToLocateServiceProvider(this));
                        ShowViewModel<ProviderLookupProviderTypeViewModel>();
                    }
                });
            }
        }

        public ICommand ShowDrugLookupCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
                    _messenger.Publish<RequestNavToDrugLookup>(new RequestNavToDrugLookup(this));
                    ShowViewModel<ViewModels.DrugLookupModelSelectionViewModel>();
                });
            }
        }
        #endregion

        public ICommand NextStepCommand
        {
            get
            {
                return new MvxCommand<ServiceProviderType>((spType) =>
                {
                    Unsubscribe();
                    ClearLocationAndSearchTypes();
                    SelectedServiceProviderType = spType;
                    LocateServiceProviderChooseSearchTypeViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderChooseSearchTypeViewModel.ProviderSearchNavigationHelper()
                    {
                        ProviderTypeID = spType.ID
                    };
                    _messenger.Publish<Messages.ClearLocateServiceProviderChooseSearchType>(new MobileClaims.Core.Messages.ClearLocateServiceProviderChooseSearchType(this));
                    _messenger.Publish<Messages.ClearLocateServiceProvider>(new MobileClaims.Core.Messages.ClearLocateServiceProvider(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderLocatedProviders>(new MobileClaims.Core.Messages.ClearLocateServiceProviderLocatedProviders(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    //LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper() { ProviderTypeID= _selectedserviceprovidertype.ID };
                    this.ShowViewModel<LocateServiceProviderChooseSearchTypeViewModel>(nav);
                });
            }
        }
        #endregion

        #region Helper Functions
        private void Unsubscribe()
        {
            _messenger.Unsubscribe<RetrievedServiceProviderTypes>(_retrievedserviceproviders);
        }

        private void ClearLocationAndSearchTypes()
        {
            _lookupservice.LocationTypeID = 0;
            _lookupservice.SearchTypeID = 0;
        }
        #endregion
    }
}
