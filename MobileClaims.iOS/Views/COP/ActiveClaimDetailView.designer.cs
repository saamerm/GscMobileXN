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
	[Register ("ActiveClaimDetailView")]
	partial class ActiveClaimDetailView
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView ActiveClaimDetailContainer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ClaimedAmountLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ClaimedAmountValueLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ClaimFormNumberLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ClaimFormNumberValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel EOBMessagesLabel { get; set; }

		[Outlet]
		UIKit.UILabel EOBMessagesValueLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel FootNoteLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ParticipantNameLabel { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ServiceDateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ServiceDateValueLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ServiceDescriptionLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ServiceDescriptionValueLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel UploadDocumentMessageLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MobileClaims.iOS.GSButton UploadDocumentsButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (EOBMessagesLabel != null) {
				EOBMessagesLabel.Dispose ();
				EOBMessagesLabel = null;
			}

			if (EOBMessagesValueLabel != null) {
				EOBMessagesValueLabel.Dispose ();
				EOBMessagesValueLabel = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (ActiveClaimDetailContainer != null) {
				ActiveClaimDetailContainer.Dispose ();
				ActiveClaimDetailContainer = null;
			}

			if (ClaimedAmountLabel != null) {
				ClaimedAmountLabel.Dispose ();
				ClaimedAmountLabel = null;
			}

			if (ClaimedAmountValueLabel != null) {
				ClaimedAmountValueLabel.Dispose ();
				ClaimedAmountValueLabel = null;
			}

			if (ClaimFormNumberLabel != null) {
				ClaimFormNumberLabel.Dispose ();
				ClaimFormNumberLabel = null;
			}

			if (ClaimFormNumberValueLabel != null) {
				ClaimFormNumberValueLabel.Dispose ();
				ClaimFormNumberValueLabel = null;
			}

			if (FootNoteLabel != null) {
				FootNoteLabel.Dispose ();
				FootNoteLabel = null;
			}

			if (ParticipantNameLabel != null) {
				ParticipantNameLabel.Dispose ();
				ParticipantNameLabel = null;
			}

			if (ServiceDateLabel != null) {
				ServiceDateLabel.Dispose ();
				ServiceDateLabel = null;
			}

			if (ServiceDateValueLabel != null) {
				ServiceDateValueLabel.Dispose ();
				ServiceDateValueLabel = null;
			}

			if (ServiceDescriptionLabel != null) {
				ServiceDescriptionLabel.Dispose ();
				ServiceDescriptionLabel = null;
			}

			if (ServiceDescriptionValueLabel != null) {
				ServiceDescriptionValueLabel.Dispose ();
				ServiceDescriptionValueLabel = null;
			}

			if (UploadDocumentMessageLabel != null) {
				UploadDocumentMessageLabel.Dispose ();
				UploadDocumentMessageLabel = null;
			}

			if (UploadDocumentsButton != null) {
				UploadDocumentsButton.Dispose ();
				UploadDocumentsButton = null;
			}
		}
	}
}
