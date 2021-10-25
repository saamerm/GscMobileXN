// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.COP
{
	[Register ("ConfirmationOfPaymentCompletedView")]
	partial class ConfirmationOfPaymentCompletedView
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MobileClaims.iOS.GSButton BackToClaimsButton { get; set; }

		[Outlet]
		UIKit.UILabel SuccessfulAdditionalMessageLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel SuccessfulLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BackToClaimsButton != null) {
				BackToClaimsButton.Dispose ();
				BackToClaimsButton = null;
			}

			if (SuccessfulLabel != null) {
				SuccessfulLabel.Dispose ();
				SuccessfulLabel = null;
			}

			if (SuccessfulAdditionalMessageLabel != null) {
				SuccessfulAdditionalMessageLabel.Dispose ();
				SuccessfulAdditionalMessageLabel = null;
			}
		}
	}
}
