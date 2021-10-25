using System;
using System.Globalization;
using MvvmCross.Converters;


namespace MobileClaims.Core.Converters
{
  public  class AmountZeroToBlankStringValueConverter : MvxValueConverter  
    {
        public override  object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty; 
            if ((double .Parse ( value.ToString(),CultureInfo.InvariantCulture))!= 0.00)
            {
                result = value.ToString();
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0.00;
            if(!string.IsNullOrEmpty (value .ToString()))
            {
                result = double.Parse(value.ToString(), NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture);
                
            } 
            return result;
        }
    }
}
