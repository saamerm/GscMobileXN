using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using MobileClaims.Core.Entities;
using System.Windows.Input;
using MobileClaims.Core.Messages;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;


namespace MobileClaims.Core.ViewModels
{
    public delegate void EventHandler(object sender, EventArgs e);
    public class LocateServiceProviderResultViewModel : ViewModelBase
    {
        #region Member Variables
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _lookupservice;
        private readonly ILocationService _locationservice;
        private readonly MvxSubscriptionToken _shouldcloseself;
		private readonly MvxSubscriptionToken _refreshMapView;
        private object _sync = new object();
        #endregion

        #region ctors
        public LocateServiceProviderResultViewModel(IMvxMessenger messenger, IProviderLookupService lookupservice, ILocationService locationservice)
        {
            _messenger = messenger;
            _lookupservice = lookupservice;
            _locationservice = locationservice;
            this.ServiceProviders = _lookupservice.SearchResults;
            
            _shouldcloseself = _messenger.Subscribe<ClearLocateServiceProviderResult>((message) =>
			{
                if (message.Sender.GetType().FullName != this.GetType().FullName)
                {
                    _messenger.Unsubscribe<ClearLocateServiceProviderResult>(_shouldcloseself);
                    _messenger.Unsubscribe<RequestRefreshOfServiceProviderMapView>(_refreshMapView);
                    Close(this);
                }
            });


			_refreshMapView = _messenger.Subscribe<RequestRefreshOfServiceProviderMapView> ((message) => {
				SelectedProvider = _lookupservice.SelectedServiceProvider;
			});


        }

        public void Init(NavHelper nav)
        { 
                if (nav.serviceProviderID != 0)
                {
                    this.SelectedProvider = _lookupservice.SearchResults.Where(sr => sr.ID == nav.serviceProviderID).FirstOrDefault();
                }
                if (this.ServiceProviders == null || this.ServiceProviders.Count==0)
                {
                    this.ServiceProviders = _lookupservice.SearchResults;
                } 
             {
                this.SearchTerms = new ServiceProviderSearchTerms()
                {
                    Address = !string.IsNullOrEmpty(nav.Address) ? nav.Address : "",
                    BusinessName = !string.IsNullOrEmpty(nav.BusinessName) ? nav.BusinessName : "",
                    City = !string.IsNullOrEmpty(nav.City) ? nav.City : "",
                    LastName = !string.IsNullOrEmpty(nav.LastName) ? nav.LastName : "",
                    LocationType = !string.IsNullOrEmpty(nav.LocationType) ? nav.LocationType : "",
                    Phone = !string.IsNullOrEmpty(nav.Phone) ? nav.Phone : nav.Phone,
                    PostalCode = !string.IsNullOrEmpty(nav.PostalCode) ? nav.PostalCode : "",
                    ProviderType = !string.IsNullOrEmpty(nav.ProviderType) ? nav.ProviderType : "",
    				SearchType = !string.IsNullOrEmpty(nav.ProviderType) ? nav.SearchType : "",
                    Radius = nav.Radius,
                    UsedDeviceLocation = nav.UsedDeviceLocation
                };
            }
        }

        #region NavHelper
        public class NavHelper
        {
            public string City { get; set; }
            public string Address { get; set; }
            public string PostalCode { get; set; }
            public int Radius { get; set; }
            public string Phone { get; set; }
            public string BusinessName { get; set; }
            public string LastName { get; set; }
            public string ProviderType { get; set; }
            public string LocationType { get; set; }
            public string SearchType { get; set; }
            public bool UsedDeviceLocation { get; set; }
            public int serviceProviderID { get; set; }

            public bool IsFromLandingPage { get; set; }//added by vivian
            public NavHelper()
            {
                IsFromLandingPage = false;
            }//ended of adding by vivian

           }
        #endregion

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

        private ServiceProviderSearchTerms _searchterms;
        public ServiceProviderSearchTerms SearchTerms
        {
            get { return _searchterms; }
            set
            {
                if (_searchterms != value)
                {
                    _searchterms = value;
                    RaisePropertyChanged(() => SearchTerms);
                };
            }
        }

		private ServiceProvider _selectedprovider = null;
		public ServiceProvider SelectedProvider
		{
			get
			{
                if (_selectedprovider != null)
                    return _selectedprovider;
                else
                    return _lookupservice.SelectedServiceProvider;
			}
			set
            {
                if (_selectedprovider != value)
                {
                    _selectedprovider = value;
                    _lookupservice.SelectedServiceProvider = value;
                    RaisePropertyChanged(() => SelectedProvider);                    
                    _messenger.Publish<SelectedServiceProviderChanged>(new SelectedServiceProviderChanged(this)
                    {
                        CurrentLatitude = _selectedprovider.Latitude,
                        CurrentLongitude = _selectedprovider.Longitude,
                        Address = _selectedprovider.Address,
                        FormattedAddress = _selectedprovider.FormattedAddress,
                        DoctorName = _selectedprovider.DoctorName,
                        BusnissName = _selectedprovider.BusinessName,
                        PhoneNo = _selectedprovider.Phone

                    }
                   );
                }
			}
		}

        private List<ServiceProvider> _serviceproviders;
        public List<ServiceProvider> ServiceProviders
        {
            get { return _serviceproviders; }
            set 
            { 
                _serviceproviders = value;
                RaisePropertyChanged(() => ServiceProviders);
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

        // obsolete, should use NewSearchCommand for "New Search" button
        public ICommand DoneCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    OnClosedRequested(new EventArgs());
                    ShowViewModel<LandingPageViewModel>();
                });
            }
        }

        public ICommand NewWPSearchCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    //_messenger.Publish<Messages.ClearProviderLookupProviderType>(new MobileClaims.Core.Messages.ClearProviderLookupProviderType(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderChooseSearchType>(new MobileClaims.Core.Messages.ClearLocateServiceProviderChooseSearchType(this));
                    _messenger.Publish<Messages.ClearLocateServiceProvider>(new MobileClaims.Core.Messages.ClearLocateServiceProvider(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderLocatedProviders>(new MobileClaims.Core.Messages.ClearLocateServiceProviderLocatedProviders(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    //ShowViewModel<ProviderLookupProviderTypeViewModel>();
                });
            }
        }

        public ICommand NewSearchCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _messenger.Publish<Messages.ClearProviderLookupProviderType>(new MobileClaims.Core.Messages.ClearProviderLookupProviderType(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderChooseSearchType>(new MobileClaims.Core.Messages.ClearLocateServiceProviderChooseSearchType(this));
                    _messenger.Publish<Messages.ClearLocateServiceProvider>(new MobileClaims.Core.Messages.ClearLocateServiceProvider(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderLocatedProviders>(new MobileClaims.Core.Messages.ClearLocateServiceProviderLocatedProviders(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    ShowViewModel<ProviderLookupProviderTypeViewModel>();
                });
            }
        }

        public ICommand SelectAndNavigateCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((provider) =>
                {
                    this.SelectedProvider = provider;
                });
            }
        }

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
        #endregion

        #region Events
        public event EventHandler CloseRequested;
        protected virtual void OnClosedRequested(EventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, e);
        }
        #endregion
    }
}