using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class DollarSignDoublePrefixValueConverter : MvxValueConverter<double, string>
	{
		protected override string Convert(double value, Type targetType, object parameter, CultureInfo culture)
		{
			return "$" + value;
		}

		public double ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return  double.Parse( value.Substring (1, value.Length - 1) );
		}
	}
}