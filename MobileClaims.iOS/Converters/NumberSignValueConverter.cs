using MvvmCross.Converters;
using System;
using System.Globalization;

namespace MobileClaims.Core.Converters
{
	public class NumberSignValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return "#" + value;
		}

		public string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Substring (1, value.Length - 1);
		}
	}
}