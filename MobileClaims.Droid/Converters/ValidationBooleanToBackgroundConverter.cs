using System;
using System.Globalization;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Converters
{
    public class ValidationBooleanToBackgroundConverter : MvxValueConverter<bool?, Drawable>
    {
       
        protected override Drawable Convert(bool? value, Type targetType, object parameter, CultureInfo culture)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            if (!value.HasValue)
            {
                return ContextCompat.GetDrawable(activity, Resource.Drawable.RoundEditTextNone);
            }

            if (!value.Value)
            {
                return ContextCompat.GetDrawable(activity, Resource.Drawable.round_edit_text_error);
            }
            else
            {
                return ContextCompat.GetDrawable(activity, Resource.Drawable.RoundEditTextNone);
            }
        }
    }
}