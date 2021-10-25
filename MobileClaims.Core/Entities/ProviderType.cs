using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ProviderType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("providerTypeCodes")]
        public string ProviderTypeCodes { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        [JsonProperty("lineOfBusinessCode")]
        public string LineOfBusinessCode { get; set; }

        [JsonProperty("links")]
        public string[] Links { get; set; }

        [JsonProperty("displayRating")]
        public string DisplayRating { get; set; }

        [JsonProperty("isSearchable")]
        public bool IsSearchable { get; set; }
    }
}
