using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ServiceProvider
    {
        private int _id = -1;
        [JsonProperty("id")]
        public int ID
        { 
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [JsonProperty("providerTypeCode")]
        public string ProviderTypeCode { get; set; }

        [JsonProperty("name")]
        public string BusinessName { get; set; }

        [JsonIgnore]
        public string DoctorName
        {
            get
            {
                return BusinessName;
            }
            private set
            {
                //do nothing
            }
        }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("address2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("provinceCode")]
        public string Province { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("canSubmitClaimsOnline")]
        public bool CanSubmitClaimsOnline { get; set; }

        [JsonProperty("canAcceptPaymentDirectly")]
        public bool CanAcceptPaymentDirectly { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        public int ProviderTypeID { get; set; }

        public string RegistrationNumber { get; set; }

        [JsonIgnore]
        public string FormattedAddress
        {
            get
            {
                string address = "";
                address += !string.IsNullOrEmpty(City) ? City  : "";
                address += !string.IsNullOrEmpty(Province) ? ", " + Province  : "";
                address += !string.IsNullOrEmpty(PostalCode) ? ", " + PostalCode : "";
                return address;
            }
        }       

        [JsonIgnore]
        public string FormattedCityProv
        {
            get
            {
                string address = "";
                address += !string.IsNullOrEmpty(City) ? City : "";
                address += !string.IsNullOrEmpty(Province) ? ", " + Province : "";
                return address;
            }
        }

        [JsonIgnore]
        public string ProviderNameAndContactInfo
        {
            get
            {
                return $"{DoctorName}{System.Environment.NewLine}{Address}{System.Environment.NewLine}{Phone}";
            }
        }
    }
}
