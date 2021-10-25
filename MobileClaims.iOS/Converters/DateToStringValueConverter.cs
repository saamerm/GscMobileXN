using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class DateToStringValueConverter : MvxValueConverter
	{
		public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (((DateTime)value).ToShortDateString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
			return value;
		}
	}
}