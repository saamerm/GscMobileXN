using System.Drawing;
using MvvmCross.Plugin.Color;

namespace MobileClaims.Droid
{
	public class TextColorErrorValueConverter : MvxColorValueConverter
	{
		private static readonly Color TrueColor = Color.FromArgb(0xFF, 0x00, 0x00);
		private static readonly Color FalseColor = Color.FromArgb(0x00, 0x00, 0x00);
		private static readonly Color DisableColor = Color.FromArgb(0xD2, 0xD4, 0xD4);

		protected override Color Convert(object value, object parameter, System.Globalization.CultureInfo culture)
		{
			if (parameter != null) {
				if (!(bool)parameter) {
					return DisableColor;
				}
			}

			return (bool)value ? TrueColor : FalseColor;
		}
	}
}