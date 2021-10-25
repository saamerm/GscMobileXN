using System;
using System.Globalization;
using MvvmCross.Converters;


namespace MobileClaims.Core.Converters
{
    public class DollarSignDoublePrefixValueConverter : IMvxValueConverter
    {
        public object  Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (value != null)
            {
                result = value.ToString();
                if (!result.Contains("$"))
                {
                    result = "$" + value.ToString();
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string valueStr = value.ToString();
            if (valueStr.Contains("$"))
            {
                valueStr = valueStr.Remove(0, 1);
            }
            return double.Parse(valueStr);
        } 
         
    }
}
   