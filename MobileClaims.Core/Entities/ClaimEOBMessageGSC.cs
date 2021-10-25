using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimEOBMessageGSC
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
