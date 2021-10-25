// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.CellTemplates
{
	[Register ("TextFieldValidationCellView")]
	partial class TextFieldValidationCellView
	{
		[Outlet]
		UIKit.UILabel ErrorMessage { get; set; }

		[Outlet]
		UIKit.UIImageView ErrorSignImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ErrorSignImageView != null) {
				ErrorSignImageView.Dispose ();
				ErrorSignImageView = null;
			}

			if (ErrorMessage != null) {
				ErrorMessage.Dispose ();
				ErrorMessage = null;
			}
		}
	}
}
