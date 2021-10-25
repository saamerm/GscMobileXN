
using MvvmCross.ViewModels;

namespace MobileClaims.Core.Models
{
    public class AuditClaimSummaryFooter : MvxNotifyPropertyChanged
    {
        public string AuditDetailsLabel => Resource.AuditDetails;
        public string AuditDueDateLabel => Resource.AuditDueDateLabel;
        public string AuditDueDate { get; set; }
        public string AuditInformationLabel => Resource.AuditInformation;
        public string AuditInformation { get; set; }
        public string ClaimStatusLabel { get; set; } = Resource.ClaimStatus;
        public string ClaimStatusValue { get; set; } = Resource.AuditClaimStatus;
    }
}