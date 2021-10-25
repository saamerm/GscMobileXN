using System;
namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopDisclaimerProperties : IDisclaimerProperties
    {
        public string Title => string.Empty;

        public string FirstParagraph => BrandResource.DisclaimerFirstParagraph;

        public string SecondParagraph => BrandResource.DisclaimerSecondParagraph;

        public string ThirdParagraph => string.Empty;
    }
}
