using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class BoolToStringValueConverter : MvxValueConverter<bool, string>
	{
		protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			string output = value ? "yes".tr () : "no".tr ();

			return output;
		}

		public bool ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == "yes".tr();
		}
	}
}