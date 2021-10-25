using System.Globalization;
using MobileClaims.iOS.Views;
using System;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class ClaimsHistoryCountToTitleValueConverter :MvxValueConverter<int, string>
    {
        protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return  value+ "claims".tr();
        }

        public int ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            string letters= "claims".tr();
            string numberStr = string.Empty ;
            if (value.Contains("letters"))
            {
                int indexOfClaim = value.IndexOf(letters);

                numberStr = value.Substring(0, indexOfClaim);
            }
            return  int.Parse( numberStr);
        }
    }
}