using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class EligibilityProvince
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
    }
}
