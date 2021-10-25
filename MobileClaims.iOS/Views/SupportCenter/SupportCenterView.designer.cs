// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.SupportCenter
{
	[Register ("SupportCenterView")]
	partial class SupportCenterView
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
		WebKit.WKWebView SupportCenterWebView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SupportCenterWebViewBottomConstraint { get; set; }
		
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

			if (SupportCenterWebView != null) {
				SupportCenterWebView.Dispose ();
				SupportCenterWebView = null;
			}

			if (SupportCenterWebViewBottomConstraint != null) {
				SupportCenterWebViewBottomConstraint.Dispose ();
				SupportCenterWebViewBottomConstraint = null;
			}
		}
	}
}
