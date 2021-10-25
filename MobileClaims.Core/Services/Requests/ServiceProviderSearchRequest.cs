using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Requests
{
    public enum SortByChoice
    {
        SortByDistanceAsc = 1,
        SortByRatingDescAndDistanceAsc = 2,
        SortByProviderNamesAsc = 3,
        SortByRatingDescAndProviderNameAsc = 4
    }

    public enum SearchTypeChoice
    {
        PlanMembersFavoriteProviders = 1,
        ByProviderType = 2,
    }

    public enum ProvidersId
    {
        Favourites = 5,
        Pharmacy = 7
    }

    public class HealthProviderSearchCriteriaRequest
    {
        [JsonProperty("providerSearchQuery")]
        public string ProviderSearchQuery { get; set; }
        [JsonProperty("providerTypeCodes")]
        public string ProviderTypeCodes { get; set; }
        [JsonProperty("providerRating")]
        public double? ProviderRating { get; set; }
        [JsonProperty("distance")]
        public double Distance { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("sortByChoice")]
        public SortByChoice SortByChoice { get; set; }
        [JsonProperty("searchTypeChoice")]
        public SearchTypeChoice SearchTypeChoice { get; set; }
        [JsonProperty("startRowNumber")]
        public int StartRowNumber { get; set; }
        [JsonProperty("endRowNumber")]
        public int EndRowNumber { get; set; }
        [JsonProperty("recentProvidersInd")] 
        public bool RecentlyVisited { get; set; }
    }
}
