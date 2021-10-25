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
	[Register ("DirectDepositParentCellView")]
	partial class DirectDepositParentCellView
	{
		[Outlet]
		UIKit.UIView BottomBorder { get; set; }

		[Outlet]
		UIKit.UIImageView ExpandImageView { get; set; }

		[Outlet]
		UIKit.UIImageView RoundImageView { get; set; }

		[Outlet]
		UIKit.UIButton SelectButton { get; set; }

		[Outlet]
		UIKit.UILabel StepNameLabel { get; set; }

		[Outlet]
		UIKit.UIView TopBorder { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ExpandImageView != null) {
				ExpandImageView.Dispose ();
				ExpandImageView = null;
			}

			if (RoundImageView != null) {
				RoundImageView.Dispose ();
				RoundImageView = null;
			}

			if (SelectButton != null) {
				SelectButton.Dispose ();
				SelectButton = null;
			}

			if (StepNameLabel != null) {
				StepNameLabel.Dispose ();
				StepNameLabel = null;
			}

			if (TopBorder != null) {
				TopBorder.Dispose ();
				TopBorder = null;
			}

			if (BottomBorder != null) {
				BottomBorder.Dispose ();
				BottomBorder = null;
			}
		}
	}
}
