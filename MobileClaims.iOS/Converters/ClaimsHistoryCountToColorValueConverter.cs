using System;
using System.Globalization;
using MobileClaims.iOS;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.Core.Converters
{
    public class ClaimsHistoryCountToColorValueConverter: MvxValueConverter<int, UIColor>
    {
        protected override UIColor Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value > 0) ? Colors.LightGrayColor : Colors.VeryLightGrayColor; 
        }

        public int ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

