// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MobileClaims.iOS.Views.HealthProvider
{
    [Register ("HealthProviderCellView")]
    partial class HealthProviderCellView
    {
        [Outlet]
        UIKit.UILabel addressLabel { get; set; }


        [Outlet]
        UIKit.UIImageView billingImage { get; set; }


        [Outlet]
        UIKit.UILabel distanceLabel { get; set; }


        [Outlet]
        UIKit.UIImageView heartImageView { get; set; }


        [Outlet]
        UIKit.UILabel openingHoursLabel { get; set; }


        [Outlet]
        UIKit.UILabel providerNameLabel { get; set; }


        [Outlet]
        UIKit.UILabel providerTypeLabel { get; set; }


        [Outlet]
        UIKit.UILabel ratingLabel { get; set; }


        [Outlet]
        UIKit.UIStackView ratingStackView { get; set; }


        [Outlet]
        UIKit.UIView scoreView { get; set; }


        [Outlet]
        UIKit.UIImageView star1Image { get; set; }


        [Outlet]
        UIKit.UIImageView star2Image { get; set; }


        [Outlet]
        UIKit.UIImageView star3Image { get; set; }


        [Outlet]
        UIKit.UIImageView star4Image { get; set; }


        [Outlet]
        UIKit.UIImageView star5Image { get; set; }


        [Outlet]
        UIKit.UIStackView starsStackView { get; set; }


        [Outlet]
        UIKit.UIView toggleFavouriteView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel notAvailableLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addressLabel != null) {
                addressLabel.Dispose ();
                addressLabel = null;
            }

            if (billingImage != null) {
                billingImage.Dispose ();
                billingImage = null;
            }

            if (distanceLabel != null) {
                distanceLabel.Dispose ();
                distanceLabel = null;
            }

            if (heartImageView != null) {
                heartImageView.Dispose ();
                heartImageView = null;
            }

            if (notAvailableLabel != null) {
                notAvailableLabel.Dispose ();
                notAvailableLabel = null;
            }

            if (openingHoursLabel != null) {
                openingHoursLabel.Dispose ();
                openingHoursLabel = null;
            }

            if (providerNameLabel != null) {
                providerNameLabel.Dispose ();
                providerNameLabel = null;
            }

            if (providerTypeLabel != null) {
                providerTypeLabel.Dispose ();
                providerTypeLabel = null;
            }

            if (ratingLabel != null) {
                ratingLabel.Dispose ();
                ratingLabel = null;
            }

            if (ratingStackView != null) {
                ratingStackView.Dispose ();
                ratingStackView = null;
            }

            if (scoreView != null) {
                scoreView.Dispose ();
                scoreView = null;
            }

            if (star1Image != null) {
                star1Image.Dispose ();
                star1Image = null;
            }

            if (star2Image != null) {
                star2Image.Dispose ();
                star2Image = null;
            }

            if (star3Image != null) {
                star3Image.Dispose ();
                star3Image = null;
            }

            if (star4Image != null) {
                star4Image.Dispose ();
                star4Image = null;
            }

            if (star5Image != null) {
                star5Image.Dispose ();
                star5Image = null;
            }

            if (starsStackView != null) {
                starsStackView.Dispose ();
                starsStackView = null;
            }

            if (toggleFavouriteView != null) {
                toggleFavouriteView.Dispose ();
                toggleFavouriteView = null;
            }
        }
    }
}