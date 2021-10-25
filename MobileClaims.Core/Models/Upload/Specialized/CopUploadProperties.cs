namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopUploadProperties : IUploadProperties
    {
        public string Title => Resource.ClaimSummaryTitle.ToUpperInvariant();

        public string UploadDocuments => Resource.UploadDocuments;
    }
}