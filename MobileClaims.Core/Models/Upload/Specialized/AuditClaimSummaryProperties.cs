namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditClaimSummaryProperties : IClaimSummaryProperties
    {
        public string Title => Resource.AuditSummaryPageTitle;
        public bool IsUploadSectionVisible => true;
        public string UploadButtonText => Resource.GenericUploadDocuments;
    }
}