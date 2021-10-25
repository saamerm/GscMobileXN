using System.Drawing;
using MvvmCross.Plugin.Color;

namespace MobileClaims.Droid
{
	public class InvertTextColorEnableDisableValueConverter : MvxColorValueConverter
	{
		private static readonly Color TrueColor = Color.FromArgb(0x00, 0x00, 0x00);
		private static readonly Color FalseColor = Color.FromArgb(0xD2, 0xD4, 0xD4);

		protected override Color Convert(object value, object parameter, System.Globalization.CultureInfo culture)
		{
			return (bool)value ? FalseColor:TrueColor;
		}
	}
}
