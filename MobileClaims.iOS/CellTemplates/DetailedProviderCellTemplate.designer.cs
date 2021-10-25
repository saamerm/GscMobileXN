// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS
{
    [Register ("DetailedProviderCellTemplate")]
    partial class DetailedProviderCellTemplate
    {
        [Foundation.Outlet]
        UIKit.UILabel lblCityProvincePostCode { get; set; }


        [Foundation.Outlet]
        UIKit.UILabel lblProviderName { get; set; }


        [Foundation.Outlet]
        UIKit.UILabel lblStreetAddress { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblCityProvincePostCode != null) {
                lblCityProvincePostCode.Dispose ();
                lblCityProvincePostCode = null;
            }

            if (lblProviderName != null) {
                lblProviderName.Dispose ();
                lblProviderName = null;
            }

            if (lblStreetAddress != null) {
                lblStreetAddress.Dispose ();
                lblStreetAddress = null;
            }
        }
    }
}