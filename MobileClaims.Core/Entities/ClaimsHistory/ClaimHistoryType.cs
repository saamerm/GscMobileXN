using Newtonsoft.Json;

namespace MobileClaims.Core.Entities.ClaimsHistory
{
    public class ClaimHistoryType
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("earliestYear")]
        public int EarliestYear { get; set; }
    }
}
