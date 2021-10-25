using MobileClaims.Core.Services.Requests;
using MvvmCross.Converters;
using System;
using System.Globalization;

namespace MobileClaims.iOS.Converters
{
    public class SortByChoiceToBoolValueConverter : MvxValueConverter<SortByChoice, bool>
    {
        protected override bool Convert(SortByChoice value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == (SortByChoice)parameter;
        }
    }
}