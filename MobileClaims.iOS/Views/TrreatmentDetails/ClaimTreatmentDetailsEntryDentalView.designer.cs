// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.TrreatmentDetails
{
	[Register ("ClaimTreatmentDetailsEntryDentalView")]
	partial class ClaimTreatmentDetailsEntryDentalView
	{
		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledTextField AmountPaidTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton DeleteButton { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledTextField DentistFeeTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledTextField LaboratoryChargeTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledTextField ProcedureCodeTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledTextField ToothCodeTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledTextField ToothSurfaceTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.EnhancedLabeledDatePicker TreatmentDatePicker { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AmountPaidTextField != null) {
				AmountPaidTextField.Dispose ();
				AmountPaidTextField = null;
			}

			if (DentistFeeTextField != null) {
				DentistFeeTextField.Dispose ();
				DentistFeeTextField = null;
			}

			if (LaboratoryChargeTextField != null) {
				LaboratoryChargeTextField.Dispose ();
				LaboratoryChargeTextField = null;
			}

			if (ProcedureCodeTextField != null) {
				ProcedureCodeTextField.Dispose ();
				ProcedureCodeTextField = null;
			}

			if (ToothCodeTextField != null) {
				ToothCodeTextField.Dispose ();
				ToothCodeTextField = null;
			}

			if (ToothSurfaceTextField != null) {
				ToothSurfaceTextField.Dispose ();
				ToothSurfaceTextField = null;
			}

			if (TreatmentDatePicker != null) {
				TreatmentDatePicker.Dispose ();
				TreatmentDatePicker = null;
			}

			if (DeleteButton != null) {
				DeleteButton.Dispose ();
				DeleteButton = null;
			}
		}
	}
}
