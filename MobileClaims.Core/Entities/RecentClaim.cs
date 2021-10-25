using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class RecentClaim
    {
        [JsonProperty("claimFormId")]
        public long ClaimFormId { get; set; }

        [JsonProperty("claimDetailId")]
        public long ClaimDetailId { get; set; }

        [JsonProperty("serviceDescription")]
        public string ServiceDescription { get; set; }

        [JsonProperty("serviceDate")]
        public DateTime ServiceDate { get; set; }

        [JsonProperty("totalCdRendAmt")]
        public double totalCdRendAmt { get; set; }

        [JsonProperty("claimActionStatus")]
        public ClaimActionState ClaimActionStatus { get; set; }

        [JsonProperty("links")]
        public IList<string> Links { get; set; }
    }
}