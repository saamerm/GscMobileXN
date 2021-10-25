using MvvmCross.ViewModels;

namespace MobileClaims.Core.Entities
{
    public class MyAlertsAuditClaim : MvxNotifyPropertyChanged
    {
        public string ClaimFormId { get; set; }

        public string ClaimSubmissionDate { get; set; }

        public string ClaimDueDate { get; set; }

        public string ServiceDescription { get; set; }

        public ClaimActionState ClaimActionState { get; set; }
    }
}