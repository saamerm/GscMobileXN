using System;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimAudit
    {
        [JsonProperty("claimFormId")]
        public string ClaimFormID { get; set; }

        [JsonProperty("submissionDate")]
        public DateTime SubmissionDate { get; set; }

        [JsonProperty("dueDate")]
        public DateTime DueDate { get; set; }
        
        [JsonProperty("benefitType_Descr")]
        public string BenefitTypeDescr { get; set; }
    }
}