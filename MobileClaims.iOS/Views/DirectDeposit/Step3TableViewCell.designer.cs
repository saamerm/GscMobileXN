// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.DirectDeposit
{
	[Register ("Step3TableViewCell")]
	partial class Step3TableViewCell
	{
		[Outlet]
		UIKit.UIView DoNotReceiveNotificationContainerView { get; set; }

		[Outlet]
		UIKit.UILabel DoNotReceiveNotificationLabel { get; set; }

		[Outlet]
		UIKit.UIImageView DoNotReceiveNotificationRoundImageView { get; set; }

		[Outlet]
		UIKit.UIView ReceiveNotificationContainerView { get; set; }

		[Outlet]
		UIKit.UILabel ReceiveNotificationLabel { get; set; }

		[Outlet]
		UIKit.UIImageView ReceiveNotificationRoundImageView { get; set; }

		[Outlet]
		UIKit.UIButton SelectDoNotReceiveNotification { get; set; }

		[Outlet]
		UIKit.UIButton SelectReceiveNotificationButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DoNotReceiveNotificationContainerView != null) {
				DoNotReceiveNotificationContainerView.Dispose ();
				DoNotReceiveNotificationContainerView = null;
			}

			if (DoNotReceiveNotificationLabel != null) {
				DoNotReceiveNotificationLabel.Dispose ();
				DoNotReceiveNotificationLabel = null;
			}

			if (DoNotReceiveNotificationRoundImageView != null) {
				DoNotReceiveNotificationRoundImageView.Dispose ();
				DoNotReceiveNotificationRoundImageView = null;
			}

			if (ReceiveNotificationContainerView != null) {
				ReceiveNotificationContainerView.Dispose ();
				ReceiveNotificationContainerView = null;
			}

			if (ReceiveNotificationLabel != null) {
				ReceiveNotificationLabel.Dispose ();
				ReceiveNotificationLabel = null;
			}

			if (ReceiveNotificationRoundImageView != null) {
				ReceiveNotificationRoundImageView.Dispose ();
				ReceiveNotificationRoundImageView = null;
			}

			if (SelectReceiveNotificationButton != null) {
				SelectReceiveNotificationButton.Dispose ();
				SelectReceiveNotificationButton = null;
			}

			if (SelectDoNotReceiveNotification != null) {
				SelectDoNotReceiveNotification.Dispose ();
				SelectDoNotReceiveNotification = null;
			}
		}
	}
}
