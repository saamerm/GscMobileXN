using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;
using System;
using System.Globalization;

namespace MobileClaims.Droid.Converters
{
    public class EligibilityNullToMsgValueConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            string result = string.Empty;
            string status = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
#if FPPM
                    result = string.Format(
                        activity.Resources.GetString(Resource.String.unablebenefitEligibilityLabelFPPM),
                        activity.Resources.GetString(Resource.String.phonenumberFPPM));
#else
                    result = string.Format(
                        activity.Resources.GetString(Resource.String.unablebenefitEligibilityLabel),
                        activity.Resources.GetString(Resource.String.phonenumber));
#endif
                }
                else
                {
                    var dt = DateTime.Parse(value.ToString());
                    var dtMin = DateTime.MinValue;
                    if (dt.Year == dtMin.Year && dt.Month == dtMin.Month && dt.Day == dtMin.Day)
                    {
#if FPPM
                        result = string.Format(
                            activity.Resources.GetString(Resource.String.unablebenefitEligibilityLabelFPPM),
                            activity.Resources.GetString(Resource.String.phonenumberFPPM));
#else
                        result = string.Format(
                            activity.Resources.GetString(Resource.String.unablebenefitEligibilityLabel),
                            activity.Resources.GetString(Resource.String.phonenumber));
#endif
                    }
                    else
                    {
                        result = dt.ToString("MMM dd, yyyy");
                        status = parameter.ToString();
                        result = result + ' ' + status;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }

            return result;
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}