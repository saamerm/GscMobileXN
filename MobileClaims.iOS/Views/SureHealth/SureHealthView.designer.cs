// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.SureHealth
{
	[Register ("SureHealthView")]
	partial class SureHealthView
	{
		[Outlet]
		UIKit.UIButton BackButton { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView BusyIndicator { get; set; }

		[Outlet]
		UIKit.UIButton ForwardButton { get; set; }

		[Outlet]
		UIKit.UIView NavigationContainerView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint NavigationContainerViewTopConstraint { get; set; }

		[Outlet]
		WebKit.WKWebView SureHealthWebView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SureHealthWebViewBottomConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}

			if (BusyIndicator != null) {
				BusyIndicator.Dispose ();
				BusyIndicator = null;
			}

			if (ForwardButton != null) {
				ForwardButton.Dispose ();
				ForwardButton = null;
			}

			if (NavigationContainerView != null) {
				NavigationContainerView.Dispose ();
				NavigationContainerView = null;
			}

			if (NavigationContainerViewTopConstraint != null) {
				NavigationContainerViewTopConstraint.Dispose ();
				NavigationContainerViewTopConstraint = null;
			}

			if (SureHealthWebView != null) {
				SureHealthWebView.Dispose ();
				SureHealthWebView = null;
			}

			if (SureHealthWebViewBottomConstraint != null) {
				SureHealthWebViewBottomConstraint.Dispose ();
				SureHealthWebViewBottomConstraint = null;
			}
		}
	}
}
