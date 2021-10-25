using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class BoolOppositeValueConverter : MvxValueConverter<bool, bool>
	{
		protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(value is bool && (bool)value);
		}

		public bool ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(value is bool && (bool)value);
		}
	}
}