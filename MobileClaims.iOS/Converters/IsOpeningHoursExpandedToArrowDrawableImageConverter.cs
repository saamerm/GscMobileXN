using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class IsOpeningHoursExpandedToArrowDrawableImageConverter : MvxValueConverter<bool, UIImage>
    {
        protected override UIImage Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value)
            {
                return UIImage.FromBundle("ArrowUp");
            }
            else
            {
                return UIImage.FromBundle("ArrowDown");
            }
        }

        protected override bool ConvertBack(UIImage value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}