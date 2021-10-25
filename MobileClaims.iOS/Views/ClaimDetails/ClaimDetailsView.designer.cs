// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.ClaimDetails
{
	[Register ("ClaimDetailsView")]
	partial class ClaimDetailsView
	{
		[Outlet]
		MobileClaims.iOS.UI.LabeledDatePicker AccidentDatePicker { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledTextField CaseNumberTextField { get; set; }

		[Outlet]
		UIKit.UILabel ClaimDetailsHeaderLabel { get; set; }

		[Outlet]
		UIKit.UIView ContentOverlayView { get; set; }

		[Outlet]
		UIKit.UIView ContentView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle GSCCoverageToggle { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle GstHstToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint GstHstToggleHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel HealthProviderAddressLabel { get; set; }

		[Outlet]
		UIKit.UILabel HealthProviderCityLabel { get; set; }

		[Outlet]
		UIKit.UIStackView HealthProviderInfoStackView { get; set; }

		[Outlet]
		UIKit.UILabel HealthProviderNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel HealthProviderPhoneNumberLabel { get; set; }

		[Outlet]
		UIKit.UILabel HealthProviderRegNumberLabel { get; set; }

		[Outlet]
		UIKit.UILabel HealthProviderTitleLabel { get; set; }

		[Outlet]
		UIKit.UIStackView IsMedicalPrescriptionAvailableStackView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle IsMedicalPrescriptionAvailableToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint IsMedicalPrescriptionAvailableToggleHeightConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledTableView MedicalProfessionTypeTableView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledDatePicker MedPrescriptionReferralDatePicker { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MedProfessionTypeTableViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIStackView MotorVehicleAccidentStackView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle MotorVehicleAccidentToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MotorVehicleAccidentToggleHeightConstraint { get; set; }

		[Outlet]
		UIKit.UILabel NoticeLabel { get; set; }

		[Outlet]
		UIKit.UILabel OtherBenefitsHCSALabel { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle OtherBenefitsHCSAToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint OtherBenefitsHCSAToggleHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIStackView OtherBenefitsStackView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle OtherBenefitsToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint OtherBenefitsToggleHeightConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledTextField OtherGSCNumberTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle OtherTypeOfAccidentToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint OtherTypeOfAccidentToggleHightConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle PayUnpaidAmountToggle { get; set; }

		[Outlet]
		UIKit.UIStackView ReasonOfAccidentStackView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle ReasonOfAccidentToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ReasonOfAccidentToggleHeightConstraint { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ScrollViewBottomConstraint { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton SubmitButton { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle SubmittedWithOtherBenefitsToggle { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledDatePicker WorkInjuryDatePicker { get; set; }

		[Outlet]
		UIKit.UIStackView WorkRelatedInjuryStackView { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.LabeledToggle WorkRelatedInjuryToggle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint WorkRelatedInjuryToggleHeightConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AccidentDatePicker != null) {
				AccidentDatePicker.Dispose ();
				AccidentDatePicker = null;
			}

			if (CaseNumberTextField != null) {
				CaseNumberTextField.Dispose ();
				CaseNumberTextField = null;
			}

			if (ClaimDetailsHeaderLabel != null) {
				ClaimDetailsHeaderLabel.Dispose ();
				ClaimDetailsHeaderLabel = null;
			}

			if (ContentOverlayView != null) {
				ContentOverlayView.Dispose ();
				ContentOverlayView = null;
			}

			if (ContentView != null) {
				ContentView.Dispose ();
				ContentView = null;
			}

			if (GSCCoverageToggle != null) {
				GSCCoverageToggle.Dispose ();
				GSCCoverageToggle = null;
			}

			if (GstHstToggle != null) {
				GstHstToggle.Dispose ();
				GstHstToggle = null;
			}

			if (GstHstToggleHeightConstraint != null) {
				GstHstToggleHeightConstraint.Dispose ();
				GstHstToggleHeightConstraint = null;
			}

			if (HealthProviderAddressLabel != null) {
				HealthProviderAddressLabel.Dispose ();
				HealthProviderAddressLabel = null;
			}

			if (HealthProviderCityLabel != null) {
				HealthProviderCityLabel.Dispose ();
				HealthProviderCityLabel = null;
			}

			if (HealthProviderInfoStackView != null) {
				HealthProviderInfoStackView.Dispose ();
				HealthProviderInfoStackView = null;
			}

			if (HealthProviderNameLabel != null) {
				HealthProviderNameLabel.Dispose ();
				HealthProviderNameLabel = null;
			}

			if (HealthProviderPhoneNumberLabel != null) {
				HealthProviderPhoneNumberLabel.Dispose ();
				HealthProviderPhoneNumberLabel = null;
			}

			if (HealthProviderRegNumberLabel != null) {
				HealthProviderRegNumberLabel.Dispose ();
				HealthProviderRegNumberLabel = null;
			}

			if (HealthProviderTitleLabel != null) {
				HealthProviderTitleLabel.Dispose ();
				HealthProviderTitleLabel = null;
			}

			if (IsMedicalPrescriptionAvailableStackView != null) {
				IsMedicalPrescriptionAvailableStackView.Dispose ();
				IsMedicalPrescriptionAvailableStackView = null;
			}

			if (IsMedicalPrescriptionAvailableToggle != null) {
				IsMedicalPrescriptionAvailableToggle.Dispose ();
				IsMedicalPrescriptionAvailableToggle = null;
			}

			if (IsMedicalPrescriptionAvailableToggleHeightConstraint != null) {
				IsMedicalPrescriptionAvailableToggleHeightConstraint.Dispose ();
				IsMedicalPrescriptionAvailableToggleHeightConstraint = null;
			}

			if (MedicalProfessionTypeTableView != null) {
				MedicalProfessionTypeTableView.Dispose ();
				MedicalProfessionTypeTableView = null;
			}

			if (MedPrescriptionReferralDatePicker != null) {
				MedPrescriptionReferralDatePicker.Dispose ();
				MedPrescriptionReferralDatePicker = null;
			}

			if (MedProfessionTypeTableViewHeightConstraint != null) {
				MedProfessionTypeTableViewHeightConstraint.Dispose ();
				MedProfessionTypeTableViewHeightConstraint = null;
			}

			if (MotorVehicleAccidentStackView != null) {
				MotorVehicleAccidentStackView.Dispose ();
				MotorVehicleAccidentStackView = null;
			}

			if (MotorVehicleAccidentToggle != null) {
				MotorVehicleAccidentToggle.Dispose ();
				MotorVehicleAccidentToggle = null;
			}

			if (MotorVehicleAccidentToggleHeightConstraint != null) {
				MotorVehicleAccidentToggleHeightConstraint.Dispose ();
				MotorVehicleAccidentToggleHeightConstraint = null;
			}

			if (NoticeLabel != null) {
				NoticeLabel.Dispose ();
				NoticeLabel = null;
			}

			if (OtherBenefitsHCSALabel != null) {
				OtherBenefitsHCSALabel.Dispose ();
				OtherBenefitsHCSALabel = null;
			}

			if (OtherBenefitsHCSAToggle != null) {
				OtherBenefitsHCSAToggle.Dispose ();
				OtherBenefitsHCSAToggle = null;
			}

			if (OtherBenefitsHCSAToggleHeightConstraint != null) {
				OtherBenefitsHCSAToggleHeightConstraint.Dispose ();
				OtherBenefitsHCSAToggleHeightConstraint = null;
			}

			if (OtherBenefitsStackView != null) {
				OtherBenefitsStackView.Dispose ();
				OtherBenefitsStackView = null;
			}

			if (OtherBenefitsToggle != null) {
				OtherBenefitsToggle.Dispose ();
				OtherBenefitsToggle = null;
			}

			if (OtherBenefitsToggleHeightConstraint != null) {
				OtherBenefitsToggleHeightConstraint.Dispose ();
				OtherBenefitsToggleHeightConstraint = null;
			}

			if (OtherGSCNumberTextField != null) {
				OtherGSCNumberTextField.Dispose ();
				OtherGSCNumberTextField = null;
			}

			if (OtherTypeOfAccidentToggle != null) {
				OtherTypeOfAccidentToggle.Dispose ();
				OtherTypeOfAccidentToggle = null;
			}

			if (OtherTypeOfAccidentToggleHightConstraint != null) {
				OtherTypeOfAccidentToggleHightConstraint.Dispose ();
				OtherTypeOfAccidentToggleHightConstraint = null;
			}

			if (PayUnpaidAmountToggle != null) {
				PayUnpaidAmountToggle.Dispose ();
				PayUnpaidAmountToggle = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (ScrollViewBottomConstraint != null) {
				ScrollViewBottomConstraint.Dispose ();
				ScrollViewBottomConstraint = null;
			}

			if (SubmitButton != null) {
				SubmitButton.Dispose ();
				SubmitButton = null;
			}

			if (SubmittedWithOtherBenefitsToggle != null) {
				SubmittedWithOtherBenefitsToggle.Dispose ();
				SubmittedWithOtherBenefitsToggle = null;
			}

			if (WorkInjuryDatePicker != null) {
				WorkInjuryDatePicker.Dispose ();
				WorkInjuryDatePicker = null;
			}

			if (WorkRelatedInjuryStackView != null) {
				WorkRelatedInjuryStackView.Dispose ();
				WorkRelatedInjuryStackView = null;
			}

			if (WorkRelatedInjuryToggle != null) {
				WorkRelatedInjuryToggle.Dispose ();
				WorkRelatedInjuryToggle = null;
			}

			if (WorkRelatedInjuryToggleHeightConstraint != null) {
				WorkRelatedInjuryToggleHeightConstraint.Dispose ();
				WorkRelatedInjuryToggleHeightConstraint = null;
			}

			if (ReasonOfAccidentToggle != null) {
				ReasonOfAccidentToggle.Dispose ();
				ReasonOfAccidentToggle = null;
			}

			if (ReasonOfAccidentStackView != null) {
				ReasonOfAccidentStackView.Dispose ();
				ReasonOfAccidentStackView = null;
			}

			if (ReasonOfAccidentToggleHeightConstraint != null) {
				ReasonOfAccidentToggleHeightConstraint.Dispose ();
				ReasonOfAccidentToggleHeightConstraint = null;
			}
		}
	}
}
