namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopClaimPropertiesBase : IClaimPropertiesBase
    {
        public string Title => Resource.ClaimSummaryTitle.ToUpperInvariant();
    }
}