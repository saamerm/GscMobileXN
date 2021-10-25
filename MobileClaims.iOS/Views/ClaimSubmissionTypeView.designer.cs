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
    [Register ("ClaimSubmissionTypeView")]
    partial class ClaimSubmissionTypeView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView claimSubmissionTypeSubview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCUINavigationBar claimSubmissionTypeViewNavigationBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView claimSubmissionTypeViewScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCWarningLabel noClaimsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCTableView submissionTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCPageHeader submissionTypeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (claimSubmissionTypeSubview != null) {
                claimSubmissionTypeSubview.Dispose ();
                claimSubmissionTypeSubview = null;
            }

            if (claimSubmissionTypeViewNavigationBar != null) {
                claimSubmissionTypeViewNavigationBar.Dispose ();
                claimSubmissionTypeViewNavigationBar = null;
            }

            if (claimSubmissionTypeViewScrollView != null) {
                claimSubmissionTypeViewScrollView.Dispose ();
                claimSubmissionTypeViewScrollView = null;
            }

            if (noClaimsLabel != null) {
                noClaimsLabel.Dispose ();
                noClaimsLabel = null;
            }

            if (submissionTableView != null) {
                submissionTableView.Dispose ();
                submissionTableView = null;
            }

            if (submissionTypeLabel != null) {
                submissionTypeLabel.Dispose ();
                submissionTypeLabel = null;
            }
        }
    }
}