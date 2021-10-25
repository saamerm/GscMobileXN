using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Responses
{
    public class DirectDepositResponse
    {
        [JsonProperty("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty("eftEmailIndicator")]
        public string EftEmailIndicator { get; set; }

        [JsonProperty("transitNumber")]
        public string TransitNumber { get; set; }

        [JsonProperty("bankNumber")]
        public string BankNumber { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("bankFullName")]
        public string BankFullName { get; set; }

        [JsonProperty("eftThruEeInd")]
        public string EftThruEeInd { get; set; }

        [JsonProperty("links")]
        public object[] Links { get; set; }
    }
}