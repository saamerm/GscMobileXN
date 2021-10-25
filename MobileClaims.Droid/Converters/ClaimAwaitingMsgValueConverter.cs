using System;

using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Droid
{
	public class ClaimAwaitingMsgValueConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			string responseKey = string.Empty;
			string param = string.Empty;
			param = (string)parameter;

			switch ((string)value) {
			case "2":
				if (Java.Util.Locale.Default.Language.Contains ("fr")) {
					responseKey = "*Le paiement des demandes de règlement au statut de «En attente de paiement» sera traité le jour ouvrable suivant, selon votre solde de {0}.";
				} else {
					responseKey = "*Payments for claims with a status of \"Awaiting Payment\" will be processed on the next business day, based on your {0} balance";
				}
					
					break;
				}

			if (param != null) {
				responseKey = string.Format (responseKey, param);
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
