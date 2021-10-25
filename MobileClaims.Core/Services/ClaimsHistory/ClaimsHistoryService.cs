//#define FakingIt
//#define FakeStrikethrough

using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services.ClaimsHistory
{
    public class ClaimsHistoryService : ServiceBase, IClaimsHistoryService
    {
        private readonly IMvxMessenger _messenger;
        private readonly ILoginService _loginservice;
        private readonly ILanguageService _languageservice;
        private readonly IDeviceService _deviceservice;
        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private const string CLAIMS_HISTORY_SEARCH_DATE_FORMAT_IOS = GSCHelper.CLAIMS_HISTORY_SEARCH_DATE_FORMAT_IOS;
        private const string CLAIMS_HISTORY_SEARCH_DATE_FORMAT_ANDROID = GSCHelper.CLAIMS_HISTORY_SEARCH_DATE_FORMAT_ANDROID;
        private const string CLAIMS_HISTORY_SEARCH_DATE_FORMAT_QUERYSTRING = "yyyy-MM-dd";
        private string CURRENT_LANGUAGE;
        private readonly MvxSubscriptionToken _languageupdated;

        public ClaimsHistoryService(IMvxMessenger messenger, ILoginService loginservice, ILanguageService languageservice, IDeviceService deviceservice)
        {
            _messenger = messenger;
            _languageservice = languageservice;
            _loginservice = loginservice;
            _deviceservice = deviceservice;

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
                CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;

            ClaimHistoryTypes = new List<ClaimHistoryType>();
            ClaimHistoryPayees = new List<ClaimHistoryPayee>();
            ClaimHistoryBenefits = new List<ClaimHistoryBenefit>();
            ClaimsHistorySearchResults = new List<ClaimState>();
            DentalSearchResults = new List<ClaimState>();
            DrugSearchResults = new List<ClaimState>();
            EHSSearchResults = new List<ClaimState>();
            HCSASearchResults = new List<ClaimState>();
            NonHealthSearchResults = new List<ClaimState>();
            SearchResultsSummary = new List<ClaimHistorySearchResultSummary>();
            SelectedSearchResult = null;
            SelectedParticipant = null;
            SelectedClaimHistoryType = null;
            SelectedDisplayBy = new KeyValuePair<string,string>(string.Empty, string.Empty);
            SelectedStartDate = new DateTime();
            SelectedEndDate = new DateTime();
            SelectedYear = -1;
        }

        public List<ClaimHistoryType> ClaimHistoryTypes { get; private set; }
        public List<ClaimHistoryPayee> ClaimHistoryPayees { get; private set; }
        public List<ClaimHistoryBenefit> ClaimHistoryBenefits { get; private set; }
        public string AllClaimHistoryBenefits { get; private set; }
        public List<ClaimState> ClaimsHistorySearchResults { get; private set; }
        public List<ClaimState> DentalSearchResults { get; private set; }
        public List<ClaimState> DrugSearchResults { get; private set; }
        public List<ClaimState> EHSSearchResults { get; private set; }
        public List<ClaimState> HCSASearchResults { get; private set; }
        public List<ClaimState> NonHealthSearchResults { get; private set; }
        public List<ClaimHistorySearchResultSummary> SearchResultsSummary { get; private set; }
        public ClaimHistorySearchResultSummary SelectedSearchResultType { get; set; }
        public Participant SelectedParticipant { get; set; }
        public ClaimState SelectedSearchResult { get; set; }
        public ClaimHistoryType SelectedClaimHistoryType { get; set; }
        public KeyValuePair<string, string> SelectedDisplayBy { get; set; }
        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }
        public int SelectedYear { get; set; }
        public DateTime DateOfInquiry { get; private set; }
        public bool IsAllBenefitsSelected { get; set; }
        public ClaimHistoryPayee SelectedClaimHistoryPayee { get; set; }

        private bool _isSearchRightSideGreyedOut = false;
        public bool IsSearchRightSideGreyedOut
        {
            get { return _isSearchRightSideGreyedOut; }
            set
            {
                if (_isSearchRightSideGreyedOut != value)
                {
                    _isSearchRightSideGreyedOut = value;
                    _messenger.Publish<IsRightSideGreyedOutUpdated>(new IsRightSideGreyedOutUpdated(this));
                }
            }
        }

        private bool _isResultsCountSearchResultsSummarySelected = false;
        public bool IsResultsCountSearchResultsSummarySelected
        {
            get { return _isResultsCountSearchResultsSummarySelected; }
            set
            {
                if (_isResultsCountSearchResultsSummarySelected != value)
                {
                    _isResultsCountSearchResultsSummarySelected = value;
                    _messenger.Publish<IsRightSideGreyedOutUpdated>(new IsRightSideGreyedOutUpdated(this));
                }
            }
        }

        public bool IsRightSideGreyedOutMaster
        {
            get { return (IsSearchRightSideGreyedOut || !IsResultsCountSearchResultsSummarySelected); }
        }

        public string Period
        { 
            get
            {
                if (SelectedDisplayBy.Key == string.Empty)
                {
                    return string.Empty;
                }
                else if (SelectedDisplayBy.Key == GSCHelper.DisplayByYearKey)
                {
                    return SelectedYear.ToString();
                }
                else // date range or year to date
                {
                    if (_deviceservice.CurrentDevice == GSCHelper.OS.Droid)
                        return string.Format("{0} - {1}", SelectedStartDate.ToString(CLAIMS_HISTORY_SEARCH_DATE_FORMAT_ANDROID), SelectedEndDate.ToString(CLAIMS_HISTORY_SEARCH_DATE_FORMAT_ANDROID));
                    else
                        return string.Format("{0} - {1}", SelectedStartDate.ToString(CLAIMS_HISTORY_SEARCH_DATE_FORMAT_IOS), SelectedEndDate.ToString(CLAIMS_HISTORY_SEARCH_DATE_FORMAT_IOS));
                }
            }
        }

        private List<ClaimHistoryBenefit> _selectedClaimHistoryBenefits = new List<ClaimHistoryBenefit>();
        public List<ClaimHistoryBenefit> SelectedClaimHistoryBenefits
        { 
            get
            {
                return _selectedClaimHistoryBenefits;
            }
            set
            {
                if (_selectedClaimHistoryBenefits != value)
                {
                    _selectedClaimHistoryBenefits = value;
                    string linesOfBusiness = string.Empty;
                    foreach (ClaimHistoryBenefit chb in SelectedClaimHistoryBenefits)
                    {
                        if (string.IsNullOrEmpty(linesOfBusiness))
                            linesOfBusiness = chb.Name;
                        else
                            linesOfBusiness += ", " + chb.Name;
                    }
                    _linesOfBusiness = linesOfBusiness;
                }
            }
        }

        private string _linesOfBusiness;
        public string LinesOfBusiness
        {
            get
            {
                if (IsAllBenefitsSelected)
                {
                    return Resource.ClaimHistorySearch_AllBenefits;
                }
                else
                {
                    return _linesOfBusiness; //set when SelectedClaimHistoryBenefits is set
                }
            }
        }

        public async Task GetClaimHistoryTypes(Action success, Action<string, int> failure)
        {
            var service = new ApiClient<List<ClaimHistoryType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimhistorytype");
           // var service = new ApiClient<List<ClaimSubmissionType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/planmemberservices/api/planmember/{0}/claimsubmissiontype", planMemberID));
            try
            {
                List<ClaimHistoryType> typesList = await service.ExecuteRequest();
                ClaimHistoryTypes = typesList;
                success();
                _messenger.Publish<GetClaimHistoryTypesComplete>(new GetClaimHistoryTypesComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimHistoryTypes(success, failure));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    string message = "No Claims History Types Found";
                    failure(message, (int)System.Net.HttpStatusCode.NotFound);
                    _messenger.Publish<NoClaimHistoryTypesFound>(new NoClaimHistoryTypesFound(this)); 
                }
                else
                {
                    failure(ex.ReasonPhrase, (int)ex.StatusCode);
                    _messenger.Publish<GetClaimHistoryTypesError>(new GetClaimHistoryTypesError(this) { Message = ex.ReasonPhrase });
                }
            }

        }

        public async Task GetClaimHistoryPayees(Action success, Action<string, int> failure)
        {
            var service = new ApiClient<List<ClaimHistoryPayee>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimhistorypayee");
            try
            {
                List<ClaimHistoryPayee> payeesList = await service.ExecuteRequest();
                ClaimHistoryPayees = payeesList;
                success();
                _messenger.Publish<GetClaimHistoryPayeesComplete>(new GetClaimHistoryPayeesComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimHistoryPayees(success, failure));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    string message = "No Claims History Payees Found";
                    failure(message, (int)System.Net.HttpStatusCode.NotFound);
                    _messenger.Publish<NoClaimHistoryPayeesFound>(new NoClaimHistoryPayeesFound(this)); 
                }
                else
                {
                    failure(ex.ReasonPhrase, (int)ex.StatusCode);
                    _messenger.Publish<GetClaimHistoryPayeesError>(new GetClaimHistoryPayeesError(this) { Message = ex.ReasonPhrase });
                }
            }
        }

        public async Task GetClaimHistoryBenefits(string planMemberID, Action success, Action<string, int> failure)
        {

            string trimmedPlanMemberID = GSCHelper.TrimParticipantNumberFromPlanMemberID(planMemberID);

            var service = new ApiClient<List<ClaimHistoryBenefit>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/claimhistorybenefit", trimmedPlanMemberID));

            try
            {
                List<ClaimHistoryBenefit> benefitsList = await service.ExecuteRequest();
                ClaimHistoryBenefits = benefitsList;

                if (ClaimHistoryBenefits != null && ClaimHistoryBenefits.Count > 0)
                {
                    AllClaimHistoryBenefits = string.Join("-", ClaimHistoryBenefits.Select(x => x.ID));
                }
                else
                {
                    AllClaimHistoryBenefits = string.Empty;
                }
                success();
                _messenger.Publish<GetClaimHistoryBenefitsComplete>(new GetClaimHistoryBenefitsComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await (GetClaimHistoryBenefits(planMemberID, success, failure)));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    string message = "No Claims History Benefits Found";
                    failure(message, (int)System.Net.HttpStatusCode.NotFound);
                    _messenger.Publish<NoClaimHistoryBenefitsFound>(new NoClaimHistoryBenefitsFound(this)); 
                }
                else
                {
                    failure(ex.ReasonPhrase, (int)ex.StatusCode);
                    _messenger.Publish<GetClaimHistoryBenefitsError>(new GetClaimHistoryBenefitsError(this) { Message = ex.ReasonPhrase });
                }
            }
        }

        public async Task SearchClaimsHistory(string planMemberID, string type, string payee, string benefits, DateTime startdate, DateTime enddate, Action success, Action<string, int> failure)
        {

            string startDateForQueryString = startdate.ToString(CLAIMS_HISTORY_SEARCH_DATE_FORMAT_QUERYSTRING);
            string endDateForQueryString = enddate.ToString(CLAIMS_HISTORY_SEARCH_DATE_FORMAT_QUERYSTRING);
            DateOfInquiry = DateTime.Now;
            var service = new ApiClient<List<ClaimState>>(new Uri(SERVICE_BASE_URL),
                                                          HttpMethod.Get,
                                                          string.Format("/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/claimstate?type={1}&payee={2}&benefits={3}&startdate={4}&enddate={5}",
                                                                        planMemberID,
                                                                        type,
                                                                        payee,
                                                                        benefits,
                                                                        startDateForQueryString,
                                                                        endDateForQueryString));

            try
            {
                List<ClaimState> results = await service.ExecuteRequest();
                ClaimHistoryType cht = SelectedClaimHistoryType;
                ClearOldResults();
                ClaimsHistorySearchResults = SortAndFleshOutSearchResults(results, cht);
                CategorizeSearchResults();
                success();
                _messenger.Publish<SearchClaimsHistoryComplete>(new SearchClaimsHistoryComplete(this));

            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await SearchClaimsHistory(planMemberID, type, payee, benefits, startdate, enddate, success, failure));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ClearOldResults();
                    ConstructSearchResultsSummary();
                    string message = Resource.ClaimHistorySearch_MessageNotFound;
                    failure(message, (int)System.Net.HttpStatusCode.NotFound);
                    _messenger.Publish<NoClaimsHistorySearchResultsFound>(new NoClaimsHistorySearchResultsFound(this));
                     
                }
                else
                {
                    failure(ex.ReasonPhrase, (int)ex.StatusCode);
                    _messenger.Publish<SearchClaimsHistoryError>(new SearchClaimsHistoryError(this) { Message = ex.ReasonPhrase });
                }
            }
        }

        public ObservableCollection<ClaimState> GetSearchResultsByBenefitID(string benefitID)
        {
            List<ClaimState> results = new List<ClaimState>();
            switch (benefitID)
            {
                case GSCHelper.ClaimHistoryBenefitIDForDental:
                    results = DentalSearchResults;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForDrug:
                    results = DrugSearchResults;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForEHS:
                    results = EHSSearchResults;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForHCSA:
                    results = HCSASearchResults;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForNonHealth:
                    results = NonHealthSearchResults;
                    break;
            }
            return new ObservableCollection<ClaimState>(results);
        }

        private void ClearOldResults()
        {
            ClaimsHistorySearchResults = new List<ClaimState>();
            DentalSearchResults = new List<ClaimState>();
            DrugSearchResults = new List<ClaimState>();
            EHSSearchResults = new List<ClaimState>();
            HCSASearchResults = new List<ClaimState>();
            NonHealthSearchResults = new List<ClaimState>();
            SearchResultsSummary = new List<ClaimHistorySearchResultSummary>();
            SelectedSearchResult = null;
            //SelectedClaimHistoryType = null; //removing this due to a really weird issue where the response from the service comes back twice, causing issues down the line
        }

        public void ClearAllHistory()
        {
            ClaimHistoryTypes = new List<ClaimHistoryType>();
            ClaimHistoryPayees = new List<ClaimHistoryPayee>();
            ClaimHistoryBenefits = new List<ClaimHistoryBenefit>();
            ClaimsHistorySearchResults = new List<ClaimState>();
            DentalSearchResults = new List<ClaimState>();
            DrugSearchResults = new List<ClaimState>();
            EHSSearchResults = new List<ClaimState>();
            HCSASearchResults = new List<ClaimState>();
            NonHealthSearchResults = new List<ClaimState>();
            SearchResultsSummary = new List<ClaimHistorySearchResultSummary>();
            SelectedSearchResult = null;
            SelectedParticipant = null;
            SelectedClaimHistoryType = null;
            SelectedDisplayBy = new KeyValuePair<string, string>(string.Empty, string.Empty);
            SelectedStartDate = new DateTime();
            SelectedEndDate = new DateTime();
            SelectedYear = -1;
            SelectedClaimHistoryPayee = null;
        }

        private List<ClaimState> SortAndFleshOutSearchResults(List<ClaimState> results, ClaimHistoryType cht)
        {
            int counter = 1;
            List<ClaimState> sorted = results.OrderBy(r3 => r3.ClaimFormRevisionNumber).OrderByDescending(r2 => r2.ClaimFormID).OrderByDescending(r1 => r1.ServiceDate).ToList();
            foreach (ClaimState cs in sorted)
            {
                cs.Payee = GetClaimStatePayee(cs.PayeeID);
                cs.ClaimHistoryType = cht;
#if FakeStrikethrough
                if (counter % 2 == 0)
                    cs.IsStricken = true;
                counter++;
#endif
            }
            return sorted;
        }

        private void CategorizeSearchResults()
        {
            if (ClaimsHistorySearchResults != null && ClaimsHistorySearchResults.Count > 0)
            {
                DentalSearchResults = ClaimsHistorySearchResults.FindAll(sr => sr.BenefitID == GSCHelper.ClaimHistoryBenefitIDForDental);
                DrugSearchResults = ClaimsHistorySearchResults.FindAll(sr => sr.BenefitID == GSCHelper.ClaimHistoryBenefitIDForDrug);
                EHSSearchResults = ClaimsHistorySearchResults.FindAll(sr => sr.BenefitID == GSCHelper.ClaimHistoryBenefitIDForEHS);
                HCSASearchResults = ClaimsHistorySearchResults.FindAll(sr => sr.BenefitID == GSCHelper.ClaimHistoryBenefitIDForHCSA);
                NonHealthSearchResults = ClaimsHistorySearchResults.FindAll(sr => sr.BenefitID == GSCHelper.ClaimHistoryBenefitIDForNonHealth);
                ConstructSearchResultsSummary();
            }
            else
            {
                ClearOldResults();
            }
        }

        private void ConstructSearchResultsSummary()
        {
            if (SelectedClaimHistoryBenefits != null && SelectedClaimHistoryBenefits.Count > 0)
            {
                List<ClaimHistorySearchResultSummary> summaryItems = new List<ClaimHistorySearchResultSummary>();
                foreach (ClaimHistoryBenefit chb in SelectedClaimHistoryBenefits)
                {
                    ClaimHistorySearchResultSummary summary = new ClaimHistorySearchResultSummary();
                    summary.BenefitID = chb.ID;
                    summary.BenefitName = chb.Name;
                    summary.SearchResultCount = GetSearchResultCountByBenefitID(chb.ID);

                    summaryItems.Add(summary);
                }
                SearchResultsSummary = summaryItems;
            }
            else
            {
                SearchResultsSummary = new List<ClaimHistorySearchResultSummary>();
            }
        }

        private int GetSearchResultCountByBenefitID(string benefitID)
        {
            int count = -1;
            switch (benefitID)
            {
                case GSCHelper.ClaimHistoryBenefitIDForDental:
                    count = DentalSearchResults.Count;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForDrug:
                    count = DrugSearchResults.Count;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForEHS:
                    count = EHSSearchResults.Count;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForHCSA:
                    count = HCSASearchResults.Count;
                    break;
                case GSCHelper.ClaimHistoryBenefitIDForNonHealth:
                    count = NonHealthSearchResults.Count;
                    break;
            }
            return count;
        }

        private ClaimHistoryPayee GetClaimStatePayee(string payeeID)
        {
            if (string.IsNullOrEmpty(payeeID))
                return null;

            ClaimHistoryPayee chp = null;
            if (ClaimHistoryPayees != null & ClaimHistoryPayees.Count > 0)
            {
                chp = ClaimHistoryPayees.FirstOrDefault(p => p.ID == payeeID);
            }
            return chp;
        }

        private List<ClaimHistoryType> GetClaimHistoryTypesFakeData()
        {
            List<ClaimHistoryType> types = new List<ClaimHistoryType>();
            types.Add(new ClaimHistoryType { ID = "CL", Name = "Claims", EarliestYear = 2008 });
            types.Add(new ClaimHistoryType { ID = "PD", Name = "Dental Predeterminations", EarliestYear = 0 });
            types.Add(new ClaimHistoryType { ID = "MI", Name = "Medical Item Authorizations", EarliestYear = 0 });
            return types;
        }

        private List<ClaimHistoryPayee> GetClaimHistoryPayeesFakeData()
        {
            List<ClaimHistoryPayee> payees = new List<ClaimHistoryPayee>();
            payees.Add(new ClaimHistoryPayee { ID = "AL", Name = "All" });
            payees.Add(new ClaimHistoryPayee { ID = "SU", Name = "Plan Member" });
            payees.Add(new ClaimHistoryPayee { ID = "PR", Name = "Service Provider" });
            return payees;
        }

        private List<ClaimHistoryBenefit> GetClaimHistoryBenefitsFakeData()
        {
            List<ClaimHistoryBenefit> benefits = new List<ClaimHistoryBenefit>();
            benefits.Add(new ClaimHistoryBenefit { ID = "100000", Name = "All Benefits" });
            benefits.Add(new ClaimHistoryBenefit { ID = "100005", Name = "Dental" });
            benefits.Add(new ClaimHistoryBenefit { ID = "100006", Name = "Drug" });
            benefits.Add(new ClaimHistoryBenefit { ID = "100007", Name = "Extended Health Services (EHS)" });
            benefits.Add(new ClaimHistoryBenefit { ID = "110048", Name = "Health Care Spending Account" });
            benefits.Add(new ClaimHistoryBenefit { ID = "100004", Name = "Non-Health/Personal Spending Account" });
            return benefits;
        }

        private List<ClaimState> GetClaimsHistorySearchResultsFakeData()
        {
            List<ClaimState> results = new List<ClaimState>();

            long planMemberIDWithoutParticipantNumber = long.Parse(GSCHelper.TrimParticipantNumberFromPlanMemberID(_loginservice.CurrentPlanMemberID));
            string participantNumber = GSCHelper.GetParticipantNumberFromPlanMemberID(_loginservice.CurrentPlanMemberID);
            long claimFormID = 429818035;
            long claimFormRevisionNumber = 380182411;
            long claimID = 120191389;
            long claimRevisionNumber = 61672394;
            long claimDetailID = 211742847;
            DateTime serviceDate = new DateTime(2014, 01, 11);
            DateTime processedDate = new DateTime(2014, 01, 28);

            int counter = 0;

            results.Add(new ClaimState
                {
                    ClaimFormID = claimFormID + counter,
                    ClaimFormRevisionNumber = claimFormRevisionNumber + counter,
                    ClaimID = claimID + counter,
                    ClaimRevisionNumber = claimRevisionNumber + counter,
                    ClaimDetailID = claimDetailID + counter,
                    PlanMemberID = planMemberIDWithoutParticipantNumber,
                    ParticipantNumber = participantNumber,
                    BenefitID = GSCHelper.ClaimHistoryBenefitIDForDental,
                    ServiceDate = serviceDate.AddDays(counter),
                    ServiceDescription = "Physiotherapy, treatment, subsequent",
                    ClaimedAmount = 60d,
                    OtherPaidAmount = 0d,
                    CopayAmount = 12d,
                    PaidAmount = 48d,
                    Quantity = 0d,
                    PayeeID = "SU",
                    ProcessedDate = processedDate.AddDays(counter),
                    IsStricken = false,
                    Status = "In Process",
                    EOBMessages = new List<ClaimEOBMessageGSC>(),
                    Payment = new ClaimPayment
                        {
                            PlanMemberDisplayID = "552749",
                            PaymentMethodCode = "EF",
                            Amount = 142.64d,
                            CurrencyCode = "CAN",
                            DepositNumber = "4321",
                            StatementDate = new DateTime(2014, 02, 22),
                            DepositDate = new DateTime(2014, 02, 25)
                        }
                });
            counter++;

            results.Add(new ClaimState
            {
                ClaimFormID = claimFormID + counter,
                ClaimFormRevisionNumber = claimFormRevisionNumber + counter,
                ClaimID = claimID + counter,
                ClaimRevisionNumber = claimRevisionNumber + counter,
                ClaimDetailID = claimDetailID + counter,
                PlanMemberID = planMemberIDWithoutParticipantNumber,
                ParticipantNumber = participantNumber,
                BenefitID = GSCHelper.ClaimHistoryBenefitIDForDental,
                ServiceDate = serviceDate.AddDays(counter),
                ServiceDescription = "Dental, treatment, subsequent",
                ClaimedAmount = 60d,
                OtherPaidAmount = 0d,
                CopayAmount = 12d,
                PaidAmount = 48d,
                Quantity = 0d,
                PayeeID = "AL",
                ProcessedDate = processedDate.AddDays(counter),
                IsStricken = false,
                Status = "In Process",
                EOBMessages = new List<ClaimEOBMessageGSC>(),
                Payment = new ClaimPayment
                {
                    PlanMemberDisplayID = "552749",
                    PaymentMethodCode = "CH",
                    Amount = 142.64d,
                    CurrencyCode = "CAN",
                    ChequeNumber = "1234",
                    PaymentDate = new DateTime(2014, 02, 22),
                    CashedDate = new DateTime(2014, 02, 25),
                    Status = "Paid"
                }
            });
            counter++;

            results.Add(new ClaimState
            {
                ClaimFormID = claimFormID + counter,
                ClaimFormRevisionNumber = claimFormRevisionNumber + counter,
                ClaimID = claimID + counter,
                ClaimRevisionNumber = claimRevisionNumber + counter,
                ClaimDetailID = claimDetailID + counter,
                PlanMemberID = planMemberIDWithoutParticipantNumber,
                ParticipantNumber = participantNumber,
                BenefitID = GSCHelper.ClaimHistoryBenefitIDForDrug,
                ServiceDate = serviceDate.AddDays(counter),
                ServiceDescription = "Drug, treatment, subsequent",
                ClaimedAmount = 60d,
                OtherPaidAmount = 0d,
                CopayAmount = 12d,
                PaidAmount = 48d,
                Quantity = 0d,
                PayeeID = "SU",
                ProcessedDate = processedDate.AddDays(counter),
                IsStricken = false,
                Status = "In Process",
                EOBMessages = new List<ClaimEOBMessageGSC>(),
                Payment = new ClaimPayment
                {
                    PlanMemberDisplayID = "552749",
                    PaymentMethodCode = "EF",
                    Amount = 142.64d,
                    CurrencyCode = "CAN",
                    DepositNumber = "4321",
                    StatementDate = new DateTime(2014, 02, 22),
                    DepositDate = new DateTime(2014, 02, 25)
                }
            });
            counter++;

            results.Add(new ClaimState
            {
                ClaimFormID = claimFormID + counter,
                ClaimFormRevisionNumber = claimFormRevisionNumber + counter,
                ClaimID = claimID + counter,
                ClaimRevisionNumber = claimRevisionNumber + counter,
                ClaimDetailID = claimDetailID + counter,
                PlanMemberID = planMemberIDWithoutParticipantNumber,
                ParticipantNumber = participantNumber,
                BenefitID = GSCHelper.ClaimHistoryBenefitIDForHCSA,
                ServiceDate = serviceDate.AddDays(counter),
                ServiceDescription = "HCSA, treatment, subsequent",
                ClaimedAmount = 60d,
                OtherPaidAmount = 0d,
                CopayAmount = 12d,
                PaidAmount = 48d,
                Quantity = 0d,
                PayeeID = "PR",
                ProcessedDate = processedDate.AddDays(counter),
                IsStricken = false,
                Status = "In Process",
                EOBMessages = new List<ClaimEOBMessageGSC>(),
                Payment = new ClaimPayment()
            });
            counter++;

            results.Add(new ClaimState
            {
                ClaimFormID = claimFormID + counter,
                ClaimFormRevisionNumber = claimFormRevisionNumber + counter,
                ClaimID = claimID + counter,
                ClaimRevisionNumber = claimRevisionNumber + counter,
                ClaimDetailID = claimDetailID + counter,
                PlanMemberID = planMemberIDWithoutParticipantNumber,
                ParticipantNumber = participantNumber,
                BenefitID = GSCHelper.ClaimHistoryBenefitIDForDental,
                ServiceDate = serviceDate.AddDays(counter),
                ServiceDescription = "Physiotherapy, treatment, subsequent",
                ClaimedAmount = 60d,
                OtherPaidAmount = 0d,
                CopayAmount = 12d,
                PaidAmount = 48d,
                Quantity = 0d,
                PayeeID = "AL",
                ProcessedDate = processedDate.AddDays(counter),
                IsStricken = false,
                Status = "In Process",
                EOBMessages = new List<ClaimEOBMessageGSC>(),
                Payment = new ClaimPayment
                {
                    PlanMemberDisplayID = "552749",
                    PaymentMethodCode = "CH",
                    Amount = 142.64d,
                    CurrencyCode = "CAN",
                    ChequeNumber = "1234",
                    PaymentDate = new DateTime(2014, 02, 22),
                    CashedDate = new DateTime(2014, 02, 25),
                    Status = "Paid"
                }
            });
            counter++;

            return results;

            //results.Add(new ClaimState
            //{
            //    ClaimFormID = claimFormID + counter,
            //    ClaimFormRevisionNumber = claimFormRevisionNumber + counter,
            //    ClaimID = claimID + counter,
            //    ClaimRevisionNumber = claimRevisionNumber + counter,
            //    ClaimDetailID = claimDetailID + counter,
            //    PlanMemberID = planMemberIDWithoutParticipantNumber,
            //    ParticipantNumber = participantNumber,
            //    BenefitID = GSCHelper.ClaimHistoryBenefitIDForDental,
            //    ServiceDate = serviceDate.AddDays(counter),
            //    ServiceDescription = "Physiotherapy, treatment, subsequent",
            //    ClaimedAmount = 60d,
            //    OtherPaidAmount = 0d,
            //    CopayAmount = 12d,
            //    PaidAmount = 48d,
            //    Quantity = 0d,
            //    PayeeID = "SU",
            //    ProcessedDate = processedDate.AddDays(counter),
            //    IsStricken = false,
            //    Status = "In Process",
            //    EOBMessages = new List<ClaimEOBMessageGSC>(),
            //    Payment = new ClaimPayment
            //    {
            //        PlanMemberDisplayID = "552749",
            //        PaymentMethodCode = "CH",
            //        TransactionNumber = "50726340",
            //        Amount = 142.64d,
            //        CurrencyCode = "CAN",
            //        StatementDate = new DateTime(2014, 02, 22),
            //        TransactionDate = new DateTime(2014, 02, 25),
            //        Status = "Paid"
            //    }
            //});
            //counter++;
        }
    }
}
