using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimCOP
    {
        [JsonProperty("claimFormId")]
        public long ClaimFormID { get; set; }

        [JsonProperty("benefitType_Descr")]
        public string BenefitTypeDescr { get; set; }

        [JsonProperty("minServDate")]
        public DateTime MinServeDate { get; set; }

        [JsonProperty("maxServDate")]
        public DateTime MaxServeDate { get; set; }

        [JsonProperty("totalCdRendAmt")]
        public double? TotalCdRendAmt { get; set; }

        [JsonProperty("links")]
        public IList<string> Links { get; set; }

        [JsonProperty("isCOP")]
        public bool IsCop { get; set; }

        [JsonProperty("isSelectedForAudit")]
        public bool IsSelectedForAudit { get; set; }
    }
}
