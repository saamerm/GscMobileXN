using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class GooglePlacesStringDataToHiddenBoolConverter : MvxValueConverter<string, bool>
    {
        protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        protected override string ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}