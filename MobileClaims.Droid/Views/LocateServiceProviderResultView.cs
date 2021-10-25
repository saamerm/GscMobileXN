using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Fragging;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Droid.Views;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Views;
using MobileClaims.Core.Entities;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Common;
using Android.Util;
using Cirrious.MvvmCross.Plugins.Messenger;
using MobileClaims.Core.Services;
using MobileClaims.Core.Messages;
using Android.Views.InputMethods;


namespace MobileClaims.Droid
{
	[Region(Resource.Id.right_region)]			
	public class LocateServiceProviderResultView : BaseFragment, IMvxView 
	{
		bool dialogShown;
		GoogleMap map;
		SupportMapFragment mapView;
		List<Marker> markers;
		public LocateServiceProviderResultViewModel _model;
		MvxSubscriptionToken _selectedproviderchanged;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			var view = this.View;

			if (view != null) {
				ViewGroup parent = (ViewGroup) View.Parent;
				if (parent != null)
					parent.RemoveView (View);
			}
			try {
				view = this.BindingInflate(Resource.Layout.LocateServiceProviderResultView, null);
			} catch (InflateException e) {
				/* map is already there, just return view as it is */
			}
			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			_model = (LocateServiceProviderResultViewModel)ViewModel;

			mapView = SupportMapFragment.NewInstance ();
			Android.Support.V4.App.FragmentTransaction fragmentTransaction = ChildFragmentManager.BeginTransaction ();
			fragmentTransaction.Add(Resource.Id.map, mapView);
			fragmentTransaction.Commit ();

