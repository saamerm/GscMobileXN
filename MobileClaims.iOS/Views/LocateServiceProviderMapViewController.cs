	using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Input;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.AddressBook;

namespace MobileClaims.iOS
{
	public class LocateServiceProviderMapViewController : MvxViewController
	{
		private List<CustomAnnotation> _cachedAnnotations = new List<CustomAnnotation>();
		private LocateServiceProviderResultViewModel _model;

		protected UIBarButtonItem doneButton;
		protected MKMapView mapView;
		private MapDelegate mapDelegate;
		//protected MiniTabBar tabBar;

		public LocateServiceProviderMapViewController (LocateServiceProviderResultViewModel model)
		{
			this.View = new UIView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			this._model = model;
			base.NavigationItem.Title = "mapProviderTitle".tr();

			doneButton = new UIBarButtonItem();
			doneButton.Style = UIBarButtonItemStyle.Plain;
			doneButton.Clicked += DoneClicked;
			doneButton.Title = "locateProvidersDone".tr();
			base.NavigationItem.RightBarButtonItem = doneButton;

			mapView = new MKMapView ();
			mapDelegate = new MapDelegate ();
			mapDelegate.Model = _model;
			mapDelegate.Parent = this;
			mapView.Delegate = mapDelegate;
			View.AddSubview (mapView);

			Providers = _model.ServiceProviders;
		}

		public override void ViewDidLayoutSubviews ()
		{
			float statusBarHeight = 20; //Status bar height acting peculiar in landscape. TODO: Figure better way to get status bar height. Tried: UIApplication.SharedApplication.StatusBarFrame.Size.Height
			float viewWidth = base.View.Bounds.Width;
			float viewHeight = base.View.Bounds.Height;
			float startY = base.NavigationController.NavigationBar.Frame.Height + statusBarHeight;

			//mapView.Frame = new RectangleF (0, startY, viewWidth, viewHeight - startY - Constants.TAB_BAR_HEIGHT);
			mapView.Frame = new RectangleF (0, startY, viewWidth, viewHeight - startY);
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
				mapView.AddAnnotation(annotationItem);
				_cachedAnnotations.Add (annotationItem);	
				if (Providers[i].ID == _model.SelectedProvider.ID)
					selectedAnnotation = annotationItem;
			}

			if (((CustomAnnotation)selectedAnnotation).Provider == null)	//select first provider in case the ID is null for some reason
				selectedAnnotation = _cachedAnnotations [0];
				
			MKCoordinateSpan span = new MKCoordinateSpan (KilometresToLatitudeDegrees(_model.SearchTerms.Radius), KilometresToLongitudeDegrees(_model.SearchTerms.Radius, Providers [0].Latitude));
					mapView.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (_model.SelectedProvider.Latitude, _model.SelectedProvider.Longitude), span);
			mapView.SelectAnnotation (selectedAnnotation,true);
			View.SetNeedsLayout ();
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
			_model.DoneCommand.Execute(null);
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
				DirectionsMode = MonoTouch.MapKit.MKDirectionsMode.Driving,
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
		#endregion
	}

	class MapDelegate : MKMapViewDelegate
	{
		MKAnnotationView anView;

		public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
		{
			string pId = "test";

			// show pin annotation
			anView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation (pId);	

			if (anView == null)
				anView = new MKPinAnnotationView (annotation, pId);

			((MKPinAnnotationView)anView).PinColor = MKPinAnnotationColor.Red;
			anView.CanShowCallout = true;
			((MKPinAnnotationView)anView).AnimatesDrop = true;
			anView.Selected = true;
			anView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
			return anView;
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public LocateServiceProviderResultViewModel Model { get; set; }
		public LocateServiceProviderMapViewController Parent { set; get; }

		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
		{
			var annotation = view.Annotation as CustomAnnotation;
			if (annotation != null) {
				Parent.openDetailsView (annotation.Provider);
			}
		}
	}

	public class CustomAnnotation : MKAnnotation
	{
		public override CLLocationCoordinate2D Coordinate {get;set;}
		string title, subtitle;
		public override string Title { get{ return title; }}
		public override string Subtitle { get{ return subtitle; }}

		public CustomAnnotation(ServiceProvider provider)
		{
			if (provider != null) 
			{
				Provider = provider;
				title = provider.DoctorName;
				subtitle = provider.Address;
				var coord = new CLLocationCoordinate2D (provider.Latitude, provider.Longitude);
				this.Coordinate = coord;
			}
		}

		private ServiceProvider _provider;
		public ServiceProvider Provider { 
			get 
			{ 
				return _provider;
			} 
			set 
			{ 
				if (value != null) {
					_provider = value;
				}
			} 
		}
	}
}

