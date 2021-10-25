// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.DirectDeposit
{
	[Register ("Step2TableViewCell")]
	partial class Step2TableViewCell
	{
		[Outlet]
		UIKit.UILabel AccountNumberErrorLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.VerticalLabeledTextField AccountNumberTextField { get; set; }

		[Outlet]
		UIKit.UILabel BankNumberErrorLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.VerticalLabeledTextField BankNumberTextField { get; set; }

		[Outlet]
		UIKit.UILabel EnterBankInfoLabel { get; set; }

		[Outlet]
		UIKit.UIImageView SampleChequeImageView { get; set; }

		[Outlet]
		MobileClaims.iOS.GSButton SaveAndContinueButton { get; set; }

		[Outlet]
		UIKit.UILabel TransitNumberErrorLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.VerticalLabeledTextField TransitNumberTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AccountNumberTextField != null) {
				AccountNumberTextField.Dispose ();
				AccountNumberTextField = null;
			}

			if (BankNumberTextField != null) {
				BankNumberTextField.Dispose ();
				BankNumberTextField = null;
			}

			if (EnterBankInfoLabel != null) {
				EnterBankInfoLabel.Dispose ();
				EnterBankInfoLabel = null;
			}

			if (SampleChequeImageView != null) {
				SampleChequeImageView.Dispose ();
				SampleChequeImageView = null;
			}

			if (SaveAndContinueButton != null) {
				SaveAndContinueButton.Dispose ();
				SaveAndContinueButton = null;
			}

			if (TransitNumberTextField != null) {
				TransitNumberTextField.Dispose ();
				TransitNumberTextField = null;
			}

			if (TransitNumberErrorLabel != null) {
				TransitNumberErrorLabel.Dispose ();
				TransitNumberErrorLabel = null;
			}

			if (BankNumberErrorLabel != null) {
				BankNumberErrorLabel.Dispose ();
				BankNumberErrorLabel = null;
			}

			if (AccountNumberErrorLabel != null) {
				AccountNumberErrorLabel.Dispose ();
				AccountNumberErrorLabel = null;
			}
		}
	}
}
