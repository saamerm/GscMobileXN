using System;
using System.Collections.Generic;

using System.Globalization;
using MobileClaims.Core.Entities;
using MvvmCross.Converters;


namespace MobileClaims.Droid
{
	public class ObjectCountToVisibleValueConverter : IMvxValueConverter
    {  

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			bool result = true;
            if (value != null)
            {
                
				List<ServiceProvider> list = value as List<ServiceProvider>;
				if (list.Count > 0)
				    {
						result = false;
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
