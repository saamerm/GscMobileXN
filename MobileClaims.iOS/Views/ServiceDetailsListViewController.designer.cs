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
    [Register ("ServiceDetailsListViewController")]
    partial class ServiceDetailsListViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint bottomTableViewMarginConstraint { get; set; }


        [Outlet]
        UIKit.UIButton HideDetailsList { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (bottomTableViewMarginConstraint != null) {
                bottomTableViewMarginConstraint.Dispose ();
                bottomTableViewMarginConstraint = null;
            }

            if (HideDetailsList != null) {
                HideDetailsList.Dispose ();
                HideDetailsList = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}