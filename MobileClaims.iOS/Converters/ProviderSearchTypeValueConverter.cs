using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class ProviderSearchTypeValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			string responseKey;

			switch ((string)value)
			{
			case "My Current Location":
				responseKey = "locateProviderMyCurrentLocationLabel".tr();
				break;
			case "Address":
				responseKey = "locateProviderAddressLabel".tr();
				break;
			case "Postal Code":
				responseKey = "locateProviderPostalCodeLabel".tr();
				break;
			case "No Filter":
				responseKey = "locateProiverNoFilter".tr();
				break;
			case "Last Name":
				responseKey = "locateProviderLastNameLabel".tr();
				break;
			case "Business Name":
				responseKey = "locateProviderBusinessNameLabel".tr();
				break;
			case "City":
				responseKey = "locateProviderCityLabel".tr();
				break;
			case "Phone Number":
				responseKey = "locateProviderPhoneNumberLabel".tr();
				break;
			default:
				responseKey = (string)value;
				break;
			}
			return responseKey;
		}

		public string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Substring (1, value.Length - 1);
		}
	}
}
