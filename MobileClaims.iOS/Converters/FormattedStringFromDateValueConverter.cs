using System;
using Foundation;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.iOS
{
	public class FormattedStringFromDateValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.Length > 10) {
				NSDateFormatter dtf = new NSDateFormatter ();
				dtf.DateFormat = "MM/dd/yyyy HH:mm:ss a";
				NSDate dtStart = dtf.Parse (value);

				dtf.DateFormat = "yyyy-MM-dd";
				string strStart = dtf.ToString (dtStart);		
				return strStart;

			} else {			
				return value;
			}
		}

		public string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Substring (1, value.Length - 1);
		}
	}

	public class StringFromDateValueConverter : MvxValueConverter<DateTime, string>
	{

        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            string strStart = "";
            strStart = value.ToString("d");
            return strStart;
        }
    }
}