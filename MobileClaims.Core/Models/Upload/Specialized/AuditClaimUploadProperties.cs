using System.Resources;

namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditClaimUploadProperties : IClaimUploadProperties
    {
        public string Title => Resource.GenericUploadDocuments;

        public string ActionSheetTitle => Resource.GenericUploadDocuments;
    }
}