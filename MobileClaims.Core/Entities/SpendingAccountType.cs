using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class SpendingAccountType
    {
        [JsonProperty("modelId")]
        public string ModelID{ get; set; }

        [JsonProperty("name")]
        public string ModelName { get; set; }
    }
}
