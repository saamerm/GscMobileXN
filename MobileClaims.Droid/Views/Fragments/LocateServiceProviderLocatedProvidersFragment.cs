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


namespace MobileClaims.Droid
{
	[Region(Resource.Id.left_region)]			
	public class LocateServiceProviderLocatedProvidersFragment : BaseFragment, IMvxView 
	{
		bool dialogShown;
		LocateServiceProviderLocatedProvidersViewModel _model;

		GoogleMap map;
		SupportMapFragment mapView;
		List<Marker> markers;

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

			if (list.Count > 0) {
				Utility.setFullListViewHeight (list);
			}

			_model = (LocateServiceProviderLocatedProvidersViewModel)ViewModel;
			try{
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
			}catch(Exception ex){
				System.Console.Write (ex.StackTrace);
			}
		}

		public override void OnDestroy()
		{
//			if (!this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
//				_model.GoBackCommand.Execute (null);
//			}
			base.OnDestroy();
		}

		private void PopulateFields()
		{
			var firstLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedFirstLabel);
			var secondLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedSecondLabel);
			var thirdLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedThirdLabel);
			var fourthLabel = this.View.FindViewById(Resource.Id.searchProviderSearchedFourthLabel);


			((TextView)firstLabel).Text = Resources.GetString(Resource.String.searchProviderServiceType).ToUpper() + 
				" " + _model.SearchTerms.ProviderType.ToUpper();

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
				((TextView)thirdLabel).Text = Resources.GetString(Resource.String.locateProviderPhoneNumberLabel).ToUpper() + 
					Resources.GetString(Resource.String.colon) + 
					" " + _model.SearchTerms.Phone.ToUpper();
				break;
			case "Last Name":
				((TextView)thirdLabel).Text = Resources.GetString(Resource.String.locateProviderLastNameLabel).ToUpper() + 
					Resources.GetString(Resource.String.colon) + 
					" " + _model.SearchTerms.LastName.ToUpper();
				break;
			case "Business Name":
				((TextView)thirdLabel).Text = Resources.GetString(Resource.String.locateProviderLastNameLabel).ToUpper() + 
					Resources.GetString(Resource.String.colon) + 
					" " + _model.SearchTerms.BusinessName.ToUpper();
				break;
			case "City":
				((TextView)thirdLabel).Text = Resources.GetString(Resource.String.locateProviderCityLabel).ToUpper() +
					Resources.GetString(Resource.String.colon) + 
					" " + _model.SearchTerms.City.ToUpper();
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