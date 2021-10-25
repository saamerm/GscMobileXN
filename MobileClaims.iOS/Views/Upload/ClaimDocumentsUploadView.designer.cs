// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.Upload
{
	[Register ("ClaimDocumentsUploadView")]
	partial class ClaimDocumentsUploadView
	{
		[Outlet]
		MobileClaims.iOS.UI.RightAlignIconButton AddDocumentButton { get; set; }

		[Outlet]
		UIKit.UITextView AdditionalInformationTextView { get; set; }

		[Outlet]
		UIKit.UILabel AdditionalInfornationLabel { get; set; }

		[Outlet]
		UIKit.UITableView DocumentsTableView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint DocumentsTableViewHeightConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton NextButton { get; set; }

		[Outlet]
		UIKit.UILabel PhotoSizeLimitLabel { get; set; }

		[Outlet]
		UIKit.UIView SeperatorView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SeperatorViewLeadingConstraint { get; set; }

		[Outlet]
		UIKit.UILabel SubmitDocumentsLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AddDocumentButton != null) {
				AddDocumentButton.Dispose ();
				AddDocumentButton = null;
			}

			if (AdditionalInformationTextView != null) {
				AdditionalInformationTextView.Dispose ();
				AdditionalInformationTextView = null;
			}

			if (AdditionalInfornationLabel != null) {
				AdditionalInfornationLabel.Dispose ();
				AdditionalInfornationLabel = null;
			}

			if (DocumentsTableView != null) {
				DocumentsTableView.Dispose ();
				DocumentsTableView = null;
			}

			if (DocumentsTableViewHeightConstraint != null) {
				DocumentsTableViewHeightConstraint.Dispose ();
				DocumentsTableViewHeightConstraint = null;
			}

			if (PhotoSizeLimitLabel != null) {
				PhotoSizeLimitLabel.Dispose ();
				PhotoSizeLimitLabel = null;
			}

			if (SeperatorView != null) {
				SeperatorView.Dispose ();
				SeperatorView = null;
			}

			if (SeperatorViewLeadingConstraint != null) {
				SeperatorViewLeadingConstraint.Dispose ();
				SeperatorViewLeadingConstraint = null;
			}

			if (SubmitDocumentsLabel != null) {
				SubmitDocumentsLabel.Dispose ();
				SubmitDocumentsLabel = null;
			}

			if (NextButton != null) {
				NextButton.Dispose ();
				NextButton = null;
			}
		}
	}
}
