// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.DirectDeposit.DirectDepositConfirmation
{
	[Register ("DDConfirmationViewController")]
	partial class DDConfirmationViewController
	{
		[Outlet]
		UIKit.UILabel AccountNumber { get; set; }

		[Outlet]
		UIKit.UILabel AccountNumberLabel { get; set; }

		[Outlet]
		UIKit.UIView AuthoriseDepositFundContainerView { get; set; }

		[Outlet]
		UIKit.UILabel AuthoriseDepositFundLabel { get; set; }

		[Outlet]
		UIKit.UIImageView AuthoriseDepositFundRoundImageView { get; set; }

		[Outlet]
		UIKit.UILabel BankAccountInformationLabel { get; set; }

		[Outlet]
		UIKit.UILabel BankNumber { get; set; }

		[Outlet]
		UIKit.UILabel BankNumberLabel { get; set; }

		[Outlet]
		UIKit.UIButton ChangeInformationButton { get; set; }

		[Outlet]
		UIKit.UILabel ConfirmDDUpdateLabel { get; set; }

		[Outlet]
		UIKit.UIButton ContinueButton { get; set; }

		[Outlet]
		UIKit.UILabel NoteConfirmationLabel { get; set; }

		[Outlet]
		UIKit.UIButton SelectAuthoriseDepositFund { get; set; }

		[Outlet]
		UIKit.UILabel TransitNumber { get; set; }

		[Outlet]
		UIKit.UILabel TransitNumberLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AccountNumber != null) {
				AccountNumber.Dispose ();
				AccountNumber = null;
			}

			if (AccountNumberLabel != null) {
				AccountNumberLabel.Dispose ();
				AccountNumberLabel = null;
			}

			if (AuthoriseDepositFundContainerView != null) {
				AuthoriseDepositFundContainerView.Dispose ();
				AuthoriseDepositFundContainerView = null;
			}

			if (AuthoriseDepositFundLabel != null) {
				AuthoriseDepositFundLabel.Dispose ();
				AuthoriseDepositFundLabel = null;
			}

			if (AuthoriseDepositFundRoundImageView != null) {
				AuthoriseDepositFundRoundImageView.Dispose ();
				AuthoriseDepositFundRoundImageView = null;
			}

			if (BankAccountInformationLabel != null) {
				BankAccountInformationLabel.Dispose ();
				BankAccountInformationLabel = null;
			}

			if (BankNumber != null) {
				BankNumber.Dispose ();
				BankNumber = null;
			}

			if (BankNumberLabel != null) {
				BankNumberLabel.Dispose ();
				BankNumberLabel = null;
			}

			if (ChangeInformationButton != null) {
				ChangeInformationButton.Dispose ();
				ChangeInformationButton = null;
			}

			if (ConfirmDDUpdateLabel != null) {
				ConfirmDDUpdateLabel.Dispose ();
				ConfirmDDUpdateLabel = null;
			}

			if (ContinueButton != null) {
				ContinueButton.Dispose ();
				ContinueButton = null;
			}

			if (NoteConfirmationLabel != null) {
				NoteConfirmationLabel.Dispose ();
				NoteConfirmationLabel = null;
			}

			if (SelectAuthoriseDepositFund != null) {
				SelectAuthoriseDepositFund.Dispose ();
				SelectAuthoriseDepositFund = null;
			}

			if (TransitNumber != null) {
				TransitNumber.Dispose ();
				TransitNumber = null;
			}

			if (TransitNumberLabel != null) {
				TransitNumberLabel.Dispose ();
				TransitNumberLabel = null;
			}
		}
	}
}
