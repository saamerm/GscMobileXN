using System;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditClaimSubmitProperties : IClaimSubmitProperties
    {
        public string Title => Resource.GenericUploadDocuments;
        public string UploadDocumentType => UploadDocumentProcessType.Audit;
        public bool IsCommentVisible => false;
    }
}