using System;
using System.Globalization;
using Foundation;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class StringToAttributedStringConverter : MvxValueConverter<string, NSMutableAttributedString>
    {
        protected override NSMutableAttributedString Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new NSMutableAttributedString();
            var attributeString = new NSMutableAttributedString(value);
            attributeString.SetAttributes(parameter as UIStringAttributes, new NSRange(0, value.Length));
            attributeString.AddAttribute(new NSString("NSParagraphStyleAttributeName"),
                (parameter as UIStringAttributes).ParagraphStyle,
                new NSRange(0, value.Length));
            return attributeString;
        }
    }
}