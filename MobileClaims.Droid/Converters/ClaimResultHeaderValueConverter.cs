using System;

using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Droid
{
	public class ClaimResultHeaderValueConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			string responseKey = string.Empty;
			string param = string.Empty;
			param = (string)parameter;

			switch ((string)value) {
			case "2":
				if (Java.Util.Locale.Default.Language.Contains ("fr")) {
					responseKey = "Résultat de soumission de demande de règlement";
				} else {
					responseKey = "Claim submission result";
				}
					break;
			}
			
			if (param != null) {
				responseKey = responseKey + " - " + param;
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
