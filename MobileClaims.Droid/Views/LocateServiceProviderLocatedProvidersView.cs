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
using Cirrious.MvvmCross.Plugins.Messenger;
using MobileClaims.Core.Services;
using MobileClaims.Core.Messages;


namespace MobileClaims.Droid
{
	[Region(Resource.Id.left_region)]			
	public class LocateServiceProviderLocatedProvidersView : BaseFragment, IMvxView 
	{
		//bool dialogShown;
		LocateServiceProviderLocatedProvidersViewModel _model;
		MvxSubscriptionToken _selectedproviderchanged;

		//GoogleMap map;
		//SupportMapFragment mapView;
		//List<Marker> markers;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			return this.BindingInflate(Resource.Layout.LocateServiceProviderLocatedProvidersView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			var list = Activity.FindViewById(Resource.Id.serviceProviderLocatedListView) as MvxListView;
			//list.Adapter = new SingleSelectionMvxAdapter (this.Activity, (IMvxAndroidBindingContext)BindingContext);

		/*	if (list.Count > 0) {
				Utility.setFullListViewHeight (list);
			}*/

			_model = (LocateServiceProviderLocatedProvidersViewModel)ViewModel;
			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
			IMvxMessenger _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
			_selectedproviderchanged = _messenger.Subscribe<Core.Messages.SelectedServiceProviderChanged>((message) =>
				{
						IProviderLookupService service = Mvx.IoCProvider.Resolve<IProviderLookupService>();
						ServiceProvider sp = service.SelectedServiceProvider;
						int index = _model.ServiceProviders.FindIndex (x => x.ID == sp.ID); 
						list.SetItemChecked (index,true);
						list.SetSelection(index);
				});
			}
			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName=="SearchTerms") {
					PopulateFields();
				}
			};
			PopulateFields ();
			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				if (list.Count > 0) {
					list.SetItemChecked(0,true);
				}
				_model.ShowMapViewCommand.Execute (_model.ServiceProviders [0]);
			}

			/*mapView = SupportMapFragment.NewInstance ();
			Android.Support.V4.App.FragmentTransaction fragmentTransaction = ChildFragmentManager.BeginTransaction ();
			fragmentTransaction.Add(Resource.Id.map, mapView);
			fragmentTransaction.Commit ();

			markers = new List<Marker> ();
			mapView.OnCreate (savedInstanceState);*/

//			var theList = this.View.FindViewById(Resource.Id.serviceProviderLocatedListView);
//			((MvxListView)theList).OnItemClickListener += (object sender, EventArgs e) => {
//				((MvxListItemView)sender).FindViewById(
//			};
		}

		public override void OnDestroy ()
		{
			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				if (_selectedproviderchanged != null) {
					IMvxMessenger _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger> ();
					_messenger.Unsubscribe<SelectedServiceProviderChanged> (_selectedproviderchanged);
				}
			}
			base.OnDestroy ();
		}

		/*public override void OnStart ()
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
			base.OnDestroy ();
		}

		*/

		private void PopulateFields()
		{
			var firstLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedFirstLabel);
			var secondLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedSecondLabel);
			var thirdLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedThirdLabel);
			var fourthLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedFourthLabel);


			((TextView)firstLabel).Text = Resources.GetString(Resource.String.searchProviderServiceTypeValue).ToUpper() + 
				" " + _model.SearchTerms.ProviderType.ToUpper();

//			((TextView)firstLabel).Text = Resources.GetString(Resource.String.searchProviderServiceType).ToUpper() + 
//				" " + Resources.GetString(Resource.String.searchProviderServiceTypeValue).ToUpper();

			switch(_model.SearchTerms.LocationType)
			{
			case "Address":
				((TextView)secondLabel).Text = Resources.GetString(Resource.String.locateProviderAddressLabel).ToUpper() + 
					Resources.GetString(Resource.String.colon) + 
					" " + _model.SearchTerms.Address.ToUpper();
				break;
			case "Postal Code":
				((TextView)secondLabel).Text = Resources.GetString(Resource.String.locateProviderPostalCodeLabel).ToUpper() + 
					Resources.GetString(Resource.String.colon) + 
					" " + _model.SearchTerms.PostalCode.ToUpper();
				break;
			default:
				((TextView)secondLabel).Text = Resources.GetString(Resource.String.locateProviderMyCurrentLocationLabel).ToUpper();
				break;
			}

			switch(_model.SearchTerms.SearchType) {
			case "Phone Number":
				if (_model.SearchTerms.Phone != null) {
					((TextView)thirdLabel).Text = Resources.GetString (Resource.String.locateProviderPhoneNumberLabel).ToUpper () +
					Resources.GetString (Resource.String.colon) +
					" " + _model.SearchTerms.Phone.ToUpper ();
				}
				break;
			case "Last Name":
				if (_model.SearchTerms.LastName != null) {
					((TextView)thirdLabel).Text = Resources.GetString (Resource.String.locateProviderLastNameLabel).ToUpper () +
					Resources.GetString (Resource.String.colon) +
					" " + _model.SearchTerms.LastName.ToUpper ();
				}
				break;
			case "Business Name":
				if (_model.SearchTerms.BusinessName != null) {
					((TextView)thirdLabel).Text = Resources.GetString (Resource.String.locateProviderLastNameLabel).ToUpper () +
					Resources.GetString (Resource.String.colon) +
					" " + _model.SearchTerms.BusinessName.ToUpper ();
				}
				break;
			case "City":
				if (_model.SearchTerms.City != null) {
					((TextView)thirdLabel).Text = Resources.GetString (Resource.String.locateProviderCityLabel).ToUpper () +
					Resources.GetString (Resource.String.colon) +
					" " + _model.SearchTerms.City.ToUpper ();
				}
				break;
			default:
				((TextView)thirdLabel).Visibility = ViewStates.Gone;
//				((TextView)secondLabel).Text = Resources.GetString(Resource.String.locateProviderAllProvidersLabel).ToUpper();
				break;
			}

			((TextView)fourthLabel).Text = Resources.GetString(Resource.String.locateProviderRangeLabel).ToUpper() + 
				Resources.GetString(Resource.String.colon) + 
				" " + _model.SearchTerms.Radius + " KM";
		}
	}

}