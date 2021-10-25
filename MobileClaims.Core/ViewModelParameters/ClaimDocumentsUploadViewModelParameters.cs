using MobileClaims.Core.Models.Upload;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ClaimDocumentsUploadViewModelParameters
    {
        public INonRealTimeClaimUploadProperties NonRealTimeClaimUploadProperties { get; set; }

        public ClaimDocumentsUploadViewModelParameters(INonRealTimeClaimUploadProperties nonRealTimeClaimUploadProperties)
        {
            NonRealTimeClaimUploadProperties = nonRealTimeClaimUploadProperties;
        }
    }
}
