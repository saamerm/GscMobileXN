using System;

using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Droid
{
	public class DrugSearchModeValueConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			string responseKey = (string)value;

			if (cultureInfo.TwoLetterISOLanguageName.Contains ("en")) {
				switch ((string)value) {
				case "Search by DIN":
					responseKey = "By Drug Identification Number (DIN)";
					break;
				case "Search by Drug Name":
					responseKey = "By Drug Name";
					break;
				default:
					responseKey = (string)value;
					break;
				}
			}
			else if (cultureInfo.TwoLetterISOLanguageName.Contains ("fr")) {
				switch ((string)value) {
				case "Search by DIN":
					responseKey = "Par numéro d'identification du médicament (DIN)";
					break;
				case "Search by Drug Name":
					responseKey = "Par nom du médicament";
					break;
				default:
					responseKey = (string)value;
					break;
				}
			}
			return responseKey;
		}

		// No need to implement converting back on a one-way binding
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			return null;
		}
	}
}
