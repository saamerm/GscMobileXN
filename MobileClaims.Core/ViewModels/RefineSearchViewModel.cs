using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class RefineSearchViewModel : ViewModelBase<SearchFilterData>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IGeocodingService _geocodingService;
        private ObservableCollection<ItemList> _locationSugestions;

        //TODO check if it can be changed to converter on iOS
        public event EventHandler LocationTextChanged;

        public ObservableCollection<ItemList> LocationSugestions
        {
            get => _locationSugestions;
            set
            {
                _locationSugestions = value;
                RaisePropertyChanged(() => LocationSugestions);
                RaisePropertyChanged(() => LocationSugestionsHeight);
                LocationTextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // ReSharper disable once NotAccessedField.Local
        private readonly MvxSubscriptionToken _subscriptionToken;

        // ReSharper disable once NotAccessedField.Local
        private readonly MvxSubscriptionToken _subscriptionGoToCurrentToken;

        private ItemList _selectedLocation;
        public ItemList SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                SetSelectedLocation(_selectedLocation, false);
            }
        }

        public RefineSearchViewModel(IMvxMessenger messenger, IGeocodingService geocodingService)            
        {
            _messenger = messenger;
            _geocodingService = geocodingService;
            SetRatingCommand = new MvxCommand<double>(ExecuteSetRatingCommand);
            SetLocationAsCurrentLocationCommand = new MvxCommand(SetCurrentLocation);
            CloseCommand = new MvxCommand(GoBack);
            _subscriptionToken = _messenger.Subscribe<ProviderTypeSelectedMessage>(OnProviderTypeSelected);
            _subscriptionGoToCurrentToken = _messenger.Subscribe<SearchGoToCurrentLocationMessage>(message => SetCurrentLocation());
        }

        public override void Prepare(SearchFilterData parameter)
        {
            base.Prepare(parameter);
            if (ViewModelParameter != null)
            {
                ViewModelParameter.SearchFilterDataChanged += SearchFilterDataOnSearchFilterDataChanged;
                LocationAdress = ViewModelParameter.LocationName;
            }
        }

        protected override void GoBack()
        {
            Unsubscribe();

            Close(this);
            OnStop();
        }

        private void Unsubscribe()
        {
            ViewModelParameter.SearchFilterDataChanged -= SearchFilterDataOnSearchFilterDataChanged;
            _messenger.Unsubscribe<ProviderTypeSelectedMessage>(_subscriptionToken);
            _messenger.Unsubscribe<SearchGoToCurrentLocationMessage>(_subscriptionGoToCurrentToken);
        }

        public void OnStop()
        {
            if (!string.IsNullOrWhiteSpace(LocationAdress))
            {
                SetSelectedLocation(new ItemList() { Title = SelectedLocation?.Title ?? LocationAdress }, false);
            }
            else
            {
                SetCurrentLocation();
            }
        }

        public string LocationAutocompleteHint => Resource.LocationAutocompleteHint;

        private string _locationAdress;
        public string LocationAdress
        {
            get => _locationAdress;
            set
            {
                _locationAdress = value;
                UpdateSuggestionsFromAddress(value);
                RaisePropertyChanged(() => LocationAdress);
            }
        }

        public bool IsProviderNotMyFavorites => ViewModelParameter.SelectedProviderType.Id != (int)ProvidersId.Favourites;

        public IMvxCommand ShowHealthProviderTypeListCommand => new MvxCommand(ShowHealthProviderTypeList);
        public IMvxCommand SetRatingCommand { get; }
        public IMvxCommand SetLocationAsCurrentLocationCommand { get; }
        public IMvxCommand CloseCommand { get; }

        public float LocationSugestionsHeight => (LocationSugestions?.Count ?? 0) * 20;

        private void SearchFilterDataOnSearchFilterDataChanged(object sender, EventArgs e)
        {
            _messenger.Publish(new SearchFilterDataChangedMessage(this, ViewModelParameter));
        }

        private void OnProviderTypeSelected(ProviderTypeSelectedMessage message)
        {
            ViewModelParameter.SelectedProviderType = message.SelectedProviderType;
            OnProviderTypeChanged?.Invoke();

        }
        public Action OnProviderTypeChanged { get; set; }

        private void SetCurrentLocation()
        {
            LocationAdress = null;
            ViewModelParameter.LocationName = null;
            ViewModelParameter.Location = null;
        }

        private void ExecuteSetRatingCommand(double val)
        {
            ViewModelParameter.StarRating = val;
        }

        private async Task UpdateSuggestionsFromAddress(string address)
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                var predictions = await _geocodingService.GetSuggestions(address);
                LocationSugestions =
                    new ObservableCollection<ItemList>(predictions.Select(p => new ItemList() { Title = p.Description }));
            }
            else
            {
                LocationSugestions = new ObservableCollection<ItemList>();
            }

        }

        private async Task SetSelectedLocation(ItemList selected, bool refreshSuggestions)
        {
            var location = await _geocodingService.GetLocations(selected.Title);
            if (location != null)
            {
                if (refreshSuggestions)
                {
                    LocationAdress = selected.Title;
                }
                else
                {
                    _locationAdress = selected.Title;
                    await RaisePropertyChanged(() => LocationAdress);
                }
                ViewModelParameter.LocationName = LocationAdress;
                ViewModelParameter.Location = location;
            }
        }

        private void ShowHealthProviderTypeList()
        {
            if (ViewModelParameter.SelectedProviderType != null)
            {
                ShowViewModel<HealthProviderTypeListViewModel, HealthProviderTypeViewModel>(ViewModelParameter.SelectedProviderType);
            }
        }

        public class ItemList
        {
            public string Title { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }
    }
}
