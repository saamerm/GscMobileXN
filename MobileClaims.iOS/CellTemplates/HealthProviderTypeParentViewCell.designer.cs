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
    [Register ("HealthProviderTypeParentViewCell")]
    partial class HealthProviderTypeParentViewCell
    {
        [Outlet]
        UIKit.UIImageView checkedImage { get; set; }


        [Outlet]
        UIKit.UIButton expandButton { get; set; }


        [Outlet]
        FFImageLoading.Cross.MvxCachedImageView providerTypeImage { get; set; }


        [Outlet]
        UIKit.UIButton selectButton { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkedImage != null) {
                checkedImage.Dispose ();
                checkedImage = null;
            }

            if (expandButton != null) {
                expandButton.Dispose ();
                expandButton = null;
            }

            if (providerTypeImage != null) {
                providerTypeImage.Dispose ();
                providerTypeImage = null;
            }

            if (selectButton != null) {
                selectButton.Dispose ();
                selectButton = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}