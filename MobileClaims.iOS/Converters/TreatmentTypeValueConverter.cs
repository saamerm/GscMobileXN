using System;
using System.Globalization;
using MobileClaims.iOS.Views;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
	public class TreatmentTypeValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			string responseKey;

			switch ((string)value)
			{
			case "Acupuncture":
				responseKey = "locateProviderMyCurrentLocationLabel".tr();
				break;
			case "Chiropody":
				responseKey = "locateProviderAddressLabel".tr();
				break;
			case "Chiropractic":
				responseKey = "locateProviderPostalCodeLabel".tr();
				break;
			case "Counselling Services":
				responseKey = "locateProiverNoFilter".tr();
				break;
			case "Massage Therapy":
				responseKey = "locateProviderLastNameLabel".tr();
				break;
			case "Medical Items":
				responseKey = "locateProviderBusinessNameLabel".tr();
				break;
			case "Naturopathic":
				responseKey = "locateProviderCityLabel".tr();
				break;
			case "Orthodontics":
				responseKey = "locateProviderPhoneNumberLabel".tr();
				break;
			case "Physiotherapy":
				responseKey = "locateProviderMyCurrentLocationLabel".tr();
				break;
			case "Podiatry":
				responseKey = "locateProviderMyCurrentLocationLabel".tr();
				break;
			case "Prescription Contacts":
				responseKey = "locateProviderAddressLabel".tr();
				break;
			case "Prescription Glasses":
				responseKey = "locateProviderPostalCodeLabel".tr();
				break;
			case "Eye Examination":
				responseKey = "locateProiverNoFilter".tr();
				break;
			case "Speech Therapy":
				responseKey = "locateProviderLastNameLabel".tr();
				break;
			case "Dental Provider":
				responseKey = "locateProviderBusinessNameLabel".tr();
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