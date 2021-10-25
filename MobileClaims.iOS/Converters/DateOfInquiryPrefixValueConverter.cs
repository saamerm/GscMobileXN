using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class DateOfInquiryPrefixValueConverter : MvxValueConverter<DateTime, string>
	{
		protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
		{
			return "dateOfInquiry".tr () + " " + value.ToLongDateString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}