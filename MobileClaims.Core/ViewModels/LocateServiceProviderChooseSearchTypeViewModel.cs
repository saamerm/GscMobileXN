using MobileClaims.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Entities;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public sealed class LocateServiceProviderChooseSearchTypeViewModel : ViewModelBase
    {
        #region Member variables
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _lookupservice;
        private readonly MvxSubscriptionToken _navtolocatedproviders;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private object _sync = new object();
        #endregion

        #region ctors
        public LocateServiceProviderChooseSearchTypeViewModel(IMvxMessenger messenger, IProviderLookupService lookupservice)
        {
            _messenger = messenger;
            _lookupservice = lookupservice;
            this.LocationTypes = new List<LocationType>();

            this.SearchTypes = new List<SearchType>();
            PopulateLocationTypes();
            PopulateSearchTypes();

            _navtolocatedproviders = _messenger.Subscribe<RequestNavToServiceProviderSearchResults>((message) =>
            {
                //_messenger.Unsubscribe<RequestNavToServiceProviderSearchResults>(_navtolocatedproviders);
                LocateServiceProviderLocatedProvidersViewModel.NavHelper nav = new LocateServiceProviderResultViewModel.NavHelper()
                {
                    Address = message.Address,
                    BusinessName = message.BusinessName,
                    City = message.City,
                    LastName = message.LastName,
                    LocationType = message.LocationType,
                    Phone = message.Phone,
                    PostalCode =  message.PostalCode,
                    ProviderType =  message.ProviderType,
                    Radius = message.Radius,
                    SearchType = message.SearchType,
                    serviceProviderID = message.serviceProviderID,
                    UsedDeviceLocation = message.UsedDeviceLocation
                };
                ShowViewModel<LocateServiceProviderLocatedProvidersViewModel>(nav);
            });

            _shouldcloseself = _messenger.Subscribe<ClearLocateServiceProviderChooseSearchType>((message) =>
            {
                _messenger.Unsubscribe<ClearLocateServiceProviderChooseSearchType>(_shouldcloseself);
                Unsubscribe();
                Close(this);
            });
        }

        public void Init(ProviderSearchNavigationHelper navhelper)
        {
            if (navhelper == null)
                return;
            this.ProviderType = _lookupservice.ServiceProviderTypes.Where(pt => pt.ID == navhelper.ProviderTypeID).FirstOrDefault();
            this.SelectedSearchType = _lookupservice.SearchTypeID >0 ? this.SearchTypes.Where(st => st.TypeId == _lookupservice.SearchTypeID).FirstOrDefault() : this.SearchTypes.First();
            this.SelectedLocationType = _lookupservice.SearchTypeID > 0 ? this.LocationTypes.Where(lt => lt.TypeId == _lookupservice.LocationTypeID).FirstOrDefault() : this.LocationTypes.First();

            // HasNavigated = true;
            //this.SetLocationTypeWithoutNavigationCommand.Execute(_lookupservice.LocationTypeID > 0 ? this.LocationTypes.Where(lt => lt.TypeId == _lookupservice.LocationTypeID).FirstOrDefault() : this.LocationTypes.First());
            //this.SetSearchTypeWithoutNavigationCommand.Execute(_lookupservice.SearchTypeID > 0 ? this.SearchTypes.Where(st => st.TypeId == _lookupservice.SearchTypeID).FirstOrDefault() : this.SearchTypes.First());
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
        // Making has navigated public so that windows view will be able to update this
        private bool _hasNavigated = false;
        public bool HasNavigated
        {
            get { return _hasNavigated; }
            set
            {
                _hasNavigated = value;
                RaisePropertyChanged(() => HasNavigated);
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

        public ICommand SetLocationTypeWithNavigationCommand
        {
            get
            {
                return new MvxCommand<LocationType>((lType) =>
                {
                    this.SelectedLocationType = lType;
                    _lookupservice.LocationTypeID = lType.TypeId;
                    if(this.SelectedSearchType != null)
                    {
                        _lookupservice.SearchTypeID = this.SelectedSearchType.TypeId;
                    }
                    if (SelectedSearchType != null && ProviderType != null)
                    {
                        LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper();
                        nav.ProviderTypeID = this.ProviderType.ID;
						ShowViewModel<LocateServiceProviderViewModel>(nav);
						//this.ShowViewModel<LocateServiceProviderViewModel>();
                        HasNavigated = true;
                    }
                });
            }
        }

        public ICommand SetLocationTypeWithoutNavigationCommand
        {
            get
            {
                return new MvxCommand<LocationType>((lType) =>
                {
                    SelectedLocationType = lType;
                    if (!HasNavigated && SelectedSearchType != null && ProviderType != null)
                    {
                        _lookupservice.LocationTypeID = lType.TypeId;
                        _lookupservice.SearchTypeID = this.SelectedSearchType.TypeId;
                        LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper()
                        {
                            LocationTypeID = lType.TypeId,
                            ProviderTypeID = ProviderType.ID,
                            SearchTypeID = SelectedSearchType.TypeId
                        };
                        HasNavigated = true;
                        ShowViewModel<LocateServiceProviderViewModel>(nav);
                        return;
                    }
                    if (SelectedSearchType != null && ProviderType != null)
                    {
                        _messenger.Publish<LocateProviderSearchTypeSet>(new LocateProviderSearchTypeSet(this)
                        {
                            LocationType = lType,
                            ProviderType = ProviderType,
                            SearchType = SelectedSearchType
                        });
                    }	
                });
            }
        }

        // Not sure what SetLocationTypeWithoutNavigationCommand does, but this is actually doing the setting without navigating
        // Created for windows phone
        public ICommand SetLocationTypeCommand
        {
            get
            {
                return new MvxCommand<LocationType>((lType) =>
                {
                    this.SelectedLocationType = lType;
                    _lookupservice.LocationTypeID = lType.TypeId;

                    if (this.SelectedSearchType != null && ProviderType != null)
                    {
                        _lookupservice.SearchTypeID = this.SelectedSearchType.TypeId;
                        _messenger.Publish<LocateProviderSearchTypeSet>(new LocateProviderSearchTypeSet(this)
                        {
                            LocationType = lType,
                            ProviderType = ProviderType,
                            SearchType = this.SelectedSearchType
                        });
                    }
                });
            }
        }

        // Not sure what SetSearchTypeWithoutNavigationCommand does, but this is actually doing the setting without navigating
        // Created for windows phone
        public ICommand SetSearchTypeCommand
        {
            get
            {
                return new MvxCommand<SearchType>((sType) =>
                {
                    this.SelectedSearchType = sType;
                    _lookupservice.SearchTypeID = sType.TypeId;
                    if (SelectedLocationType != null && ProviderType != null)
                    {
                        _lookupservice.LocationTypeID = this.SelectedLocationType.TypeId;
                        _messenger.Publish<LocateProviderSearchTypeSet>(new LocateProviderSearchTypeSet(this)
                        {
                            SearchType = sType,
                            LocationType = this.SelectedLocationType,
                            ProviderType = ProviderType
                        });
                    }
                });
            }
        }

        public ICommand SetSearchTypeWithNavigationCommand
        {
            get
            {
                return new MvxCommand<SearchType>((sType) =>
                {
                    this.SelectedSearchType = sType;
                    _lookupservice.SearchTypeID = sType.TypeId;
                    if(SelectedLocationType != null)
                    {
                        _lookupservice.LocationTypeID = SelectedLocationType.TypeId;
                    }
                    if (SelectedLocationType != null && ProviderType != null)
                    {
                        LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper();
                        nav.ProviderTypeID = this.ProviderType.ID;
                        nav.SearchTypeID = sType.TypeId;
                        nav.LocationTypeID = SelectedLocationType.TypeId;
                        ShowViewModel<LocateServiceProviderViewModel>(nav);
                    }
                    /*LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new ProviderSearchNavigationHelper()
                    {
                        ProviderTypeID = this.ProviderType.ID,
                        SearchTypeID = sType.TypeId
                    };
                    ShowViewModel<LocateServiceProviderViewModel>(nav);*/
                });
            }
        }

        public ICommand SetSearchTypeWithoutNavigationCommand
        {
            get
            {
                return new MvxCommand<SearchType>((sType) =>
                {

                    if(!HasNavigated && SelectedLocationType != null && ProviderType != null)
                    {
                        _lookupservice.SearchTypeID = sType.TypeId;
                        _lookupservice.LocationTypeID = SelectedLocationType.TypeId;
                        LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper()
                        {
                            ProviderTypeID=  this.ProviderType.ID,
                            SearchTypeID = sType.TypeId,
                            LocationTypeID = SelectedLocationType.TypeId
                        };
                        HasNavigated = true;
                        ShowViewModel<LocateServiceProviderViewModel>(nav);
                    }
                    this.SelectedSearchType = sType;
                    if(SelectedLocationType != null && ProviderType !=null)
                    {
                        _messenger.Publish<LocateProviderSearchTypeSet>(new LocateProviderSearchTypeSet(this)
                        {
                            SearchType = sType,
                            LocationType = SelectedLocationType,
                            ProviderType = ProviderType
                        });
                    }
                });
            }
        }

        // This command will always navigate - used for the Continue button to confirm current selection
        public ICommand FindProviderCommand
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        if (ProviderType != null)
                        {
                            LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper()
                            {
                                ProviderTypeID = ProviderType.ID,
                                SearchTypeID = _lookupservice.SearchTypeID,
                                LocationTypeID = _lookupservice.LocationTypeID
                            };
                            HasNavigated = true;
                            _messenger.Publish<Messages.ClearLocateServiceProvider>(new MobileClaims.Core.Messages.ClearLocateServiceProvider(this));
                            _messenger.Publish<Messages.ClearLocateServiceProviderLocatedProviders>(new MobileClaims.Core.Messages.ClearLocateServiceProviderLocatedProviders(this));
                            _messenger.Publish<Messages.ClearLocateServiceProviderResult>(new MobileClaims.Core.Messages.ClearLocateServiceProviderResult(this));
                            ShowViewModel<LocateServiceProviderViewModel>(nav);
                        }
                    });
            }
        }
        #endregion

        #region Helper Functions
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

        // Used in presenter to display sub dependent view
        public void ShowLocateServiceProvider()
        {
            LocateServiceProviderViewModel.ProviderSearchNavigationHelper nav = new LocateServiceProviderViewModel.ProviderSearchNavigationHelper();
            nav.ProviderTypeID = this.ProviderType.ID;
            nav.SearchTypeID = this.SelectedSearchType.TypeId;
            nav.LocationTypeID = this.SelectedLocationType.TypeId;
            HasNavigated = true;
            ShowViewModel<LocateServiceProviderViewModel>(nav);
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<RequestNavToServiceProviderSearchResults>(_navtolocatedproviders);
        }
        #endregion
    }
}
