using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class GooglePlaceData
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }

    public class OpeningHours
    {
        [JsonProperty("periods")]
        public Period[] Periods { get; set; }
        [JsonProperty("weekday_text")]
        public string[] WeekdayText { get; set; }
    }

    public class Period
    {
        [JsonProperty("open")]
        public DayOpeningClosingTime OpeningTime { get; set; }
        [JsonProperty("close")]
        public DayOpeningClosingTime ClosingTime { get; set; }
    }

    public class DayOpeningClosingTime
    {
        [JsonProperty("day")]
        public int Day { get; set; }
        [JsonProperty("hours")]
        public int Hours { get; set; }
        [JsonProperty("minutes")]
        public int Minutes { get; set; }
    }
}
