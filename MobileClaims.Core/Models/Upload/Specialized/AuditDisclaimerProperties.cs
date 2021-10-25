using System;

namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditDisclaimerProperties : IDisclaimerProperties
    {
        public string Title => string.Empty;

        public string FirstParagraph => BrandResource.AuditDisclaimerFirstParagraph;

        public string SecondParagraph => BrandResource.AuditDisclaimerSecondParagraph;

        public string ThirdParagraph => BrandResource.AuditDisclaimerThirdParagraph;
    }
}
