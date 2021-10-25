using MobileClaims.Core.Entities.HCSA;
using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class ObjectToHideValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;
            if (null != value)
            {
                result = false;
                if (value is ExpenseType)
                {
                    ExpenseType claimvalue = value as ExpenseType;
                    if (string.IsNullOrEmpty(claimvalue.Name))
                        result = true;
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
