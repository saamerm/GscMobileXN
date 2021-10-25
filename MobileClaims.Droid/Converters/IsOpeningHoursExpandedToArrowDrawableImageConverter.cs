using System;
using System.Globalization;
using Android.Support.V4.Content;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;


namespace MobileClaims.Droid.Converters
{
    public class IsOpeningHoursExpandedToArrowDrawableImageConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentActiviy = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            return (bool)value
                ? ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.icon_arrowup)
                : ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.icon_arrowdown);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}