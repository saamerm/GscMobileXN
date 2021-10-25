// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.WebClaimSubmissionAgreement
{
	[Register ("WebAgreementView")]
	partial class WebAgreementView
	{
		[Outlet]
		MobileClaims.iOS.GSButton AcceptButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ButtonBottomConstraint { get; set; }

		[Outlet]
		UIKit.UIView ButtonContainer { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton CancelButton { get; set; }

		[Outlet]
		WebKit.WKWebView WCSAWebView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint WebViewBottomConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AcceptButton != null) {
				AcceptButton.Dispose ();
				AcceptButton = null;
			}

			if (ButtonBottomConstraint != null) {
				ButtonBottomConstraint.Dispose ();
				ButtonBottomConstraint = null;
			}

			if (ButtonContainer != null) {
				ButtonContainer.Dispose ();
				ButtonContainer = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (WCSAWebView != null) {
				WCSAWebView.Dispose ();
				WCSAWebView = null;
			}

			if (WebViewBottomConstraint != null) {
				WebViewBottomConstraint.Dispose ();
				WebViewBottomConstraint = null;
			}
		}
	}
}
