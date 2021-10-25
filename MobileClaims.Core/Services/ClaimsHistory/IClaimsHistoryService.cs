using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Entities.ClaimsHistory;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services.ClaimsHistory
{
    public interface IClaimsHistoryService
    {
        List<ClaimHistoryType> ClaimHistoryTypes { get; }
        List<ClaimHistoryPayee> ClaimHistoryPayees { get; }
        List<ClaimHistoryBenefit> ClaimHistoryBenefits { get; }
        string AllClaimHistoryBenefits { get; }
        List<ClaimState> ClaimsHistorySearchResults { get; }
        List<ClaimState> DentalSearchResults { get; }
        List<ClaimState> DrugSearchResults { get; }
        List<ClaimState> EHSSearchResults { get; }
        List<ClaimState> HCSASearchResults { get; }
        List<ClaimState> NonHealthSearchResults { get; }
        List<ClaimHistorySearchResultSummary> SearchResultsSummary { get; }
        ClaimHistorySearchResultSummary SelectedSearchResultType { get; set; }
        Participant SelectedParticipant { get; set; }
        ClaimState SelectedSearchResult { get; set; }
        ClaimHistoryType SelectedClaimHistoryType { get; set; }
        KeyValuePair<string, string> SelectedDisplayBy { get; set; }
        DateTime SelectedStartDate { get; set; }
        DateTime SelectedEndDate { get; set; }
        int SelectedYear { get; set; }
        DateTime DateOfInquiry { get; }
        string Period { get; }
        bool IsRightSideGreyedOutMaster { get; }
        bool IsSearchRightSideGreyedOut { get; set; }
        bool IsResultsCountSearchResultsSummarySelected { get; set; }
        List<ClaimHistoryBenefit> SelectedClaimHistoryBenefits { get; set; }
        bool IsAllBenefitsSelected { get; set; }
        string LinesOfBusiness { get; }
        ClaimHistoryPayee SelectedClaimHistoryPayee { get; set; }

        Task GetClaimHistoryTypes(Action success, Action<string, int> failure);
        Task GetClaimHistoryPayees(Action success, Action<string, int> failure);
        Task GetClaimHistoryBenefits(string planMemberID, Action success, Action<string, int> failure);
        Task  SearchClaimsHistory(string planMemberID, string type, string payee, string benefits, DateTime startdate, DateTime enddate, Action success, Action<string, int> failure);

        ObservableCollection<ClaimState> GetSearchResultsByBenefitID(string benefitID);

        void ClearAllHistory();
    }
}
