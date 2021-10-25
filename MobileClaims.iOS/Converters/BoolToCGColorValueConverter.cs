using System;
using System.Globalization;
using CoreGraphics;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class BoolToCGColorValueConverter : MvxValueConverter<bool?, CGColor>
    {
        protected override CGColor Convert(bool? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.HasValue)
            {
                return Colors.MED_GREY_COLOR.CGColor;
            }
            else
            {
                if (!value.Value)
                {
                    return Colors.ERROR_COLOR.CGColor;
                }
                else
                {
                    return Colors.MED_GREY_COLOR.CGColor;
                }
            }
        }
    }
}
