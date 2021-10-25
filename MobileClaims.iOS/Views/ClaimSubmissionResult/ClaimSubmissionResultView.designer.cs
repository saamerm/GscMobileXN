// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.ClaimSubmissionResult
{
	[Register ("ClaimSubmissionResultView")]
	partial class ClaimSubmissionResultView
	{
		[Outlet]
		MobileClaims.iOS.GSButton AuditUploadButton { get; set; }

		[Outlet]
		UIKit.UIStackView AuditUploadStackView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint AuditUploadStackViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UICollectionView ClaimDetailsCollectionView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ClaimDetailsCollectionViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel ClaimDetailsTitleLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton CopButton { get; set; }

		[Outlet]
		UIKit.UIStackView CopUploadStackView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint CopUploadStackViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint CopUploadStackViewTopConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.AlignableLabel GscIdLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.AlignableLabel GscIdValue { get; set; }

		[Outlet]
		MobileClaims.iOS.AlignableLabel ParticipantNameLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.AlignableLabel ParticipantNameValueLabel { get; set; }

		[Outlet]
		UIKit.UILabel PlanInformationLabel { get; set; }

		[Outlet]
		UIKit.UICollectionView PlanLimitationCollectionView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint PlanLimitationCollectionViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint PlanLimitationCollectionViewTopConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint PlanLimitationLabelTopConstraint { get; set; }

		[Outlet]
		UIKit.UILabel PlanLimitationTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel RequiresAuditLabel { get; set; }

		[Outlet]
		UIKit.UILabel RequiresCOPLabel { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ScrollViewBottomConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.AlignableLabel SubmissionDateLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.AlignableLabel SubmissionDateValueLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton SubmitAnotherClaimButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint SubmitAnotherClaimButtonTopConstraint { get; set; }

		[Outlet]
		UIKit.UICollectionView TotalsCollectionView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint TotalsCollectionViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel TotalTitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AuditUploadButton != null) {
				AuditUploadButton.Dispose ();
				AuditUploadButton = null;
			}

			if (ClaimDetailsCollectionView != null) {
				ClaimDetailsCollectionView.Dispose ();
				ClaimDetailsCollectionView = null;
			}

			if (ClaimDetailsCollectionViewHeightConstraint != null) {
				ClaimDetailsCollectionViewHeightConstraint.Dispose ();
				ClaimDetailsCollectionViewHeightConstraint = null;
			}

			if (ClaimDetailsTitleLabel != null) {
				ClaimDetailsTitleLabel.Dispose ();
				ClaimDetailsTitleLabel = null;
			}

			if (CopButton != null) {
				CopButton.Dispose ();
				CopButton = null;
			}

			if (CopUploadStackViewTopConstraint != null) {
				CopUploadStackViewTopConstraint.Dispose ();
				CopUploadStackViewTopConstraint = null;
			}

			if (GscIdLabel != null) {
				GscIdLabel.Dispose ();
				GscIdLabel = null;
			}

			if (GscIdValue != null) {
				GscIdValue.Dispose ();
				GscIdValue = null;
			}

			if (ParticipantNameLabel != null) {
				ParticipantNameLabel.Dispose ();
				ParticipantNameLabel = null;
			}

			if (ParticipantNameValueLabel != null) {
				ParticipantNameValueLabel.Dispose ();
				ParticipantNameValueLabel = null;
			}

			if (PlanInformationLabel != null) {
				PlanInformationLabel.Dispose ();
				PlanInformationLabel = null;
			}

			if (PlanLimitationCollectionView != null) {
				PlanLimitationCollectionView.Dispose ();
				PlanLimitationCollectionView = null;
			}

			if (PlanLimitationCollectionViewHeightConstraint != null) {
				PlanLimitationCollectionViewHeightConstraint.Dispose ();
				PlanLimitationCollectionViewHeightConstraint = null;
			}

			if (PlanLimitationCollectionViewTopConstraint != null) {
				PlanLimitationCollectionViewTopConstraint.Dispose ();
				PlanLimitationCollectionViewTopConstraint = null;
			}

			if (PlanLimitationLabelTopConstraint != null) {
				PlanLimitationLabelTopConstraint.Dispose ();
				PlanLimitationLabelTopConstraint = null;
			}

			if (PlanLimitationTitleLabel != null) {
				PlanLimitationTitleLabel.Dispose ();
				PlanLimitationTitleLabel = null;
			}

			if (RequiresAuditLabel != null) {
				RequiresAuditLabel.Dispose ();
				RequiresAuditLabel = null;
			}

			if (RequiresCOPLabel != null) {
				RequiresCOPLabel.Dispose ();
				RequiresCOPLabel = null;
			}

			if (SubmissionDateLabel != null) {
				SubmissionDateLabel.Dispose ();
				SubmissionDateLabel = null;
			}

			if (SubmissionDateValueLabel != null) {
				SubmissionDateValueLabel.Dispose ();
				SubmissionDateValueLabel = null;
			}

			if (SubmitAnotherClaimButton != null) {
				SubmitAnotherClaimButton.Dispose ();
				SubmitAnotherClaimButton = null;
			}

			if (SubmitAnotherClaimButtonTopConstraint != null) {
				SubmitAnotherClaimButtonTopConstraint.Dispose ();
				SubmitAnotherClaimButtonTopConstraint = null;
			}

			if (TotalsCollectionView != null) {
				TotalsCollectionView.Dispose ();
				TotalsCollectionView = null;
			}

			if (TotalsCollectionViewHeightConstraint != null) {
				TotalsCollectionViewHeightConstraint.Dispose ();
				TotalsCollectionViewHeightConstraint = null;
			}

			if (TotalTitleLabel != null) {
				TotalTitleLabel.Dispose ();
				TotalTitleLabel = null;
			}

			if (CopUploadStackViewHeightConstraint != null) {
				CopUploadStackViewHeightConstraint.Dispose ();
				CopUploadStackViewHeightConstraint = null;
			}

			if (CopUploadStackView != null) {
				CopUploadStackView.Dispose ();
				CopUploadStackView = null;
			}

			if (AuditUploadStackView != null) {
				AuditUploadStackView.Dispose ();
				AuditUploadStackView = null;
			}

			if (AuditUploadStackViewHeightConstraint != null) {
				AuditUploadStackViewHeightConstraint.Dispose ();
				AuditUploadStackViewHeightConstraint = null;
			}

			if (ScrollViewBottomConstraint != null) {
				ScrollViewBottomConstraint.Dispose ();
				ScrollViewBottomConstraint = null;
			}
		}
	}
}
