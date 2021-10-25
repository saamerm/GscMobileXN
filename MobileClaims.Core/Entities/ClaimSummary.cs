using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimSummary : ClaimCOP
    {
        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("participantName")]
        public string ParticipantName { get; set; }

        [JsonProperty("eobMessages")]
        public IList<ClaimEOBMessageGSC> EOBMessages  { get; set; }

        [JsonProperty("totalPaidAmt")]
        public double? TotalPaidAmt { get; set; }

        [JsonProperty("totalCoPayAmt")]
        public double? TotalCoPayAmt { get; set; }

        [JsonProperty("payeeType")]
        public string PayeeType { get; set; }

        [JsonProperty("paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonProperty("otherPaidAmt")]
        public double? OtherPaidAmt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}