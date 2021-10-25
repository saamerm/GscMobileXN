using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Responses
{
    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}
