using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class EligibilityBenefit
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
