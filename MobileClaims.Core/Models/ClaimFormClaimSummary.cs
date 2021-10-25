
using MobileClaims.Core.Entities;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.Models
{
    public class ClaimFormClaimSummary : MvxNotifyPropertyChanged
    {
        public string ServiceDate => Resource.ServiceDate;
        public string ServiceDescription { get; set; } = Resource.ServiceDescription;
        public string ClaimedAmount { get; set; } = Resource.ClaimedAmount;
        public string OtherPaidAmount { get; set; } = Resource.OtherPaidAmount;
        public string PaidAmount { get; set; } = Resource.PaidAmount;
        public string Copay { get; set; } = Resource.CopayDeductible;
        public string ClaimStatus { get; set; } = Resource.ClaimStatus;

        public string CountValue { get; set; }
        public string ServiceDateValue { get; set; }
        public string ServiceDescriptionValue { get; set; }
        public string ClaimedAmountValue { get; set; }
        public string OtherPaidAmountValue { get; set; }
        public string PaidAmountValue { get; set; }
        public string CopayValue { get; set; }
        public string ClaimStatusValue { get; set; }
        public ClaimActionState ClaimActionState { get; set; }
    }
}