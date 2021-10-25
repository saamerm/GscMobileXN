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
using Cirrious.MvvmCross.Binding.Touch.Views;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
    public class LocateServiceProviderMapDetailsController : GSCBaseViewController
	{
		protected UIScrollView scrollContainer;
		protected UIView textContainer;
		protected UILabel providerLabel;
		protected UIButton directionButton;
		protected LocateServiceProviderResultView parent;

		public LocateServiceProviderMapDetailsController (LocateServiceProviderResultView par, ServiceProvider pro)
		{
			this.View = new UIView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationItem.Title = "mapProviderDetailsTitle".tr();
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Constants.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Constants.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
			}

			scrollContainer = new UIScrollView ();
			scrollContainer.BackgroundColor = Constants.BACKGROUND_COLOR;
			View.AddSubview (scrollContainer);

			textContainer = new UIView ();
			textContainer.BackgroundColor = UIColor.White;
			scrollContainer.AddSubview (textContainer);

			providerLabel = new UILabel();
			providerLabel.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			providerLabel.Lines = 11;
			providerLabel.TextAlignment = UITextAlignment.Left;
			providerLabel.Text = "temp";
			textContainer.AddSubview (providerLabel);

			directionButton = new UIButton ();
			directionButton.BackgroundColor = UIColor.White;
			directionButton.Layer.CornerRadius = 6.0f;
			directionButton.Font = UIFont.SystemFontOfSize ((nfloat)Constants.SMALL_FONT_SIZE);
			directionButton.SetTitleColor (Constants.LIGHT_BLUE_COLOR, UIControlState.Normal);
			directionButton.SetTitle ("Get Directions", UIControlState.Normal);
			directionButton.TouchUpInside += DoGetDirectionsAction;
			scrollContainer.AddSubview (directionButton);

			parent = par;
			Provider = pro;
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			float startY = Constants.IS_OS_7_OR_LATER () ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;;
			float viewWidth = (float)base.View.Bounds.Width;
			float viewHeight = (float)base.View.Bounds.Height-Constants.NAV_HEIGHT;
			float contentWidth = viewWidth - Constants.DRUG_LOOKUP_LEFT_TEXT_INSET * 2;
			float boxHeight = (float)providerLabel.Font.LineHeight * (int)providerLabel.Lines;

			providerLabel.Frame = new CGRect (Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, Constants.DRUG_LOOKUP_TOP_PADDING, contentWidth, boxHeight+2);
			textContainer.Frame = new CGRect (0, startY, viewWidth, (float)providerLabel.Frame.Height+Constants.DRUG_LOOKUP_TOP_PADDING*2);
			directionButton.Frame = new CGRect((float)(viewWidth*0.3)/2, (float)textContainer.Frame.Y + (float)textContainer.Frame.Height+25, (float)(viewWidth*0.7), Constants.LIST_BUTTON_HEIGHT/2+5);
			scrollContainer.Frame = new CGRect (0, 0, viewWidth, viewHeight);
			scrollContainer.ContentSize = new CGSize (viewWidth, (float)directionButton.Frame.Y + (float)directionButton.Frame.Height+10);
		}

		void DoGetDirectionsAction(object sender, EventArgs ea)
		{
			base.NavigationController.PopViewController (true);
			((LocateServiceProviderResultView)parent).DoGetDirections (Provider);
		}

		#region properties
		private ServiceProvider _provider;
		public ServiceProvider Provider {
			get 
			{
				return _provider;
			}
			set {
				_provider = value;
				//providerLabel.Text = "locateProviderNameLabel".tr() + ": " + _provider.DoctorName + "\r\n\r\n";
				//providerLabel.Text += "locateProviderAddressLabel".tr() + ": " + _provider.Address + "\r\n";
				//providerLabel.Text += "locateProviderCityLabel".tr() + ": " + _provider.City + ", " + _provider.Province + ", " + _provider.PostalCode + "\r\n\r\n";

				providerLabel.Text = _provider.DoctorName + "\r\n\r\n";
				providerLabel.Text += _provider.Address + "\r\n";
				providerLabel.Text += _provider.City + ", " + _provider.Province + ", " + _provider.PostalCode + "\r\n\r\n";

				providerLabel.Text += "locateProviderPhoneNumberLabel".tr() + "colon".tr() + _provider.Phone + "\r\n\r\n";

				if (_provider.CanSubmitClaimsOnline)
					providerLabel.Text += "mapProviderDetailsSubmit".tr();
				else
					providerLabel.Text += "mapProviderDetailsNotSubmit".tr();
				providerLabel.Text += "\r\n";

				if (_provider.CanAcceptPaymentDirectly)
					providerLabel.Text += "mapProviderDetailsAccept".tr();
				else
					providerLabel.Text += "mapProviderDetailsNotAccept".tr();
				providerLabel.Text += "\r\n";
			}
		}
		#endregion
	}
}

