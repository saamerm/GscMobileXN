using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Helpers;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Responses;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Helper;
using MobileClaims.iOS.Services;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using UIKit;
using static MobileClaims.Core.ViewModels.FindHealthProviderViewModel;

namespace MobileClaims.iOS.Views
{
    public partial class FindHealthProviderViewController : GSCBaseViewController, IUITextFieldDelegate
    {
        private FindHealthProviderViewModel _viewModel;
        private MapView _googleMapView;
        private int _tabbarHeight;
        private List<ProviderMarker> _markers;

        public UIView MapView => MapHolderView;

        private Marker _userPosition;

        private bool _progressIndicatorVisible;

        public bool ProgressIndicatorVisible
        {
            get => _progressIndicatorVisible;
            set
            {
                _progressIndicatorVisible = value;
                StartOrStopProgressIndicator();
            }
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            View.EndEditing(true);
            _viewModel.SearchViewModel.PerformSearchCommand.Execute(false);
            return false;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.NavigationController.NavigationBarHidden = true;

            Initialize();

            SetupMap();

            alertNoProvidersFoundLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 16);
            alertNoProvidersFoundLabel.LayoutMargins = new UIEdgeInsets(4, 4, 4, 4);

            var transform = CGAffineTransform.MakeScale(1.0f, ProgressView.Frame.Height);
            ProgressView.Transform = transform;

            _viewModel.ServiceProvidersChanged += ServiceProvidersChanged;

            var usingTheHealthProviderTapGestureRecognizer = new UITapGestureRecognizer(() => _viewModel.GoUsingTheSearchProviderCommand.Execute(null));
            var understandingPharmacyTapGestureRecognizer = new UITapGestureRecognizer(() => _viewModel.GoUnderstandingPharmacyCommand.Execute(null));
            UsingTheHealthProviderContainer.AddGestureRecognizer(usingTheHealthProviderTapGestureRecognizer);
            UnderstandingPharmacyContainer.AddGestureRecognizer(understandingPharmacyTapGestureRecognizer);

            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var set = this.CreateBindingSet<FindHealthProviderViewController, FindHealthProviderViewModel>();

            set.Bind(ShowDetailsList).To(vm => vm.ShowDetailsListCommand);
            set.Bind(ShowDetailsList).For(prop => prop.Enabled).To(vm => vm.IsShowDetailsListButtonEnabled);
            set.Bind(ShowDetailsList).For(prop => prop.Hidden).To(vm => vm.IsShowDetailsListButtonEnabled).WithConversion(boolOppositeValueConverter);
            set.Bind(searchTextField).To(vm => vm.SearchViewModel.SearchQuery);
            set.Bind(searchTextField).For("Placeholder").To(vm => vm.SearchViewModel.SearchHintText);
            set.Bind(searchButton).To(vm => vm.SearchViewModel.PerformSearchCommand);
            set.Bind(hamburgerButton).To(vm => vm.ShowRefineSearchCommand);
            set.Bind(this).For(v => v.ProgressIndicatorVisible).To(vm => vm.ProgressIndicatorVisible);

            set.Bind(LearnMoreDialog).For(x => x.Hidden).To(vm => vm.LearnMoreDialogVisible).WithConversion(boolOppositeValueConverter);
            set.Bind(UsingTheHealthProviderHeaderLabel).To(vm => vm.UsingTheHealthProviderText);
            set.Bind(ClickHereForDetailsLabel).To(vm => vm.ClickHereForDetailsText);
            set.Bind(LearnMoreHeaderLabel).To(vm => vm.LearnMoreText);
            set.Bind(UnderstandingPharmacyContainer).For(x => x.Hidden).To(vm => vm.ShowPharmacyUnderstanding).WithConversion(boolOppositeValueConverter);
            set.Bind(UnderstandingPharmacyHeaderLabel).To(vm => vm.UnderstandingPharmacyText);
            set.Bind(EverythingYouNeedToKnowLabel).To(vm => vm.EverythingYouNeedToKnowText);
            set.Bind(CloseButton).To(vm => vm.GoShowHideLearnMoreCommand);
            set.Bind(CloseButton).For("Title").To(vm => vm.CloseText);
            set.Bind(ShowLearnMoreButton).To(vm => vm.GoShowHideLearnMoreCommand);
            set.Bind(alertNoProvidersFoundView).For(x => x.Hidden).To(vm => vm.ShowProvidersNotFoundPopUp).WithConversion(boolOppositeValueConverter);
            set.Bind(alertNoProvidersFoundLabel).To(vm => vm.ProvidersNotFoundWithCurrentCriteriaText);

