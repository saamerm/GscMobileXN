using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class ClaimsHistoryCountToBoolValueConverter : MvxValueConverter<int, bool>
    {
        protected override bool Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value>0) ? true : false ; 
        }

        public int ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

