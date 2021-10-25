using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MobileClaims.Core.Helpers;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MobileClaims.Core.ViewModels
{
    public class FindHealthProviderViewModel : ViewModelBase<FindHealthProviderViewModelParameter>
    {
        public SearchHealthProviderViewModel SearchViewModel { get; }
        private readonly IMvxLog _log;
        private readonly ISearchHealthProviderService _searchHealthProviderService;
        private readonly IHealthProviderService _healthProviderService;
        private readonly IUserDialogs _userDialogs;
        private readonly IGeolocationService _geolocationService;
        private readonly INativeUrlService _nativeUrlService;
        private readonly Position _locationToronto = new Position(43.653908, -79.384293);
        private CancellationTokenSource _cancellationTokenSource;
        private IList<HealthProviderSummaryModel> _serviceProviders;
        public const int InitialRadiusInKm = 8;
        private const int EarthRadiusInKm = 6378;
        private double _currentDistance = InitialRadiusInKm;
        public const double LatitudeDifference = (double)InitialRadiusInKm / EarthRadiusInKm * 180 / Math.PI;
        public const string ProviderSearchKey = "ProviderSearchKey";
        private MvxSubscriptionToken _subscriptionToken;
        private MvxSubscriptionToken _updateMapSubscriptionToken;
        private MvxSubscriptionToken _subscriptionTokenProviderType;
        private MvxSubscriptionToken _favouriteProviderSubscriptionToken;
        private MvxSubscriptionToken _centerToServiceProviderToken;
        private MvxSubscriptionToken _popToRootSubscriptionToken;
        private ProvidersId? _providerSearch;

        public event EventHandler<GeolocationResultEventArgs> UserPositionChanged;
        public event EventHandler ServiceProvidersChanged;

        public string LearnMoreText => Resource.LearnMore;
        public string UsingTheHealthProviderText => Resource.UsingTheHealthProvider;
        public string ClickHereForDetailsText => Resource.ClickHereForDetails;
        public string UnderstandingPharmacyText => Resource.UnderstandingPharmacy;
        public string EverythingYouNeedToKnowText => BrandResource.EverythingYouNeedToKnow;
        public string CloseText => Resource.Close;
        public string ProvidersNotFoundWithCurrentCriteriaText => Resource.ProvidersNotFoundWithCurrentCriteria;

        public IMvxMessenger Messenger { get; }

        public double CurrentDistance
        {
            get => _currentDistance;
            set => SetProperty(ref _currentDistance, value);
        }

        private bool _learnMoreDialogVisible;

        public bool LearnMoreDialogVisible
        {
            get => _learnMoreDialogVisible;
            set => SetProperty(ref _learnMoreDialogVisible, value);
        }

        private bool _progressIndicatorVisible;

        public bool ProgressIndicatorVisible
        {
            get => _progressIndicatorVisible;
            set => SetProperty(ref _progressIndicatorVisible, value);
        }

        private bool _isShowDetailsListButtonEnabled = false;

        public bool IsShowDetailsListButtonEnabled
        {
            get => _isShowDetailsListButtonEnabled;
            set => SetProperty(ref _isShowDetailsListButtonEnabled, value);
        }

        private bool _showProvidersNotFoundPopUp;

        public bool ShowProvidersNotFoundPopUp
        {
            get => _showProvidersNotFoundPopUp;
            set => SetProperty(ref _showProvidersNotFoundPopUp, value);
        }

        public bool IsEnglishLanguage { get; set; }

        public GeolocationResult UserPosition { get; set; }

        private Location _mapPosition;

        public Location MapPosition
        {
            get => _mapPosition ?? new Location()
            {
                Lat = UserPosition.Position.Latitude,
                Lng = UserPosition.Position.Longitude
            };
            set
            {
                _mapPosition = value;
                RaisePropertyChanged(() => MapPosition);
            }
        }

        public Location UserMarkerPosition => SearchFilterData.Location ??
                                              new Location()
                                              {
                                                  Lat = UserPosition.Position.Latitude,
                                                  Lng = UserPosition.Position.Longitude
                                              };

        public event EventHandler OnMapLocationChanged;
        public event EventHandler OnCenterMapToLocationChanged;

        public IList<HealthProviderSummaryModel> ServiceProviders
        {
            get => _serviceProviders;
            set
            {
                _serviceProviders = value;
                ServiceProvidersChanged?.Invoke(this, EventArgs.Empty);
                RaisePropertyChanged(() => ServiceProviders);
            }
        }

        public SearchFilterData SearchFilterData { get; private set; } = new SearchFilterData();

        public IMvxAsyncCommand<bool> RefreshHealthProvidersCommand { get; }

        public IMvxAsyncCommand ShowRefineSearchCommand { get; }

        public IMvxAsyncCommand ShowDetailsListCommand { get; }

        public IMvxAsyncCommand<HealthProviderSummaryModel> ShowProviderDetailsInformationCommand { get; }

        public event EventHandler<int> FavouriteProviderRefreshed;

        public FindHealthProviderViewModel(
            SearchHealthProviderViewModel searchViewModel,
            IGeolocationService geolocationService,
            ISearchHealthProviderService searchHealthProviderService,
            IHealthProviderService healthProviderService,
            IUserDialogs userDialogs,
            INativeUrlService nativeUrlService,
            IMvxMessenger messenger,
            ILanguageService languageService,
            IMvxLog log)
        {
            SearchViewModel = searchViewModel;
            _searchHealthProviderService = searchHealthProviderService;
            _nativeUrlService = nativeUrlService;
            _healthProviderService = healthProviderService;
            _userDialogs = userDialogs;
            Messenger = messenger;
            _geolocationService = geolocationService;
            _log = log;
            RefreshHealthProvidersCommand = new MvxAsyncCommand<bool>(async (refreshMapPosition) => await RefreshHealthProviders(refreshMapPosition));
            ShowRefineSearchCommand = new MvxAsyncCommand(ShowRefineSearch);
            ShowDetailsListCommand = new MvxAsyncCommand(ShowDetailsList);
            ShowProviderDetailsInformationCommand = new MvxAsyncCommand<HealthProviderSummaryModel>(async (healthProvider) => await ShowProviderDetailsInformation(healthProvider));
            searchViewModel.PerformSearchCommand = RefreshHealthProvidersCommand;
            searchViewModel.ShowRefineSearchCommand = ShowRefineSearchCommand;
            IsEnglishLanguage = languageService.IsEnglishLanguage;
            Subscribe();
        }

        private void OnFavouriteHealthProviderChanged(FavouriteHealthProviderChange msg)
        {
            var provider = ServiceProviders.SingleOrDefault(p => p.Model.ProviderId == msg.ProviderId);

            if (provider != null)
            {
                provider.IsFavourite = msg.IsFavourite;
                FavouriteProviderRefreshed?.Invoke(this, ServiceProviders.IndexOf(provider));
            }
        }

        private void OnCenterMapToServiceProvider(CenterMapToProviderArgs args)
        {
            var provider = ServiceProviders.FirstOrDefault(p => p.Model.ProviderId == args.ProviderId);
            if (provider != null)
            {
                MapPosition = new Location() { Lat = provider.Model.Latitude, Lng = provider.Model.Longitude };
                OnCenterMapToLocationChanged?.Invoke(this, new MapLocationChangedEventArgs(MapPosition, false));
            }
        }

        private void OnUpdateMapMessageReceived(UpdateMapMessage updateMapMessage)
        {
            if (updateMapMessage.Sender.ToString() == "Pharmacy" && _providerSearch != ProvidersId.Pharmacy)
            {
                _providerSearch = ProvidersId.Pharmacy;
                InitSearchParams();
            }
            else if (updateMapMessage.Sender.ToString() == "Favourites" && _providerSearch != ProvidersId.Favourites)
            {
                _providerSearch = ProvidersId.Favourites;
                InitSearchParams();
            }
        }

        public async Task ShowProviderDetailsInformation(HealthProviderSummaryModel provider)
        {
            try
            {
                ProgressIndicatorVisible = true;
                var detailedProviderEntity = await _healthProviderService.GetProviderDetails(provider.Model.ProviderId);
                if (detailedProviderEntity == null)
                {
                    throw new NullResponseException();
                }

                var detailedProvider = new HealthProviderSummaryModel(detailedProviderEntity, _searchHealthProviderService);                
                await ShowViewModel<ProviderDetailsInformationViewModel, HealthProviderSummaryModel>(detailedProvider);
                ProgressIndicatorVisible = false;
            }
            catch (NullResponseException)
            {
                ProgressIndicatorVisible = false;
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
        }

        private void OnProviderTypeSelected(ProviderTypeSelectedMessage message)
        {
            if (!SearchFilterData.SelectedProviderType.Equals(message.SelectedProviderType))
            {
                SearchViewModel.SearchQuery = null;
            }

            SearchFilterData.SelectedProviderType = message.SelectedProviderType;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            ProgressIndicatorVisible = true;

            await InitSearchParams();
        }

        private async Task InitSearchParams()
        {
            try
            {
                await _searchHealthProviderService.ResetProviderTypes();
            }
            catch (NullResponseException)
            {
                ProgressIndicatorVisible = false;
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
                return;
            }

            if (_providerSearch.HasValue)
            {
                SetSearchParams(_providerSearch.Value);
            }
            else if (SearchFilterData.SelectedProviderType == null)
            {
                SetSearchParams(ProvidersId.Favourites);
            }

            await RaisePropertyChanged(() => ShowRefineSearchCommand);

            if (_providerSearch == null)
            {
                await ShowRefineSearch();
            }

            await GetLocation();
            RefreshHealthProviders(false);
        }

        public event EventHandler ViewModelWillDestroy;

        protected override void GoBack()
        {
            ViewModelWillDestroy?.Invoke(this, EventArgs.Empty);
            Unsubscribe();
            base.GoBack();
        }

        public override void Prepare(FindHealthProviderViewModelParameter parameter)
        {
            _providerSearch = parameter.ProviderSearchKey;
        }

        private void OnSearchFilterDataChanged(SearchFilterDataChangedMessage message)
        {
            SearchFilterData = message.SearchFilterData;
            RefreshHealthProviders(true);
        }

        private async Task ShowDetailsList()
        {
            ProgressIndicatorVisible = true;
            await ShowViewModel<ServiceDetailsListViewModel, IList<HealthProviderSummaryModel>>(ServiceProviders);
            ProgressIndicatorVisible = false;
        }

        private async Task ShowRefineSearch()
        {
            if (SearchFilterData == null || SearchFilterData.SelectedProviderType == null)
            {
                return;
            }

            await ShowViewModel<RefineSearchViewModel, SearchFilterData>(SearchFilterData);
        }

        private async Task RefreshHealthProviders(bool refreshMapPosition)
        {
            if ((UserPosition == null || MapPosition == null) && SearchFilterData.Location == null ||
                SearchFilterData.SelectedProviderType == null)
            {
                return;
            }

            if (_cancellationTokenSource != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
            }

            //TODO: we should introduce threshold to not call it when previous and current distance aren't much different 
            try
            {
                ProgressIndicatorVisible = true;
                ShowProvidersNotFoundPopUp = ShowProvidersNotFoundPopUp && !ProgressIndicatorVisible;

                _cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _cancellationTokenSource.Token;

                var rating = SearchFilterData.IsStarRating &&
                             SearchFilterData.SelectedProviderType?.Id == (int)ProvidersId.Pharmacy
                    ? SearchFilterData.StarRating
                    : (double?)null;

                var sortType = SearchFilterData.IsDisplayRating
                    ? SearchFilterData.SortByChoicePharmacy
                    : SearchFilterData.SortByChoice;

                var latitude = (refreshMapPosition ? SearchFilterData.Location?.Lat : MapPosition?.Lat) ??
                               UserPosition.Position.Latitude;
                var longitude = (refreshMapPosition ? SearchFilterData.Location?.Lng : MapPosition?.Lng) ??
                                UserPosition.Position.Longitude;

                var parameters = new HealthProviderSearchParameters()
                {
                    Distance = CurrentDistance,
                    Latitude = latitude,
                    Longitude = longitude,
                    SortByChoice = sortType,
                    PageIndex = 0,
                    ProviderRating = rating,
                    SearchQuery = SearchViewModel.SearchQuery?.Trim(),
                    IsDirectBill = SearchFilterData.IsDirectBill,
                    IsRecentlyVisited = SearchFilterData.IsRecentlyVisited,
                    SearchTypeChoice = SearchFilterData.SelectedProviderType?.Id == (int)ProvidersId.Favourites
                        ? SearchTypeChoice.PlanMembersFavoriteProviders
                        : SearchTypeChoice.ByProviderType,
                    ProviderTypeCodes = SearchFilterData.SelectedProviderType?.ProviderTypeCodes,
                    ProviderTypeId = (int)SearchFilterData.SelectedProviderType?.Id
                };

                var healthProviders = _searchHealthProviderService.GetHealthProviders(parameters)
                    .WithCancellation(cancellationToken);

                ServiceProviders = await healthProviders;
                ServiceProviders = ServiceProviders.Distinct().ToList();

                ShowProvidersNotFoundPopUp = ServiceProviders.Count == 0;
                ShowPharmacyUnderstanding = ServiceProviders.Any(x => x.DisplayRating);

                MapPosition = new Location() { Lat = parameters.Latitude, Lng = parameters.Longitude };
                OnMapLocationChanged?.Invoke(this, new MapLocationChangedEventArgs(MapPosition, refreshMapPosition));

                Messenger.Publish(new HealthProvidersRefreshedMessage(this, ServiceProviders));

                ProgressIndicatorVisible = false;
            }
            catch (ApiException)
            {
                ProgressIndicatorVisible = false;
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
            catch (OperationCanceledException)
            {
                //do nothing, we have cancelled old request due to a new that is being processed
            }

            IsShowDetailsListButtonEnabled = ServiceProviders != null && ServiceProviders.Any();
            ShowProvidersNotFoundPopUp = ShowProvidersNotFoundPopUp && !ProgressIndicatorVisible;
        }

        private void SetSearchParams(ProvidersId provider)
        {
            var providerFavourites =
                _searchHealthProviderService.ProviderTypes?.FirstOrDefault(p => p.Id == (int)provider);
            PopulateSearchParams(providerFavourites);
        }

        private void PopulateSearchParams(HealthProviderTypeViewModel provider)
        {
            SearchFilterData.SelectedProviderType = new HealthProviderTypeViewModel()
            {
                Title = provider?.Title,
                ChildItems = provider?.ChildItems.Select(
                    p => new HealthProviderTypeViewModel()
                    {
                        Id = p.Id,
                        ImageUrl = p.ImageUrl,
                        ProviderTypeCodes = p.ProviderTypeCodes,
                        LineOfBusinessCode = p.LineOfBusinessCode,
                        ParentId = p.ParentId,
                        Title = p.Title,
                        SortOrder = p.SortOrder
                    }).ToList(),
                Id = provider?.Id ?? 0,
                DisplayRating = provider?.DisplayRating ?? false,
                IsSelected = true,
                LineOfBusinessCode = provider?.LineOfBusinessCode,
                ProviderTypeCodes = provider?.ProviderTypeCodes
            };
        }

        public ICommand GoUsingTheSearchProviderCommand => new MvxCommand(GoUsingTheSearchProvider);
        public ICommand GoUnderstandingPharmacyCommand => new MvxCommand(GoUnderstandingPharmacy);
        public ICommand GoShowHideLearnMoreCommand => new MvxCommand(GoShowHideLearnMore);

        private bool _showPharmacyUnderstanding;

        public bool ShowPharmacyUnderstanding
        {
            get => _showPharmacyUnderstanding;
            set => SetProperty(ref _showPharmacyUnderstanding, value);
        }

        private void GoUsingTheSearchProvider()
        {
            _nativeUrlService.OpenUrl(BrandResource.Find_a_health_provider_faq_url);
            LearnMoreDialogVisible = false;
        }

        private void GoUnderstandingPharmacy()
        {
            _nativeUrlService.OpenUrl(BrandResource.Understanding_pharmacy_quality_ratings_url);
            LearnMoreDialogVisible = false;
        }

        private void GoShowHideLearnMore()
        {
            LearnMoreDialogVisible = !LearnMoreDialogVisible;
        }

        private async Task GetLocation()
        {
            try
            {
                var geolocationResult = await _geolocationService.GetGeolocationAsync(false);
                if (geolocationResult.Status == GeolocationStatus.PermissionNotGranted)
                {
                    await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                }

                var locationResult = await _geolocationService.GetGeolocationAsync(true);
                if (locationResult.Status == GeolocationStatus.SuccessGps ||
                    locationResult.Status == GeolocationStatus.SuccessIp)
                {
                    UserPosition = locationResult;
                }
                else
                {
                    UserPosition = new GeolocationResult(GeolocationStatus.UnableToGetLocation, _locationToronto);
                }

                await RaisePropertyChanged(() => MapPosition);
                UserPositionChanged?.Invoke(this, new GeolocationResultEventArgs(UserPosition));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }
        }

        public class GeolocationResultEventArgs : EventArgs
        {
            public GeolocationResultEventArgs(GeolocationResult geolocationResult)
            {
                GeolocationResult = geolocationResult;
            }

            public GeolocationResult GeolocationResult { get; }
        }

        public class MapLocationChangedEventArgs : EventArgs
        {
            public MapLocationChangedEventArgs(Location location, bool refreshMap)
            {
                Location = location;
                RefreshMap = refreshMap;
            }

            public Location Location { get; }

            public bool RefreshMap { get; }
        }

        public HealthProviderSummaryModel GetSelectedProvider(int providerId)
        {
            return ServiceProviders.Single(x => x.Model.ProviderId == providerId);
        }

        private void Subscribe()
        {
            _subscriptionToken = Messenger.Subscribe<SearchFilterDataChangedMessage>(OnSearchFilterDataChanged);
            _subscriptionTokenProviderType = Messenger.Subscribe<ProviderTypeSelectedMessage>(OnProviderTypeSelected);
            _favouriteProviderSubscriptionToken =
                Messenger.Subscribe<FavouriteHealthProviderChange>(OnFavouriteHealthProviderChanged);
            _popToRootSubscriptionToken = Messenger.Subscribe<RequestNavToRoot>(root => GoBack());
            _centerToServiceProviderToken = Messenger.Subscribe<CenterMapToProviderArgs>(OnCenterMapToServiceProvider);
            _updateMapSubscriptionToken = Messenger.Subscribe<UpdateMapMessage>(OnUpdateMapMessageReceived);
        }

        private void Unsubscribe()
        {
            Messenger.Unsubscribe<SearchFilterDataChangedMessage>(_subscriptionToken);
            Messenger.Unsubscribe<ProviderTypeSelectedMessage>(_subscriptionTokenProviderType);
            Messenger.Unsubscribe<FavouriteHealthProviderChange>(_favouriteProviderSubscriptionToken);
            Messenger.Unsubscribe<RequestNavToRoot>(_popToRootSubscriptionToken);
            Messenger.Unsubscribe<CenterMapToProviderArgs>(_centerToServiceProviderToken);
            Messenger.Unsubscribe<UpdateMapMessage>(_updateMapSubscriptionToken);
        }

        public async Task onAndroidPharmacyOrFavourite(ProvidersId providersId)
        {
            ProgressIndicatorVisible = true;
            _providerSearch = providersId;
            await InitSearchParams();
            ProgressIndicatorVisible = false;
        }

        public async Task onAndroidHealthProvider()
        {
            ProgressIndicatorVisible = true;
            _providerSearch = null;
            await InitSearchParams();
            ProgressIndicatorVisible = false;
        }
    }
}