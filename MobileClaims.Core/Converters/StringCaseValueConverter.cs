using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class StringCaseValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToUpper ();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}