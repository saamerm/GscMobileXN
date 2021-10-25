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
	[Register ("AuditListView")]
	partial class AuditListView
	{
		[Outlet]
		UIKit.UICollectionView AuditClaimsCollection { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint AuditClaimsCollectionHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel AuditListInstructionLabel { get; set; }

		[Outlet]
		UIKit.UILabel AuditListLabel { get; set; }

		[Outlet]
		UIKit.UILabel AuditListNotes { get; set; }

		[Outlet]
		UIKit.UIView ContentView { get; set; }

		[Outlet]
		UIKit.UILabel NoAuditLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ScrollViewBottomConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AuditClaimsCollection != null) {
				AuditClaimsCollection.Dispose ();
				AuditClaimsCollection = null;
			}

			if (AuditClaimsCollectionHeightConstraint != null) {
				AuditClaimsCollectionHeightConstraint.Dispose ();
				AuditClaimsCollectionHeightConstraint = null;
			}

			if (AuditListInstructionLabel != null) {
				AuditListInstructionLabel.Dispose ();
				AuditListInstructionLabel = null;
			}

			if (AuditListLabel != null) {
				AuditListLabel.Dispose ();
				AuditListLabel = null;
			}

			if (AuditListNotes != null) {
				AuditListNotes.Dispose ();
				AuditListNotes = null;
			}

			if (ScrollViewBottomConstraint != null) {
				ScrollViewBottomConstraint.Dispose ();
				ScrollViewBottomConstraint = null;
			}

			if (NoAuditLabel != null) {
				NoAuditLabel.Dispose ();
				NoAuditLabel = null;
			}

			if (ContentView != null) {
				ContentView.Dispose ();
				ContentView = null;
			}
		}
	}
}
