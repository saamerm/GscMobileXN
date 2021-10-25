using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class NullObjectToBoolValueConverter<T> : MvxValueConverter<T, bool>
        where T : new()
    {
        protected override bool Convert(T value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInverted = false;
            if (parameter is bool)
            {
                isInverted = (bool)parameter;
            }
            object newValue = (object)value;
            return isInverted ? newValue != null : newValue == null;
        }
    }
}