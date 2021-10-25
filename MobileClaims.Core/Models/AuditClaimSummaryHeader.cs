
using MvvmCross.ViewModels;

namespace MobileClaims.Core.Models
{
    public class AuditClaimSummaryHeader : MvxNotifyPropertyChanged
    {
        public string PlanInformationLabel { get; set; } = Resource.PlanInformation;

        public string GscNumberLabel { get; set; } = BrandResource.GreenShieldIDNumber;

        public string GscNumber { get; set; }

        public string ClaimFormNumberLabel { get; set; } = Resource.ClaimFormNumber;

        public string ClaimFormNumber { get; set; }

        public string SubmissionDateLabel { get; set; } = Resource.SubmissionDateLabel;

        public string SubmissionDate { get; set; }
    }
}