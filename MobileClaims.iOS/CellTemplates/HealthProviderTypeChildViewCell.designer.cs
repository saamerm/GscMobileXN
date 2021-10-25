// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.CellTemplates
{
    [Register ("HealthProviderTypeChildViewCell")]
    partial class HealthProviderTypeChildViewCell
    {
        [Outlet]
        UIKit.UIImageView checkedImage { get; set; }


        [Outlet]
        UIKit.UIButton expandButton { get; set; }


        [Outlet]
        UIKit.UIImageView expandImage { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FFImageLoading.Cross.MvxCachedImageView providerTypeImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkedImage != null) {
                checkedImage.Dispose ();
                checkedImage = null;
            }

            if (providerTypeImage != null) {
                providerTypeImage.Dispose ();
                providerTypeImage = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}