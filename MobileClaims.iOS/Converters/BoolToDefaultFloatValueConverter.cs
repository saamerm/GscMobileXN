using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class BoolToDefaultFloatValueConverter : MvxValueConverter<bool, nfloat>
    {
        private bool _inverted;

        public BoolToDefaultFloatValueConverter()
        { 
        }

        public BoolToDefaultFloatValueConverter(bool inverted = false)
        {
            _inverted = inverted;
        }

        protected override nfloat Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            value = _inverted ? !value : value;
            return value ? (nfloat)parameter : 0;
        }
    }
}
