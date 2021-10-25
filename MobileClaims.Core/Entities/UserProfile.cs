using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class UserProfile
    {
        [JsonProperty("planMemberId")]
        public int PlanMemberIDWithoutParticipantCode { get; set; }
        
        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }
    }
}
