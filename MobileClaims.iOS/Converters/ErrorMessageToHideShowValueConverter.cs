using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.core.Converters
{
    public class ErrorMessageToHideShowValueConverter : MvxValueConverter<string, bool>
    {
        protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {   
            return (string.IsNullOrEmpty(value )|| value .Length ==0)? true: false;
        }

        public object ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }  
    }
}