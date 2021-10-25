// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.TermsAndConditions
{
	[Register ("TermsAndConditionsView")]
	partial class TermsAndConditionsView
	{
		[Outlet]
		UIKit.UIButton CheckBoxButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint CheckBoxButtonYCenterConstraint { get; set; }

		[Outlet]
		UIKit.UIView CheckBoxContainer { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint CheckBoxContainerHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel CheckBoxLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint CheckBoxLabelYCenterConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.GreyNavButton GeneralButton { get; set; }

		[Outlet]
		MobileClaims.iOS.GreyNavButton LegalButton { get; set; }

		[Outlet]
		MobileClaims.iOS.GreyNavButton PrivacyButton { get; set; }

		[Outlet]
		MobileClaims.iOS.GreyNavButton SecurityButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint TCButtonContainerTopConstraint { get; set; }

		[Outlet]
		WebKit.WKWebView TermsAndConditionsWebView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint WebViewBottomConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CheckBoxButton != null) {
				CheckBoxButton.Dispose ();
				CheckBoxButton = null;
			}

			if (CheckBoxButtonYCenterConstraint != null) {
				CheckBoxButtonYCenterConstraint.Dispose ();
				CheckBoxButtonYCenterConstraint = null;
			}

			if (CheckBoxContainer != null) {
				CheckBoxContainer.Dispose ();
				CheckBoxContainer = null;
			}

			if (CheckBoxContainerHeightConstraint != null) {
				CheckBoxContainerHeightConstraint.Dispose ();
				CheckBoxContainerHeightConstraint = null;
			}

			if (CheckBoxLabel != null) {
				CheckBoxLabel.Dispose ();
				CheckBoxLabel = null;
			}

			if (CheckBoxLabelYCenterConstraint != null) {
				CheckBoxLabelYCenterConstraint.Dispose ();
				CheckBoxLabelYCenterConstraint = null;
			}

			if (GeneralButton != null) {
				GeneralButton.Dispose ();
				GeneralButton = null;
			}

			if (LegalButton != null) {
				LegalButton.Dispose ();
				LegalButton = null;
			}

			if (PrivacyButton != null) {
				PrivacyButton.Dispose ();
				PrivacyButton = null;
			}

			if (SecurityButton != null) {
				SecurityButton.Dispose ();
				SecurityButton = null;
			}

			if (TCButtonContainerTopConstraint != null) {
				TCButtonContainerTopConstraint.Dispose ();
				TCButtonContainerTopConstraint = null;
			}

			if (TermsAndConditionsWebView != null) {
				TermsAndConditionsWebView.Dispose ();
				TermsAndConditionsWebView = null;
			}

			if (WebViewBottomConstraint != null) {
				WebViewBottomConstraint.Dispose ();
				WebViewBottomConstraint = null;
			}
		}
	}
}
