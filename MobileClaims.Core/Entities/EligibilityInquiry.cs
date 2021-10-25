using Newtonsoft.Json;
using System.Collections.Generic;

namespace MobileClaims.Core.Entities
{
    public class EligibilityInquiry
    {
        [JsonProperty("planMemberId")]
        public long PlanMemberID { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("participantEligibilityResults")]
        public List<ParticipantEligibilityResult> ParticipantEligibilityResults { get; set; }
    }
}
