using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class DoubleScoreToStarImageUrlConverter : MvxValueConverter<double, string>
    {
        protected override string Convert(double value, Type targetType, object parameter, CultureInfo culture)
        {
            return value >= (double)parameter ? "StarRatingFull" : "StarRatingNone";
        }
    }
}