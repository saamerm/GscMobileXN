namespace MobileClaims.Core.Models.Upload.Specialized.PerType
{
    public class DrugClaimUploadProperties : INonRealTimeClaimUploadProperties
    {
        public string Title => Resource.GenericUploadDocuments;
        public string ActionSheetTitle => Resource.GenericUploadDocuments;
        public NonRealTimeClaimType ClaimType => NonRealTimeClaimType.Drug;
    }

    public class RealTimeClaimNonUploadProperties : INonRealTimeClaimUploadProperties
    {
        public string Title => Resource.GenericUploadDocuments;
        public string ActionSheetTitle => Resource.GenericUploadDocuments;
        public NonRealTimeClaimType ClaimType => NonRealTimeClaimType.NotDefined;
    }
}
