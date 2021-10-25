using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Requests
{
    public class GoogleGeolocationRequest
    {
        public GoogleGeolocationRequest()
        {
            ConsiderIp = bool.TrueString;
            Carrier = string.Empty;
            CellTowers = new List<object>();
            WifiAccessPoints = new List<object>();
            RadioType = "gsm";
        }

        [JsonProperty("homeMobileCountryCode")]
        public int HomeMobileCountryCode { get; set; }

        [JsonProperty("homeMobileNetworkCode")]
        public int HomeMobileNetworkCode { get; set; }

        [JsonProperty("radioType")]
        public string RadioType { get; set; }

        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        [JsonProperty("considerIp")]
        public string ConsiderIp { get; set; }

        [JsonProperty("cellTowers")]
        public List<object> CellTowers { get; set; }

        [JsonProperty("wifiAccessPoints")]
        public List<object> WifiAccessPoints { get; set; }

    }

}
