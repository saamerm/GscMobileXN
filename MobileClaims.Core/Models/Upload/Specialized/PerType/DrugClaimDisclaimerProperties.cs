using System;

namespace MobileClaims.Core.Models.Upload.Specialized.PerType
{
    public class DrugClaimDisclaimerProperties : IDisclaimerProperties
    {
        public string Title => Resource.SubmitClaim;
        public string FirstParagraph => BrandResource.DisclaimerFirstParagraph;
        public string SecondParagraph => BrandResource.DisclaimerSecondParagraph;
        public string ThirdParagraph => string.Empty;
    }
}