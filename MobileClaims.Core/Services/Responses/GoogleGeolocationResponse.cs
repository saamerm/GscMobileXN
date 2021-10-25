using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Responses
{
    public class GoogleGeolocationResponse
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }
    }
}
