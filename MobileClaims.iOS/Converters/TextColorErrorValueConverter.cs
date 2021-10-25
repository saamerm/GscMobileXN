using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS
{
	public class TextColorErrorValueConverter: MvxValueConverter<bool, UIColor>
	{
		protected override UIColor Convert(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			UIColor textColor = Colors.DARK_GREY_COLOR;
			if (value)
				textColor = Colors.ERROR_COLOR;
			return textColor;
		}
	}
}