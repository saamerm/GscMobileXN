using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class DollarSignPrefixValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return "$" + value;
		}

		public string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Substring (1, value.Length - 1);
		}
	}
}