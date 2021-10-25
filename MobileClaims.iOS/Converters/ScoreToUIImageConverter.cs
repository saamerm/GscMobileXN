using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class ScoreToUIImageConverter : MvxValueConverter<double, UIImage>
    {
        protected override UIImage Convert(double value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                UIImage.FromBundle("StarRatingNone");
            }

            double difference = value - System.Convert.ToDouble(parameter);

            if (difference == 0.5)
            {
                return UIImage.FromBundle("StarRatingHalf");
            }

            if (difference > 0.5)
            {
                return UIImage.FromBundle("StarRatingFull");
            }

            return UIImage.FromBundle("StarRatingNone");
        }
    }
}