using System;
using System.Threading.Tasks;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.HealthProvider
{
    public partial class HealthProviderCellView : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("HealthProviderCellView");

        public HealthProviderCellView()
        {
            InitialiseBindings();

            //TODO MSGSC-705 - label is visible but font is white!
            //distanceLabel.Hidden = true;
        }

        protected HealthProviderCellView(IntPtr handle) : base(handle)
        {
            InitialiseBindings();

            //TODO MSGSC-705 - label is visible but font is white!
            //distanceLabel.Hidden = true;
        }

        public Func<HealthProviderSummaryModel, Task> ToggleFavourite { get; set; }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            this.SeparatorInset = UIEdgeInsets.Zero;

            providerNameLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
            providerTypeLabel.Font = UIFont.FromName("Roboto-Regular", 13.0f);
            addressLabel.Font = UIFont.FromName("Roboto-Regular", 11.0f);
            openingHoursLabel.Font = UIFont.FromName("Roboto-Regular", 11.0f);
            ratingLabel.Font = UIFont.FromName("Roboto-Regular", 13.0f);
            var usingTheHealthProviderTapGestureRecognizer =
                new UITapGestureRecognizer(async () =>
            {
                if (DataContext is HealthProviderSummaryModel vm)
                {
                    await ToggleFavourite(vm);
                }
            });

            toggleFavouriteView.AddGestureRecognizer(usingTheHealthProviderTapGestureRecognizer);
        }

        private void InitialiseBindings()
        {
            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<HealthProviderCellView, HealthProviderSummaryModel>();

                set.Bind(providerNameLabel).To(x => x.Model.ProviderTradingName);
                set.Bind(addressLabel).To(x => x.FullAddressMultiline);
                set.Bind(distanceLabel).To(x => x.DistanceText);
                set.Bind(ratingLabel).To(x => x.ScoreString);
                set.Bind(scoreView).For(x => x.Hidden).To(x => x.DisplayRatingAndRatingAvailable).WithConversion(boolOppositeValueConverter);
                set.Bind(notAvailableLabel).For(x => x.Hidden).To(x => x.DisplayRatingAndRatingNotAvailable).WithConversion(boolOppositeValueConverter);
                set.Bind(billingImage).For(x => x.Hidden).To(x => x.IsDirectBill).WithConversion(boolOppositeValueConverter);
                set.Bind(providerTypeLabel).To(x => x.ProviderType);
                set.Bind(heartImageView).To(x => x.AlternateHeartImageUrl).WithConversion(new ImageUrlToUIImageConverter());
                set.Bind(openingHoursLabel).To(x => x.OpeningHoursText).Mode(MvxBindingMode.OneTime);

                var scoreConverter = new ScoreToUIImageConverter();
                set.Bind(star1Image).To(x => x.Model.OverallScore).WithConversion(scoreConverter, 0);
                set.Bind(star2Image).To(x => x.Model.OverallScore).WithConversion(scoreConverter, 1);
                set.Bind(star3Image).To(x => x.Model.OverallScore).WithConversion(scoreConverter, 2);
                set.Bind(star4Image).To(x => x.Model.OverallScore).WithConversion(scoreConverter, 3);
                set.Bind(star5Image).To(x => x.Model.OverallScore).WithConversion(scoreConverter, 4);
                set.Bind(ratingStackView).For(x => x.Hidden).To(x => x.DisplayRating).WithConversion(boolOppositeValueConverter);

                notAvailableLabel.Text = "NotAvailable".tr();
                set.Apply();
            });
        }
    }
}
