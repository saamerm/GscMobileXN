using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class IntToBoolConverter : MvxValueConverter<int,bool>
    {
        protected override bool Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return value > 0;
        }

        protected override int ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
