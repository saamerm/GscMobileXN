using MobileClaims.Core.Services;
using System.Linq;
using MobileClaims.Core.Entities;
using System.Windows.Input;
using MobileClaims.Core.Messages;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public sealed class LocateServiceProviderLocatedProvidersViewModel : LocateServiceProviderResultViewModel
    {
        #region Member Variables
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _lookupservice;
        private readonly ILocationService _locationservice;
        private readonly MvxSubscriptionToken _navtolocatedproviders;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private object _sync = new object();
        #endregion

        #region ctors
        public LocateServiceProviderLocatedProvidersViewModel(IMvxMessenger messenger, IProviderLookupService lookupservice, ILocationService locationservice) :base(messenger,lookupservice,locationservice)
        {
            _messenger = messenger;
            _lookupservice = lookupservice;
            _locationservice = locationservice;

            _navtolocatedproviders = _messenger.Subscribe<RequestNavToServiceProviderSearchResults>((message) =>
            {
                _messenger.Unsubscribe<RequestNavToServiceProviderSearchResults>(_navtolocatedproviders);
                this.SearchTerms = new ServiceProviderSearchTerms()
                {
                    Address =   message.Address,
                    BusinessName = message.BusinessName,
                    City = message.City,
                    LastName = message.LastName,
                    LocationType = message.LocationType,
                    Phone=message.Phone,
                    PostalCode =  message.PostalCode,
                    ProviderType = message.ProviderType,
                    Radius = message.Radius,
                    SearchType = message.SearchType,
                    UsedDeviceLocation = message.UsedDeviceLocation
                };
                this.ServiceProviders=_lookupservice.SearchResults;
            });

            _shouldcloseself = _messenger.Subscribe<ClearLocateServiceProviderLocatedProviders>((message) =>
            {
                _messenger.Unsubscribe<ClearLocateServiceProviderLocatedProviders>(_shouldcloseself);
                Close(this);
            });
        }

        public new void Init(NavHelper nav)
        {
            if (nav.serviceProviderID != 0)
            {
                this.SelectedProvider = this.ServiceProviders.Where(sp => sp.ID == nav.serviceProviderID).FirstOrDefault();
                if(nav.IsFromLandingPage )//added on Jun 25 by vivian for showing map by clicking 5 providers in landing page 
                {
                    Show4LineTitle = false;
                    ShowMapViewWithoutNavigatingForLandingPageCommand.Execute(this.SelectedProvider);
                }//ended of adding on Jun 25 by vivian for showing map by clicking 5 providers in landing page 
            }
        }
        #endregion

        #region Properties
        public double CurrentLat
        {
            get
            {
                return _locationservice.Latitude;
            }
        }

        public double CurrentLng
        {
            get
            {
                return _locationservice.Longitude;
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
                    _messenger.Publish<RequestNavToDrugLookup>(new RequestNavToDrugLookup(this));
                    ShowViewModel<ViewModels.DrugLookupModelSelectionViewModel>();
                });
            }
        }
        #endregion

        public ICommand ShowMapViewWithoutNavigatingCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((sp) =>
                {
                    SelectedProvider = sp;
                    RequestNavToServiceProviderMapView _message = new RequestNavToServiceProviderMapView(this)
                    {
                        serviceProviderID = sp.ID,
                        Radius=SearchTerms.Radius,
                        Address = SearchTerms.Address,
                        BusinessName = SearchTerms.BusinessName,
                        City = SearchTerms.City,
                        LastName = SearchTerms.LastName,
                        LocationType = SearchTerms.LocationType,
                        Phone = SearchTerms.Phone,
                        PostalCode = SearchTerms.PostalCode,
                        ProviderType = SearchTerms.ProviderType,
                        SearchType = SearchTerms.SearchType,
                        UsedDeviceLocation = SearchTerms.UsedDeviceLocation
                    };
                    _messenger.Publish<RequestNavToServiceProviderMapView>(_message);
                });
            }
        }
        public ICommand ShowMapViewWithoutNavigatingForIOSCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((sp) =>
                {
                    SelectedProvider = sp;
                    RequestRefreshOfServiceProviderMapView _message = new RequestRefreshOfServiceProviderMapView(this)
                    {
                        serviceProviderID = sp.ID,
                        Radius=SearchTerms.Radius,
                        Address = SearchTerms.Address,
                        BusinessName = SearchTerms.BusinessName,
                        City = SearchTerms.City,
                        LastName = SearchTerms.LastName,
                        LocationType = SearchTerms.LocationType,
                        Phone = SearchTerms.Phone,
                        PostalCode = SearchTerms.PostalCode,
                        ProviderType = SearchTerms.ProviderType,
                        SearchType = SearchTerms.SearchType,
                        UsedDeviceLocation = SearchTerms.UsedDeviceLocation
                    };
                    _messenger.Publish<RequestRefreshOfServiceProviderMapView>(_message);
                });
            }
        }
        //Used for windows phone
        public ICommand ShowMapViewAndSelectionCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((sp) =>
                {                    
                    // After the first time, we want to close the location map and reopen it
                    if (SelectedProvider != null)
                    {
                        _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    }
                    LocateServiceProviderResultViewModel.NavHelper nav = new NavHelper()
                    {
                        serviceProviderID = sp.ID,
                        Radius = SearchTerms.Radius,
                        Address = SearchTerms.Address,
                        BusinessName = SearchTerms.BusinessName,
                        City = SearchTerms.City,
                        LastName = SearchTerms.LastName,
                        LocationType = SearchTerms.LocationType,
                        Phone = SearchTerms.Phone,
                        PostalCode = SearchTerms.PostalCode,
                        ProviderType = SearchTerms.ProviderType,
                        SearchType = SearchTerms.SearchType,
                        UsedDeviceLocation = SearchTerms.UsedDeviceLocation
                    };
                    ShowViewModel<LocateServiceProviderResultViewModel>(nav);
                });
            }
        }

        public ICommand ShowMapViewCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((sp) =>
                {
                    LocateServiceProviderResultViewModel.NavHelper nav = new NavHelper()
                    {
                        serviceProviderID = sp.ID,
                        Radius=SearchTerms.Radius,
                        Address = SearchTerms.Address,
                        BusinessName = SearchTerms.BusinessName,
                        City = SearchTerms.City,
                        LastName = SearchTerms.LastName,
                        LocationType = SearchTerms.LocationType,
                        Phone = SearchTerms.Phone,
                        PostalCode = SearchTerms.PostalCode,
                        ProviderType = SearchTerms.ProviderType,
                        SearchType = SearchTerms.SearchType,
                        UsedDeviceLocation = SearchTerms.UsedDeviceLocation
                    };
                    ShowViewModel<LocateServiceProviderResultViewModel>(nav);
                });
            }
        }



        #region added by vivian for landing page
        /// <summary>
        /// Show or Hide the left top 4 line title 
        /// hide them when from landing page cause only one item 
        /// </summary>
        private bool _show4LineTitle=true ;
        public bool Show4LineTitle
        {
            get
            {
                return _show4LineTitle;
            }
            set
            {
                _show4LineTitle = value;
                RaisePropertyChanged(() => Show4LineTitle);
            }
        }
        /// <summary>
        /// for  window tablet only
        /// </summary>
        public ICommand ShowMapViewWithoutNavigatingForLandingPageCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((sp) =>
                {
                    SelectedProvider = sp;
                    RequestNavToServiceProviderMapView _message = new RequestNavToServiceProviderMapView(this)
                    {
                        serviceProviderID = sp.ID,
                        Radius = SearchTerms != null ? SearchTerms.Radius : 0,
                        Address = SearchTerms != null ? SearchTerms.Address : sp.Address ,
                        BusinessName = SearchTerms != null ? SearchTerms.BusinessName : sp.BusinessName,
                        City = SearchTerms != null ? SearchTerms.City : sp.City ,
                        LastName = SearchTerms != null ? SearchTerms.LastName : "",
                        LocationType = SearchTerms != null ? SearchTerms.LocationType : null,
                        Phone = SearchTerms != null ? SearchTerms.Phone : sp.Phone,
                        PostalCode = SearchTerms != null ? SearchTerms.PostalCode : sp.PostalCode ,
                        ProviderType = SearchTerms != null ? SearchTerms.ProviderType : null,
                        SearchType = SearchTerms != null ? SearchTerms.SearchType : "1",
                        UsedDeviceLocation = SearchTerms != null ? SearchTerms.UsedDeviceLocation : false
                    };
                    _messenger.Publish<RequestNavToServiceProviderMapView>(_message);
                });
            }
        }

        /// <summary>
        /// for window phone only
        /// </summary>
        public ICommand ShowMapViewAndSelectionForLandingPageCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((sp) =>
                {
                    // After the first time, we want to close the location map and reopen it
                    if (SelectedProvider != null)
                    {
                        _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    }
                    LocateServiceProviderResultViewModel.NavHelper nav = new NavHelper()
                    {
                        serviceProviderID = sp.ID,
                        Radius = SearchTerms != null ? SearchTerms.Radius : 0,
                        Address = SearchTerms != null ? SearchTerms.Address : sp.Address,
                        BusinessName = SearchTerms != null ? SearchTerms.BusinessName : sp.BusinessName,
                        City = SearchTerms != null ? SearchTerms.City : sp.City,
                        LastName = SearchTerms != null ? SearchTerms.LastName : "",
                        LocationType = SearchTerms != null ? SearchTerms.LocationType : null,
                        Phone = SearchTerms != null ? SearchTerms.Phone : sp.Phone,
                        PostalCode = SearchTerms != null ? SearchTerms.PostalCode : sp.PostalCode,
                        ProviderType = SearchTerms != null ? SearchTerms.ProviderType : null,
                        SearchType = SearchTerms != null ? SearchTerms.SearchType : "1",
                        UsedDeviceLocation = SearchTerms != null ? SearchTerms.UsedDeviceLocation : false
                    };
                    ShowViewModel<LocateServiceProviderResultViewModel>(nav);
                });
            }
        }
        #endregion
        #endregion
		public ICommand SelectWithoutNavigationCommand
		{
			get
			{
				return new MvxCommand<ServiceProvider>((provider) =>
					{
						this.SelectedProvider = provider;
					});
			}
		}
    }
}
