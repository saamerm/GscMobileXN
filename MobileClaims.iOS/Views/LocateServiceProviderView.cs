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
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MapKit;
using CoreLocation;
using Cirrious.CrossCore;
using MobileClaims.Core;
using Cirrious.MvvmCross.Plugins.Messenger;
using MobileClaims.Core.Messages;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
	[Foundation.Register("LocateServiceProviderView")]
    public class LocateServiceProviderView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		private LocateServiceProviderViewModel _model;

		protected UIScrollView scrollContainer;

		protected UILabel distanceTitleLabel;

		protected DefaultTextField addressField;
		protected UILabel exampleAddressLabel;
		protected DefaultTextField postalCodeField;
		protected UILabel examplePostalCodeLabel;

		protected SliderWithIndicators distanceSlider;

		protected UILabel phoneNumberTitleLabel;
		protected DefaultTextField phoneNumberField;
		protected UILabel lastNameTitleLabel;
		protected DefaultTextField lastNameField;
		protected UILabel businessNameTitleLabel;
		protected DefaultTextField businessNameField;
		protected UILabel cityTitleLabel;
		protected DefaultTextField cityField;

		protected UILabel infoLabel;
		protected GSButton searchButton;
		protected UILabel locationServicesLabel;

		protected int addressCheck;
		protected int searchCheck;

        protected UIView LastNameContainer;
        protected UIView AddressContainer;
        protected UIView PhoneNumberContainer;
        protected UIView PostalCodeContainer;
        protected UIView BusinessNameContainer;
        protected UIView CityContainer;

		private float FIELD_HEIGHT = Constants.IsPhone() ? 40 : 50;

		private float BUTTON_WIDTH = Constants.IsPhone() ? 225 : 250;
		private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 60;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View = new GSCBaseView() { BackgroundColor = Constants.BACKGROUND_COLOR };

			_model = (LocateServiceProviderViewModel)ViewModel;

            base.NavigationItem.Title = "detailsProviderTitle".tr();
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			if (!Constants.IsPhone())
				base.NavigationItem.HidesBackButton = true;
			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Constants.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Constants.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
			}

            #region Setup the Container views
            LastNameContainer = new UIView();
            AddressContainer = new UIView();
            PhoneNumberContainer = new UIView();
            PostalCodeContainer = new UIView();
            BusinessNameContainer = new UIView();
            CityContainer = new UIView();
            #endregion

            #region Setup the Scroll View
			scrollContainer = ((GSCBaseView)View).baseScrollContainer;
            scrollContainer.BackgroundColor = Constants.BACKGROUND_COLOR;
			((GSCBaseView)View).baseContainer.AddSubview (scrollContainer);

			((GSCBaseView)View).ViewTapped += HandleViewTapped;
            #endregion

            #region Static Elements -> these don't disappear
            distanceTitleLabel = new UILabel();
            distanceTitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			distanceTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			distanceTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
            distanceTitleLabel.TextAlignment = UITextAlignment.Left;
			distanceTitleLabel.Lines = 3;
            distanceTitleLabel.Text = "Find services within ";
			distanceTitleLabel.BackgroundColor = UIColor.Clear;
            scrollContainer.Add (distanceTitleLabel);

            distanceSlider = new SliderWithIndicators ();
            distanceSlider.ThumbTintColor = Constants.BRANDED_GREEN;
			//if (Constants.IS_OS_7_OR_LATER())
				//distanceSlider.TintColor = Constants.LIGHT_BLUE_COLOR;
            distanceSlider.MinValue = 5.0f;
			distanceSlider.MaxValue = 50.0f;
            distanceSlider.Value = 5.0f;
			distanceSlider.NumberOfIndicators = 4;
			distanceSlider.Suffix = "km";
            distanceSlider.ValueChanged += (sender, ea) => {
                _model.SearchRadius = Convert.ToInt32(distanceSlider.Value);
                Distance = Convert.ToInt32(distanceSlider.Value);
            } ;
            scrollContainer.Add (distanceSlider);

            //"Up to 50 providers will be shown"
            infoLabel = new UILabel ();
            infoLabel.Text = "detailsProviderInfoLabel".tr();
			infoLabel.TextColor = Constants.DARK_GREY_COLOR;
			infoLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
            infoLabel.TextAlignment = UITextAlignment.Center;
			infoLabel.BackgroundColor = UIColor.Clear;
			infoLabel.Lines = 0;
			infoLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (infoLabel);


			searchButton = new GSButton();
            searchButton.SetTitle("detailsProviderTitle".tr(), UIControlState.Normal);
            searchButton.TouchUpInside += DoFindAction;
            scrollContainer.Add (searchButton);

			locationServicesLabel = new UILabel ();
			locationServicesLabel.Text = "ifCurrentLocationServicesNotWorking".tr();
			locationServicesLabel.TextColor = Constants.DARK_GREY_COLOR;
			locationServicesLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			locationServicesLabel.TextAlignment = UITextAlignment.Center;
			locationServicesLabel.BackgroundColor = UIColor.Clear;
			locationServicesLabel.Lines = 0;
			locationServicesLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (locationServicesLabel);

            #endregion

            #region Populate disappearing containers!
            #region Address
            //upper text field
            //address components
			addressField = new DefaultTextField ();
			addressField.Placeholder = "locateProviderAddressLabelLower".tr ();
            addressField.ShouldReturn += (textField) => {
                textField.ResignFirstResponder();
                return true; 
            } ;
			AddressContainer.AddSubview (addressField); 

            exampleAddressLabel = new UILabel();
            exampleAddressLabel.Text = "detailsProviderExampleAddressLabel".tr();
			exampleAddressLabel.TextColor = Constants.DARK_GREY_COLOR;
            exampleAddressLabel.TextAlignment = UITextAlignment.Left;
			exampleAddressLabel.BackgroundColor = UIColor.Clear;
			exampleAddressLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			AddressContainer.AddSubview (exampleAddressLabel);
            scrollContainer.AddSubview(AddressContainer);
            #endregion

            #region Postal Code
            //postal code components
			postalCodeField = new DefaultTextField ();
			postalCodeField.Placeholder = "locateProviderPostalCodeLabelLower".tr();
            postalCodeField.ShouldReturn += (textField) => { 
                textField.ResignFirstResponder();
                return true; 
            } ;
			PostalCodeContainer.AddSubview (postalCodeField);  

            examplePostalCodeLabel = new UILabel();
            examplePostalCodeLabel.Text = "detailsProviderExamplePostalCodeLabel".tr();
			examplePostalCodeLabel.TextColor = Constants.DARK_GREY_COLOR;
            examplePostalCodeLabel.TextAlignment = UITextAlignment.Left;
			examplePostalCodeLabel.BackgroundColor = UIColor.Clear;
			examplePostalCodeLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			PostalCodeContainer.AddSubview (examplePostalCodeLabel);
            scrollContainer.AddSubview(PostalCodeContainer);
            #endregion

            #region Phone Number
            //phone number components
            phoneNumberTitleLabel = new UILabel ();
			phoneNumberTitleLabel.Text = "detailsProviderSearchLabel".tr () + "detailsProviderSearchLabelEnd".tr () + "colon".tr();
			phoneNumberTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			phoneNumberTitleLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
            phoneNumberTitleLabel.TextAlignment = UITextAlignment.Left;
			phoneNumberTitleLabel.BackgroundColor = UIColor.Clear;
            phoneNumberTitleLabel.Lines = 2;
			PhoneNumberContainer.AddSubview (phoneNumberTitleLabel);

			phoneNumberField = new DefaultTextField ();
			phoneNumberField.Placeholder = "locateProviderPhoneNumberLabelLower".tr ();
            phoneNumberField.ShouldReturn += (textField) => { 
                textField.ResignFirstResponder();
                return true; 
            } ;
			PhoneNumberContainer.AddSubview (phoneNumberField);
            scrollContainer.AddSubview(PhoneNumberContainer);
            #endregion

            #region Business Name
            //business name components
            businessNameTitleLabel = new UILabel ();
			businessNameTitleLabel.Text = "detailsProviderSearchLabel".tr () + "detailsProviderSearchLabelEnd".tr () + "colon".tr();
			businessNameTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			businessNameTitleLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
            businessNameTitleLabel.TextAlignment = UITextAlignment.Left;
			businessNameTitleLabel.BackgroundColor = UIColor.Clear;
            businessNameTitleLabel.Lines = 2;
            BusinessNameContainer.AddSubview(businessNameTitleLabel);

			businessNameField = new DefaultTextField ();
			businessNameField.Placeholder = "locateProviderBusinessNameLabelLower".tr ();
            businessNameField.ShouldReturn += (textField) => { 
                textField.ResignFirstResponder();
                return true; 
            } ;
            BusinessNameContainer.AddSubview(businessNameField);
            scrollContainer.AddSubview(BusinessNameContainer);
            #endregion

            #region Last Name
            //last name components
            lastNameTitleLabel = new UILabel ();
			lastNameTitleLabel.Text = "detailsProviderSearchLabel".tr () + "detailsProviderSearchLabelEnd".tr () + "colon".tr();
			lastNameTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
            lastNameTitleLabel.TextAlignment = UITextAlignment.Left;
			lastNameTitleLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			lastNameTitleLabel.BackgroundColor = UIColor.Clear;
            lastNameTitleLabel.Lines = 2;
            LastNameContainer.AddSubview (lastNameTitleLabel);

			lastNameField = new DefaultTextField ();
			lastNameField.Placeholder = "locateProviderLastNameLabelLower".tr ();
            lastNameField.ShouldReturn += (textField) => { 
                textField.ResignFirstResponder();
                return true; 
            } ;
            LastNameContainer.AddSubview (lastNameField);
            scrollContainer.AddSubview(LastNameContainer);
            #endregion

            #region City
            //city components
            cityTitleLabel = new UILabel ();
			cityTitleLabel.Text = "detailsProviderSearchLabel".tr () + "detailsProviderSearchLabelEnd".tr () + "colon".tr();
			cityTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
            cityTitleLabel.TextAlignment = UITextAlignment.Left;
			cityTitleLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );;
			cityTitleLabel.BackgroundColor = UIColor.Clear;
            cityTitleLabel.Lines = 2;
            CityContainer.AddSubview (cityTitleLabel);

			cityField = new DefaultTextField ();
			cityField.Placeholder = "locateProviderCityLabelLower".tr ();
            cityField.ShouldReturn += (textField) => { 
                textField.ResignFirstResponder();
                return true; 
            } ;
            CityContainer.AddSubview (cityField);
            scrollContainer.AddSubview(CityContainer);
            #endregion
			#endregion

            #region Bindings
			var set = this.CreateBindingSet<LocateServiceProviderView, LocateServiceProviderViewModel> ();
			set.Bind (ProviderType).To (vm => vm.ProviderType.Type);
			set.Bind (this).For (v => v.SelectedLocationType).To (vm => vm.SelectedLocationType);
			set.Bind (SelectedSearchType).To (vm => vm.SelectedSearchType.TypeName);

			set.Bind(AddressContainer).For(ac => ac.Hidden).To(vm => vm.ShowLocationTypeAddress).TwoWay();
			set.Bind(PostalCodeContainer).For(ac => ac.Hidden).To(vm => vm.ShowLocationTypePostalCode).TwoWay();
			set.Bind(PhoneNumberContainer).For(ac => ac.Hidden).To(vm => vm.ShowSearchTypePhoneNumber).TwoWay();
			set.Bind(LastNameContainer).For(ac => ac.Hidden).To(vm => vm.ShowSearchTypeLastName).TwoWay();
			set.Bind(BusinessNameContainer).For(ac => ac.Hidden).To(vm => vm.ShowSearchTypeBusinessName).TwoWay();
			set.Bind(CityContainer).For(ac => ac.Hidden).To(vm => vm.ShowSearchtypeCity).TwoWay();

			set.Bind(addressField).To(vm => vm.Address).TwoWay();
			set.Bind(postalCodeField).To(vm => vm.PostalCode).TwoWay();
			set.Bind(phoneNumberField).To(vm => vm.PhoneNumber).TwoWay();
			set.Bind(lastNameField).To(vm => vm.LastName).TwoWay();
			set.Bind(businessNameField).To(vm => vm.BusinessName).TwoWay();
			set.Bind(cityField).To(vm => vm.City);
            set.Apply ();
            #endregion

			//set empty field error check string lengths
			if (_model.SelectedLocationType != null) {
				switch (_model.SelectedLocationType.TypeName) {
				case "Address":
					addressCheck = 5;
					break;
				case "Postal Code":
					addressCheck = 5;
					break;
				}
			}

            if (_model.SelectedSearchType != null) {
				switch (_model.SelectedSearchType.TypeName) {
				case "Phone Number":
					searchCheck = 10;
					break;
				case "Last Name":
					searchCheck = 2;
					break;
				case "Business Name":
					searchCheck = 2;
					break;
				case "City":
					searchCheck = 2;
					break;
				}
			}

			_model.NoProvidersFound += ErrorNoResults;
			_model.MissingAddressOrPostalCode += ErrorEmptyFields;
			_model.InvalidPhoneNumber += ErrorInvalidPhoneField;
			_model.InvalidPostalCode += ErrorInvalidPostalField;

			//System.Console.WriteLine("Passed: "+_model.SelectedLocationType.TypeName);
			Distance = 5;
            setDistanceLabel ();
			setSearchLabel ();

		}

		private void DoFindAction (object sender, EventArgs ea) {
			//if ((phoneNumberField.Text.Length >= searchCheck || PhoneNumberContainer.Hidden) && (lastNameField.Text.Length >= searchCheck || LastNameContainer.Hidden) && (businessNameField.Text.Length >= searchCheck || BusinessNameContainer.Hidden) && (cityField.Text.Length >= searchCheck || CityContainer.Hidden) && (addressField.Text.Length >= addressCheck || AddressContainer.Hidden) && (postalCodeField.Text.Length >= addressCheck || PostalCodeContainer.Hidden)) {
			//if (_model.FindProviderCommand != null && _model.FindProviderCommand.CanExecute (null))
			if (Constants.IsPhone())		
				_model.FindProviderCommand.Execute (null);
			else
				_model.FindProviderCommand.Execute (null);
			//}  else {
			//ErrorEmptyFields (null, null);
			//}
		}

		private void ErrorNoResults (object sender, EventArgs ea)
		{
			InvokeOnMainThread (() => {

				if( !AddressContainer.Hidden || !PostalCodeContainer.Hidden){
					var alert = new UIAlertView ("detailsProviderErrorTitle".tr (), "detailsProviderErrorDesc2".tr (), null, "OK", null);
					alert.Show ();
				}else{
					var alert = new UIAlertView ("detailsProviderErrorTitle".tr (), "detailsProviderErrorDesc".tr (), null, "OK", null);
					alert.Show ();
				}



			});
		}

		private void ErrorEmptyFields (object sender, EventArgs ea)
		{
			InvokeOnMainThread (() => {
				var alert = new UIAlertView ("detailsProviderErrorTitle".tr (), "detailsProviderErrorAddress".tr (), null, "OK", null);
				alert.Show ();
			});
		}

		private void ErrorInvalidPostalField (object sender, EventArgs ea)
		{
			InvokeOnMainThread (() => {
				var alert = new UIAlertView ("detailsProviderErrorTitle".tr (), "detailsProviderErrorPostal".tr(), null, "OK", null);
				alert.Show ();
			});
		}

		private void ErrorInvalidPhoneField (object sender, EventArgs ea)
		{
			InvokeOnMainThread (() => {
				var alert = new UIAlertView ("detailsProviderErrorTitle".tr(), "detailsProviderErrorPhone".tr(), null, "OK", null);
				alert.Show ();
			});
		}

		void HandleViewTapped (object sender, EventArgs e)
		{
			dismissKeyboard ();
		}

		void dismissKeyboard()
		{
			this.View.EndEditing (true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			((GSCBaseView)View).subscribeToBusyIndicator ();
			View.SetNeedsLayout ();
			
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			dismissKeyboard ();
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);

			((GSCBaseView)View).unsubscribeFromBusyIndicator ();
		}


		public override void ViewDidLayoutSubviews ()
		{
            base.ViewDidLayoutSubviews();
            float startY = ViewContentYPositionPadding;

			float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
			float textFieldSidePadding = 5;

			float fieldHeight = FIELD_HEIGHT;

			distanceTitleLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING+textFieldSidePadding, startY+Constants.DRUG_LOOKUP_TOP_PADDING, contentWidth-textFieldSidePadding*2, Constants.DRUG_LOOKUP_LABEL_HEIGHT*2);

			//upper field
			addressField.Frame = new CGRect (0, 0, contentWidth, fieldHeight);
			exampleAddressLabel.Frame = new CGRect (textFieldSidePadding, fieldHeight, contentWidth-textFieldSidePadding*2, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			postalCodeField.Frame = (CGRect)addressField.Frame;
			examplePostalCodeLabel.Frame = (CGRect)exampleAddressLabel.Frame;
			AddressContainer.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)distanceTitleLabel.Frame.Y+(float)distanceTitleLabel.Frame.Height+4, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT+fieldHeight);
			PostalCodeContainer.Frame = (CGRect)AddressContainer.Frame;
			//AddressContainer.BackgroundColor = UIColor.Blue;

			distanceSlider.Frame = AddressContainer.Hidden == true ? new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING+5, (float)AddressContainer.Frame.Y + (float)AddressContainer.Frame.Height+7, contentWidth-10, 50) : new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING+5, (float)AddressContainer.Frame.Y + (float)AddressContainer.Frame.Height+7, contentWidth-10, 50);

			//lower field
            #region Use these bounds to layout container controls
			phoneNumberTitleLabel.Frame = new CGRect (textFieldSidePadding, 0, contentWidth-textFieldSidePadding*2, Constants.DRUG_LOOKUP_FIELD_HEIGHT);
			phoneNumberField.Frame = new CGRect (0, Constants.DRUG_LOOKUP_FIELD_HEIGHT+4, contentWidth, fieldHeight);
            #endregion
            lastNameTitleLabel.Frame = (CGRect)phoneNumberTitleLabel.Frame;
			lastNameField.Frame = (CGRect)phoneNumberField.Frame;
			businessNameTitleLabel.Frame = (CGRect)phoneNumberTitleLabel.Frame;
			businessNameField.Frame = (CGRect)phoneNumberField.Frame;
			cityTitleLabel.Frame = (CGRect)phoneNumberTitleLabel.Frame;
			cityField.Frame = (CGRect)phoneNumberField.Frame;
			PhoneNumberContainer.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, distanceSlider.Frame.Y + distanceSlider.Frame.Height + 60, contentWidth, Constants.DRUG_LOOKUP_FIELD_HEIGHT*2+4);
			BusinessNameContainer.Frame = (CGRect)PhoneNumberContainer.Frame;
			CityContainer.Frame = (CGRect)PhoneNumberContainer.Frame;
			LastNameContainer.Frame = (CGRect)PhoneNumberContainer.Frame;

			bool allHidden = LastNameContainer.Hidden && BusinessNameContainer.Hidden && CityContainer.Hidden && PhoneNumberContainer.Hidden;

			float yPos = (float)PhoneNumberContainer.Frame.Y + (float)PhoneNumberContainer.Frame.Height + 15;

			if (Constants.IsPhone () && allHidden) {
				yPos = (float)PhoneNumberContainer.Frame.Y + 15;
			}

			infoLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, contentWidth, (float)infoLabel.Frame.Height);
			infoLabel.SizeToFit ();
			infoLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, contentWidth, (float)infoLabel.Frame.Height);

			yPos += (float)infoLabel.Frame.Height + 10;

			searchButton.Frame = new CGRect (ViewContainerWidth / 2 - BUTTON_WIDTH / 2, yPos, BUTTON_WIDTH, Constants.DRUG_BUTTON_HEIGHT);

			yPos += (float)searchButton.Frame.Height + 15;

			locationServicesLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, contentWidth, (float)locationServicesLabel.Frame.Height);
			locationServicesLabel.SizeToFit ();
			locationServicesLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, yPos, contentWidth, (float)locationServicesLabel.Frame.Height);

            yPos += (float)locationServicesLabel.Frame.Height; // + 15;

            scrollContainer.Frame = new CGRect (0, 0, ViewContainerWidth, ViewContainerHeight);			

            var bottomPadding = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            scrollContainer.ContentSize = new CGSize (ViewContainerWidth, 
                                                      yPos + GetBottomPadding(Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING, 15));
		}

		void setSearchLabel()
		{
			if (_model.SelectedSearchType != null) {
					phoneNumberTitleLabel.Text = "detailsShowProviderSearchLabelName".tr ();
					phoneNumberTitleLabel.Text += "locateProviderPhoneNumberLabel".tr ();
					phoneNumberTitleLabel.Text += "detailsProviderSearchLabelEnd".tr () + "colon".tr();

					lastNameTitleLabel.Text = "detailsShowProviderSearchLabelName".tr ();
					lastNameTitleLabel.Text += "locateProviderLastNameLabel".tr ();
					lastNameTitleLabel.Text += "detailsProviderSearchLabelEnd".tr () + "colon".tr();

					businessNameTitleLabel.Text = "detailsShowProviderSearchLabelName".tr ();
					businessNameTitleLabel.Text += "locateProviderBusinessNameLabel".tr ();
					businessNameTitleLabel.Text += "detailsProviderSearchLabelEnd".tr () + "colon".tr();

					cityTitleLabel.Text = "detailsShowProviderSearchLabelCity".tr ();
					cityTitleLabel.Text += "locateProviderCityLabel".tr ();
					cityTitleLabel.Text += "detailsProviderSearchLabelEnd2".tr () + "colon".tr();
			}  else {
				phoneNumberField.Text = "test";
			}
		}

		void setDistanceLabel()
		{
			if (_model.SelectedLocationType != null) {

				string radiusInfoLabel = "";

				switch (_model.SelectedLocationType.TypeName) {
				case "Address":
					radiusInfoLabel = "detailsProviderRadiusInfoLabelEnd2".tr ();
					break;
				case "Postal Code":
					radiusInfoLabel = "detailsProviderRadiusInfoLabelEnd3".tr ();
					break;
				default:
					radiusInfoLabel = "detailsProviderRadiusInfoLabelEnd1".tr ();
					break;
				}

				distanceTitleLabel.Text = "distanceHealthProviderLabel".tr ();
				distanceTitleLabel.Text += Distance + " KM";
				distanceTitleLabel.Text += radiusInfoLabel;
				distanceTitleLabel.Text += convertSearchString( _model.SelectedLocationType.TypeName);
				distanceTitleLabel.Text += "colon".tr();

				switch (_model.SelectedLocationType.TypeName) {
				case "Address":
					exampleAddressLabel.Text = "detailsProviderExampleAddressLabel".tr ().ToUpper();
					break;
				case "Postal Code":
					exampleAddressLabel.Text = "detailsProviderExamplePostalCodeLabel".tr ().ToUpper();
					break;
				}
			}  else {
				distanceTitleLabel.Text = "error";
			}
		}

		private string convertSearchString(string type)
		{
			string responseKey;

			switch ((string)type)
			{
			case "My Current Location":
				responseKey = "locateProviderMyCurrentLocationLabel".tr();
				break;
			case "Address":
				responseKey = "locateProviderAddressLabel".tr();
				break;
			case "Postal Code":
				responseKey = "locateProviderPostalCodeLabel".tr();
				break;
			case "No Filter":
				responseKey = " ";
				break;
			case "Last Name":
				responseKey = "locateProviderLastNameLabel".tr();
				break;
			case "Business Name":
				responseKey = "locateProviderBusinessNameLabel".tr();
				break;
			case "City":
				responseKey = "locateProviderCityLabel".tr();
				break;
			case "Phone Number":
				responseKey = "locateProviderPhoneNumberLabel".tr();
				break;
			default:
				responseKey = (string)type;
				break;
			}
			return responseKey;
		}

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)base.View.Bounds.Width;
            }
        }

        public float GetViewContainerHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Constants.NAV_HEIGHT;
            }
            else
            {
                return (float)base.View.Bounds.Height - Constants.NAV_HEIGHT;
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return 0;
            }
            else
            {
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            }
        }

        #region properties


        public string ProviderType {
			get { return null; }
			set{
				ProviderType = value;
				setDistanceLabel ();
			}
		}

		public LocationType SelectedLocationType {
			get { return null; }
			set{
				setDistanceLabel ();
				setSearchLabel ();
			}
		}

		public string SelectedSearchType {
			get { return null; }
			set{
				SelectedSearchType = value;
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

