using System;
using System.Globalization;
using MobileClaims.iOS;
using MvvmCross.Converters;
using UIKit; 

namespace MobileClaims.Core.Converters
{
    public class ClaimsHistoryCountToTextColorValueConverter: MvxValueConverter<int, UIColor>
    {
        protected override UIColor Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value > 0) ? Colors.HealthProviderTypeTextNotSelectedColor : Colors.MED_GREY_COLOR;
        }

        public int ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}