// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views
{
	[Register ("ChooseClaimOrHistoryView")]
	partial class ChooseClaimOrHistoryView
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ActiveClaimsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView ActiveClaimsTableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView ClaimActionStackView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ClaimButtonViewTopConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ClaimButtonViewTopConstraintLandscape { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MobileClaims.iOS.GCButtonFixedFontSize ClaimHistoryButton { get; set; }

		[Outlet]
		UIKit.UILabel NoActiveClaimsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MobileClaims.iOS.GCButtonFixedFontSize SubmitClaimButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NoActiveClaimsLabel != null) {
				NoActiveClaimsLabel.Dispose ();
				NoActiveClaimsLabel = null;
			}

			if (ActiveClaimsLabel != null) {
				ActiveClaimsLabel.Dispose ();
				ActiveClaimsLabel = null;
			}

			if (ActiveClaimsTableView != null) {
				ActiveClaimsTableView.Dispose ();
				ActiveClaimsTableView = null;
			}

			if (ClaimActionStackView != null) {
				ClaimActionStackView.Dispose ();
				ClaimActionStackView = null;
			}

			if (ClaimButtonViewTopConstraint != null) {
				ClaimButtonViewTopConstraint.Dispose ();
				ClaimButtonViewTopConstraint = null;
			}

			if (ClaimButtonViewTopConstraintLandscape != null) {
				ClaimButtonViewTopConstraintLandscape.Dispose ();
				ClaimButtonViewTopConstraintLandscape = null;
			}

			if (ClaimHistoryButton != null) {
				ClaimHistoryButton.Dispose ();
				ClaimHistoryButton = null;
			}

			if (SubmitClaimButton != null) {
				SubmitClaimButton.Dispose ();
				SubmitClaimButton = null;
			}
		}
	}
}
