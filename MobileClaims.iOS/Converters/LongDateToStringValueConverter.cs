using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class LongDateToStringValueConverter : MvxValueConverter
	{
		public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (((DateTime)value).ToLongDateString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
			return value;
		}
	}
}