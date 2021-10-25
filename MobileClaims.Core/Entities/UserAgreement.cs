using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class UserAgreement
    {
        [JsonProperty("agreementTypeId")]
        public string AgreementTypeId { get; set; }

        [JsonProperty("isAccepted")]
        public bool IsAccepted { get; set; }

        public UserAgreement()
        {
        }

        public UserAgreement(bool accepted)
        {
            AgreementTypeId = "C4L"; // documentation said this is the only valid value
            IsAccepted = accepted;
        }

        public UserAgreement(string agreementTypeId, bool isAccepted)
        {
            AgreementTypeId = agreementTypeId;
            IsAccepted = isAccepted;
        }
    }
}
