// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.DirectDeposit
{
	[Register ("DirectDepositView")]
	partial class DirectDepositView
	{
		[Outlet]
		UIKit.UITableView DirectDepositTableView { get; set; }

		[Outlet]
		UIKit.UILabel GetPaidFasterLabel { get; set; }

		[Outlet]
		UIKit.UINavigationBar NavBar { get; set; }

		[Outlet]
		UIKit.UINavigationItem NavBarTitle { get; set; }

		[Outlet]
		UIKit.UILabel SignUpForDDLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint UITableViewBottomConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DirectDepositTableView != null) {
				DirectDepositTableView.Dispose ();
				DirectDepositTableView = null;
			}

			if (GetPaidFasterLabel != null) {
				GetPaidFasterLabel.Dispose ();
				GetPaidFasterLabel = null;
			}

			if (NavBar != null) {
				NavBar.Dispose ();
				NavBar = null;
			}

			if (NavBarTitle != null) {
				NavBarTitle.Dispose ();
				NavBarTitle = null;
			}

			if (SignUpForDDLabel != null) {
				SignUpForDDLabel.Dispose ();
				SignUpForDDLabel = null;
			}

			if (UITableViewBottomConstraint != null) {
				UITableViewBottomConstraint.Dispose ();
				UITableViewBottomConstraint = null;
			}
		}
	}
}
