using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimResultResponse : ClaimResultGSC
    {
        [JsonProperty("claimActionStatus")]
        public ClaimActionState ClaimActionStatus { get; set; }

        [JsonProperty("auditDueDate")]
        public DateTime AuditDueDate { get; set; }

        [JsonProperty("actionReqdEobMessages")]
        public IList<ClaimEOBMessageGSC> ActionRequiredEobMessages { get; set; }
    }
}
