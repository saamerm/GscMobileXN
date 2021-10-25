// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.Menu
{
	[Register ("LandingPageView")]
	partial class LandingPageView
	{
		[Outlet]
		UIKit.UIView DynamicMenuContainerView { get; set; }

		[Outlet]
		UIKit.UIScrollView DynamicMenuScroll { get; set; }

		[Outlet]
		UIKit.UITableView MenuTableView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MenuTableViewBottomConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MenuTableViewHeight { get; set; }

		[Outlet]
		UIKit.UILabel WelcomeLabelView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DynamicMenuContainerView != null) {
				DynamicMenuContainerView.Dispose ();
				DynamicMenuContainerView = null;
			}

			if (DynamicMenuScroll != null) {
				DynamicMenuScroll.Dispose ();
				DynamicMenuScroll = null;
			}

			if (MenuTableView != null) {
				MenuTableView.Dispose ();
				MenuTableView = null;
			}

			if (MenuTableViewHeight != null) {
				MenuTableViewHeight.Dispose ();
				MenuTableViewHeight = null;
			}

			if (WelcomeLabelView != null) {
				WelcomeLabelView.Dispose ();
				WelcomeLabelView = null;
			}

			if (MenuTableViewBottomConstraint != null) {
				MenuTableViewBottomConstraint.Dispose ();
				MenuTableViewBottomConstraint = null;
			}
		}
	}
}
