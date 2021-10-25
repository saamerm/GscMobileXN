// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [Register ("LaunchScreenView")]
    partial class LaunchScreenView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (viewContainer != null) {
                viewContainer.Dispose ();
                viewContainer = null;
            }
        }
    }
}