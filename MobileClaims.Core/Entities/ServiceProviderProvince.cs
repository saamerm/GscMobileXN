using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ServiceProviderProvince
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
