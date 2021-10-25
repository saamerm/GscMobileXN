using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimServiceProvider
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("provinceCode")]
        public string ProvinceCode { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }
    }
}
