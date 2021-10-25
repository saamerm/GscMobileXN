using System.Collections.ObjectModel;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ConfirmationOfPaymentSubmitViewModelParameters
    {
        public TopCardViewData TopCardViewData { get; set; }
        public UploadDocumentsFormData PlanMemberData { get; set; }
        public IClaimSubmitProperties ClaimSubmitProperties { get; set; }
        public ObservableCollection<DocumentInfo> Attachments { get; set; }

        public ConfirmationOfPaymentSubmitViewModelParameters(TopCardViewData topCardViewData, UploadDocumentsFormData planMemberData, IClaimSubmitProperties claimSubmitProperties, ObservableCollection<DocumentInfo> attachments)
        {
            TopCardViewData = topCardViewData;
            PlanMemberData = planMemberData;
            ClaimSubmitProperties = claimSubmitProperties;
            Attachments = attachments;
        }
    }
}
