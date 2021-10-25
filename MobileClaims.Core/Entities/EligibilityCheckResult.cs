using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobileClaims.Core.Entities
{
    public class EligibilityCheckResult
    {
        [JsonProperty("planMemberDisplayId")]
        public string PlanMemberDisplayID { get; set; }

        [JsonProperty("participantFullName")]
        public string ParticipantFullName { get; set; }

        [JsonProperty("serviceDescription")]
        public string ServiceDescription { get; set; }

        [JsonProperty("submissionDate")]
        public DateTime SubmissionDate { get; set; }

        [JsonProperty("claimedAmount")]
        public double ClaimedAmount { get; set; }

        [JsonProperty("deductibleAmount")]
        public double DeductibleAmount { get; set; }

        [JsonProperty("copayAmount")]
        public double CopayAmount { get; set; }

        [JsonProperty("paidAmount")]
        public double PaidAmount { get; set; }

        [JsonProperty("rxChangedPaidAmount")]
        public double RxChangedPaidAmount { get; set; }

        [JsonProperty("paidAmountIndicator")]
        public int PaidAmountIndicator { get; set; }

        [JsonProperty("deductionMessages")]
        public List<string> DeductionMessages { get; set; }

        [JsonProperty("eligibilityDate")]
        public string EligibilityDateAsString { get; set; }

        public DateTime EligibilityDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(EligibilityDateAsString, out dt);
                return dt;
            }
        }

        [JsonProperty("eligibilityDateIndicator")]
        public bool EligibilityDateIndicator { get; set; }

        [JsonProperty("eligibilityNote")]
        public string EligibilityNote { get; set; }

        [JsonProperty("planLimitations")]
        public List<ClaimPlanLimitationGSC> PlanLimitations { get; set; }

        [JsonProperty("participantEligibilityResults")]
        public List<ParticipantEligibilityResult> ParticipantEligibilityResults { get; set; }

        public bool IsPlanLimitationsVisible => (PlanLimitations.Count > 0);
    }
}
