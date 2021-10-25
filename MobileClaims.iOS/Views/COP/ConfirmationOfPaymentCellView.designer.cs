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
    [Register ("ConfirmationOfPaymentCellView")]
    partial class ConfirmationOfPaymentCellView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ActionRequiredLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BenefitTypeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClaimedAmountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClaimFormIdLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CurrencyLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DateLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActionRequiredLabel != null) {
                ActionRequiredLabel.Dispose ();
                ActionRequiredLabel = null;
            }

            if (BenefitTypeLabel != null) {
                BenefitTypeLabel.Dispose ();
                BenefitTypeLabel = null;
            }

            if (ClaimedAmountLabel != null) {
                ClaimedAmountLabel.Dispose ();
                ClaimedAmountLabel = null;
            }

            if (ClaimFormIdLabel != null) {
                ClaimFormIdLabel.Dispose ();
                ClaimFormIdLabel = null;
            }

            if (CurrencyLabel != null) {
                CurrencyLabel.Dispose ();
                CurrencyLabel = null;
            }

            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }
        }
    }
}