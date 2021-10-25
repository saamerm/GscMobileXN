using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class AltCarrierPrefixValueConverter : MvxValueConverter<double, string>
	{ 

		protected override string Convert(double value, Type targetType, object parameter, CultureInfo culture)
		{
			return "amountPaidAlt".tr () + " $" + value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
			return value;
		}
	}
}