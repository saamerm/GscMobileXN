namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class NonActionClaimSummaryProperties : IClaimSummaryProperties
    {
        public string Title => Resource.ClaimSummaryTitle.ToUpperInvariant();
        public bool IsUploadSectionVisible => false;
        public string UploadButtonText => string.Empty;
    }
}