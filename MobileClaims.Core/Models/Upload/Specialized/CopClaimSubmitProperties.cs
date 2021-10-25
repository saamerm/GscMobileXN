using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopClaimSubmitProperties : IClaimSubmitProperties
    {
        public string Title => Resource.UploadDocuments;
        public string UploadDocumentType => UploadDocumentProcessType.COP;
        public bool IsCommentVisible => true;
    }
}