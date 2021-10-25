using System;
using System.Globalization;
using CoreGraphics;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class ImageFilePathToUIImageViewConverter : MvxValueConverter<string, UIImage>
    {
        protected override UIImage Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = UIImage.FromFile(value);
            if (!Constants.IsPhone())
            {
                return image;
            }

            var targetSize = CalculateRequireSize(parameter, image);

            var targetWidth = targetSize.Width;
            var targetHeight = targetSize.Height;

            var radian = 90 * (float)Math.PI / 180;
            var view = new UIView(new CGRect(0, 0, image.Size.Width, image.Size.Height));
            var transform = CGAffineTransform.MakeRotation(radian);
            view.Transform = transform;
            var size = view.Frame.Size;

            UIGraphics.BeginImageContext(size);
            var context = UIGraphics.GetCurrentContext();
            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(radian);
            context.ScaleCTM(1, -1);
            context.DrawImage(new CGRect(-image.Size.Width / 2, -image.Size.Height / 2, image.Size.Width, image.Size.Height), image.CGImage);

            var duplicateImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            UIGraphics.BeginImageContext(targetSize);
            var newContext = UIGraphics.GetCurrentContext();
            CGRect cg = new CGRect(0, 0, targetWidth, targetHeight);
            var newImage = duplicateImage.Scale(targetSize, UIScreen.MainScreen.Scale);
            UIGraphics.EndImageContext();

            return newImage;
        }

        private CGSize CalculateRequireSize(object parameter, UIImage image)
        {
            var imageWidth = image.Size.Height;
            var imageHeight = image.Size.Width;
            var viewWidth = UIScreen.MainScreen.Bounds.Width;
            nfloat availableHeight = (nfloat)parameter;

            float percentagePadding = 0.10f;
            float _requiredWidth, _requiredHeight;
            do
            {
                _requiredWidth = (float)(viewWidth - 2 * (viewWidth * percentagePadding));
                _requiredHeight = (float)(imageHeight * _requiredWidth / imageWidth);
                percentagePadding = percentagePadding + 0.01f;
            }
            while (_requiredHeight > availableHeight);

            return new CGSize(_requiredWidth, _requiredHeight);
        }
    }
}