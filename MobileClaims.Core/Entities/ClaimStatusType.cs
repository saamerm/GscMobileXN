using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimStatusType
    {
        [JsonProperty("claimType")]
        public string ClaimType { get; set; }
    }
}
