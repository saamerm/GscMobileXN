using System;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class DrugEligibility
    {
        [JsonProperty("id")]
        public int ID
        {
            get;
            set;
        }

        [JsonProperty("drugId")]
        public int DrugID
        {
            get;
            set;
        }

        [JsonProperty("din")]
        public int DIN
        {
            get;
            set;
        }

        [JsonProperty("drugName")]
        public string DrugName
        {
            get;
            set;
        }

        [JsonProperty("accumMessage")]
        public string AccumMessage
        {
            get;
            set;
        }

        [JsonProperty("coverageMessage")]
        public string CoverageMessage
        {
            get;
            set;
        }

        [JsonProperty("requiresSpecialAuthorization")]
        public bool RequiresSpecialAuthorization
        {
            get;
            set;
        }

        [JsonProperty("specialAuthorizationFormUri")]
        public string SpecialAuthorizationFormUri
        {
            get;
            set;
        }

        [JsonProperty("authorizationMessage")]
        public string AuthorizationMessage
        {
            get;
            set;
        }

        [JsonProperty("lowCostReplacementOccurred")]
        public bool LowCostReplacementOccurred
        {
            get;
            set;
        }

        [JsonProperty("planMemberId")]
        public long PlanMemberID
        {
            get;
            set;
        }

        [JsonProperty("participantNumber")]
        public string ParticipantNumber
        {
            get;
            set;
        }

        [JsonProperty("participantFirstName")]
        public string ParticipantFirstName
        {
            get;
            set;
        }

        [JsonProperty("participantLastName")]
        public string ParticipantLastName
        {
            get;
            set;
        }

        [JsonProperty("timestamp")]
        public DateTime Timestamp
        {
            get;
            set;
        }
    }
}
