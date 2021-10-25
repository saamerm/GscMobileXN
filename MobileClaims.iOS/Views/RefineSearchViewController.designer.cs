// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views
{
	[Register ("RefineSearchViewController")]
	partial class RefineSearchViewController
	{
		[Outlet]
		MobileClaims.iOS.UI.RadioButton _distanceNonPharmacyButton { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.RadioButton _nameNonPharmacyButton { get; set; }

		[Outlet]
		UIKit.UIStackView _sortGroupNonPharmacy { get; set; }

		[Outlet]
		UIKit.UIStackView _sortGroupPharmacy { get; set; }

		[Outlet]
		UIKit.UIButton backToCurrentLocationButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.NSLayoutConstraint bottomScrollViewMargin { get; set; }

		[Outlet]
		UIKit.UIButton chooseProviderTypeButton { get; set; }

		[Outlet]
		UIKit.UILabel directBillLabel { get; set; }

		[Outlet]
		UIKit.UISwitch directBillSwitch { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.RadioButton distanceButton { get; set; }

		[Outlet]
		UIKit.UIButton doneRefineButton { get; set; }

		[Outlet]
		UIKit.UILabel filterLabel { get; set; }

		[Outlet]
		UIKit.UILabel headerTextView { get; set; }

		[Outlet]
		UIKit.UIView locationContainer { get; set; }

		[Outlet]
		UIKit.UILabel locationLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.AutoCompleteTextField locationTextField { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.RadioButton nameButton { get; set; }

		[Outlet]
		UIKit.UILabel providerTypeLabel { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.RadioButton ratingAndDistanceButton { get; set; }

		[Outlet]
		MobileClaims.iOS.UI.RadioButton ratingAndNameButton { get; set; }

		[Outlet]
		UIKit.UILabel ratingLabel { get; set; }

		[Outlet]
		UIKit.UIStackView ratingStars { get; set; }

		[Outlet]
		UIKit.UISwitch ratingSwitch { get; set; }

		[Outlet]
		UIKit.UIView ratingView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel recentlyVisitedLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UISwitch recentlyVisitedSwitch { get; set; }

		[Outlet]
		UIKit.UIView RecentlyVisitedView { get; set; }

		[Outlet]
		UIKit.UIButton searchLocationButton { get; set; }

		[Outlet]
		UIKit.UILabel sortLabel { get; set; }

		[Outlet]
		UIKit.UIStackView stackViewContainer { get; set; }

		[Outlet]
		UIKit.UIButton star1Button { get; set; }

		[Outlet]
		UIKit.UIButton star2Button { get; set; }

		[Outlet]
		UIKit.UIButton star3Button { get; set; }

		[Outlet]
		UIKit.UIButton star4Button { get; set; }

		[Outlet]
		UIKit.UIButton star5Button { get; set; }

		[Action ("clickACtion:")]
		partial void clickACtion (UIKit.UISwitch sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_distanceNonPharmacyButton != null) {
				_distanceNonPharmacyButton.Dispose ();
				_distanceNonPharmacyButton = null;
			}

			if (_nameNonPharmacyButton != null) {
				_nameNonPharmacyButton.Dispose ();
				_nameNonPharmacyButton = null;
			}

			if (_sortGroupNonPharmacy != null) {
				_sortGroupNonPharmacy.Dispose ();
				_sortGroupNonPharmacy = null;
			}

			if (_sortGroupPharmacy != null) {
				_sortGroupPharmacy.Dispose ();
				_sortGroupPharmacy = null;
			}

			if (backToCurrentLocationButton != null) {
				backToCurrentLocationButton.Dispose ();
				backToCurrentLocationButton = null;
			}

			if (chooseProviderTypeButton != null) {
				chooseProviderTypeButton.Dispose ();
				chooseProviderTypeButton = null;
			}

			if (directBillLabel != null) {
				directBillLabel.Dispose ();
				directBillLabel = null;
			}

			if (directBillSwitch != null) {
				directBillSwitch.Dispose ();
				directBillSwitch = null;
			}

			if (distanceButton != null) {
				distanceButton.Dispose ();
				distanceButton = null;
			}

			if (RecentlyVisitedView != null) {
				RecentlyVisitedView.Dispose ();
				RecentlyVisitedView = null;
			}

			if (doneRefineButton != null) {
				doneRefineButton.Dispose ();
				doneRefineButton = null;
			}

			if (filterLabel != null) {
				filterLabel.Dispose ();
				filterLabel = null;
			}

			if (headerTextView != null) {
				headerTextView.Dispose ();
				headerTextView = null;
			}

			if (locationContainer != null) {
				locationContainer.Dispose ();
				locationContainer = null;
			}

			if (locationLabel != null) {
				locationLabel.Dispose ();
				locationLabel = null;
			}

			if (locationTextField != null) {
				locationTextField.Dispose ();
				locationTextField = null;
			}

			if (nameButton != null) {
				nameButton.Dispose ();
				nameButton = null;
			}

			if (providerTypeLabel != null) {
				providerTypeLabel.Dispose ();
				providerTypeLabel = null;
			}

			if (ratingAndDistanceButton != null) {
				ratingAndDistanceButton.Dispose ();
				ratingAndDistanceButton = null;
			}

			if (ratingAndNameButton != null) {
				ratingAndNameButton.Dispose ();
				ratingAndNameButton = null;
			}

			if (ratingLabel != null) {
				ratingLabel.Dispose ();
				ratingLabel = null;
			}

			if (ratingStars != null) {
				ratingStars.Dispose ();
				ratingStars = null;
			}

			if (ratingSwitch != null) {
				ratingSwitch.Dispose ();
				ratingSwitch = null;
			}

			if (ratingView != null) {
				ratingView.Dispose ();
				ratingView = null;
			}

			if (searchLocationButton != null) {
				searchLocationButton.Dispose ();
				searchLocationButton = null;
			}

			if (sortLabel != null) {
				sortLabel.Dispose ();
				sortLabel = null;
			}

			if (stackViewContainer != null) {
				stackViewContainer.Dispose ();
				stackViewContainer = null;
			}

			if (star1Button != null) {
				star1Button.Dispose ();
				star1Button = null;
			}

			if (star2Button != null) {
				star2Button.Dispose ();
				star2Button = null;
			}

			if (star3Button != null) {
				star3Button.Dispose ();
				star3Button = null;
			}

			if (star4Button != null) {
				star4Button.Dispose ();
				star4Button = null;
			}

			if (star5Button != null) {
				star5Button.Dispose ();
				star5Button = null;
			}

			if (bottomScrollViewMargin != null) {
				bottomScrollViewMargin.Dispose ();
				bottomScrollViewMargin = null;
			}

			if (recentlyVisitedLabel != null) {
				recentlyVisitedLabel.Dispose ();
				recentlyVisitedLabel = null;
			}

			if (recentlyVisitedSwitch != null) {
				recentlyVisitedSwitch.Dispose ();
				recentlyVisitedSwitch = null;
			}
		}
	}
}
