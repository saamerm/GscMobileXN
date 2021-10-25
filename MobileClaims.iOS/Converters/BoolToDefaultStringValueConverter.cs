using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class BoolToDefaultStringValueConverter : MvxValueConverter<bool, string>
    {
        private bool _inverted;

        public BoolToDefaultStringValueConverter()
        { 
        }

        public BoolToDefaultStringValueConverter(bool inverted = false)
        {
            _inverted = inverted;
        }

        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            value = _inverted ? !value : value;
            return value ? parameter.ToString() : string.Empty;
        }
    }
}