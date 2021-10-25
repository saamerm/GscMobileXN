using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Helpers;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using static Android.Gms.Maps.GoogleMap;
using static MobileClaims.Core.ViewModels.FindHealthProviderViewModel;
using Location = MobileClaims.Core.Services.Responses.Location;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.right_region)]
    public sealed class MapView : BaseFragment, IOnMapReadyCallback, IOnMapLoadedCallback, IOnMarkerClickListener, IOnMyLocationButtonClickListener
    {
        private GoogleMap _map;
        private List<ProviderMarker> _markers;
        private Marker _userPositionMarker;
        private FindHealthProviderViewModel _model;
        private Button _showDetailsListButton;
        private View _view;
        private TextView _popupTextView;
        private Android.Gms.Maps.MapView _mapView;
        private double _oldLatitude;
        private double _oldLongitude;

        public bool IsMarkerSelected { get; set; }

        private void OnMapLocationChanged(object sender, EventArgs e)
        {
            UpdatePosition();
            if (((MapLocationChangedEventArgs) e).RefreshMap)
            {
                CenterMapWithDefaultZoom();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.MapViewLayout, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Init();
            CreateMap(savedInstanceState);
            _model.OnMapLocationChanged += OnMapLocationChanged;
            _model.FavouriteProviderRefreshed += OnFavouriteProviderRefreshed;
            _model.OnCenterMapToLocationChanged += OnCenterMapToLocationChanged;
        }

        private void OnFavouriteProviderRefreshed(object sender, int position)
        {
            var provider = _model.ServiceProviders[position];
            int iconResource = GetMarkerIconResource(provider);
            _markers?.ElementAtOrDefault(position)?.Marker.SetIcon(BitmapDescriptorFactory.FromResource(iconResource));
        }

        private void OnCenterMapToLocationChanged(object sender, EventArgs e)
        {
            CenterMapToServiceProvider(((MapLocationChangedEventArgs)e).Location);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            _model.OnMapLocationChanged -= OnMapLocationChanged;
        }

        bool _mapMovedByUser;

        void OnCameraMoveStarted(object sender, CameraMoveStartedEventArgs e)
        {
            _mapMovedByUser = e.Reason != OnCameraMoveStartedListener.ReasonDeveloperAnimation;
        }

        private void OnCameraIdle(object sender, EventArgs e)
        {
            if (_mapMovedByUser)
            {
                OnCameraIdleAfterUserInteraction();
            }
            else
            {
                OnCameraIdleAfterDeveloperInteraction();
            }
        }

        private void OnCameraIdleAfterUserInteraction()
        {
            UpdateMapPosition();
            _model.RefreshHealthProvidersCommand.Execute(false);
        }

        private void OnCameraIdleAfterDeveloperInteraction()
        {
            _model.CurrentDistance = InitialRadiusInKm;
            _model.RefreshHealthProvidersCommand.Execute(false);
        }

        private void UpdateMapPosition()
        {
            LatLngBounds bounds = _map.Projection.VisibleRegion.LatLngBounds;
            double distanceInKm =
                GeolocationHelper.ConvertLatitudeDifferenceToKm(bounds.Northeast.Latitude, bounds.Southwest.Latitude);

            _model.MapPosition = new Location() { Lat = bounds.Center.Latitude, Lng = bounds.Center.Longitude };

            if (!MinThresholdMoved(_oldLatitude, _oldLongitude, bounds.Center.Latitude, bounds.Center.Longitude) &&
                IsMarkerSelected)
            {
                return;
            }
            _model.CurrentDistance = distanceInKm / 2;
        }

        private bool MinThresholdMoved(double oldLatitude, double oldLongtitude, double newLatitude, double newLongtitude)
        {
            var isMinTresholdMoved = Math.Abs(oldLatitude - newLatitude) > 0.001 || Math.Abs(oldLongtitude - newLongtitude) > 0.001;

            _oldLatitude = newLatitude;
            _oldLongitude = newLongtitude;

            return isMinTresholdMoved;
        }

        private void Init()
        {
            _model = (FindHealthProviderViewModel) ViewModel;
            _markers = new List<ProviderMarker>();

            _popupTextView = _view.FindViewById<TextView>(Resource.Id.popupTextView);

            var typeface = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, path: "fonts/NunitoSansBold.ttf");
            _popupTextView.SetTypeface(typeface, TypefaceStyle.Normal);

            _showDetailsListButton = _view.FindViewById<Button>(Resource.Id.showDetailsListButton);
            _showDetailsListButton.Click += ShowDetailsListButtonClick;
        }

        private void ShowDetailsListButtonClick(object sender, EventArgs e)
        {
            View.RequestFocus();
        }

        private void CreateMap(Bundle savedInstanceState)
        {
            if (_map != null)
            {
                return;
            }

            _mapView = _view.FindViewById<Android.Gms.Maps.MapView>(Resource.Id.map);
            _mapView.OnCreate(savedInstanceState);
            _mapView.GetMapAsync(this);
        }

        public override void OnResume()
        {
            base.OnResume();
            _mapView.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
            _mapView.OnPause();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            _mapView.OnLowMemory();
        }

        public override void OnDestroy()
        {
            _map?.Dispose();
            _map = null;
            _mapView.Dispose();
            _mapView = null;
            base.OnDestroy();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            if (_map == null)
            {
                return;
            }

            _model.ServiceProvidersChanged += ServiceProvidersChanged;            

            _map.SetOnMapLoadedCallback(this);
            _map.SetOnMarkerClickListener(this);
        }

        private void ServiceProvidersChanged(object sender, EventArgs e)
        {
            RemoveOldPins();
            AddNewPins();
        }

        private void AddNewPins()
        {
            if (_model.ServiceProviders == null || Activity == null)
            {
                return;
            }

            foreach (var serviceProvider in _model.ServiceProviders)
            {
                var markerOptions = new MarkerOptions();
                markerOptions.SetPosition(new LatLng(serviceProvider.Model.Latitude, serviceProvider.Model.Longitude))
                    .Anchor(0.5f, 1.0f);
                markerOptions.SetTitle(serviceProvider.Model.ProviderTradingName);

                int resourceId = GetMarkerIconResource(serviceProvider);
                
                //markerOptions.SetIcon(VectorToBitmap(resourceId));
                markerOptions.SetIcon(BitmapDescriptorFactory.FromResource(resourceId));

                if (_map != null)
                {
                    _markers?.Add(new ProviderMarker
                    {
                        Marker = _map?.AddMarker(markerOptions),
                        ProviderId = serviceProvider.Model.ProviderId
                    });
                }
            }
        }

        private BitmapDescriptor VectorToBitmap(int resourceId)
        {
            var vectorDrawable = ContextCompat.GetDrawable(Activity, resourceId);
            var bitmap = Bitmap.CreateBitmap(vectorDrawable.IntrinsicWidth,
                    vectorDrawable.IntrinsicHeight, Bitmap.Config.Argb8888);
            var canvas = new Canvas(bitmap);
            vectorDrawable.SetBounds(0, 0, canvas.Width, canvas.Height);
            vectorDrawable.Draw(canvas);
            return BitmapDescriptorFactory.FromBitmap(bitmap);
        }

        private void RemoveOldPins()
        {
            if (_markers?.Any() == true)
            {
                foreach (var marker in _markers)
                {
                    marker?.Marker?.Remove();
                }
                _markers.Clear();
            }
        }

        private void UserPositionChanged(object sender, GeolocationResultEventArgs e)
        {
            _model.UserPositionChanged -= UserPositionChanged;

            if (_map == null)
            {
                return;
            }

            UpdatePosition();

            _map.CameraIdle += OnCameraIdle;
            _map.CameraMoveStarted += OnCameraMoveStarted;
            _map.MapClick += OnMapClick;
            _map.CameraMoveStarted += MapOnCameraMoveStarted;
            CenterMapWithDefaultZoom();
        }

        private void UpdatePosition()
        {
            if (_model.UserMarkerPosition == null || _map == null)
            {
                return;
            }

            _userPositionMarker?.Remove();
            var markerOptions = new MarkerOptions();
            markerOptions
                .SetPosition(new LatLng(_model.UserMarkerPosition.Lat,
                                                 _model.UserMarkerPosition.Lng))
                .Anchor(0.5f, 0.5f);
            markerOptions.SetIcon(VectorToBitmap(Resource.Drawable.pin_mylocation));
            _userPositionMarker = _map?.AddMarker(markerOptions);
        }

        private void CenterMapWithDefaultZoom()
        {
            if (_model.MapPosition == null || _model.UserPosition == null)
            {
                return;
            }

            if (_model.UserPosition.Status == GeolocationStatus.SuccessGps)
            {
                if (!_map.MyLocationEnabled)
                {
                    _map.MyLocationEnabled = true;
                    _map.UiSettings.MyLocationButtonEnabled = true;
                    _map.SetOnMyLocationButtonClickListener(this);

                    try
                    {
                        var locationButton = ((View)_mapView.FindViewById(1).Parent).FindViewById(2);
                        var layoutParams = (RelativeLayout.LayoutParams)
                            locationButton.LayoutParameters;
                        layoutParams.AddRule(LayoutRules.AlignParentTop, 0);
                        layoutParams.AddRule(LayoutRules.AlignParentBottom, -1);
                        layoutParams.SetMargins(0, 0, 30, 30);
                    }
                    catch (Exception)
                    {
                        //we couldn't reposition 'go to my position' button
                    }
                }
            }

            var padding = (_view.MeasuredHeight - _view.MeasuredWidth) / 2;

            var cameraUpdate = CameraUpdateFactory.NewLatLngBounds(
                new LatLngBounds(
                    new LatLng(
                        _model.MapPosition.Lat - LatitudeDifference,
                        _model.MapPosition.Lng),
                    new LatLng(
                        _model.MapPosition.Lat + LatitudeDifference,
                        _model.MapPosition.Lng)),
                (padding < _view.MeasuredWidth / 2 && padding < _view.MeasuredHeight / 2) ? padding : 0);
            _map?.AnimateCamera(cameraUpdate);
        }

        private void CenterMapToServiceProvider(Location location)
        {
            var newLocation = new LatLng(location.Lat, location.Lng);
            var camera = CameraUpdateFactory.NewLatLngZoom(newLocation, 16);
            _map?.AnimateCamera(camera);
        }

        private void MapOnCameraMoveStarted(object sender, CameraMoveStartedEventArgs e)
        {
            View.RequestFocus();
        }

        private void OnMapClick(object sender, MapClickEventArgs e)
        {
            IsMarkerSelected = false;
            View.RequestFocus();
        }

        public void OnMapLoaded()
        {
            if (_model.UserPosition != null)
            {
                UserPositionChanged(this,
                    new GeolocationResultEventArgs(_model.UserPosition));
                _model.UserPositionChanged -= UserPositionChanged;
            }
            else
            {
                _model.UserPositionChanged += UserPositionChanged;
            }
        }

        public bool OnMarkerClick(Marker marker)
        {
            var providerMarker = _markers?.SingleOrDefault(x => x.Marker.Id == marker.Id);

            if (providerMarker != null)
            {
                var provider = _model.GetSelectedProvider(providerMarker.ProviderId);
                _model.ShowProviderDetailsInformationCommand.Execute(provider);
            }
            return true;
        }

        private int GetMarkerIconResource(HealthProviderSummaryModel serviceProvider)
        {
            int resourceId;
            if (serviceProvider.DisplayRatingAndRatingAvailable)
            {
                //drop-pins with scoring
                if (serviceProvider.IsFavourite)
                {
                    //favourite drop-pins with scoring
                    resourceId = MarkerIconHelper.ScoreToFavouriteResourceId(serviceProvider.Model.OverallScore);
                }
                else
                {
                    //not favourite drop-pins with scoring
                    resourceId = MarkerIconHelper.ScoreToNotFavouriteResourceId(serviceProvider.Model.OverallScore);
                }
            }
            else
            {
                //w/o scoring 
                if (serviceProvider.IsFavourite)
                {
                    //heart drop-pin w/o scoring
                    resourceId = MarkerIconHelper.ScoreToFavouriteResourceId(0);
                }
                else
                {
                    //provider type drop-pin w/o scoring
                    var providerId = _model.SearchFilterData.SelectedProviderType.ParentId ??
                                     _model.SearchFilterData.SelectedProviderType.Id;
                    resourceId = MarkerIconHelper.ProviderTypeToResourceId(providerId);
                }
            }

            return resourceId;
        }

        public bool OnMyLocationButtonClick()
        {            
            _model.SearchFilterData.LocationName = null;
            _model.SearchFilterData.Location = null;
            _model.Messenger.Publish(new SearchGoToCurrentLocationMessage(this));
            UpdatePosition();
            return false;
        }
    }

    public class OnInfoWindowClickListener : Java.Lang.Object, IOnInfoWindowClickListener
    {
        private readonly MapView _parent;
        public List<Marker> Markers;

        internal OnInfoWindowClickListener(MapView parent)
        {
            _parent = parent;
        }

        public void OnInfoWindowClick(Marker clickedMarker)
        {
            foreach (var marker in Markers)
            {
                if (marker.Equals(clickedMarker))
                {
                    var uri = "http://maps.google.com/maps?f=d&daddr=" + clickedMarker.Position.Latitude + "," + clickedMarker.Position.Longitude + "&mode=driving";
                    var mapIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
                    _parent.StartActivity(mapIntent);
                }
            }
        }
    }
}