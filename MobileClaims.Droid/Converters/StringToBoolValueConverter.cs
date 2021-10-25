using System;

using System.Globalization;
using MvvmCross.Converters;


namespace MobileClaims.Droid
{
	public class StringToBoolValueConverter : IMvxValueConverter
	{
	
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{

			if (!string.IsNullOrEmpty((string)value))
			{
				return true;
			}
			return false;
		}

		// No need to implement converting back on a one-way binding
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			return null;
		}
	}
}
