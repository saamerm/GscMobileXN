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
	[Register ("Step1TableViewCell")]
	partial class Step1TableViewCell
	{
		[Outlet]
		UIKit.UILabel AuthorizeDirectDepositLabel { get; set; }

		[Outlet]
		UIKit.UIView ContainerView { get; set; }

		[Outlet]
		UIKit.UIImageView RoundImageView { get; set; }

		[Outlet]
		UIKit.UIButton SelectAuthorizeDirectDepositButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ContainerView != null) {
				ContainerView.Dispose ();
				ContainerView = null;
			}

			if (SelectAuthorizeDirectDepositButton != null) {
				SelectAuthorizeDirectDepositButton.Dispose ();
				SelectAuthorizeDirectDepositButton = null;
			}

			if (RoundImageView != null) {
				RoundImageView.Dispose ();
				RoundImageView = null;
			}

			if (AuthorizeDirectDepositLabel != null) {
				AuthorizeDirectDepositLabel.Dispose ();
				AuthorizeDirectDepositLabel = null;
			}
		}
	}
}
