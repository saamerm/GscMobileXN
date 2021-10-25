namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopClaimUploadProperties : IClaimUploadProperties
    {
        public string Title => Resource.UploadDocuments;

        public string ActionSheetTitle => Resource.UploadDocuments;
    }
}