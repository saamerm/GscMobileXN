namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopUploadCompleteProperties : IUploadCompleteProperties
    {
        public string Title => Resource.SubmissionConfirmation;

        public string UploadDocuments => Resource.UploadDocuments;

        public string UploadCompletedNote => string.Empty;

        public string BackToMyClaimsText => Resource.BackToMyClaims;
    }
}