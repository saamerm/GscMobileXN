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
    [Register ("EligibilityCheckEyeView")]
    partial class EligibilityCheckEyeView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndDatePicker dateOfPurchaseOrServiceComponent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView eligibilityCheckStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView eligibilityCheckViewContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCSmallFontSizeLabel eligibilityNote1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCSmallFontSizeLabel eligibilityNote2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndList lensTypesList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCPageHeader planParticipantLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollableContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndLabel serviceDescriptionComponent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndList serviceProvinceList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.GSButton submitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndTextField totalCharge { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (dateOfPurchaseOrServiceComponent != null) {
                dateOfPurchaseOrServiceComponent.Dispose ();
                dateOfPurchaseOrServiceComponent = null;
            }

            if (eligibilityCheckStackView != null) {
                eligibilityCheckStackView.Dispose ();
                eligibilityCheckStackView = null;
            }

            if (eligibilityCheckViewContainer != null) {
                eligibilityCheckViewContainer.Dispose ();
                eligibilityCheckViewContainer = null;
            }

            if (eligibilityNote1 != null) {
                eligibilityNote1.Dispose ();
                eligibilityNote1 = null;
            }

            if (eligibilityNote2 != null) {
                eligibilityNote2.Dispose ();
                eligibilityNote2 = null;
            }

            if (lensTypesList != null) {
                lensTypesList.Dispose ();
                lensTypesList = null;
            }

            if (planParticipantLabel != null) {
                planParticipantLabel.Dispose ();
                planParticipantLabel = null;
            }

            if (scrollableContainer != null) {
                scrollableContainer.Dispose ();
                scrollableContainer = null;
            }

            if (serviceDescriptionComponent != null) {
                serviceDescriptionComponent.Dispose ();
                serviceDescriptionComponent = null;
            }

            if (serviceProvinceList != null) {
                serviceProvinceList.Dispose ();
                serviceProvinceList = null;
            }

            if (submitButton != null) {
                submitButton.Dispose ();
                submitButton = null;
            }

            if (totalCharge != null) {
                totalCharge.Dispose ();
                totalCharge = null;
            }
        }
    }
}