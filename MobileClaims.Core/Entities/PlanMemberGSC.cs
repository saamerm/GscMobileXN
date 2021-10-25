using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class PlanMemberGSC
    {
        [JsonProperty("planMemberId")]
        public long PlanMemberID { get; set; }

        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("planMemberFirstName")]
        public string FirstName { get; set; }

        [JsonProperty("planMemberLastName")]
        public string LastName { get; set; }

        [JsonProperty("planMemberStatus")]
        public string Status { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("participants")]
        public List<ParticipantGSC> Participants { get; set; }

        [JsonProperty("planConditions")]
        public PlanConditions PlanConditions { get; set; }

        [JsonProperty("planMemberBirthDate")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("planMemberProvinceCode")]
        public string ProviceCode { get; set; }
    }
}