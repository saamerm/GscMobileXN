using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class IpLocationData
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
