using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class EmailRequest
    {
        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }

        [JsonProperty("senderAddress")]
        public string SenderAddress { get; set; }

        [JsonProperty("senderName")]
        public string SenderName { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
