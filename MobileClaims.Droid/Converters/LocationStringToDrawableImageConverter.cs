using Android.Support.V4.Content;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;
using System;
using System.Globalization;

namespace MobileClaims.Droid.Converters
{
	public class LocationStringToDrawableImageConverter : IMvxValueConverter
	{
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
	        var currentActiviy = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

	        return string.IsNullOrEmpty(value as string)
	            ? ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.location_highlight_icon) 
	            : ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.location_icon);
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
	        throw new NotImplementedException();
	    }
	}
}
