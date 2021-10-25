using CoreGraphics;
using Google.Maps;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [FromBottomTransition]
    public partial class ProviderDetailsInformationViewController : GSCBaseViewController
    {
        private ProviderDetailsInformationViewModel _viewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ProviderDetailsInformationViewModel)ViewModel;

            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var set = this.CreateBindingSet<ProviderDetailsInformationViewController, ProviderDetailsInformationViewModel>();
            set.Bind(closeButton).To(vm => vm.HideProviderDetailsInformationCommand);
            set.Bind(providerTypeLabel).To(vm => vm.ViewModelParameter.ProviderType);
            set.Bind(providerNameLabel).To(vm => vm.ViewModelParameter.Model.ProviderTradingName);
            set.Bind(addressLabel).To(vm => vm.ViewModelParameter.FullAddress);
            set.Bind(hyperlinkTextView).To(vm => vm.ViewModelParameter.Hyperlink);
            set.Bind(openingHoursLabel).To(vm => vm.ViewModelParameter.OpeningHoursText);
            set.Bind(ratingLabel).To(vm => vm.ViewModelParameter.ScoreString);
            set.Bind(phoneNumberTextView).To(vm => vm.ViewModelParameter.PhoneFormatted);

            var scoreConverter = new ScoreToUIImageConverter();
            var googlePlacesStringDataToHiddenBoolConverter = new GooglePlacesStringDataToHiddenBoolConverter();
            set.Bind(star1).To(x => x.ViewModelParameter.Model.OverallScore).WithConversion(scoreConverter, 0);
            set.Bind(star2).To(x => x.ViewModelParameter.Model.OverallScore).WithConversion(scoreConverter, 1);
            set.Bind(star3).To(x => x.ViewModelParameter.Model.OverallScore).WithConversion(scoreConverter, 2);
            set.Bind(star4).To(x => x.ViewModelParameter.Model.OverallScore).WithConversion(scoreConverter, 3);
            set.Bind(star5).To(x => x.ViewModelParameter.Model.OverallScore).WithConversion(scoreConverter, 4);
            set.Bind(ratingView).For(x => x.Hidden).To(x => x.ViewModelParameter.DisplayRatingAndRatingAvailable).WithConversion(boolOppositeValueConverter);
            set.Bind(starsView).For(x => x.Hidden).To(x => x.ViewModelParameter.DisplayRating).WithConversion(boolOppositeValueConverter);
            set.Bind(notAvailableLabel).For(x => x.Hidden).To(x => x.ViewModelParameter.DisplayRatingAndRatingNotAvailable).WithConversion(boolOppositeValueConverter);
            set.Bind(favouritesImageView).To(x => x.ViewModelParameter.AlternateHeartImageUrl).WithConversion(new ImageUrlToUIImageConverter());
            set.Bind(directionsButton).To(vm => vm.ShowDirectionsCommand);
            set.Bind(favouritesButton).To(vm => vm.ToggleFavouriteProviderCommand);
            set.Bind(shareButton).To(vm => vm.AddToContactsCommand);
            set.Bind(billingImage).For(x => x.Hidden).To(x => x.ViewModelParameter.IsDirectBill).WithConversion(boolOppositeValueConverter);

            set.Bind(weekdayTextLabel).To(x => x.ViewModelParameter.WeekdayText);
            set.Bind(toggleWeekdayTextImage).For(x => x.Image).To(x => x.IsOpeningHoursExpanded).WithConversion(new IsOpeningHoursExpandedToArrowDrawableImageConverter());
            set.Bind(expandWeekdayTextButton).To(vm => vm.ToggleOpeningHoursCommand);
            set.Bind(weekdayTextLabel).For(x => x.Hidden).To(x => x.IsOpeningHoursExpanded).WithConversion(boolOppositeValueConverter);
            set.Bind(toggleWeekdayTextImage).For(x => x.Hidden).To(x => x.ViewModelParameter.WeekdayText).WithConversion(googlePlacesStringDataToHiddenBoolConverter);
            set.Bind(expandWeekdayTextButton).For(x => x.Hidden).To(x => x.ViewModelParameter.WeekdayText).WithConversion(googlePlacesStringDataToHiddenBoolConverter);

            set.Apply();
            SetTexts();
            SetFonts();

            if (!string.IsNullOrWhiteSpace(_viewModel.ViewModelParameter.WeekdayText))
            {
                scrollViewBottomConstraint.Constant = Constants.NAV_HEIGHT + 7 * weekdayTextLabel.Frame.Height;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!_viewModel.ViewModelParameter.IsDirectBill)
            {
                // This will position the provider name and type back to propery location. 
                providerNameLeadingConstraint.Constant = -33;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var panoramaView = PanoramaView.FromFrame(new CGRect(0, 0, View.Frame.Width, panoramaContainerView.Frame.Height),
                                                      new CoreLocation.CLLocationCoordinate2D(latitude: _viewModel.ViewModelParameter.Model.Latitude,
                                                                                              longitude: _viewModel.ViewModelParameter.Model.Longitude));
            panoramaContainerView.AddSubview(panoramaView);
        }

        private void SetFonts()
        {
            providerTypeLabel.Font = UIFont.FromName("Roboto-Regular", 10.0f);
            providerNameLabel.Font = UIFont.FromName("Roboto-Bold", 16.0f);

            favouritesLabel.Font = UIFont.FromName("Roboto-Regular", 9.0f);
            shareLabel.Font = UIFont.FromName("Roboto-Regular", 9.0f);
            directionsLabel.Font = UIFont.FromName("Roboto-Regular", 9.0f);

            addressLabel.Font = UIFont.FromName("Roboto-Regular", 12.0f);
            hyperlinkTextView.Font = UIFont.FromName("Roboto-Regular", 12.0f);
            openingHoursLabel.Font = UIFont.FromName("Roboto-Regular", 12.0f);
            phoneNumberTextView.Font = UIFont.FromName("Roboto-Regular", 12.0f);

        }

        private void SetTexts()
        {
            notAvailableLabel.Text = "NotAvailable".tr();
            favouritesLabel.Text = _viewModel.AddToFavouritiesText;
            shareLabel.Text = _viewModel.AddToContactsText;
            directionsLabel.Text = _viewModel.ShowDirectionsText;
        }
    }
}