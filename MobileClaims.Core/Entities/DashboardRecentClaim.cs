using MvvmCross.ViewModels;

namespace MobileClaims.Core.Entities
{
    public enum ClaimActionState
    {
        None,
        Cop,
        Audit
    }

    public class DashboardRecentClaim : MvxNotifyPropertyChanged
    {
        public string ClaimFormId { get; set; }

        public string ClaimDetailId { get; set; }

        public string ServiceDescription { get; set; }

        public string ServiceDate { get; set; }

        public string ClaimedAmount { get; set; }

        public bool ActionRequired { get; set; }

        public ClaimActionState ClaimActionState { get; set; }
    }
}