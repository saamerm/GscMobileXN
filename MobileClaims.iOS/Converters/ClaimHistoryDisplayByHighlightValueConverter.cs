using System;
using System.Globalization;
using MobileClaims.Core;
using System.Collections.Generic;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class ClaimHistoryDisplayByHighlightValueConverter : MvxValueConverter<KeyValuePair<string, string>, bool>
    {  
        protected override bool Convert(KeyValuePair<string, string> value, Type targetType, object parameter, CultureInfo culture)
        { 
            bool result = false;
            KeyValuePair<string, string> displayBy = (KeyValuePair<string, string>)value;
            string para = parameter.ToString();
            switch (para)
            {
                case "1":
                    if (displayBy.Key.Equals(GSCHelper.DisplayByYearToDateKey))
                        result = true;
                    break;
                case "2":
                    if (displayBy.Key.Equals(GSCHelper.DisplayByDateRangeKey))
                        result = true;
                    break;
                case "3":
                    if (displayBy.Key.Equals(GSCHelper.DisplayByYearKey))
                        result = true;
                    break;
            }  
            return result;
        }

        public object ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            return  null;
        }
    }
}

