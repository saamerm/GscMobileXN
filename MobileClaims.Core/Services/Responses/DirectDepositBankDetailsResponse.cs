using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Responses
{
    public class DirectDepositBankDetailsResponse
    {
        [JsonProperty("isValidBankInfo")]
        public bool ValidBankInfo { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("validationStatusCode")]
        public long ValidationStatusCode { get; set; }
    }
}