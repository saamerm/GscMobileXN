// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.AuditClaims
{
	[Register ("AuditClaimCellView")]
	partial class AuditClaimCellView
	{
		[Outlet]
		UIKit.UILabel ActionRequiredLabel { get; set; }

		[Outlet]
		UIKit.UILabel ClaimFormNumberLabel { get; set; }

		[Outlet]
		UIKit.UILabel ClaimTypeLabel { get; set; }

		[Outlet]
		UIKit.UILabel DueDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel DueDateValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel SubmissionDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel SubmissionDateValueLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ActionRequiredLabel != null) {
				ActionRequiredLabel.Dispose ();
				ActionRequiredLabel = null;
			}

			if (ClaimFormNumberLabel != null) {
				ClaimFormNumberLabel.Dispose ();
				ClaimFormNumberLabel = null;
			}

			if (SubmissionDateLabel != null) {
				SubmissionDateLabel.Dispose ();
				SubmissionDateLabel = null;
			}

			if (SubmissionDateValueLabel != null) {
				SubmissionDateValueLabel.Dispose ();
				SubmissionDateValueLabel = null;
			}

			if (DueDateLabel != null) {
				DueDateLabel.Dispose ();
				DueDateLabel = null;
			}

			if (DueDateValueLabel != null) {
				DueDateValueLabel.Dispose ();
				DueDateValueLabel = null;
			}

			if (ClaimTypeLabel != null) {
				ClaimTypeLabel.Dispose ();
				ClaimTypeLabel = null;
			}
		}
	}
}
