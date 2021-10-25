using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Models.Upload.Specialized.PerType
{
    public class DrugClaimSubmissionProperties : IClaimSubmitProperties
    {
        public string Title => Resource.ClaimSubmissionConfirmationTitle;
        public string UploadDocumentType => UploadDocumentProcessType.NewClaimSubmission;
        public bool IsCommentVisible => true;
    }

    public class RealTimeClaimSubmissionProperties : IClaimSubmitProperties
    {
        public string Title => Resource.ClaimSubmissionConfirmationTitle;
        public string UploadDocumentType => string.Empty;
        public bool IsCommentVisible => false;
    }
}