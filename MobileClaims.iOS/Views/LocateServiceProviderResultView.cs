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
using MapKit;
using CoreLocation;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using AddressBook;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using MobileClaims.Core.Messages;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
	[Foundation.Register("LocateServiceProviderResultView")]
    public class LocateServiceProviderResultView : GSCBaseViewController
	{
		private List<CustomAnnotation> _cachedAnnotations = new List<CustomAnnotation>();
		private LocateServiceProviderResultViewModel _model;

		protected UIBarButtonItem doneButton;
		protected MKMapView mapView;
		private MapDelegate mapDelegate;
		//protected MiniTabBar tabBar;
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            var messenger = Mvx.Resolve<IMvxMessenger>();
            messenger.Publish<ClearLocateServiceProviderResult>(new ClearLocateServiceProviderResult(this));
        }
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            this.View = new GSCBaseView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationItem.Title = "mapProviderTitle".tr().ToUpper();

			if (Constants.IsPhone ()) {
				base.NavigationItem.SetHidesBackButton (false, false);
			} else {
				base.NavigationItem.SetHidesBackButton (true, false);
			}


			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Constants.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Constants.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
			}

			_model = (LocateServiceProviderResultViewModel)ViewModel;

			doneButton = new UIBarButtonItem();
			doneButton.Style = UIBarButtonItemStyle.Plain;
			doneButton.Clicked += DoneClicked;
			doneButton.Title = "newSearch".tr();
			doneButton.TintColor = Constants.HIGHLIGHT_COLOR;
			UITextAttributes attributes = new UITextAttributes ();
			attributes.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
			doneButton.SetTitleTextAttributes (attributes, UIControlState.Normal);
			base.NavigationItem.RightBarButtonItem = doneButton;

			mapView = new MKMapView ();
			mapDelegate = new MapDelegate ();
			mapDelegate.Model = _model;
			mapDelegate.Parent = this;
			mapView.Delegate = mapDelegate;
            ((GSCBaseView)View).baseContainer.AddSubview (mapView);

			var set = this.CreateBindingSet<LocateServiceProviderResultView, LocateServiceProviderResultViewModel> ();
			set.Bind (Providers).To (vm => vm.ServiceProviders);
			set.Bind (this).For (v => v.SelectedProvider).To (vm => vm.SelectedProvider);
			set.Apply ();


			Providers = _model.ServiceProviders;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			CLLocationCoordinate2D center = new CLLocationCoordinate2D (_model.SelectedProvider.Latitude, _model.SelectedProvider.Longitude);
			center.Latitude += mapView.Region.Span.LatitudeDelta * 0.20;
			mapView.SetCenterCoordinate (center, true);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		public void RemoveAnnotations()
		{
			if (mapView != null) {
				mapView.RemoveAnnotations (mapView.Annotations);
			}
		}


		public override void ViewDidLayoutSubviews ()
		{
            float startY = Constants.CLAIMS_DETAILS_SUB_ITEM_PADDING;
            float viewWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Constants.NAV_HEIGHT;

			//mapView.Frame = new RectangleF (0, startY, viewWidth, viewHeight - startY - Constants.TAB_BAR_HEIGHT);
			mapView.Frame = new CGRect (0, startY, viewWidth, viewHeight);

		}

		public void RefreshMap()
		{
			CustomAnnotation selectedAnnotation = new CustomAnnotation(null);

			if (mapView.Annotations.Length > 0) 
				mapView.RemoveAnnotations (mapView.Annotations);

			_cachedAnnotations.Clear ();
			for (var i = 0; i < Providers.Count; i++)
			{
				CustomAnnotation annotationItem = new CustomAnnotation ((ServiceProvider)Providers [i]);
				mapView.AddAnnotation((MKAnnotation)annotationItem);
				_cachedAnnotations.Add (annotationItem);	
				if (Providers[i].ID == _model.SelectedProvider.ID)
					selectedAnnotation = annotationItem;
			}

			if (((CustomAnnotation)selectedAnnotation).Provider == null)	//select first provider in case the ID is null for some reason
				selectedAnnotation = _cachedAnnotations [0];

			CLLocationCoordinate2D center = new CLLocationCoordinate2D (_model.SelectedProvider.Latitude, _model.SelectedProvider.Longitude);
			center.Latitude += mapView.Region.Span.LatitudeDelta * 0.20;

			MKCoordinateSpan span = new MKCoordinateSpan (KilometresToLatitudeDegrees(_model.SearchTerms.Radius), KilometresToLongitudeDegrees(_model.SearchTerms.Radius, center.Latitude));

			mapView.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (center.Latitude, center.Longitude), span);
			if (Constants.IS_OS_7_OR_LATER())
				mapView.SelectAnnotation (selectedAnnotation,true);
			else
				mapView.SelectAnnotation (selectedAnnotation, true);

			View.SetNeedsLayout ();

		}

		public void SelectAnnotation(ServiceProvider provider)
		{
			if (Providers.Count == _cachedAnnotations.Count) {
				CustomAnnotation selectedAnnotation = new CustomAnnotation (null);

				for (var i = 0; i < Providers.Count; i++) {
					if (Providers [i].ID == _model.SelectedProvider.ID) {
						selectedAnnotation = _cachedAnnotations [i];
						break;
					}
					if (i == Providers.Count) {
						RefreshMap ();
						return;
					}
				}

				if (((CustomAnnotation)selectedAnnotation).Provider == null) {
					RefreshMap ();
					return;
				}

				CLLocationCoordinate2D center = new CLLocationCoordinate2D (_model.SelectedProvider.Latitude, _model.SelectedProvider.Longitude);
				center.Latitude += mapView.Region.Span.LatitudeDelta * 0.20;

				MKCoordinateSpan span = mapView.Region.Span;

				mapView.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (center.Latitude, center.Longitude), span);

				if (Constants.IS_OS_7_OR_LATER ())
					mapView.SelectAnnotation (selectedAnnotation, true);
				else
					mapView.SelectAnnotation (selectedAnnotation, true);
			} else {
				RefreshMap ();
			}
		}

		/// <summary>Converts kilometres to latitude degrees</summary>
		public double KilometresToLatitudeDegrees(double kms)
		{
			double earthRadius = 6371.0; // in kms
			double radiansToDegrees = 180.0/Math.PI;
			return (kms/earthRadius) * radiansToDegrees;
		}

		/// <summary>Converts kilometres to longitudinal degrees at a specified latitude</summary>
		public double KilometresToLongitudeDegrees(double kms, double atLatitude)
		{
			double earthRadius = 6371.0; // in kms
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (kms / radiusAtLatitude) * radiansToDegrees;
		}

		private void DoneClicked (object sender, EventArgs ea) {
			_model.NewSearchCommand.Execute(null);
		}

		public void openDetailsView(ServiceProvider provider)
		{
			LocateServiceProviderMapDetailsController _controller = new LocateServiceProviderMapDetailsController(this,provider);
			//ChooseSpendingAccountTypeView _controller = new ChooseSpendingAccountTypeView();
			base.NavigationController.PushViewController(_controller, true);


		}

		public void DoGetDirections(ServiceProvider provider)
		{
			System.Console.WriteLine ("DoGetDirections");
			NSMutableDictionary address = new NSMutableDictionary();
			address.Add(ABPersonAddressKey.City, new NSString(provider.City));
			address.Add(ABPersonAddressKey.Street, new NSString(provider.Address));
			address.Add(ABPersonAddressKey.State, new NSString(provider.Province));
			address.Add(ABPersonAddressKey.Zip, new NSString(provider.PostalCode));

			//LocateServiceProviderMapDetailsController _controller = new LocateServiceProviderMapDetailsController(provider);
			MKPlacemark mp = new MKPlacemark(new CLLocationCoordinate2D(provider.Latitude, provider.Longitude),address);
			MKMapItem mapItem = new MKMapItem(mp);
			MKLaunchOptions options = new MKLaunchOptions()
			{
				DirectionsMode = MapKit.MKDirectionsMode.Driving,
				MapCenter = new CLLocationCoordinate2D(provider.Latitude, provider.Longitude),
				ShowTraffic = true
			};
			mapItem.OpenInMaps(options);
		}

		#region properties
		private List<ServiceProvider> _providers;
		public List<ServiceProvider> Providers
		{
			get
			{
				return _providers;
			}
			set
			{
				_providers = value;
				RefreshMap ();
			}
		}

		private ServiceProvider _selectedProvider;
		public ServiceProvider SelectedProvider
		{
			get { return _selectedProvider; }
			set { 
				if (_selectedProvider != value) {
					_selectedProvider = value; 
					SelectAnnotation (SelectedProvider); 
				}

			}
		}
		#endregion
	}

	class MapDelegate : MKMapViewDelegate
	{
		MKAnnotationView anView;

		UILabel nameLabel;
		UILabel addressLabel;
		UILabel telephoneLabel;
		UILabel paymentSubmitLabel;
		UILabel paymentLabel;
		UIView containerView;
		UIButton directionsButton;
		UIImageView arrowImage;

		float containerWidth = 150;
		float padding = 5;

		ServiceProvider currentProvider;

		static string customIdentifier  = "CustomAnnotation";
		static string calloutIdentifier = "CalloutAnnotation";

		public override void DidSelectAnnotationView (MKMapView mapView, MKAnnotationView view)
		{
			if (view.Annotation is CustomAnnotation) {

				var annotation = view.Annotation as CustomAnnotation;
				if (annotation != null) {
					currentProvider = annotation.Provider;
				}

				if(Model.SelectedProvider != annotation.Provider)
					Model.SelectedProvider = annotation.Provider;

				CLLocationCoordinate2D center = annotation.Coordinate;
				center.Latitude += mapView.Region.Span.LatitudeDelta * 0.20;
				mapView.SetCenterCoordinate (center, true);

				CalloutAnnotation calloutAn = new CalloutAnnotation (currentProvider);

				mapView.AddAnnotation((MKAnnotation)calloutAn);

				//			InvokeOnMainThread (() => {
				//				mapView.SelectAnnotation (calloutAnnotation, true);
				//			});


			}
		}

		public override void DidAddAnnotationViews (MKMapView mapView, MKAnnotationView[] views)
		{
			foreach(MKAnnotationView anView in views){
			
				if (anView.Annotation is CalloutAnnotation) {
					anView.Superview.BringSubviewToFront (anView);
				} else {

					anView.Superview.SendSubviewToBack (anView);
				}
			}
		}

		void HandleTouchUpInside (object sender, EventArgs e)
		{
			Parent.DoGetDirections (currentProvider);
		}
        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null; 

			if (annotation is CustomAnnotation) {


				anView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation (customIdentifier);


				if (anView == null)
					anView = new MKPinAnnotationView (annotation, customIdentifier);

				((MKPinAnnotationView)anView).PinColor = MKPinAnnotationColor.Green;
				//anView.Selected = true;

				anView.CanShowCallout = false;
			} else if (annotation is CalloutAnnotation) {

				anView = (MKAnnotationView)mapView.DequeueReusableAnnotation (calloutIdentifier);

				if (anView == null)
					anView = new MKAnnotationView (annotation, calloutIdentifier);
			
				foreach (UIView v in anView.Subviews) {
					v.RemoveFromSuperview ();
				}

				containerView = new UIView ();
				containerView.BackgroundColor = Constants.BACKGROUND_COLOR;
				containerView.Layer.BorderColor = Constants.MED_GREY_COLOR.CGColor;
				containerView.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
				anView.AddSubview (containerView);

				nameLabel = new UILabel();
				nameLabel.Text = ((CalloutAnnotation)annotation).Provider.DoctorName;
				nameLabel.BackgroundColor = UIColor.Clear;
				nameLabel.TextColor = Constants.DARK_GREY_COLOR;
				nameLabel.LineBreakMode = UILineBreakMode.WordWrap;
				nameLabel.Lines = 0;
				nameLabel.TextAlignment = UITextAlignment.Left;
				nameLabel.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.SMALL_FONT_SIZE);
				containerView.AddSubview(nameLabel);

				addressLabel = new UILabel();
				addressLabel.Text = ((CalloutAnnotation)annotation).Provider.Address + "\r\n" + ((CalloutAnnotation)annotation).Provider.FormattedCityProv + "\r\n" + ((CalloutAnnotation)annotation).Provider.PostalCode ;
				addressLabel.BackgroundColor = UIColor.Clear;
				addressLabel.TextColor = Constants.DARK_GREY_COLOR;
				addressLabel.LineBreakMode = UILineBreakMode.WordWrap;
				addressLabel.Lines = 0;
				addressLabel.TextAlignment = UITextAlignment.Left;
				addressLabel.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.SMALL_FONT_SIZE);
				containerView.AddSubview(addressLabel);

				telephoneLabel = new UILabel();
				telephoneLabel.Text = ((CalloutAnnotation)annotation).Provider.Phone;
				telephoneLabel.BackgroundColor = UIColor.Clear;
				telephoneLabel.TextColor = Constants.DARK_GREY_COLOR;
				telephoneLabel.LineBreakMode = UILineBreakMode.WordWrap;
				telephoneLabel.Lines = 0;
				telephoneLabel.TextAlignment = UITextAlignment.Left;
				telephoneLabel.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.SMALL_FONT_SIZE);
				containerView.AddSubview(telephoneLabel);

				paymentSubmitLabel = new UILabel();
				paymentSubmitLabel.Text = ((CalloutAnnotation)annotation).Provider.CanSubmitClaimsOnline ? "mapProviderDetailsSubmit".tr() : "";
				paymentSubmitLabel.BackgroundColor = UIColor.Clear;
				paymentSubmitLabel.TextColor = Constants.DARK_GREY_COLOR;
				paymentSubmitLabel.LineBreakMode = UILineBreakMode.WordWrap;
				paymentSubmitLabel.Lines = 0;
				paymentSubmitLabel.TextAlignment = UITextAlignment.Left;
				paymentSubmitLabel.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.SMALL_FONT_SIZE);
				containerView.AddSubview(paymentSubmitLabel);

				paymentLabel = new UILabel();
				paymentLabel.Text = ((CalloutAnnotation)annotation).Provider.CanAcceptPaymentDirectly ? "mapProviderDetailsAccept".tr() : "";
				paymentLabel.BackgroundColor = UIColor.Clear;
				paymentLabel.TextColor = Constants.DARK_GREY_COLOR;
				paymentLabel.LineBreakMode = UILineBreakMode.WordWrap;
				paymentLabel.Lines = 0;
				paymentLabel.TextAlignment = UITextAlignment.Left;
				paymentLabel.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.SMALL_FONT_SIZE);
				containerView.AddSubview(paymentLabel);

				directionsButton = new GSButton ();
				directionsButton.SetTitle ("getDirections".tr (), UIControlState.Normal);
				directionsButton.TouchUpInside += HandleTouchUpInside;
				containerView.AddSubview (directionsButton);

				float yPos = 5;
				float contentWidth = containerWidth - padding * 2;

				nameLabel.Frame = new CGRect (padding, yPos, contentWidth, nameLabel.Frame.Height);
				nameLabel.SizeToFit ();
				yPos += (float)nameLabel.Frame.Height + padding;

				addressLabel.Frame = new CGRect (padding, yPos, contentWidth, addressLabel.Frame.Height);
				addressLabel.SizeToFit ();
                yPos += (float)addressLabel.Frame.Height + padding;

				telephoneLabel.Frame = new CGRect (padding, yPos, contentWidth, telephoneLabel.Frame.Height);
				telephoneLabel.SizeToFit ();
                yPos += (float)telephoneLabel.Frame.Height + padding;

				if (((CalloutAnnotation)annotation).Provider.CanSubmitClaimsOnline) {
					paymentSubmitLabel.Frame = new CGRect (padding, yPos, contentWidth, paymentSubmitLabel.Frame.Height);
					paymentSubmitLabel.SizeToFit ();
                    yPos += (float)paymentSubmitLabel.Frame.Height + padding;
				}

				if (((CalloutAnnotation)annotation).Provider.CanAcceptPaymentDirectly) {
					paymentLabel.Frame = new CGRect (padding, yPos, contentWidth, paymentLabel.Frame.Height);
					paymentLabel.SizeToFit ();
                    yPos += (float)paymentLabel.Frame.Height + padding;
				}

				directionsButton.Frame = new CGRect (padding, yPos, contentWidth, 30);
				yPos += 30 + padding;

				float containerHeight = yPos + 5;

				arrowImage = new UIImageView(UIImage.FromBundle(Constants.MISC_PATH + "popout.png"));
				arrowImage.BackgroundColor = UIColor.Clear;
				arrowImage.Opaque = false;
				anView.AddSubview(arrowImage);

				containerView.Frame = new CGRect (0, 0, containerWidth, containerHeight);

				if (arrowImage != null && arrowImage.Image != null) {
                    float buttonImageWidth = (float)arrowImage.Image.Size.Width;
                    float buttonImageHeight = (float)arrowImage.Image.Size.Height;
					float buttonImageX = (containerWidth/2 - buttonImageWidth/2);
					float buttonImageY = containerHeight-2;
					arrowImage.Frame = new CGRect (buttonImageX, buttonImageY, buttonImageWidth, buttonImageHeight);
				}

                anView.Frame = (CGRect)containerView.Frame;
				anView.CenterOffset = new CGPoint (0, -containerHeight/2 - 40);

				anView.CanShowCallout = false;
			}


			return anView;
		}

		public override void DidDeselectAnnotationView (MKMapView mapView, MKAnnotationView view)
		{

			if (view.Annotation is CustomAnnotation) {

				foreach(MKAnnotation an in mapView.Annotations){

					if (an is CalloutAnnotation) {
						mapView.RemoveAnnotation (an);
						break;
					}
				}
				  
			}

//			if (view.Annotation is CalloutAnnotation) {
//
//				InvokeOnMainThread (() => {
//					if (view.Annotation != null) {
//
//						try
//						{
//							mapView.RemoveAnnotation (view.Annotation);
//						}
//						catch(Exception ex)
//						{
//							MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
//						}
//
//					}
//				});
//
//
//			}
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public LocateServiceProviderResultViewModel Model { get; set; }
		public LocateServiceProviderResultView Parent { set; get; }

		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
		{
			var annotation = view.Annotation as CustomAnnotation;
			if (annotation != null) {
				Parent.DoGetDirections (annotation.Provider);
			}
		}
	}

	class CustomAnnotation : MKAnnotation
    {
        public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }
        public CLLocationCoordinate2D Coords;

		public CustomAnnotation(ServiceProvider provider)
		{
			if (provider != null) 
			{
				Provider = provider;
				var coord = new CLLocationCoordinate2D (provider.Latitude, provider.Longitude);
                this.Coords = coord;
			}
		}

		private ServiceProvider _provider;
		public ServiceProvider Provider {
			get 
			{
				return _provider;
			}
			set {
				_provider = value;
			}
		}
	}



	class CalloutAnnotation : MKAnnotation
	{
        public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }
        public CLLocationCoordinate2D Coords;

		public CalloutAnnotation(ServiceProvider provider)
		{
			if (provider != null) 
			{
				Provider = provider;
				var coord = new CLLocationCoordinate2D (provider.Latitude, provider.Longitude);
                this.Coords = coord;
			}
		}

		private ServiceProvider _provider;
		public ServiceProvider Provider {
			get 
			{
				return _provider;
			}
			set {
				_provider = value;
			}
		}
	}
}

