using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClientValidationResponse
    {
        [JsonProperty("error")]
        public string ErrorMessage { get; set; }
    }

    public class CCQUnauthorizedUserResponse
    {
        [JsonProperty("message_en")]
        public string MessageEn { get; set; }

        [JsonProperty("message_fr")]
        public string MessageFr { get; set; }
    }
}