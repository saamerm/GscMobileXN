// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.COP
{
    [Register ("DocumentListCellView")]
    partial class DocumentListCellView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FileNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RemoveDocumentButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ThumbnailImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FileNameLabel != null) {
                FileNameLabel.Dispose ();
                FileNameLabel = null;
            }

            if (RemoveDocumentButton != null) {
                RemoveDocumentButton.Dispose ();
                RemoveDocumentButton = null;
            }

            if (ThumbnailImageView != null) {
                ThumbnailImageView.Dispose ();
                ThumbnailImageView = null;
            }
        }
    }
}