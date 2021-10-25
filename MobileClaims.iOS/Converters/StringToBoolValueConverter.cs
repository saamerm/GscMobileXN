using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class StringToBoolValueConverter : MvxValueConverter<string, bool>
    {
        protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inverted = false;
            if (parameter is bool)
            {
                inverted = (bool)parameter;
            }

            var newValue = inverted ? string.IsNullOrWhiteSpace(value) : !string.IsNullOrWhiteSpace(value);
            return newValue;
        }
    }
}