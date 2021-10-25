using System;

using System.Globalization;
using MobileClaims.Core.Entities;
using MvvmCross.Converters;

namespace MobileClaims.Droid
{
	public class NullToVisibleValueConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
		

			if (null != value)
				{
				ClaimOption claimvalue = value as ClaimOption;
					if (claimvalue.Name == "") {
						return false;
					}
					return true;
				}else
				{
					return false;
				}
					

		}

		// No need to implement converting back on a one-way binding
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			return null;
		}
	}
}
