using System;
using System.Collections.Generic;
using System.Linq;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System.Windows.Input;
using FluentValidation;
using System.Text.RegularExpressions;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class LocateServiceProviderViewModel : ViewModelBase
    {

        #region Member variables
        private object _sync = new object();
        private readonly ILocationService _locationservice;
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _lookupservice;
        private MvxSubscriptionToken _searchcomplete;
        private MvxSubscriptionToken _searcherror;
        private readonly MvxSubscriptionToken _searchtypeset;
        private readonly MvxSubscriptionToken _backtostart;
        private MvxSubscriptionToken _showmapview;
        private readonly MvxSubscriptionToken _locationupdated;
        private readonly MvxSubscriptionToken _shouldcloseself;
        #endregion
        
        #region ctors
        public LocateServiceProviderViewModel(IMvxMessenger messenger, IProviderLookupService lookupservice, ILocationService locationservice)
        {
            lock (_sync)
            {
                _messenger = messenger;
                _lookupservice = lookupservice;
                _locationservice = locationservice;

                if (_locationservice.LocationAvailable)
                {
                    this.Lat = _locationservice.Latitude;
                    this.Lng = _locationservice.Longitude;
                    LocationAvailable = true;
                }

                this.LocationTypes = new List<LocationType>();
                this.SearchTypes = new List<SearchType>();

                PopulateLocationTypes();
                PopulateSearchTypes();

                _locationupdated = _messenger.Subscribe<LocationUpdated>((message) =>
                {
                    this.Lat = _locationservice.Latitude;
                    this.Lng = _locationservice.Longitude;
                    InvokeOnMainThread(() =>
                    {
                        (FindProviderCommand as MvxCommandBase).RaiseCanExecuteChanged();
                        (FindProviderWithoutNavigationCommand as MvxCommandBase).RaiseCanExecuteChanged();
                    });
                    LocationAvailable = true;
                });

                _showmapview = _messenger.Subscribe<RequestNavToServiceProviderMapView>((message) =>
                {
                    LocateServiceProviderResultViewModel.NavHelper nav = new LocateServiceProviderResultViewModel.NavHelper()
                    {
                        serviceProviderID = message.serviceProviderID,
                        Radius=message.Radius,
                        Address = message.Address,
                        BusinessName = message.BusinessName,
                        City = message.City,
                        LastName = message.LastName,
                        LocationType = message.LocationType,
                        Phone = message.Phone,
                        PostalCode = message.PostalCode,
                        ProviderType = message.ProviderType,
                        SearchType = message.SearchType,
                        UsedDeviceLocation = message.UsedDeviceLocation
                    };
                    ShowViewModel<LocateServiceProviderResultViewModel>(nav);
                });

                _searchtypeset = _messenger.Subscribe<LocateProviderSearchTypeSet>((message) =>
                {
                    //Inheritance oddity!
                    if (message.Sender.GetType().FullName == this.GetType().FullName) return;
                    this.SelectedLocationType = message.LocationType;
                    this.SelectedSearchType = message.SearchType;
                    this.ProviderType = message.ProviderType;
                    SetHidingProperties();
                });

                _backtostart = _messenger.Subscribe<RequestNavToLocateServiceProvider>((message) =>
                {
                    _messenger.Unsubscribe<RequestNavToLocateServiceProvider>(_backtostart);
                    Close(this);
                });

                _shouldcloseself = _messenger.Subscribe<ClearLocateServiceProvider>((message) =>
                {
                    _messenger.Unsubscribe<ClearLocateServiceProvider>(_shouldcloseself);
                    Unsubscribe();
                    Close(this);
                });
            }
        }

        public void Init(ProviderSearchNavigationHelper navhelper)
        {
            if (navhelper == null)
                return;
            this.SelectedSearchType = _lookupservice.SearchTypeID > 0 ? this.SearchTypes.Where(st => st.TypeId == _lookupservice.SearchTypeID).FirstOrDefault() : this.SearchTypes.First();
            this.SelectedLocationType = _lookupservice.LocationTypeID > 0 ? this.LocationTypes.Where(lt => lt.TypeId == _lookupservice.LocationTypeID).FirstOrDefault() : this.LocationTypes.First();

           // this.SelectedSearchType = this.SearchTypes.Where(st => st.TypeId == _lookupservice.SearchTypeID).FirstOrDefault();
           // this.SelectedLocationType = this.LocationTypes.Where(lt => lt.TypeId == _lookupservice.LocationTypeID).FirstOrDefault();
            this.ProviderType = _lookupservice.ServiceProviderTypes.Where(pt => pt.ID == navhelper.ProviderTypeID).FirstOrDefault();
            if(ProviderType!=null && SelectedLocationType != null && SelectedSearchType != null) SetHidingProperties();
        }

        #region NavigationHelper
        public class ProviderSearchNavigationHelper
        {
            //public ServiceProviderType ProviderType { get; set; }
            public string ProviderTypeID { get; set; }
            public int? SearchTypeID { get; set; }
            public int? LocationTypeID { get; set; }
        }
        #endregion

        #endregion

        #region Properties
        private bool locationavailable;
        public bool LocationAvailable
        {
            get
            {
                return locationavailable;
            }
            set
            {
                locationavailable = value;
                RaisePropertyChanged(() => LocationAvailable);
            }
        }
        private double lat;
        public double Lat
        {
            get { return lat; }
            set
            {
                lat = value;
                RaisePropertyChanged(() => Lat);
            }
        }

        private double lng;
        public double Lng
        {
            get
            {
                return lng;
            }
            set
            {
                lng = value;
                RaisePropertyChanged(() => Lng);
            }
        }

        private ServiceProviderType _providertype;
        public ServiceProviderType ProviderType
        {
            get { return _providertype; }
            set
            {
                _providertype = value;
                RaisePropertyChanged(() => ProviderType);
            }
        }

        private List<LocationType> _locationtypes;
        public List<LocationType> LocationTypes
        {
            get { return _locationtypes; }
            set
            {
                _locationtypes = value;
                RaisePropertyChanged(() => LocationTypes);
            }
        }

        private LocationType _selectedLocationType;
        public LocationType SelectedLocationType
        {
            get { return _selectedLocationType; }
            set
            {
                _selectedLocationType = value;
                RaisePropertyChanged(() => SelectedLocationType);
                RaiseAllPropertiesChanged();
            }
        }

        private List<SearchType> _searchtypes;
        public List<SearchType> SearchTypes
        {
            get { return _searchtypes; }
            set
            {
                _searchtypes = value;
                RaisePropertyChanged(() => SearchTypes);
            }
        }

        private SearchType _selectedsearchtype;
        public SearchType SelectedSearchType
        {
            get { return _selectedsearchtype; }
            set
            {
                _selectedsearchtype = value;
                RaisePropertyChanged(() => SelectedSearchType);
            }
        }

        private int _searchradius;
        public int SearchRadius
        {
            get { return _searchradius; }
            set
            {
                _searchradius = value;
                RaisePropertyChanged(() => SearchRadius);
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        private string _postalcode;
        public string PostalCode
        {
            get { return _postalcode; }
            set
            {
                _postalcode = value;
                RaisePropertyChanged(() => PostalCode);
            }
        }

        private string _phonenumber;
        public string PhoneNumber
        {
            get { return _phonenumber; }
            set
            {
                _phonenumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        private string _lastname;
        public string LastName
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private string _businessname;
        public string BusinessName
        {
            get { return _businessname; }
            set
            {
                _businessname = value;
                RaisePropertyChanged(() => BusinessName);
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged(() => City);
            }
        }

        private bool _searching = false;
        public bool Searching
        {
            get { return _searching; }
            set
            {
                if (_searching != value)
                {                    
                    _searching = value;
                    RaisePropertyChanged(() => Searching);

                    _messenger.Publish<BusyIndicator>(new BusyIndicator(this)
                    {
                        Busy = _searching
                    });
                    //if (_findProviderCommand != null)
                      //  (_findProviderCommand as MvxCommandBase).RaiseCanExecuteChanged();
                    if (_findProviderWithoutNavigationCommand != null)
                        InvokeOnMainThread(() =>
                        {
                            (_findProviderWithoutNavigationCommand as MvxCommandBase).RaiseCanExecuteChanged();
                        });
                }
            }
        }

        #region Visibility Helpers
        private bool _showsearchtypephonenumber = true;
        public bool ShowSearchTypePhoneNumber
        {
            get { return _showsearchtypephonenumber; }
            set
            {
                _showsearchtypephonenumber = value;
                RaisePropertyChanged(() => ShowSearchTypePhoneNumber);
            }
        }
        private bool _showsearchtypelastname = true;
        public bool ShowSearchTypeLastName
        {
            get { return _showsearchtypelastname; }
            set
            {
                _showsearchtypelastname = value;
                RaisePropertyChanged(() => ShowSearchTypeLastName);
            }
        }
        private bool _showsearchtypebusinessname = true;
        public bool ShowSearchTypeBusinessName
        {
            get { return _showsearchtypebusinessname; }
            set
            {
                _showsearchtypebusinessname = value;
                RaisePropertyChanged(() => ShowSearchTypeBusinessName);
            }
        }

        private bool _showsearchtypecity = true;
        public bool ShowSearchtypeCity
        {
            get { return _showsearchtypecity; }
            set
            {
                _showsearchtypecity = value;
                RaisePropertyChanged(() => ShowSearchtypeCity);
            }
        }

		private bool _showlocationtypegps = true;
		public bool ShowLocationTypeGps
		{
			get { return _showlocationtypegps; }
			set
			{
				_showlocationtypegps = value;
				RaisePropertyChanged(() => ShowLocationTypeGps);
			}
		}

        private bool _showlocationtypeaddress = true;
        public bool ShowLocationTypeAddress
        {
            get { return _showlocationtypeaddress; }
            set
            {
                _showlocationtypeaddress = value;
                RaisePropertyChanged(() => ShowLocationTypeAddress);
            }
        }

        private bool _showlocationtypeaddressexample = true;
        public bool ShowSearchTypeAddressExample
        {
            get { return _showlocationtypeaddressexample; }
            set
            {
                _showlocationtypeaddressexample = value;
                RaisePropertyChanged(() => ShowSearchTypeAddressExample);
            }
        }

        private bool _showlocationtypepostalcode = true;
        public bool ShowLocationTypePostalCode
        {
            get { return _showlocationtypepostalcode; }
            set
            {
                _showlocationtypepostalcode = value;
                RaisePropertyChanged(() => ShowLocationTypePostalCode);
            }
        }

        private bool _showsearchtypepostalcodeexample = true;
        public bool ShowSearchTypePostalCodeExample
        {
            get { return _showsearchtypepostalcodeexample; }
            set
            {
                _showsearchtypepostalcodeexample = value;
                RaisePropertyChanged(() => ShowSearchTypePostalCodeExample);
            }
        }
        #endregion

        #endregion

        #region Events
        public event EventHandler NoProvidersFound;
        protected virtual void OnNoProvidersFound(EventArgs e)
        {
            if (NoProvidersFound != null)
                NoProvidersFound(this, e);
        }

        public event EventHandler MissingAddressOrPostalCode;
        protected virtual void OnMissingAddressOrPostalCode(EventArgs e)
        {
            if (MissingAddressOrPostalCode != null)
                MissingAddressOrPostalCode(this, e);
        }

        public event EventHandler InvalidPostalCode;
        protected virtual void OnInvalidPostalCode(EventArgs e)
        {
            if (InvalidPostalCode != null)
                InvalidPostalCode(this, e);
        }

        public event EventHandler InvalidPhoneNumber;
        protected virtual void OnInvalidPhoneNumber(EventArgs e)
        {
            if (InvalidPhoneNumber != null)
                InvalidPhoneNumber(this, e);
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

        private ICommand _findProviderWithoutNavigationCommand;
        public ICommand FindProviderWithoutNavigationCommand
        {
            get
            {
                if (_findProviderWithoutNavigationCommand == null)
                {
                    _findProviderWithoutNavigationCommand = new MvxCommand(() =>
                    {
                        Searching = true;
                        _searchcomplete = _messenger.Subscribe<SearchForServiceProvidersComplete>((message) =>
                        {
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                            HandleSearchComplete(message, false);
                        });

                        _searcherror = _messenger.Subscribe<SearchForServiceProvidersError>((message) =>
                        {
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            OnNoProvidersFound(new EventArgs());
                        });

                        double? currentLat = null;
                        if (SelectedLocationType.TypeId == 1)
                        {
                            currentLat = Lat;
                        }
                        else
                        {
                            currentLat = null;
                        }

                        double? currentLong = null;
                        if (SelectedLocationType.TypeId == 1)
                        {
                            currentLong = Lng;
                        }
                        else
                        {
                            currentLong = null;
                        }

                        var validateLocation = new LocationValidator();
						if (!validateLocation.Validate<LocateServiceProviderViewModel>(this, "Address", "PostalCode", "SelectedLocationType").IsValid)
                        {
                            OnMissingAddressOrPostalCode(new EventArgs());
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                            Searching = false;
                            return;
                        }
                        else
                        {
                            // search by postal code
                            if (SelectedLocationType.TypeId == 3)
                            {
                                var validatePostalCode = new PostalCodeValidator();
								if (!validatePostalCode.Validate<LocateServiceProviderViewModel>(this, "PostalCode").IsValid)
                                {
                                    OnInvalidPostalCode(new EventArgs());
                                    _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                                    _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                                    Searching = false;
                                    return;
                                }
                            }
                            switch (SelectedSearchType.TypeId)
                            {
                                case 1: //All Providers
                                {
                                    _lookupservice.FindProviders(this.ProviderType, this.SearchRadius, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                case 2: //Phone Number
                                {
                                    var validatePhoneNumber = new PhoneNumberValidator();
                                    if (!validatePhoneNumber.Validate<LocateServiceProviderViewModel>(this, "PhoneNumber").IsValid)
                                    {
                                        OnInvalidPhoneNumber(new EventArgs());
                                        _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                                        _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                                        Searching = false;
                                        return;
                                    }
                                    else
                                    {
                                        _lookupservice.FindProviderByPhoneNumber(this.PhoneNumber, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    }
                                    break;
                                }
                                case 3: //Last Name
                                {
                                    _lookupservice.FindProviderByLastName(this.LastName, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                case 4: //Business Name
                                {
                                    _lookupservice.FindProviderByBusinessName(this.BusinessName, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                case 5: //City
                                {
                                    _lookupservice.FindProviderByCity(this.City, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                default:
                                {
                                    break;
                                }
                            }
                        }
                    }, () => !Searching && (this.SelectedLocationType.TypeId==1 ? (Lat > 0 || Lng > 0) : true));
                }
                return _findProviderWithoutNavigationCommand;
            }
        }

        private ICommand _findProviderCommand;
        public ICommand FindProviderCommand
        {
            get
            {
                if (_findProviderCommand == null)
                {
                    _findProviderCommand = new MvxCommand(() =>
                    {
                        Searching = true;
                        _searchcomplete = _messenger.Subscribe<SearchForServiceProvidersComplete>((message) =>
                        {
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                            HandleSearchComplete(message, true);
                        });

                        _searcherror = _messenger.Subscribe<SearchForServiceProvidersError>((message) =>
                        {
							OnNoProvidersFound(new EventArgs());
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                            Searching = false;
                           
                        });

                        double? currentLat = null;
                        if (SelectedLocationType.TypeId == 1)
                        {
                            currentLat = Lat;
                        }
                        else
                        {
                            currentLat = null;
                        }

                        double? currentLong = null;
                        if (SelectedLocationType.TypeId == 1)
                        {
                            currentLong = Lng;
                        }
                        else
                        {
                            currentLong = null;
                        }

                        var validateLocation = new LocationValidator();
                        if (!validateLocation.Validate<LocateServiceProviderViewModel>(this, "Address", "PostalCode", "SelectedLocationType").IsValid)
                        {
                            OnMissingAddressOrPostalCode(new EventArgs());
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                            Searching = false;
                            return;
                        }
                        else
                        {
                            // search by postal code
                            if (SelectedLocationType.TypeId == 3)
                            {
                                var validatePostalCode = new PostalCodeValidator();
                                if (!validatePostalCode.Validate<LocateServiceProviderViewModel>(this, "PostalCode").IsValid)
                                {
                                    OnInvalidPostalCode(new EventArgs());
                                    _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                                    _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                                    Searching = false;
                                    return;
                                }
                            }
                            switch (SelectedSearchType.TypeId)
                            {
                                case 1: //All Providers
                                {
                                    _lookupservice.FindProviders(this.ProviderType, this.SearchRadius, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                case 2: //Phone Number
                                {
                                    var validatePhoneNumber = new PhoneNumberValidator();
                                    if (!validatePhoneNumber.Validate<LocateServiceProviderViewModel>(this, "PhoneNumber").IsValid)
                                    {
                                        OnInvalidPhoneNumber(new EventArgs());
                                        _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                                        _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);
                                        Searching = false;
                                        return;
                                    }
                                    else
                                    {
                                        _lookupservice.FindProviderByPhoneNumber(this.PhoneNumber, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    }
                                    break;
                                }
                                case 3: //Last Name
                                {
                                    _lookupservice.FindProviderByLastName(this.LastName, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                case 4: //Business Name
                                {
                                    _lookupservice.FindProviderByBusinessName(this.BusinessName, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                case 5: //City
                                {
                                    _lookupservice.FindProviderByCity(this.City, this.SearchRadius, this.ProviderType, this.Address, this.PostalCode, currentLat, currentLong);
                                    break;
                                }
                                default:
                                {
                                    break;
                                }
                            }
                        }
                    }, () => !Searching && (this.SelectedLocationType.TypeId==1 ? (Lat > 0 || Lng > 0) : true));
                }
                return _findProviderCommand;
            }
        }
        #endregion

        #region Private Methods
        private void Unsubscribe()
        {
            _messenger.Unsubscribe<LocationUpdated>(_locationupdated);
            _messenger.Unsubscribe<RequestNavToServiceProviderMapView>(_showmapview);
            _messenger.Unsubscribe<LocateProviderSearchTypeSet>(_searchtypeset);
        }

        private void PopulateLocationTypes()
        {
            this.LocationTypes.Add(new LocationType() { TypeId = 1, TypeName = "My Current Location" });
            this.LocationTypes.Add(new LocationType() { TypeId = 2, TypeName = "Address" });
            this.LocationTypes.Add(new LocationType() { TypeId = 3, TypeName = "Postal Code" });
        }

        private void PopulateSearchTypes()
        {
            this.SearchTypes.Add(new SearchType() { TypeId = 1, TypeName = "No Filter" });
            this.SearchTypes.Add(new SearchType() { TypeId = 3, TypeName = "Last Name" });
            this.SearchTypes.Add(new SearchType() { TypeId = 4, TypeName = "Business Name" });
            this.SearchTypes.Add(new SearchType() { TypeId = 5, TypeName = "City" });
            this.SearchTypes.Add(new SearchType() { TypeId = 2, TypeName = "Phone Number" }); // switched order
        }

        private void SetHidingProperties()
        {
            if (_selectedLocationType.TypeName == "My Current Location")
            {
				ShowLocationTypeGps = false;
                ShowLocationTypePostalCode = true;
                ShowLocationTypeAddress = true;
                ShowSearchTypeAddressExample = false;
                ShowSearchTypePostalCodeExample = false;
            }

            if (_selectedLocationType.TypeName == "Address")
            {
				ShowLocationTypeGps = true;
                ShowLocationTypePostalCode = true;
                ShowLocationTypeAddress = false;
                ShowSearchTypeAddressExample = true;
                ShowSearchTypePostalCodeExample = false;

            }

            if (_selectedLocationType.TypeName == "Postal Code")
            {
				ShowLocationTypeGps = true;
                ShowLocationTypePostalCode = false;
                ShowLocationTypeAddress = true;
                ShowSearchTypeAddressExample = false;
                ShowSearchTypePostalCodeExample = true;
            }

            if (_selectedsearchtype.TypeName == "No Filter")
            {
                ShowSearchtypeCity = true;
                ShowSearchTypeLastName = true;
                ShowSearchTypeBusinessName = true;
                ShowSearchTypePhoneNumber = true;
            }

            if (_selectedsearchtype.TypeName == "Phone Number")
            {
                ShowSearchtypeCity = true;
                ShowSearchTypeLastName = true;
                ShowSearchTypeBusinessName = true;
                ShowSearchTypePhoneNumber = false;
                //ShowLocationTypeAddress = true;
            }

            if (_selectedsearchtype.TypeName == "Last Name")
            {
                ShowSearchtypeCity = true;
                ShowSearchTypeLastName = false;
                ShowSearchTypeBusinessName = true;
                ShowSearchTypePhoneNumber = true;
            }

            if (_selectedsearchtype.TypeName == "Business Name")
            {
                ShowSearchtypeCity = true;
                ShowSearchTypeLastName = true;
                ShowSearchTypeBusinessName = false;
                ShowSearchTypePhoneNumber = true;
                //ShowLocationTypeAddress = true;
            }

            if (_selectedsearchtype.TypeName == "City")
            {
                ShowSearchtypeCity = false;
                ShowSearchTypeLastName = true;
                ShowSearchTypeBusinessName = true;
                ShowSearchTypePhoneNumber = true;
                //ShowLocationTypeAddress = true;
            }
        }

        private void HandleSearchComplete(SearchForServiceProvidersComplete message, bool Navigate)
        {
            if (!message.NoResultsFound)
            {
                if (Navigate)
                {
                    //show the detail viewmodel
                    //_messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                    LocateServiceProviderLocatedProvidersViewModel.NavHelper nav = new LocateServiceProviderResultViewModel.NavHelper()
                    {
                        Address = this.Address,
                        City = this.City,
                        LastName = this.LastName,
                        BusinessName = this.BusinessName,
                        LocationType = this.SelectedLocationType != null ? this.SelectedLocationType.TypeName : "",
                        Phone = this.PhoneNumber,
                        PostalCode = this.PostalCode,
                        ProviderType = this.ProviderType.Type,
                        Radius = this.SearchRadius,
                        SearchType = this.SelectedSearchType != null ? this.SelectedSearchType.TypeName : "",
                        UsedDeviceLocation = !(this.ShowLocationTypePostalCode && this.ShowLocationTypeAddress)
                    };
                    _messenger.Publish<Messages.ClearLocateServiceProviderLocatedProviders>(new MobileClaims.Core.Messages.ClearLocateServiceProviderLocatedProviders(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    Searching = false;
                    ShowViewModel<LocateServiceProviderLocatedProvidersViewModel>(nav);
                }
                else
                {
                    RequestNavToServiceProviderSearchResults _message = new RequestNavToServiceProviderSearchResults(this)
                    {
                        Address = this.Address,
                        City = this.City,
                        LastName = this.LastName,
                        BusinessName = this.BusinessName,
                        LocationType = this.SelectedLocationType != null ? this.SelectedLocationType.TypeName : "",
                        Phone = this.PhoneNumber,
                        PostalCode = this.PhoneNumber,
                        ProviderType = this.ProviderType.Type,
                        Radius = this.SearchRadius,
                        SearchType = this.SelectedSearchType != null ? this.SelectedSearchType.TypeName : "",
                        UsedDeviceLocation = !(this.ShowLocationTypePostalCode && this.ShowLocationTypeAddress)
                    };
                    _messenger.Publish<Messages.ClearLocateServiceProviderLocatedProviders>(new MobileClaims.Core.Messages.ClearLocateServiceProviderLocatedProviders(this));
                    _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                    Searching = false;
                    _messenger.Publish<RequestNavToServiceProviderSearchResults>(_message);
                }
            }
        }
        #endregion
    }

    #region Validator Classes
    public class PhoneNumberValidator : AbstractValidator<LocateServiceProviderViewModel>
    {
        public PhoneNumberValidator()
        {
            RuleFor(loc => loc.PhoneNumber).Must(VerifyPhoneNumber);
        }

        private bool VerifyPhoneNumber(string phoneNumber)
        {
            bool validPhoneNumber = true;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                // Remove common phone number inputs
                string pureNumber = phoneNumber.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                // Verify if the phone number is 10 digits number that doesn't begin with 911, or 0
                Regex regex = new Regex(@"^(?!911|0)\d{10}$");

                if (!regex.Match(pureNumber).Success)
                {
                    validPhoneNumber = false;
                }
            }
            else
            {
                // Since no requirements for verifying if additional filter is empty or not
                // Assume when it's empty, leave filter as empty
            }
            return validPhoneNumber;
        }
    }

    public class PostalCodeValidator : AbstractValidator<LocateServiceProviderViewModel>
    {
        public PostalCodeValidator()
        {
            RuleFor(loc => loc.PostalCode).Must(VerifyPostalCode);
        }

        private bool VerifyPostalCode(string postalCode)
        {
            string _caPostalRegEx = @"^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$";
            bool validPostalCode = true;
            if (!Regex.Match(postalCode.ToUpper(), _caPostalRegEx).Success)
            {
                validPostalCode = false;
            }
            return validPostalCode;
        }
    }
    
    public class LocationValidator : AbstractValidator<LocateServiceProviderViewModel>
    {
        public LocationValidator()
        {
            RuleFor(loc => loc.Address).Must((loc, Address) => VerifyLocationSection(loc.Address, loc.PostalCode, loc.SelectedLocationType));
        }

        private bool VerifyLocationSection(string address, string postalCode, LocationType selectedLocationType)
        {
            bool isValidLocationSelection = false;
            if (selectedLocationType.TypeId > 1) //Current Location is not selected and user is entering a city or address
            {
                if ((!string.IsNullOrEmpty(postalCode)) || (!string.IsNullOrEmpty(address)))
                {
                    isValidLocationSelection = true;
                }
            }
            else
            {
                isValidLocationSelection = true; // Current location is selected
            }
            return isValidLocationSelection;
        }
    }
#endregion
}