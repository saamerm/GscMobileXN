using System;

using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Droid
{
	public class TypeNameToStringValueConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			string responseKey = (string)value;

			if (Java.Util.Locale.Default.Language.Contains("fr")) {
				switch ((string)value) {
				case "My Current Location":
					responseKey = "Mon emplacement actuel";
					break;
				case "Address":
					responseKey = "adresse";
					break;
				case "Postal Code":
					responseKey = "Code postal";
					break;
				case "No Filter":
					responseKey = "Aucun filtre";
					break;
				case "Last Name":
					responseKey = "Nom de famille";
					break;
				case "Business Name":
					responseKey = "Nom commercial";
					break;
				case "City":
					responseKey = "Ville";
					break;
				case "Phone Number":
					responseKey = "Numéro de téléphone";
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
