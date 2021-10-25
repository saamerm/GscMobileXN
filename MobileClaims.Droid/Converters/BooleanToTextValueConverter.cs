using System;
using System.Globalization;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;


namespace MobileClaims.Droid
{
    public class BooleanToTextValueConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            return (bool) value
                ? activity.Resources.GetString(Resource.String.yes)
                : activity.Resources.GetString(Resource.String.no);
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}