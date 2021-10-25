using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class FormNumberPrefixValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return "auditNotificationFormReference".tr() + value;
		}

        protected override string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
			return value.Substring(1, value.Length - 1);
		}
	}
}