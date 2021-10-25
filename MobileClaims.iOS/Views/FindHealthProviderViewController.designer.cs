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
    [Register ("FindHealthProviderViewController")]
    partial class FindHealthProviderViewController
    {
        [Outlet]
        UIKit.UILabel alertNoProvidersFoundLabel { get; set; }


        [Outlet]
        UIKit.UILabel ClickHereForDetailsLabel { get; set; }


        [Outlet]
        UIKit.UIButton CloseButton { get; set; }


        [Outlet]
        UIKit.UILabel EverythingYouNeedToKnowLabel { get; set; }


        [Outlet]
        UIKit.UIButton hamburgerButton { get; set; }


        [Outlet]
        UIKit.UIView LearnMoreDialog { get; set; }


        [Outlet]
        UIKit.UILabel LearnMoreHeaderLabel { get; set; }


        [Outlet]
        UIKit.UIView MapHolderView { get; set; }


        [Outlet]
        UIKit.UIProgressView ProgressView { get; set; }


        [Outlet]
        UIKit.UIButton searchButton { get; set; }


        [Outlet]
        UIKit.UITextField searchTextField { get; set; }


        [Outlet]
        UIKit.UIButton ShowDetailsList { get; set; }


        [Outlet]
        UIKit.UIButton ShowLearnMoreButton { get; set; }


        [Outlet]
        UIKit.UIStackView UnderstandingPharmacyContainer { get; set; }


        [Outlet]
        UIKit.UILabel UnderstandingPharmacyHeaderLabel { get; set; }


        [Outlet]
        UIKit.UIStackView UsingTheHealthProviderContainer { get; set; }


        [Outlet]
        UIKit.UILabel UsingTheHealthProviderHeaderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView alertNoProvidersFoundView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (alertNoProvidersFoundLabel != null) {
                alertNoProvidersFoundLabel.Dispose ();
                alertNoProvidersFoundLabel = null;
            }

            if (alertNoProvidersFoundView != null) {
                alertNoProvidersFoundView.Dispose ();
                alertNoProvidersFoundView = null;
            }

            if (ClickHereForDetailsLabel != null) {
                ClickHereForDetailsLabel.Dispose ();
                ClickHereForDetailsLabel = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (EverythingYouNeedToKnowLabel != null) {
                EverythingYouNeedToKnowLabel.Dispose ();
                EverythingYouNeedToKnowLabel = null;
            }

            if (hamburgerButton != null) {
                hamburgerButton.Dispose ();
                hamburgerButton = null;
            }

            if (LearnMoreDialog != null) {
                LearnMoreDialog.Dispose ();
                LearnMoreDialog = null;
            }

            if (LearnMoreHeaderLabel != null) {
                LearnMoreHeaderLabel.Dispose ();
                LearnMoreHeaderLabel = null;
            }

            if (MapHolderView != null) {
                MapHolderView.Dispose ();
                MapHolderView = null;
            }

            if (ProgressView != null) {
                ProgressView.Dispose ();
                ProgressView = null;
            }

            if (searchButton != null) {
                searchButton.Dispose ();
                searchButton = null;
            }

            if (searchTextField != null) {
                searchTextField.Dispose ();
                searchTextField = null;
            }

            if (ShowDetailsList != null) {
                ShowDetailsList.Dispose ();
                ShowDetailsList = null;
            }

            if (ShowLearnMoreButton != null) {
                ShowLearnMoreButton.Dispose ();
                ShowLearnMoreButton = null;
            }

            if (UnderstandingPharmacyContainer != null) {
                UnderstandingPharmacyContainer.Dispose ();
                UnderstandingPharmacyContainer = null;
            }

            if (UnderstandingPharmacyHeaderLabel != null) {
                UnderstandingPharmacyHeaderLabel.Dispose ();
                UnderstandingPharmacyHeaderLabel = null;
            }

            if (UsingTheHealthProviderContainer != null) {
                UsingTheHealthProviderContainer.Dispose ();
                UsingTheHealthProviderContainer = null;
            }

            if (UsingTheHealthProviderHeaderLabel != null) {
                UsingTheHealthProviderHeaderLabel.Dispose ();
                UsingTheHealthProviderHeaderLabel = null;
            }
        }
    }
}