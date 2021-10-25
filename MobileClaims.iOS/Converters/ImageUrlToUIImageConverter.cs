using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class ImageUrlToUIImageConverter : MvxValueConverter<string, UIImage>
    {
        protected override UIImage Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return UIImage.FromBundle(value);
        }
    }
}