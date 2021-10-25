// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.Claims
{
	[Register ("ClaimSubmitTermsAndConditionsView")]
	partial class ClaimSubmitTermsAndConditionsView
	{
		[Outlet]
		MobileClaims.iOS.GSButton AgreeButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ButtonBottomConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ButtonContainerBottomConstraint { get; set; }

		[Outlet]
		UIKit.UIView ButtonContainerView { get; set; }

		[Outlet]
		UIKit.UILabel DisclaimerLabel { get; set; }

		[Outlet]
		UIKit.UILabel DisclaimerParagraph1Label { get; set; }

		[Outlet]
		UIKit.UILabel DisclaimerParagraph2Label { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AgreeButton != null) {
				AgreeButton.Dispose ();
				AgreeButton = null;
			}

			if (ButtonBottomConstraint != null) {
				ButtonBottomConstraint.Dispose ();
				ButtonBottomConstraint = null;
			}

			if (ButtonContainerView != null) {
				ButtonContainerView.Dispose ();
				ButtonContainerView = null;
			}

			if (DisclaimerLabel != null) {
				DisclaimerLabel.Dispose ();
				DisclaimerLabel = null;
			}

			if (DisclaimerParagraph1Label != null) {
				DisclaimerParagraph1Label.Dispose ();
				DisclaimerParagraph1Label = null;
			}

			if (DisclaimerParagraph2Label != null) {
				DisclaimerParagraph2Label.Dispose ();
				DisclaimerParagraph2Label = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (ButtonContainerBottomConstraint != null) {
				ButtonContainerBottomConstraint.Dispose ();
				ButtonContainerBottomConstraint = null;
			}
		}
	}
}
