namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditUploadCompleteProperties : IUploadCompleteProperties
    {
        public string Title => "#Audit".ToUpperInvariant();

        public string UploadDocuments => "#Upload documents for audit".ToUpperInvariant();

        public string UploadCompletedNote => "Please note, processing will occur in approximately 5 to 7 business days. Your claim status has been moved to 'Audit in progress'.";

        public string BackToMyClaimsText => "#Back to my alerts";
    }
}