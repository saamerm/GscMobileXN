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
    [Register ("EligibilityCheckMassageView")]
    partial class EligibilityCheckMassageView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndDatePicker dateOfTreatmentComponent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCSmallFontSizeLabel eligibilityNote1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCSmallFontSizeLabel eligibilityNote2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView eligibilityStackViewContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndList lengthOfTreatmentList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.UI.GSCPageHeader planParticipantLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndList serviceProvinceList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.GSButton submitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndTextField totalAmountOfVisit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileClaims.iOS.ClaimTreatmentDetailsTitleAndList typeOfTreatmentList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (dateOfTreatmentComponent != null) {
                dateOfTreatmentComponent.Dispose ();
                dateOfTreatmentComponent = null;
            }

            if (eligibilityNote1 != null) {
                eligibilityNote1.Dispose ();
                eligibilityNote1 = null;
            }

            if (eligibilityNote2 != null) {
                eligibilityNote2.Dispose ();
                eligibilityNote2 = null;
            }

            if (eligibilityStackViewContainer != null) {
                eligibilityStackViewContainer.Dispose ();
                eligibilityStackViewContainer = null;
            }

            if (lengthOfTreatmentList != null) {
                lengthOfTreatmentList.Dispose ();
                lengthOfTreatmentList = null;
            }

            if (planParticipantLabel != null) {
                planParticipantLabel.Dispose ();
                planParticipantLabel = null;
            }

            if (scrollContainer != null) {
                scrollContainer.Dispose ();
                scrollContainer = null;
            }

            if (serviceProvinceList != null) {
                serviceProvinceList.Dispose ();
                serviceProvinceList = null;
            }

            if (submitButton != null) {
                submitButton.Dispose ();
                submitButton = null;
            }

            if (totalAmountOfVisit != null) {
                totalAmountOfVisit.Dispose ();
                totalAmountOfVisit = null;
            }

            if (typeOfTreatmentList != null) {
                typeOfTreatmentList.Dispose ();
                typeOfTreatmentList = null;
            }

            if (viewContainer != null) {
                viewContainer.Dispose ();
                viewContainer = null;
            }
        }
    }
}