			markers = new List<Marker> ();
			mapView.OnCreate (savedInstanceState);

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
			IMvxMessenger _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
			_selectedproviderchanged= _messenger.Subscribe<Core.Messages.SelectedServiceProviderChanged>((message) =>
				{

					IProviderLookupService service = Mvx.IoCProvider.Resolve<IProviderLookupService>();
					ServiceProvider sp = service.SelectedServiceProvider;
					map = mapView.Map;

					map.SetInfoWindowAdapter (new InfoWindow (this));

					//implemented below, necessary to allow click events
					var infoWindowListener = new OnInfoWindowClickListener (this);
					map.SetOnInfoWindowClickListener (infoWindowListener);

					if (map != null) {

						MarkerOptions options = new MarkerOptions ();
						options.SetPosition (new LatLng(sp.Latitude, sp.Longitude));
						options.SetTitle (sp.DoctorName);
						options.SetSnippet ("\r\n" + sp.Address +
							"\r\n" + sp.FormattedAddress +
							"\r\n\r\n" + Resources.GetString(Resource.String.locateProviderPhoneNumberLabel) + Resources.GetString(Resource.String.colon) + " " + sp.Phone +
							"\r\n\r\n" + ClaimsOnline (sp.CanSubmitClaimsOnline) +
							"\r\n" + PaymentDirectly (sp.CanAcceptPaymentDirectly)
						);
						System.Console.WriteLine(_model.SelectedProvider.BusinessName);

						Marker tempMarker = ((GoogleMap)map).AddMarker (options);
						markers.Add (tempMarker);


						//set map constraints based on selected provider
						float zoom = calculateZoomLevel (_model.SearchTerms.Radius);
						CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (new LatLng (sp.Latitude, sp.Longitude), zoom);
						map.MoveCamera (camera);

						infoWindowListener.Markers = markers;
						int index = _model.ServiceProviders.FindIndex (x => x.ID == sp.ID); 
						markers [index].ShowInfoWindow();



					}
				});
			}

//			InputMethodManager inputManager = 
//				(InputMethodManager) Activity.GetSystemService(Context.InputMethodService); 
//			inputManager.HideSoftInputFromWindow(  view.WindowToken, HideSoftInputFlags.NotAlways);

		}


		public override void OnStart ()
		{
			base.OnStart ();
			//mapView = (SupportMapFragment) FragmentManager.FindFragmentById(Resource.Id.map);

			map = mapView.Map;
			if (map != null) {
				map.UiSettings.MyLocationButtonEnabled = false;
				map.MyLocationEnabled = true;

				//implemented below, necessary to allow a multiline text field
				map.SetInfoWindowAdapter (new InfoWindow (this));

				//implemented below, necessary to allow click events
				var infoWindowListener = new OnInfoWindowClickListener (this);
				map.SetOnInfoWindowClickListener (infoWindowListener);

				if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
					var onMarkerClickListener = new OnMarkerClickListener (this);
					map.SetOnMarkerClickListener (onMarkerClickListener);
				}
				//add markers for each provider
				int providerIndex = 0; int i = 0;
				foreach (ServiceProvider item in _model.ServiceProviders) {
					MarkerOptions options = new MarkerOptions ();
					options.SetPosition (new LatLng(item.Latitude, item.Longitude));
					options.SetTitle (item.DoctorName);
					options.SetSnippet ("\r\n" + item.Address +
						"\r\n" + item.FormattedAddress +
						"\r\n\r\n" + Resources.GetString(Resource.String.locateProviderPhoneNumberLabel) + Resources.GetString(Resource.String.colon) + " " + item.Phone +
						"\r\n\r\n" + ClaimsOnline (item.CanSubmitClaimsOnline) +
						"\r\n" + PaymentDirectly (item.CanAcceptPaymentDirectly)
					);
					System.Console.WriteLine(_model.SelectedProvider.BusinessName);

					Marker tempMarker = ((GoogleMap)map).AddMarker (options);
					markers.Add (tempMarker);

					if (item.ID == _model.SelectedProvider.ID)
						providerIndex = i;
					i++;
				}

				//set map constraints based on selected provider
				float zoom = calculateZoomLevel (_model.SearchTerms.Radius);
				CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (new LatLng (_model.SelectedProvider.Latitude, _model.SelectedProvider.Longitude), zoom);
				map.MoveCamera (camera);

				infoWindowListener.Markers = markers;
				markers [providerIndex].ShowInfoWindow();
			}
		}
		public override void OnResume ()
		{
			mapView.OnResume ();
			base.OnResume ();
		}

		public override void OnPause ()
		{
			mapView.OnPause ();
			base.OnPause ();
		}

		public override void OnDestroy ()
		{
			//mapView.OnDestroy ();
			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				if (_selectedproviderchanged != null) {
					IMvxMessenger _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger> ();
					_messenger.Unsubscribe<SelectedServiceProviderChanged> (_selectedproviderchanged);
				}
			}

			base.OnDestroy ();

		}

		public string ClaimsOnline(bool submit)
		{
			return submit ? Resources.GetString(Resource.String.mapProviderDetailsSubmit) : Resources.GetString(Resource.String.mapProviderDetailsNotSubmit);
		}
		public string PaymentDirectly(bool submit)
		{
			return submit ? Resources.GetString(Resource.String.mapProviderDetailsAccept) : Resources.GetString(Resource.String.mapProviderDetailsNotAccept);
		}
		private int calculateZoomLevel(int radius) {
			double equatorLength = 40075004; // in meters
			DisplayMetrics display = new DisplayMetrics();
			Activity.WindowManager.DefaultDisplay.GetMetrics (display);
			double widthInPixels = display.WidthPixels;
			double metersPerPixel = equatorLength / 256;
			int zoomLevel = 1;
			while ((metersPerPixel * widthInPixels) > radius*1000) {
				metersPerPixel /= 2;
				++zoomLevel;
			}
			return zoomLevel;
		}
	}
		
	public class OnInfoWindowClickListener : Java.Lang.Object, GoogleMap.IOnInfoWindowClickListener
	{
		#region IOnMarkerClickListener implementation
		private LocateServiceProviderResultViewModel model;
		private LocateServiceProviderResultView parent;
		public List<Marker> Markers;

		internal OnInfoWindowClickListener(LocateServiceProviderResultView parent)
		{
			this.parent = parent;
			this.model = parent._model;
		}

		public void OnInfoWindowClick (Marker marker)
		{
			for (var i=0; i<Markers.Count; i++) {
				if (Markers[i].Equals(marker))
				{
					//launch maps app with directions dialog from current location to targeted location
					String uri = "http://maps.google.com/maps?f=d&daddr="+marker.Position.Latitude.ToString()+","+marker.Position.Longitude.ToString()+"&mode=driving";
					var mapIntent = new Intent (Intent.ActionView, Android.Net.Uri.Parse(uri));
					parent.StartActivity (mapIntent);
				}
			}
		}
		#endregion
	}

	public class InfoWindow : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
	{
		#region IInfoWindowAdapter implementation
		private LocateServiceProviderResultView parent;

		internal InfoWindow (LocateServiceProviderResultView parent) 
		{
			this.parent = parent;
		}

		public View GetInfoContents(Marker p0)
		{
			var popup = parent.BindingInflate (Resource.Layout.custom_infowindow, null);

			var titleTextView = popup.FindViewById<TextView>(Resource.Id.infoTitle);
			titleTextView.Text = p0.Title;

			var contentTextView = popup.FindViewById<TextView>(Resource.Id.infoContent);
			contentTextView.Text = p0.Snippet;

			return popup;
		}

		public View GetInfoWindow(Marker p0)
		{
			return null;
		}
		#endregion
	}


	public class OnMarkerClickListener : Java.Lang.Object, GoogleMap.IOnMarkerClickListener
	{
		#region IOnMarkerClickListener implementation
		private LocateServiceProviderResultViewModel model;
		private LocateServiceProviderResultView parent;
		public List<Marker> Markers;

		internal OnMarkerClickListener(LocateServiceProviderResultView parent)
		{
			this.parent = parent;
			this.model = parent._model;
		}
		public bool OnMarkerClick (Marker marker)
		{
			List<ServiceProvider> ServiceProviders = model.ServiceProviders;
			foreach (ServiceProvider sp in ServiceProviders)
			{    

				String snippet="\r\n" + sp.Address +
				"\r\n" + sp.FormattedAddress +
					"\r\n\r\n" + Application.Context.Resources.GetString(Resource.String.locateProviderPhoneNumberLabel) + Application.Context.Resources.GetString(Resource.String.colon) + " " + sp.Phone +
				"\r\n\r\n" + ClaimsOnlineMarker (sp.CanSubmitClaimsOnline) +
				"\r\n" + PaymentDirectlyMarker (sp.CanAcceptPaymentDirectly);

				if (marker.Snippet == snippet)
				{
					model.SelectWithoutNavigationCommand.Execute(sp);
					return true;
				}
			}
			return true;
		}
		public string ClaimsOnlineMarker(bool submit)
		{

			return submit ?  Application.Context.Resources.GetString(Resource.String.mapProviderDetailsSubmit) : Application.Context.Resources.GetString(Resource.String.mapProviderDetailsNotSubmit);
		}
		public string PaymentDirectlyMarker(bool submit)
		{
			return submit ?  Application.Context.Resources.GetString(Resource.String.mapProviderDetailsAccept): Application.Context.Resources.GetString(Resource.String.mapProviderDetailsNotAccept);
		}
		#endregion
	}

}