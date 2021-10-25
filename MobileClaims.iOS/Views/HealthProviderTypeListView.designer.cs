// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views
{
    [Register ("HealthProviderTypeListView")]
    partial class HealthProviderTypeListView
    {
        [Outlet]
        UIKit.UIButton backButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint bottomTableViewSafeAreaMarginConstraint { get; set; }


        [Outlet]
        UIKit.UILabel headerLabel { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (backButton != null) {
                backButton.Dispose ();
                backButton = null;
            }

            if (bottomTableViewSafeAreaMarginConstraint != null) {
                bottomTableViewSafeAreaMarginConstraint.Dispose ();
                bottomTableViewSafeAreaMarginConstraint = null;
            }

            if (headerLabel != null) {
                headerLabel.Dispose ();
                headerLabel = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}