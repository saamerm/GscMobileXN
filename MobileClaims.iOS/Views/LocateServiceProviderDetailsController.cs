using System;
using CoreGraphics;
using System.Collections.Generic;
using System.Windows.Input;
using CoreFoundation;
using UIKit;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using Cirrious.MvvmCross.Touch.Views;
//using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MapKit;
using CoreLocation;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
    public class LocateServiceProviderDetailsController : GSCBaseViewController
	{
		private LocateServiceProviderViewModel _model;

		protected UIScrollView scrollContainer;
		protected UILabel distanceTitleLabel;
		protected UITextField addressField;
		protected UILabel exampleAddressLabel;
		protected SliderWithIndicators distanceSlider;

		protected UILabel searchTitleLabel;
		protected UITextField searchField;

		protected UILabel infoLabel;
		protected UIButton searchButton;

		protected int addressCheck;
		protected int searchCheck;

		public LocateServiceProviderDetailsController (LocateServiceProviderViewModel model)
		{
			this.View = new MvxView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationItem.Title = "detailsProviderTitle".tr();
			this._model = model;

			scrollContainer = new UIScrollView ();
			scrollContainer.BackgroundColor = Constants.BACKGROUND_COLOR;
			View.AddSubview (scrollContainer);

			distanceTitleLabel = new UILabel();
			distanceTitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			distanceTitleLabel.TextColor = UIColor.Black;
			distanceTitleLabel.TextAlignment = UITextAlignment.Left;
			distanceTitleLabel.Lines = 2;
			distanceTitleLabel.Text = "Find services within ";
			scrollContainer.Add (distanceTitleLabel);

			addressField = new UITextField ();
			addressField.TextAlignment = UITextAlignment.Left;
			addressField.BackgroundColor = UIColor.White;
			addressField.Layer.CornerRadius = 5.0f;
			addressField.ShouldReturn += (textField) => { 
				textField.ResignFirstResponder();
				return true; 
			};
			scrollContainer.Add (addressField);	

			exampleAddressLabel = new UILabel();
			exampleAddressLabel.Text = "detailsProviderExampleAddressLabel".tr();
			exampleAddressLabel.TextColor = UIColor.Black;
			exampleAddressLabel.TextAlignment = UITextAlignment.Left;
			exampleAddressLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			scrollContainer.Add (exampleAddressLabel);

			distanceSlider = new SliderWithIndicators ();
			distanceSlider.ThumbTintColor = Constants.BRANDED_GREEN;
			distanceSlider.MinimumTrackTintColor = new UIColor (18 / 255f, 125 / 255f, 249 / 255f, 1f);
			distanceSlider.MaximumTrackTintColor = new UIColor (18 / 255f, 125 / 255f, 249 / 255f, 1f);
			distanceSlider.MinValue = 5.0f;
			distanceSlider.MaxValue = 50.0f;
			distanceSlider.Value = 5.0f;
			distanceSlider.NumberOfIndicators = 4;
			distanceSlider.Suffix = "km";
			distanceSlider.ValueChanged += (sender, ea) => {
				_model.SearchRadius = Convert.ToInt32(distanceSlider.Value);
				Distance = Convert.ToInt32(distanceSlider.Value);
			};
			scrollContainer.Add (distanceSlider);
			setDistanceLabel ();

			searchTitleLabel = new UILabel ();
			searchTitleLabel.Text = "detailsProviderSearchLabel".tr () + _model.SelectedSearchType.TypeName + "detailsProviderSearchLabelEnd".tr ();
			searchTitleLabel.TextColor = UIColor.Black;
			searchTitleLabel.TextAlignment = UITextAlignment.Left;
			searchTitleLabel.Lines = 2;
			scrollContainer.Add (searchTitleLabel);

			searchField = new UITextField ();
			searchField.TextAlignment = UITextAlignment.Left;
			searchField.BackgroundColor = UIColor.White;
			searchField.Layer.CornerRadius = 5.0f;
			searchField.ShouldReturn += (textField) => { 
				textField.ResignFirstResponder();
				return true; 
			};
			scrollContainer.Add (searchField);
			setSearchLabel ();

			infoLabel = new UILabel ();
			infoLabel.Text = "detailsProviderInfoLabel".tr();
			infoLabel.TextColor = UIColor.Black;
			infoLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			infoLabel.TextAlignment = UITextAlignment.Center;
			scrollContainer.Add (infoLabel);

			searchButton = UIButton.FromType (UIButtonType.RoundedRect);
			searchButton.BackgroundColor = UIColor.White;
			searchButton.Layer.CornerRadius = 6.0f;
			searchButton.SetTitle("detailsProviderTitle".tr(), UIControlState.Normal);
			searchButton.TouchUpInside += DoFindAction;
			scrollContainer.Add (searchButton);

			var set = this.CreateBindingSet<LocateServiceProviderDetailsController, LocateServiceProviderViewModel> ();
			set.Bind (ProviderType).To (vm => vm.ProviderType.Type).OneWay();
			set.Bind (SelectedLocationType).To (vm => vm.SelectedLocationType.TypeName).OneWay();

			switch (_model.SelectedLocationType.TypeName) {
			case "Address":
				addressCheck = 5;
				addressField.Placeholder = "locateProviderAddressLabel".tr ();
				set.Bind (addressField).To (vm => vm.Address);
				break;
			case "Postal Code":
				addressCheck = 5;
				addressField.Placeholder = "locateProviderPostalCodeLabel".tr ();
				set.Bind (addressField).To (vm => vm.PostalCode);
				break;
			}

			switch (_model.SelectedSearchType.TypeName) {
			case "Phone Number":
				searchCheck = 10;
				searchField.Placeholder = "locateProviderPhoneNumberLabel".tr ();
				set.Bind (searchField).To (vm => vm.PhoneNumber);
				break;
			case "Last Name":
				searchCheck = 2;
				searchField.Placeholder = "locateProviderLastNameLabel".tr ();
				set.Bind (searchField).To (vm => vm.LastName);
				break;
			case "Business Name":
				searchCheck = 2;
				searchField.Placeholder = "locateProviderBusinessNameLabel".tr ();
				set.Bind (searchField).To (vm => vm.BusinessName);
				break;
			case "City":
				searchCheck = 2;
				searchField.Placeholder = "locateCityLabel".tr ();
				set.Bind (searchField).To (vm => vm.City);
				break;
			}
				
			set.Apply ();

			_model.NoProvidersFound += ErrorNoResults;
			Distance = 5;
		}
			
		private void DoFindAction (object sender, EventArgs ea) {
			if ((searchField.Text.Length >= searchCheck || searchField.Hidden) && (addressField.Text.Length >= addressCheck || addressField.Hidden)) {
				/*if (_model.SelectedLocationType.TypeName == "My Current Location") {
					var tempMap = new MKMapView ();
					tempMap.DidUpdateUserLocation += (s, e) => {
						CLGeocoder geocoder = new CLGeocoder ();
						CLLocation currentLocation = new CLLocation(tempMap.UserLocation.Coordinate.Latitude, tempMap.UserLocation.Coordinate.Longitude);
						geocoder.ReverseGeocodeLocation(currentLocation,ReverseGeocodeLocationHandler);
					};
					tempMap.ShowsUserLocation = true;
					//must implement case where location permission is denied
				}
				else {*/
					if (_model.FindProviderCommand != null && _model.FindProviderCommand.CanExecute (null))
						_model.FindProviderCommand.Execute (null);
				//}
			} else {
				ErrorEmptyFields (null, null);
			}
		}

		/*public void ReverseGeocodeLocationHandler (CLPlacemark[] placemarks, NSError error)
		{
			//must implement case where location is not found or returns invalid data
			System.Console.WriteLine ("Handler called");
			System.Console.WriteLine ("Success: "+placemarks [0].Thoroughfare);
			System.Console.WriteLine (placemarks [0].SubThoroughfare);
			System.Console.WriteLine (placemarks [0].Country);
			System.Console.WriteLine (placemarks [0].Locality);
			System.Console.WriteLine (placemarks [0].SubLocality);
			System.Console.WriteLine (placemarks [0].PostalCode);
		}*/

		private void ErrorNoResults (object sender, EventArgs ea)
		{
			var alert = new UIAlertView ("detailsProviderErrorTitle".tr(), "detailsProviderErrorDesc".tr(), null, "OK", null);
			alert.Show ();
		}

		private void ErrorEmptyFields (object sender, EventArgs ea)
		{
			System.Console.WriteLine("Address"+_model.Address);
			System.Console.WriteLine("Number:"+_model.PhoneNumber);
			var alert = new UIAlertView ("detailsProviderErrorFieldTitle".tr(), "detailsProviderErrorFieldDesc".tr(), null, "OK", null);
			alert.Show ();
		}

		public override void ViewDidLayoutSubviews ()
		{
			float statusBarHeight = 20; //Status bar height acting peculiar in landscape. TODO: Figure better way to get status bar height. Tried: UIApplication.SharedApplication.StatusBarFrame.Size.Height
			float viewWidth = (float)base.View.Bounds.Width;
			float viewHeight = (float)base.View.Bounds.Height;
			float startY = (float)base.NavigationController.NavigationBar.Frame.Height + statusBarHeight;
			float contentWidth = viewWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;

			distanceTitleLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, Constants.DRUG_LOOKUP_TOP_PADDING, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT*2+10);
			addressField.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, (float)distanceTitleLabel.Frame.Y + Constants.DRUG_LOOKUP_LABEL_HEIGHT*2+7, contentWidth, Constants.DRUG_LOOKUP_FIELD_HEIGHT_SMALL);
			exampleAddressLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING+5, (float)addressField.Frame.Y + Constants.DRUG_LOOKUP_FIELD_HEIGHT_SMALL+4, contentWidth-5, Constants.DRUG_LOOKUP_LABEL_HEIGHT/2);
			distanceSlider.Frame = exampleAddressLabel.Hidden == true ? new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING+5, (float)distanceTitleLabel.Frame.Y + (float)distanceTitleLabel.Frame.Height+7, contentWidth-10, 50) : new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING+5, (float)exampleAddressLabel.Frame.Y + (float)exampleAddressLabel.Frame.Height+7, contentWidth-10, 50);
			searchTitleLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, distanceSlider.Frame.Y + distanceSlider.Frame.Height + 40, contentWidth, Constants.DRUG_LOOKUP_FIELD_HEIGHT);
			searchField.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, (float)searchTitleLabel.Frame.Y + Constants.DRUG_LOOKUP_LABEL_HEIGHT*2+2, contentWidth, Constants.DRUG_LOOKUP_FIELD_HEIGHT_SMALL);
			infoLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, (float)searchField.Frame.Y + Constants.DRUG_LOOKUP_FIELD_HEIGHT_SMALL+15, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			searchButton.Frame = new CGRect (viewWidth / 2 - (float)(contentWidth*0.8) / 2, (float)infoLabel.Frame.Y + Constants.DRUG_LOOKUP_LABEL_HEIGHT+10, (float)(contentWidth*0.8), Constants.LIST_BUTTON_HEIGHT-20);
			scrollContainer.Frame = new CGRect (0, startY-60, viewWidth, viewHeight);
			scrollContainer.ContentSize = new CGSize (viewWidth, (float)searchButton.Frame.Y + (float)searchButton.Frame.Height);
		}

		void setSearchLabel()
		{
			searchTitleLabel.Text = "detailsProviderSearchLabel".tr ();
			searchTitleLabel.Text += _model.SelectedSearchType.TypeName;
			searchTitleLabel.Text += "detailsProviderSearchLabelEnd".tr ();

			searchTitleLabel.Hidden = _model.SelectedSearchType.TypeName == "All Providers" ? true : false;
			searchField.Hidden = searchTitleLabel.Hidden;
		}

		void setDistanceLabel()
		{
			distanceTitleLabel.Text = "detailsProviderRadiusInfoLabel".tr ();
			distanceTitleLabel.Text += _model.ProviderType.Type + "detailsProviderRadiusInfoLabelMiddle".tr () + Distance + "km";
			distanceTitleLabel.Text += "detailsProviderRadiusInfoLabelEnd".tr();
			distanceTitleLabel.Text += _model.SelectedLocationType.TypeName;
			distanceTitleLabel.Text += ":";


			switch (_model.SelectedLocationType.TypeName) {
			case "Address":
				exampleAddressLabel.Text = "detailsProviderExampleAddressLabel".tr ();
				break;
			case "Postal Code":
				exampleAddressLabel.Text = "detailsProviderExamplePostalLabel".tr ();
				break;
			}

			exampleAddressLabel.Hidden = _model.SelectedLocationType.TypeName == "My Current Location" ? true : false;
			addressField.Hidden = exampleAddressLabel.Hidden;
		}

		#region properties
		public string ProviderType {
			get { return null; }
			set{
				ProviderType = value;
				setDistanceLabel ();
			}
		}

		public string SelectedLocationType {
			get { return null; }
			set{
				SelectedLocationType = value;
				setSearchLabel ();
			}
		}

		public int Distance
		{
			get
			{ 
				return Convert.ToInt32(distanceSlider.Value);
			}
			set
			{ 
				distanceSlider.Value = (float)value;
				_model.SearchRadius = Distance;
				setDistanceLabel ();
			}
		}
		#endregion
	}
}

