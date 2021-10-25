using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;


namespace MobileClaims.Droid.Converters
{
    public class StringToVisibilityConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value as string) ? ViewStates.Visible : ViewStates.Gone;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}