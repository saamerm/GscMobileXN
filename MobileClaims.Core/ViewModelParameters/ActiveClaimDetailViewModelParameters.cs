using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ActiveClaimDetailViewModelParameters
    {
        public TopCardViewData TopCardWithSummary { get; set; }
        public UploadDocumentsFormData CopPlanData { get; set; }
        public IClaimSummaryProperties ClaimSummary { get; set; }

        public ActiveClaimDetailViewModelParameters(TopCardViewData topCardWithSummary, UploadDocumentsFormData copPlanData, IClaimSummaryProperties claimSummary)
        {
            TopCardWithSummary = topCardWithSummary;
            CopPlanData = copPlanData;
            ClaimSummary = claimSummary;
        }
    }
}
