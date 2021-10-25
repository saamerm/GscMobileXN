// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS
{
	[Foundation.Register("ProviderCellTemplatePhone")]
	partial class ProviderCellTemplatePhone
	{
		[Foundation.Outlet]
		UIKit.UILabel lblProvider { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblProvider != null) {
				lblProvider.Dispose ();
				lblProvider = null;
			}
		}
	}
}
