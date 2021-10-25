// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Controls
{
	[Register ("DrugLookupByNameControl")]
	public partial class DrugLookupByNameControl
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblDrugName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tableParticipants { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField tbDrugName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblDrugName != null) {
				lblDrugName.Dispose ();
				lblDrugName = null;
			}

			if (tbDrugName != null) {
				tbDrugName.Dispose ();
				tbDrugName = null;
			}

			if (tableParticipants != null) {
				tableParticipants.Dispose ();
				tableParticipants = null;
			}
		}
	}
}
