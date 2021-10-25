using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Responses
{
    public class GoogleGeocodingResponse
    {
        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Result
    {
        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("bounds")]
        public Bounds Bounds { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("viewport")]
        public Bounds Viewport { get; set; }
    }

    public class Bounds
    {
        [JsonProperty("northeast")]
        public Location Northeast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }
}
