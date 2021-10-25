namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopClaimSummaryProperties : IClaimSummaryProperties
    {
        public string Title => Resource.ClaimSummaryTitle.ToUpperInvariant();
        public bool IsUploadSectionVisible => true;
        public string UploadButtonText => Resource.UploadDocuments;
    }
}