            set.Apply();

            _viewModel.OnMapLocationChanged += OnMapLocationChanged;
            _viewModel.OnCenterMapToLocationChanged += OnCenterMapToLocationChanged;
            _viewModel.FavouriteProviderRefreshed += OnFavouriteProviderRefreshed;
            _viewModel.SearchViewModel.ClearTextBox += SearchViewModel_ClearTextBox;

            if (_viewModel.UserPosition != null)
            {
                UserPositionChanged(this, new GeolocationResultEventArgs(_viewModel.UserPosition));
                _viewModel.UserPositionChanged -= UserPositionChanged;
            }
            else
            {
                _viewModel.UserPositionChanged += UserPositionChanged;
            }

            LearnMoreDialog.Layer.CornerRadius = 5;
            LearnMoreDialog.Layer.MasksToBounds = true;

            _viewModel.ViewModelWillDestroy += ViewModelOnViewModelWillDestroy;
        }

        private void ViewModelOnViewModelWillDestroy(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        public override void ViewWillDisappear(bool animated)
        {
            searchTextField.EndEditing(true);

            base.ViewWillDisappear(animated);
        }

        private void Unsubscribe()
        {
            _viewModel.ServiceProvidersChanged -= ServiceProvidersChanged;
            _viewModel.OnMapLocationChanged -= OnMapLocationChanged;
            _viewModel.OnCenterMapToLocationChanged -= OnCenterMapToLocationChanged;
            _viewModel.FavouriteProviderRefreshed -= OnFavouriteProviderRefreshed;
            _viewModel.SearchViewModel.ClearTextBox -= SearchViewModel_ClearTextBox;
            searchButton.TouchUpInside -= OnSearchButtonOnTouchUpInside;
            _viewModel.ViewModelWillDestroy -= ViewModelOnViewModelWillDestroy;
        }

        private void OnFavouriteProviderRefreshed(object sender, int position)
        {
            var provider = _viewModel.ServiceProviders[position];
            _markers[position].Icon = GetProviderIcon(provider);
        }

        private void OnMapLocationChanged(object sender, EventArgs e)
        {
            UpdateUserPosition();
            if (((MapLocationChangedEventArgs) e).RefreshMap)
            {
                CenterMap();
            }
        }

        private void OnCenterMapToLocationChanged(object sender, EventArgs e)
        {
            CenterMapToServiceProvider(((MapLocationChangedEventArgs) e).Location);
        }

        private void SearchViewModel_ClearTextBox(object sender, EventArgs e)
        {
            searchTextField.Text = null;
        }

        private void Initialize()
        {
            _viewModel = (FindHealthProviderViewModel) ViewModel;
            ProgressView.TintColor = Colors.HIGHLIGHT_COLOR;
            LearnMoreHeaderLabel.TextColor = Colors.HIGHLIGHT_COLOR;

            searchTextField.BackgroundColor = Colors.LightGrayColor;
            searchTextField.Delegate = this;
            searchTextField.LeftView = new UIView(new CGRect(0, 0, 10, 20));
            searchTextField.LeftViewMode = UITextFieldViewMode.Always;
            searchButton.TouchUpInside += OnSearchButtonOnTouchUpInside;
            if (Mvx.TryResolve<INavViewHeightProvider>(out var navViewHeightProvider))
            {
                _tabbarHeight = (int) navViewHeightProvider.NavViewHeight;
            }

            _markers = new List<ProviderMarker>();
        }

        private void OnSearchButtonOnTouchUpInside(object sender, EventArgs e)
        {
            searchTextField.ResignFirstResponder();
        }

        private void ServiceProvidersChanged(object sender, EventArgs e)
        {
            RemoveOldPins();
            AddNewPins();
        }

        private void AddNewPins()
        {
            if (_viewModel.ServiceProviders == null)
            {
                return;
            }

            foreach (var serviceProvider in _viewModel.ServiceProviders)
            {
                var icon = GetProviderIcon(serviceProvider);

                var marker = new ProviderMarker
                {
                    Position = new CLLocationCoordinate2D(serviceProvider.Model.Latitude,
                        serviceProvider.Model.Longitude),
                    GroundAnchor = new CGPoint(0.5f, 1.0f),
                    ProviderId = serviceProvider.Model.ProviderId,
                    Icon = icon,
                    Map = _googleMapView
                };
                _markers.Add(marker);
            }
        }

        private void RemoveOldPins()
        {
            foreach (var marker in _markers)
            {
                marker.Map = null;
            }

            _markers.Clear();
        }

        private void SetupMap()
        {
            _googleMapView = new MapView(new CGRect(MapHolderView.Bounds.Left,
                MapHolderView.Bounds.Top,
                MapHolderView.Bounds.Right - MapHolderView.Bounds.Left,
                MapHolderView.Bounds.Bottom - MapHolderView.Bounds.Top - _tabbarHeight));

            MapHolderView.Add(_googleMapView);
            _googleMapView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
        }

        private void CompleteSetupMap()
        {
            _googleMapView.CameraPositionIdle += OnCameraPositionIdle;
            _googleMapView.TappedMarker = (map, marker) =>
            {
                var providerMarker = marker as ProviderMarker;

                if (providerMarker != null)
                {
                    var provider = _viewModel.GetSelectedProvider(providerMarker.ProviderId);
                    _viewModel.ShowProviderDetailsInformationCommand.Execute(provider);
                }

                return true;
            };
        }

        private bool DidTapMyLocationButtonForMapView(MapView mapView)
        {
            _viewModel.SearchFilterData.LocationName = null;
            _viewModel.SearchFilterData.Location = null;
            _viewModel.Messenger.Publish(new SearchGoToCurrentLocationMessage(this));
            UpdateUserPosition();
            return false;
        }

        private void OnCameraPositionIdle(object sender, GMSCameraEventArgs e)
        {
            _viewModel.MapPosition = new Location()
                {Lat = e.Position.Target.Latitude, Lng = e.Position.Target.Longitude};
            var distanceInKm = GeolocationHelper.ConvertLatitudeDifferenceToKm(
                _googleMapView.Projection.VisibleRegion.FarRight.Latitude,
                _googleMapView.Projection.VisibleRegion.NearLeft.Latitude);
            _viewModel.CurrentDistance = distanceInKm / 2;

            if (!_mapMovedByUser)
            {
                _viewModel.CurrentDistance = InitialRadiusInKm;
                _viewModel.RefreshHealthProvidersCommand.Execute(false);
            }

            if (_viewModel.ServiceProviders != null && _mapMovedByUser)
            {
                _viewModel.RefreshHealthProvidersCommand.Execute(false);
            }

            _mapMovedByUser = true;
        }

        private void UserPositionChanged(object sender, GeolocationResultEventArgs e)
        {
            _viewModel.UserPositionChanged -= UserPositionChanged;

            _viewModel.MapPosition = new Location()
            {
                Lat = e.GeolocationResult.Position.Latitude,
                Lng = e.GeolocationResult.Position.Longitude
            };

            UpdateUserPosition();
            CenterMap();

            // We wait until the UserPositionChanged is called directly from the constructor or by raising the event.
            // Only then we get the providers and register to CameraPositionIdle and TappedMarker
            // TODO we cant test this properly in Cracow, comments will be deleted when we fix  785
            _viewModel.RefreshHealthProvidersCommand.Execute(false);
            CompleteSetupMap();
        }

        private void UpdateUserPosition()
        {
            if (_userPosition != null)
            {
                _userPosition.Map = null;
            }

            _userPosition = new Marker
            {
                Position = new CLLocationCoordinate2D(_viewModel.UserMarkerPosition.Lat,
                    _viewModel.UserMarkerPosition.Lng),
                Map = _googleMapView,
                Icon = UIImage.FromBundle("PinMyLocation"),
                GroundAnchor = new CGPoint(0.5f, 0.5f)
            };
        }

        private void CenterMap()
        {
            if (_viewModel.UserPosition.Status == GeolocationStatus.SuccessGps)
            {
                if (!_googleMapView.MyLocationEnabled)
                {
                    _googleMapView.MyLocationEnabled = true;
                    _googleMapView.Settings.MyLocationButton = true;
                    _googleMapView.DidTapMyLocationButton += DidTapMyLocationButtonForMapView;
                }
            }

            _mapMovedByUser = false;

            var height = MapHolderView.Bounds.Bottom - MapHolderView.Bounds.Top;
            var width = MapHolderView.Bounds.Right - MapHolderView.Bounds.Left;

            var diff = (height - width - _tabbarHeight / 2) / 2;

            var camera = CameraUpdate.FitBounds(new CoordinateBounds(
                    new CLLocationCoordinate2D(
                        _viewModel.MapPosition.Lat - LatitudeDifference,
                        _viewModel.MapPosition.Lng),
                    new CLLocationCoordinate2D(
                        _viewModel.MapPosition.Lat + LatitudeDifference,
                        _viewModel.MapPosition.Lng)),
                diff);

            _googleMapView.Animate(camera);
        }

        private bool _mapMovedByUser = true;

        private void CenterMapToServiceProvider(Location location)
        {
            _mapMovedByUser = false;
            var newLocation = new CLLocationCoordinate2D(location.Lat, location.Lng);
            var camera = CameraUpdate.SetTarget(newLocation, 16);
            _googleMapView.Animate(camera);
        }

        private Task _progressAnimationTask;
        private CancellationTokenSource _progressAnimationTaskCancellationTokenSource;
        private float _progressValue;

        private UIImage GetProviderIcon(HealthProviderSummaryModel serviceProvider)
        {
            UIImage icon;

            if (serviceProvider.DisplayRatingAndRatingAvailable)
            {
                //drop-pins with scoring
                if (serviceProvider.IsFavourite)
                {
                    //favourite drop-pins with scoring
                    icon = MarkerIconHelper.ScoreToFavouriteUiImage(serviceProvider.Model.OverallScore,
                        _viewModel.IsEnglishLanguage);
                }
                else
                {
                    //not favourite drop-pins with scoring
                    icon = MarkerIconHelper.ScoreToNotFavouriteUiImage(serviceProvider.Model.OverallScore,
                        _viewModel.IsEnglishLanguage);
                }
            }
            else
            {
                //w/o scoring 
                if (serviceProvider.IsFavourite)
                {
                    //heart drop-pin w/o scoring
                    icon = MarkerIconHelper.ScoreToFavouriteUiImage(0, _viewModel.IsEnglishLanguage);
                }
                else
                {
                    //provider type drop-pin w/o scoring
                    var providerId = _viewModel.SearchFilterData.SelectedProviderType.ParentId ??
                                     _viewModel.SearchFilterData.SelectedProviderType.Id;
                    icon = MarkerIconHelper.ProviderTypeToUiImage(providerId);
                }
            }

            return icon;
        }

        public void StartOrStopProgressIndicator()
        {
            ProgressView.Hidden = !ProgressIndicatorVisible;
            _progressAnimationTaskCancellationTokenSource?.Cancel();
            _progressAnimationTask = null;
            _progressValue = 0;

            if (ProgressIndicatorVisible)
            {
                _progressAnimationTaskCancellationTokenSource = new CancellationTokenSource();
                _progressAnimationTask = Task.Run(async () =>
                {
                    while (!_progressAnimationTaskCancellationTokenSource.IsCancellationRequested)
                    {
                        InvokeOnMainThread(() =>
                            ProgressView.Progress = _progressValue = (float) ((_progressValue + 0.02) % 1));
                        await Task.Delay(new Random().Next(5) * 10);
                    }
                }, _progressAnimationTaskCancellationTokenSource.Token);
            }
        }
    }
}