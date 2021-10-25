// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.DirectDeposit.DirectDepositResult
{
	[Register ("DDResultsViewController")]
	partial class DDResultsViewController
	{
		[Outlet]
		UIKit.UILabel DDResultsDisclaimer1Label { get; set; }

		[Outlet]
		UIKit.UILabel DDResultsDisclaimer2Label { get; set; }

		[Outlet]
		UIKit.UILabel DDResultsDisclaimer3Label { get; set; }

		[Outlet]
		UIKit.UILabel DDResultsDisclaimerLabel { get; set; }

		[Outlet]
		UIKit.UILabel DDResultsSubTitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DDResultsSubTitleLabel != null) {
				DDResultsSubTitleLabel.Dispose ();
				DDResultsSubTitleLabel = null;
			}

			if (DDResultsDisclaimerLabel != null) {
				DDResultsDisclaimerLabel.Dispose ();
				DDResultsDisclaimerLabel = null;
			}

			if (DDResultsDisclaimer1Label != null) {
				DDResultsDisclaimer1Label.Dispose ();
				DDResultsDisclaimer1Label = null;
			}

			if (DDResultsDisclaimer2Label != null) {
				DDResultsDisclaimer2Label.Dispose ();
				DDResultsDisclaimer2Label = null;
			}

			if (DDResultsDisclaimer3Label != null) {
				DDResultsDisclaimer3Label.Dispose ();
				DDResultsDisclaimer3Label = null;
			}
		}
	}
}
