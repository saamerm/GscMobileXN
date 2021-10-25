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
	[Register ("ConfirmationOfPaymentSubmitView")]
	partial class ConfirmationOfPaymentSubmitView
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
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextView CommentsTextView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DescribeDocumentsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton DisclaimerButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton DisclaimerCheckBoxButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DisclaimerLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView DocumentListTableView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint DocumentsTableViewHeightConstraints { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DocumentsToUploadLabel { get; set; }

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
		MobileClaims.iOS.GSButton UploadDocumentsButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DocumentsTableViewHeightConstraints != null) {
				DocumentsTableViewHeightConstraints.Dispose ();
				DocumentsTableViewHeightConstraints = null;
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

			if (CommentsTextView != null) {
				CommentsTextView.Dispose ();
				CommentsTextView = null;
			}

			if (DescribeDocumentsLabel != null) {
				DescribeDocumentsLabel.Dispose ();
				DescribeDocumentsLabel = null;
			}

			if (DisclaimerButton != null) {
				DisclaimerButton.Dispose ();
				DisclaimerButton = null;
			}

			if (DisclaimerCheckBoxButton != null) {
				DisclaimerCheckBoxButton.Dispose ();
				DisclaimerCheckBoxButton = null;
			}

			if (DisclaimerLabel != null) {
				DisclaimerLabel.Dispose ();
				DisclaimerLabel = null;
			}

			if (DocumentListTableView != null) {
				DocumentListTableView.Dispose ();
				DocumentListTableView = null;
			}

			if (DocumentsToUploadLabel != null) {
				DocumentsToUploadLabel.Dispose ();
				DocumentsToUploadLabel = null;
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

			if (UploadDocumentsButton != null) {
				UploadDocumentsButton.Dispose ();
				UploadDocumentsButton = null;
			}
		}
	}
}
