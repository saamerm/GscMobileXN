using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class RequiresCopToInfoConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? Resource.ClaimHistoryDetailRequriesCopTrue : Resource.ClaimHistoryDetailRequriesCopFalse;
        }

        protected override bool ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
