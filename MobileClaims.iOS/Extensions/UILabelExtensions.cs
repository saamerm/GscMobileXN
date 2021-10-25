using UIKit;

namespace MobileClaims.iOS.Extensions
{
    public static class UILabelExtensions
    {
        public static void SetLabel(this UILabel label, string fontName, float fontSize, UIColor textColor)
        {
            label.Font = UIFont.FromName(fontName, fontSize);
            label.TextColor = textColor;
        }

        public static void SetFontAndTextColor(this UILabel label, string fontName, float fontSize, UIColor textColor, int lines = 0, bool shouldTranslatesAutoResizingMaskIntoConstraints = false, UILineBreakMode lineBreakMode = UILineBreakMode.WordWrap)
        {
            label.Text = string.Empty;
            SetLabel(label, fontName, fontSize, textColor);
            label.TranslatesAutoresizingMaskIntoConstraints = shouldTranslatesAutoResizingMaskIntoConstraints;
            label.LineBreakMode = lineBreakMode;
            label.Lines = lines;
        }
    }
}