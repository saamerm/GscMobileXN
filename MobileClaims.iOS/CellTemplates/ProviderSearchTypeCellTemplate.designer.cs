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
    [Register ("ProviderSearchTypeCellTemplate")]
    partial class ProviderSearchTypeCellTemplate
    {
        [Foundation.Outlet]
        UIKit.UILabel lblSearchType { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblSearchType != null) {
                lblSearchType.Dispose ();
                lblSearchType = null;
            }
        }
    }
}