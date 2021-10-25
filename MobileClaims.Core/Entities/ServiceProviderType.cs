using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ServiceProviderType
    {
        [JsonProperty("id")]
        public string ID
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Type
        {
            get;
            set;
        }
    }
}
