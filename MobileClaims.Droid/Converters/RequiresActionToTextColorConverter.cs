using Android.OS;
using MobileClaims.Core.Entities;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;
using System;
using System.Globalization;

namespace MobileClaims.Droid.Converters
{
    public class RequiresActionToTextColorConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            {
                return (ClaimActionState) value == ClaimActionState.None
                    ? activity.Resources.GetColor(Resource.Color.black)
                    : activity.Resources.GetColor(Resource.Color.dark_red);
            }
            return (ClaimActionState) value == ClaimActionState.None
                    ? activity.Resources.GetColor(Resource.Color.black, activity.Theme)
                    : activity.Resources.GetColor(Resource.Color.dark_red, activity.Theme);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}