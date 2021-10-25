using MvvmCross.Converters;
using System;
using System.Globalization;

namespace MobileClaims.Core.Converters
{
    public class NonBreakingSpaceValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return culture.ToString().Contains("fr") 
                ? value.ToString().Replace(" :", "\u00A0:")
                : value.ToString();
        }
    }
}
