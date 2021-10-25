using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class DefaultDateBlankValueConverter: MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (value is DateTime)
            {
                DateTime dt = DateTime.Parse(value.ToString());
                if (dt != DateTime.MinValue)
                { 
                    result = dt.ToString("yyyy-MM-dd");
                } 
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

