using System;

using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Droid
{
	public class TreatmentDateDurationValueConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			string resp=null;
			try{
				DateTime date = (DateTime)value;
					resp= string.Format ("{0:MMMM dd, yyy}", date);
					string par=(string)parameter;
					resp=resp+" ("+ par + ")";

			}catch(Exception ex){
				//System.Console.Write (ex.StackTrace);
			}
		
			return resp;
		}

		// No need to implement converting back on a one-way binding
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			return null;
		}
	}
}
