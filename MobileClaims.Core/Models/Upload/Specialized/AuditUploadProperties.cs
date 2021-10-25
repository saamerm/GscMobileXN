namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditUploadProperties : IUploadProperties
    {
        public string Title => "#Audit".ToUpperInvariant();

        public string UploadDocuments => "#Upload documents for audit".ToUpperInvariant();
    }
}