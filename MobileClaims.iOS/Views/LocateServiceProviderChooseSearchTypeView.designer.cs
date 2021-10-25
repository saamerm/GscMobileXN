// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [Register ("LocateServiceProviderChooseSearchTypeView")]
    partial class LocateServiceProviderChooseSearchTypeView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.GSButton continueButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel locationScrollLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCTableView locationTypeTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel makeSelectionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel narrowLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel searchScrollLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCTableView tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (continueButton != null) {
                continueButton.Dispose ();
                continueButton = null;
            }

            if (locationScrollLabel != null) {
                locationScrollLabel.Dispose ();
                locationScrollLabel = null;
            }

            if (locationTypeTableView != null) {
                locationTypeTableView.Dispose ();
                locationTypeTableView = null;
            }

            if (makeSelectionLabel != null) {
                makeSelectionLabel.Dispose ();
                makeSelectionLabel = null;
            }

            if (narrowLabel != null) {
                narrowLabel.Dispose ();
                narrowLabel = null;
            }

            if (scrollContainer != null) {
                scrollContainer.Dispose ();
                scrollContainer = null;
            }

            if (searchScrollLabel != null) {
                searchScrollLabel.Dispose ();
                searchScrollLabel = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}