namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopUploadSubmitProperties : IUploadSubmitProperties
    {
        public string Title => Resource.ClaimSummaryTitle.ToUpperInvariant();

        public string UploadDocuments => Resource.UploadDocuments;

        public bool IsCommentVisible => true;
    }
}