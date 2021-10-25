namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditUploadSubmitProperties : IUploadSubmitProperties
    {
        public string Title => "#Audit".ToUpperInvariant();

        public string UploadDocuments => "#Upload documents for audit".ToUpperInvariant();

        public bool IsCommentVisible => false;
    }
}