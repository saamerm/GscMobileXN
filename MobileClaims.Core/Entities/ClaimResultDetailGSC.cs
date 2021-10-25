using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileClaims.Core.Entities
{
    public class ClaimResultDetailGSC
    {
        [JsonProperty("claimId")]
        public long ClaimID { get; set; }

        [JsonProperty("serviceDate")]
        public DateTime ServiceDate { get; set; }

        [JsonProperty("serviceDescription")]
        public string ServiceDescription { get; set; }

        [JsonProperty("claimedAmount")]
        public double ClaimedAmount { get; set; }

        [JsonProperty("otherPaidAmount")]
        public double OtherPaidAmount { get; set; }

        [JsonProperty("deductibleAmount")]
        public double DeductibleAmount { get; set; }

        [JsonProperty("copayAmount")]
        public double CopayAmount { get; set; }

        [JsonProperty("paidAmount")]
        public double PaidAmount { get; set; }

        [JsonProperty("claimStatus")]
        public string ClaimStatus { get; set; }

        [JsonProperty("eobMessages")]
        public List<ClaimEOBMessageGSC> EOBMessages { get; set; }

        [JsonProperty("isCOP")]
        public bool RequiresConfirmationOfPayment { get; set; }

        [JsonProperty("toothCode")]
        public string ToothCode { get; set; }

        [JsonIgnore]
        public string EOBMessagesText
        {
            get
            {
                return string.Join("\n\n", EOBMessages.Select(m => m.Message));
            }
        }

        public long ClaimFormID { get; set; }

        public bool HasHCSADetails { get; set; }

        [JsonIgnore]
        public string ParticipantLabel => Resource.claimConfirm_participant;

        [JsonIgnore]
        public string IDNumberLabel => BrandResource.claimConfirm_id_number;

        [JsonIgnore]
        public string RefferedLabel => Resource.claimConfirm_reffered;

        [JsonIgnore]
        public string ClaimedAmountLabel => Resource.ClaimedAmount;

        [JsonIgnore]
        public string PaidAmountLabel => Resource.PaidAmount;

        [JsonIgnore]
        public string OtherPaidLabel => Resource.OtherPaidAmount;

        [JsonIgnore]
        public string DateOfExpenseLabel => Resource.DateOfExpense;

        [JsonIgnore]
        public string GovernmentPlanLabel => Resource.OtherPaidAmount;

        [JsonIgnore]
        public string TypeExpenseLabel => Resource.claimConfirm_type_expense;

        [JsonIgnore]
        public string ClaimStatusLabel => Resource.ClaimStatus;

        [JsonIgnore]
        public string AwaitingPaymentLabel => Resource.AwaitingPayment;

        public string FormNumberLabel => Resource.ClaimFormNumber;

        public string EOBLabel => Resource.ExplanationOfBenefits;
    }
}