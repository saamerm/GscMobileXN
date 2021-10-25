using Android.Views;
using MvvmCross.Converters;
using System;
using System.Globalization;

namespace MobileClaims.Droid
{
	public class VisibleCollapseValueConverter : MvxValueConverter<bool, ViewStates>
	{
		public ViewStates  Convert(bool value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
		

			if ((bool) value)
				{
					return ViewStates.Visible;
				}else
				{
					return ViewStates.Invisible;
				}
					

		}

		// No need to implement converting back on a one-way binding
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
		{
			return null;
		}
	}
}
