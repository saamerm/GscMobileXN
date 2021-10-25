using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Models.Upload.Specialized.PerType
{
    public class DrugClaimSubmissionCompletedProperties : IClaimCompletedProperties
    {
        public string Title => Resource.UploadedClaim;

        public string UploadSuccess => Resource.UploadSuccessful;

        public string UploadCompletedNote => string.Empty;
        
        public string BackToMyClaimsText => Resource.BackToMyClaims;

        public string BackToViewModelType => nameof(ChooseClaimOrHistoryViewModel);
    }

    public class RealTimeClaimSubmissionCompletedProperties : IClaimCompletedProperties
    {
        public string Title => Resource.claimResult_title;

        public string UploadSuccess => Resource.UploadSuccessful;

        public string UploadCompletedNote => string.Empty;

        public string BackToMyClaimsText => Resource.claimResult_button;

        public string BackToViewModelType => nameof(ClaimSubmissionTypeViewModel);
    }
}