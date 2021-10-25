using Newtonsoft.Json;
using System;

namespace MobileClaims.Core.Entities
{
    public class ClaimPlanLimitationGSC
    {
        [JsonProperty("benefitDescription")]
        public string BenefitDescription { get; set; }

        [JsonProperty("limitationDescription")]
        public string LimitationDescription { get; set; }

        [JsonProperty("appliesTo")]
        public string AppliesTo { get; set; }

        [JsonProperty("accumStartDate")]
        public DateTime AccumStartDate { get; set; }

        [JsonProperty("accumAmountUsed")]
        public double AccumAmountUsed { get; set; }

        [JsonProperty("accumUnitsUsed")]
        public double AccumUnitsUsed { get; set; }

        [JsonIgnore]
        public bool IsAccumAmountUsedVisible
        {
            get
            {
                bool ret = false;
                if ((AccumAmountUsed == 0) && (AccumUnitsUsed == 0))
                {
                    ret = true;
                }
                else
                {
                    if (AccumAmountUsed > 0)
                    {
                        ret = true;
                    }
                }
                return ret;
            }
        }

        [JsonIgnore]
        public bool IsAccumUnitsUsedVisible
        {
            get
            {
                bool ret = false;
                if ((AccumAmountUsed == 0) && (AccumUnitsUsed == 0))
                {
                    ret = true;
                }
                else
                {                    
                    if (AccumUnitsUsed > 0)
                    {
                        ret = true;
                    }
                }
                return ret;
            }
        }

        [JsonIgnore]
        public bool IsParticipantFamilyLabelVisibleForEligibility => !string.IsNullOrEmpty(AppliesTo);

        [JsonIgnore]
        public bool IsAccumAmountUsedVisibleForEligibility => AccumAmountUsed >= 0;

        [JsonIgnore]
        public bool IsAccumUnitsUsedVisibleForEligibility => AccumUnitsUsed >= 0;
    }
}
