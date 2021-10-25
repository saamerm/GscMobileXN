// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.ClaimParticipants
{
	[Register ("ClaimParticipantsView")]
	partial class ClaimParticipantsView
	{
		[Outlet]
		UIKit.UILabel CommonParticipantHeaderLabel { get; set; }

		[Outlet]
		UIKit.UICollectionView OtherParticipantsCollectionView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint OtherParticipantsCollectionViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel OtherParticipantsHeaderLabel { get; set; }

		[Outlet]
		UIKit.UILabel OtherParticipantsNotes { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ParticipantCollectionViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UICollectionView ParticipantsCollectionView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ScrollViewBottomConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CommonParticipantHeaderLabel != null) {
				CommonParticipantHeaderLabel.Dispose ();
				CommonParticipantHeaderLabel = null;
			}

			if (OtherParticipantsCollectionView != null) {
				OtherParticipantsCollectionView.Dispose ();
				OtherParticipantsCollectionView = null;
			}

			if (OtherParticipantsCollectionViewHeightConstraint != null) {
				OtherParticipantsCollectionViewHeightConstraint.Dispose ();
				OtherParticipantsCollectionViewHeightConstraint = null;
			}

			if (OtherParticipantsHeaderLabel != null) {
				OtherParticipantsHeaderLabel.Dispose ();
				OtherParticipantsHeaderLabel = null;
			}

			if (OtherParticipantsNotes != null) {
				OtherParticipantsNotes.Dispose ();
				OtherParticipantsNotes = null;
			}

			if (ParticipantCollectionViewHeightConstraint != null) {
				ParticipantCollectionViewHeightConstraint.Dispose ();
				ParticipantCollectionViewHeightConstraint = null;
			}

			if (ParticipantsCollectionView != null) {
				ParticipantsCollectionView.Dispose ();
				ParticipantsCollectionView = null;
			}

			if (ScrollViewBottomConstraint != null) {
				ScrollViewBottomConstraint.Dispose ();
				ScrollViewBottomConstraint = null;
			}
		}
	}
}
