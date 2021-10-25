using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class PhonePrefixValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return "phone".tr () + "colon".tr () + value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}