// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.COP
{
    [Register ("ConfirmationOfPaymentUploadView")]
    partial class ConfirmationOfPaymentUploadView
    {
        [Outlet]
        UIKit.NSLayoutConstraint AttachmentsListHeight { get; set; }


        [Outlet]
        UIKit.UIScrollView ScrollView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint SeperatorViewLeadingConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ActiveClaimDetailContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.RightAlignIconButton AttachDocumentButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClaimedAmountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClaimedAmountValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClaimFormNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClaimFormNumberValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView COPScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView DocumentListTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DocumentsToUploadLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FootNoteLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ParticipantNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SeperatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ServiceDateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ServiceDateValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ServiceDescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ServiceDescriptionValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.GSButton UploadDocumentsButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActiveClaimDetailContainer != null) {
                ActiveClaimDetailContainer.Dispose ();
                ActiveClaimDetailContainer = null;
            }

            if (AttachDocumentButton != null) {
                AttachDocumentButton.Dispose ();
                AttachDocumentButton = null;
            }

            if (AttachmentsListHeight != null) {
                AttachmentsListHeight.Dispose ();
                AttachmentsListHeight = null;
            }

            if (ClaimedAmountLabel != null) {
                ClaimedAmountLabel.Dispose ();
                ClaimedAmountLabel = null;
            }

            if (ClaimedAmountValueLabel != null) {
                ClaimedAmountValueLabel.Dispose ();
                ClaimedAmountValueLabel = null;
            }

            if (ClaimFormNumberLabel != null) {
                ClaimFormNumberLabel.Dispose ();
                ClaimFormNumberLabel = null;
            }

            if (ClaimFormNumberValueLabel != null) {
                ClaimFormNumberValueLabel.Dispose ();
                ClaimFormNumberValueLabel = null;
            }

            if (COPScrollView != null) {
                COPScrollView.Dispose ();
                COPScrollView = null;
            }

            if (DocumentListTableView != null) {
                DocumentListTableView.Dispose ();
                DocumentListTableView = null;
            }

            if (DocumentsToUploadLabel != null) {
                DocumentsToUploadLabel.Dispose ();
                DocumentsToUploadLabel = null;
            }

            if (FootNoteLabel != null) {
                FootNoteLabel.Dispose ();
                FootNoteLabel = null;
            }

            if (ParticipantNameLabel != null) {
                ParticipantNameLabel.Dispose ();
                ParticipantNameLabel = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (SeperatorView != null) {
                SeperatorView.Dispose ();
                SeperatorView = null;
            }

            if (SeperatorViewLeadingConstraint != null) {
                SeperatorViewLeadingConstraint.Dispose ();
                SeperatorViewLeadingConstraint = null;
            }

            if (ServiceDateLabel != null) {
                ServiceDateLabel.Dispose ();
                ServiceDateLabel = null;
            }

            if (ServiceDateValueLabel != null) {
                ServiceDateValueLabel.Dispose ();
                ServiceDateValueLabel = null;
            }

            if (ServiceDescriptionLabel != null) {
                ServiceDescriptionLabel.Dispose ();
                ServiceDescriptionLabel = null;
            }

            if (ServiceDescriptionValueLabel != null) {
                ServiceDescriptionValueLabel.Dispose ();
                ServiceDescriptionValueLabel = null;
            }

            if (UploadDocumentsButton != null) {
                UploadDocumentsButton.Dispose ();
                UploadDocumentsButton = null;
            }
        }
    }
}