using MobileClaims.Core.Services.Requests;

namespace MobileClaims.Core.Models
{
    public class HealthProviderSearchParameters
    {
        public double Distance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public SortByChoice SortByChoice { get; set; }
        public int PageIndex { get; set; }
        public double? ProviderRating { get; set; }
        public string SearchQuery { get; set; }
        public bool IsDirectBill { get; set; }
        public SearchTypeChoice SearchTypeChoice { get; set; }
        public string ProviderTypeCodes { get; set; }
        public int ProviderTypeId { get; set; }
        public bool IsRecentlyVisited { get; set; }
    }
}
