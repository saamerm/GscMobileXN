using MobileClaims.Core.Entities.HCSA;
using MvvmCross.Converters;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using MvvmCross.UI;

namespace MobileClaims.Droid.Converters
{
    public  class ClaimsDetailsCountToVisibleValueConverter : IMvxValueConverter
    {

        public object  Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MvxVisibility result = MvxVisibility.Visible;
            if (value != null)
            { 
                ObservableCollection<ClaimDetail> list = value as ObservableCollection<ClaimDetail>;
                if (list.Count > 4)
                {
                    result = MvxVisibility.Collapsed;  
                }

            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}