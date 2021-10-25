using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class HealthProviderSummary
    {
        [JsonProperty("providerId")]
        public int ProviderId { get; set; }
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }
        [JsonProperty("providerTradingName")]
        public string ProviderTradingName { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("overallScore")]
        public double OverallScore { get; set; }
        [JsonProperty("typeCode")]
        public string TypeCode { get; set; }
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("googlePlaceData")]
        public GooglePlaceData GooglePlaceData { get; set; }
        [JsonProperty("distance")]
        public float Distance { get; set; }
        [JsonProperty("dbAdherenceInd")]
        public string DbAdherenceInd { get; set; }
        [JsonProperty("rowNumber")]
        public int RowNumber { get; set; }
        [JsonProperty("participatingStatusCode")]
        public string ParticipatingStatusCode { get; set; }
        [JsonProperty("lineOfBusinessCode")]
        public string LineOfBusinessCode { get; set; }
        [JsonProperty("favouriteProviderIndicator")]
        public string FavouriteProviderIndicator { get; set; }
        [JsonProperty("providerTypeLabel")]
        public string ProviderTypeLabel { get; set; }
        [JsonProperty("links")]
        public string[] Links { get; set; }
        [JsonProperty("operatingHoursLabel")]
        public string OperatingHoursLabel { get; set; }
    }
}
