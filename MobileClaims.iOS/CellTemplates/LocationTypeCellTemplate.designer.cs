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
	[Foundation.Register("LocationTypeCellTemplate")]
	partial class LocationTypeCellTemplate
	{
		[Foundation.Outlet]
		UIKit.UILabel lblLocationType { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblLocationType != null) {
				lblLocationType.Dispose ();
				lblLocationType = null;
			}
		}
	}
}
