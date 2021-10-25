using Newtonsoft.Json;
using System.Collections.Generic;

namespace MobileClaims.Core.Entities
{
    public class ClaimGSC
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("planMemberId")]
        public long PlanMemberID { get; set; }

        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("claimSubmissionTypeId")]
        public string ClaimSubmissionTypeID { get; set; }

        [JsonProperty("serviceProvider")]
        public ServiceProvider ServiceProvider { get; set; }

        [JsonProperty("isCoveredUnderAnotherPlan")]
        public bool IsCoveredUnderAnotherPlan { get; set; }

        [JsonProperty("isCoveredUnderAnotherGscPlan")]
        public bool IsCoveredUnderAnotherGSCPlan { get; set; }

        [JsonProperty("isSubmittedToOtherPlan")]
        public bool IsSubmittedToOtherPlan { get; set; }

        [JsonProperty("payUnpaidAmountThruOtherGscPlan")]
        public bool PayUnpaidAmountThroughOtherGSCPlan { get; set; }

        [JsonProperty("otherGscPlanMemberId")]
        public string OtherGSCPlanMemberID { get; set; }

        [JsonProperty("otherGscParticipantNumber")]
        public string OtherGSCParticipantNumber { get; set; }

        [JsonProperty("useSpendingAccountAutoCoordination")]
        public bool UseSpendingAccountAutoCoordination { get; set; }

        [JsonProperty("isRequiredDueToMotorVehicleAccident")]
        public bool IsRequiredDueToMotorVehicleAccident { get; set; }

        [JsonProperty("motorVehicleAccidentDate")]
        public string MotorVehicleAccidentDate { get; set; }

        [JsonProperty("isRequiredDueToWorkInjury")]
        public bool IsRequiredDueToWorkInjury { get; set; }

        [JsonProperty("workRelatedInjuryDate")]
        public string WorkRelatedInjuryDate { get; set; }

        [JsonProperty("workRelatedInjuryCaseNumber")]
        public int WorkRelatedInjuryCaseNumber { get; set; }

        [JsonProperty("isRequiredForSport")]
        public bool IsRequiredForSport { get; set; }

        [JsonProperty("isReferralSubmitted")]
        public bool IsReferralSubmitted { get; set; }

        [JsonProperty("referralDate")]
        public string ReferralDate { get; set; }

        [JsonProperty("medicalProfessionalId")]
        public string MedicalProfessionalId { get; set; }

        [JsonProperty("isGstIncluded")]
        public bool IsGSTIncluded { get; set; }

        [JsonProperty("details")]
        public List<ClaimDetailGSC> Details { get; set; }

        [JsonProperty("results")]
        public List<ClaimResultGSC> Results { get; set; }

        [JsonProperty("validationStatusCode")]
        public int ValidationStatusCode { get; set; }

        [JsonProperty("participants")]
        public List<ParticipantGSC> Participants { get; set; }

        [JsonProperty("isRequiredDueToDentalAccident")]
        public bool IsRequiredDueToDentalAccident { get; set; }

        public string HCSAQuestion
        {
            get { return Resource.HCSAQuestion; }
        }
    }
}
