using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class FormNumberLongPrefixValueConverter : MvxValueConverter
	{
		public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return "auditNotificationFormReference".tr() + ((long)value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
			return value;
		}
	}
}