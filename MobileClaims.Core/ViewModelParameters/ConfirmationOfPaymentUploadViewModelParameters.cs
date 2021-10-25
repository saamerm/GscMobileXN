using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ConfirmationOfPaymentUploadViewModelParameters: IViewModelParameters
    {
        public TopCardViewData TopCardViewData { get; set; }
        public UploadDocumentsFormData PlanMemberData { get; set; }
        public IClaimUploadProperties ClaimUploadProperties { get; set; }
        public NavigationCatalog NavigationCatalog { get; set; }

        public ConfirmationOfPaymentUploadViewModelParameters(TopCardViewData topCardViewData, UploadDocumentsFormData planMemberData, IClaimUploadProperties claimUploadProperties)
        {
            TopCardViewData = topCardViewData;
            PlanMemberData = planMemberData;
            ClaimUploadProperties = claimUploadProperties;
        }
    }
}
