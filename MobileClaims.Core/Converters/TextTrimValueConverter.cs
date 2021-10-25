using System;
using System.Globalization;
using MvvmCross.Converters;


namespace MobileClaims.Core.Converters
{
    public class TextTrimValueConverter: MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty; 
            if (!string.IsNullOrEmpty(value.ToString()))
            { 
                int count = value.ToString().Length;
                if (count > 0)
                {
                    result = value.ToString();
                    if (parameter != null)
                    {
                        int longShouldBe = int.Parse(parameter.ToString());
                        if (longShouldBe < count)
                        {
                            result = result.Substring(0, longShouldBe); 
                            result += "...";
                        }
                    }
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

