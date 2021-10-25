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
	[Register ("DisclaimerView")]
	partial class DisclaimerView
	{
		[Outlet]
		UIKit.UILabel DisclaimerLabel { get; set; }

		[Outlet]
		UIKit.UITextView DisclaimerMessageTextView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DisclaimerParagraph1Label { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DisclaimerParagraph2Label { get; set; }

		[Outlet]
		UIKit.UILabel DisclaimerParagraph3Label { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DisclaimerLabel != null) {
				DisclaimerLabel.Dispose ();
				DisclaimerLabel = null;
			}

			if (DisclaimerMessageTextView != null) {
				DisclaimerMessageTextView.Dispose ();
				DisclaimerMessageTextView = null;
			}

			if (DisclaimerParagraph1Label != null) {
				DisclaimerParagraph1Label.Dispose ();
				DisclaimerParagraph1Label = null;
			}

			if (DisclaimerParagraph2Label != null) {
				DisclaimerParagraph2Label.Dispose ();
				DisclaimerParagraph2Label = null;
			}

			if (DisclaimerParagraph3Label != null) {
				DisclaimerParagraph3Label.Dispose ();
				DisclaimerParagraph3Label = null;
			}
		}
	}
}
