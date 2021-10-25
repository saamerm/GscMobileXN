using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class LensType
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
