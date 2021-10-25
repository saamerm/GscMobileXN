using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class BoolOppositeValueConverter : MvxValueConverter<bool?, bool>
	{
		protected override bool Convert(bool? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.HasValue)
            {
                return true;
            }
            else if (value == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}