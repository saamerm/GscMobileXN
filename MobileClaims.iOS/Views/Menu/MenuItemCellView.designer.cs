// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.Menu
{
	[Register ("MenuItemCellView")]
	partial class MenuItemCellView
	{
		[Outlet]
		UIKit.UIView CounterContainerView { get; set; }

		[Outlet]
		UIKit.UILabel CounterLabel { get; set; }

		[Outlet]
		UIKit.UIView MenuItemCellContainer { get; set; }

		[Outlet]
		UIKit.UIImageView MenuItemIconImageView { get; set; }

		[Outlet]
		UIKit.UILabel MenuItemLabelView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MenuLabelTrailingConstraintWhenNoCounter { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint MenuLabelTrailingConstraintWithCounter { get; set; }

		[Outlet]
		UIKit.UIView SeperatorView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CounterContainerView != null) {
				CounterContainerView.Dispose ();
				CounterContainerView = null;
			}

			if (CounterLabel != null) {
				CounterLabel.Dispose ();
				CounterLabel = null;
			}

			if (MenuItemCellContainer != null) {
				MenuItemCellContainer.Dispose ();
				MenuItemCellContainer = null;
			}

			if (MenuItemIconImageView != null) {
				MenuItemIconImageView.Dispose ();
				MenuItemIconImageView = null;
			}

			if (MenuItemLabelView != null) {
				MenuItemLabelView.Dispose ();
				MenuItemLabelView = null;
			}

			if (MenuLabelTrailingConstraintWhenNoCounter != null) {
				MenuLabelTrailingConstraintWhenNoCounter.Dispose ();
				MenuLabelTrailingConstraintWhenNoCounter = null;
			}

			if (SeperatorView != null) {
				SeperatorView.Dispose ();
				SeperatorView = null;
			}

			if (MenuLabelTrailingConstraintWithCounter != null) {
				MenuLabelTrailingConstraintWithCounter.Dispose ();
				MenuLabelTrailingConstraintWithCounter = null;
			}
		}
	}
}
