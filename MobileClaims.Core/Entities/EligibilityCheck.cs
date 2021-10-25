using Newtonsoft.Json;
using System;

namespace MobileClaims.Core.Entities
{
    public class EligibilityCheck
    {
        [JsonProperty("planMemberId")]
        public long PlanMemberID { get; set; }

        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("eligibilityCheckTypeId")]
        public string EligibilityCheckTypeID { get; set; }

        [JsonProperty("benefitCategoryId")]
        public string BenefitCategoryID { get; set; }

        [JsonProperty("lengthOfTreatment")]
        public string LengthOfTreatment { get; set; }

        [JsonProperty("treatmentDate")]
        public string TreatmentDate { get; set; }

        [JsonIgnore]
        public DateTime TreatmentDateAsDateTime
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(TreatmentDate, out dt);
                return dt;
            }
        }

        [JsonProperty("claimAmount")]
        public double ClaimAmount { get; set; }

        [JsonProperty("provinceCode")]
        public string ProvinceCode { get; set; }

        [JsonProperty("lensTypeCode")]
        public string LensTypeCode { get; set; }

        [JsonProperty("result")]
        public EligibilityCheckResult Result { get; set; }

        public EligibilityCheckType EligibilityCheckType { get; set; }

        public EligibilityBenefit TypeOfTreatment { get; set; }

        public EligibilityProvince Province { get; set; }

        public EligibilityOption LengthOfTreatmentFull { get; set; }

        public EligibilityOption LensType { get; set; }

        public bool IsLensTypeVisible => (EligibilityCheckTypeID == "GLASSES");
    }
}