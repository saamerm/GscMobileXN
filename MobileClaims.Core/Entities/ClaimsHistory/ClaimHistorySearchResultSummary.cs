namespace MobileClaims.Core.Entities.ClaimsHistory
{
    public class ClaimHistorySearchResultSummary
    {
        public string BenefitID { get; set; }
        public string BenefitName { get; set; }
        public int SearchResultCount { get; set; }
        public string SearchResultCountLabel
        {
            get
            {
                if (SearchResultCount == 1)
                    return string.Format("{0} {1}", SearchResultCount, Resource.Claim);
                else
                    return string.Format("{0} {1}", SearchResultCount, Resource.Claims);
            }
        }
    }
}
