using System;
using System.Globalization;
using MobileClaims.Core.Entities;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class DocumentTypeToImageConverter : MvxValueConverter<DocumentType, UIImage>
    {
        protected override UIImage Convert(DocumentType value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageName = "DocumentThumbnail";
            switch (value)
            {
                case DocumentType.Image:
                    imageName = "ImageThumbnail";
                    break;
                case DocumentType.TextBasedDocument:
                    imageName = "DocumentThumbnail";
                    break;                
            }
            return UIImage.FromBundle(imageName);
        }
    }
}