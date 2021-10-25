// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.Dashboard
{
	[Register ("RecentClaimCellView")]
	partial class RecentClaimCellView
	{
		[Outlet]
		UIKit.UILabel ActionRequiredLabel { get; set; }

		[Outlet]
		UIKit.UILabel ClaimAmountLabel { get; set; }

		[Outlet]
		UIKit.UILabel ClaimSubmissionDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel ClaimTypeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ActionRequiredLabel != null) {
				ActionRequiredLabel.Dispose ();
				ActionRequiredLabel = null;
			}

			if (ClaimTypeLabel != null) {
				ClaimTypeLabel.Dispose ();
				ClaimTypeLabel = null;
			}

			if (ClaimSubmissionDateLabel != null) {
				ClaimSubmissionDateLabel.Dispose ();
				ClaimSubmissionDateLabel = null;
			}

			if (ClaimAmountLabel != null) {
				ClaimAmountLabel.Dispose ();
				ClaimAmountLabel = null;
			}
		}
	}
}
