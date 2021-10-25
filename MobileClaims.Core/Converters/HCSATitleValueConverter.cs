using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class HCSATitleValueConverter: MvxValueConverter
    {  
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty; 
            if (parameter != null)
            {  
                if (parameter.ToString()  == "2")
                {
                    result = Resource.ClaimSubmissionResult_HCSATitle;
                    if (value != null && value is string)
                    {
                        result +="-"+ (string)value; 
                    } 
                }
            } 
            return result.ToUpper();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

