using System;
using System.Globalization;
using UIKit; 
using MobileClaims.Core.Entities;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class PlaceHolderTextColorValueConverter: MvxValueConverter<ServiceProviderProvince, UIColor>
    {
        protected override UIColor Convert(ServiceProviderProvince value, Type targetType, object parameter, CultureInfo culture)
        {
            UIColor result = Colors.HealthProviderTypeTextNotSelectedColor;
            if (value is ServiceProviderProvince)
            {
                ServiceProviderProvince spp = value as ServiceProviderProvince;
                if (!string.IsNullOrEmpty(spp.Name))
                {
                    if (spp.Name.Contains("*"))
                    {
                        result = Colors.MED_GREY_COLOR;
                    }
                }
            }    
            return result;
        }

        public int ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

