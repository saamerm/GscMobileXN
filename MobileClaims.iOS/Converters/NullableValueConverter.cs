using System;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class NullableValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return parameter;
            }
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            return value;
        }
    }
}