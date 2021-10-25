//#define FakingIt

using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class SpendingAccountService : ServiceBase, ISpendingAccountService
    {
        private MvxSubscriptionToken _accounttypestoken;
        private MvxSubscriptionToken _updateddetails;
        private readonly IMvxMessenger _messenger;
        private readonly ILoginService _loginservice;
        private readonly ILanguageService _languageservice;
        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private string CURRENT_LANGUAGE;
        private MvxSubscriptionToken _languageupdated;
        private object _sync = new object();

        public SpendingAccountService(IMvxMessenger messenger, ILoginService loginservice, ILanguageService languageservice)
        {
            _messenger = messenger;
            _loginservice = loginservice;
            _languageservice = languageservice;

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
                CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;

            AccountTypes = new List<SpendingAccountType>();
            SpendingAccounts = new List<SpendingAccountDetail>();
            SpendingAccountTypeRollups = new List<SpendingAccountTypeRollup>();
            YearTotalRemainCollection = new ObservableCollection<YearTotalRemain>();
            SelectedSpendingAccountType = null;
            IsGetSpendingAccountTypesBusy = false;
            IsGetSpendingAccountTypesDone = false;
            IsGetSpendingAccountsBusy = false;
            IsGetSpendingAccountsDone = false;
        }

        public List<SpendingAccountType> AccountTypes
        {
            get;
            private set;
        }

        public List<SpendingAccountDetail> SpendingAccounts
        {
            get;
            private set;
        }

        public List<SpendingAccountTypeRollup> SpendingAccountTypeRollups
        {
            get;
            private set;
        }

        public ObservableCollection<YearTotalRemain> YearTotalRemainCollection
        {
            get;
            private set;
        }

        public SpendingAccountType SelectedSpendingAccountType
        {
            get;
            set;
        }

        public bool IsGetSpendingAccountTypesBusy { get; private set; }
        public bool IsGetSpendingAccountTypesDone { get; private set; }
        public bool IsGetSpendingAccountsBusy { get; private set; }
        public bool IsGetSpendingAccountsDone { get; private set; }

        private string PlanMemberIDLastLoaded { get; set; }

        public async Task GetAllSpendingAccountsInfo(string planMemberID, bool forceRefresh)
        {
            if (forceRefresh || planMemberID != PlanMemberIDLastLoaded)
            {
                ClearOldData();
                PlanMemberIDLastLoaded = planMemberID;

                await GetSpendingAccountTypes(planMemberID);
                if (AccountTypes.Any())
                {
                    await GetSpendingAccounts(planMemberID);
                }
            }
        }

        public async Task GetSpendingAccountTypes(string planMemberID)
        {

            planMemberID = GSCHelper.TrimParticipantNumberFromPlanMemberID(planMemberID);
            var service = new ApiClient<List<SpendingAccountType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/accountmodel", planMemberID));
            try
            {
                AccountTypes = await service.ExecuteRequest();
                IsGetSpendingAccountTypesBusy = false;
                IsGetSpendingAccountTypesDone = true;
                _messenger.Publish<RetrievedSpendingAccountTypes>(new RetrievedSpendingAccountTypes(this));
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetSpendingAccountTypes(planMemberID));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    AccountTypes = new List<SpendingAccountType>();
                    IsGetSpendingAccountTypesBusy = false;
                    IsGetSpendingAccountTypesDone = true;
                    _messenger.Publish<NoSpendingAccountTypes>(new NoSpendingAccountTypes(this));
                }
                else
                {
                    _messenger.Publish<GetSpendingAccountTypesError>(new GetSpendingAccountTypesError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<GetSpendingAccountTypesError>(new GetSpendingAccountTypesError(this) { Message = ex.Message });
            }
        }

        public async Task GetSpendingAccounts(string planMemberID)
        {
            planMemberID = GSCHelper.TrimParticipantNumberFromPlanMemberID(planMemberID);
            var service = new ApiClient<List<SpendingAccountDetail>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/spendingaccount", planMemberID));

            try
            {
                List<SpendingAccountDetail> spendingAccountList = await service.ExecuteRequest();
                List<SpendingAccountDetail> cleanedSpendingAccountList = RemoveInvalidSpendingAccountDetails(spendingAccountList);
                SetSpendingAccountTypeRollups(cleanedSpendingAccountList);
                SetYearTotalRemainCollection();
                SpendingAccounts = cleanedSpendingAccountList;
                IsGetSpendingAccountsBusy = false;
                IsGetSpendingAccountsDone = true;
                _messenger.Publish<RetrievedSpendingAccounts>(new RetrievedSpendingAccounts(this));
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetSpendingAccounts(planMemberID));
                }
                else
                {
                    IsGetSpendingAccountsDone = true;
                    _messenger.Publish<GetSpendingAccountsError>(new GetSpendingAccountsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<GetSpendingAccountsError>(new GetSpendingAccountsError(this) { Message = ex.Message});
            }
        }

        public SpendingAccountTypeRollup GetSpendingAccountDetailsByType(SpendingAccountType accountType)
        {
            SpendingAccountTypeRollup satr = null;

            if (accountType != null)
            {
                if (SpendingAccountTypeRollups != null && SpendingAccountTypeRollups.Count > 0)
                    satr = SpendingAccountTypeRollups.Find(sa => sa.AccountType == accountType);
            }

            return satr;
        }

        private void ClearOldData()
        {
            AccountTypes = new List<SpendingAccountType>();
            SpendingAccounts = new List<SpendingAccountDetail>();
            SpendingAccountTypeRollups = new List<SpendingAccountTypeRollup>();
            YearTotalRemainCollection = new ObservableCollection<YearTotalRemain>();
            SelectedSpendingAccountType = null;
            IsGetSpendingAccountTypesBusy = false;
            IsGetSpendingAccountTypesDone = false;
            IsGetSpendingAccountsBusy = false;
            IsGetSpendingAccountsDone = false;
        }

        /// <summary>
        /// DEPRECATED.
        /// </summary>
        private void SetAccountTypes()
        {
            List<SpendingAccountType> accountTypes = new List<SpendingAccountType>();
            IEnumerable<string> uniqueSpendingAccountTypes = SpendingAccounts.Select(sa => sa.ModelID).Distinct();
            foreach (string modelName in uniqueSpendingAccountTypes)
            {
                var modelID = SpendingAccounts.First(i => i.ModelID == modelName).ModelID;
                SpendingAccountType at = new SpendingAccountType() { ModelID = modelID, ModelName = modelName };
                accountTypes.Add(at);
            }
            AccountTypes = accountTypes;
        }

        private void SetSpendingAccountTypeRollups(List<SpendingAccountDetail> spendingAccounts)
        {
            List<SpendingAccountTypeRollup> rollups = new List<SpendingAccountTypeRollup>();

            foreach (SpendingAccountType type in AccountTypes)
            {
                SpendingAccountTypeRollup satr = GetSpendingAccountRollupByType(type, spendingAccounts);
                if (satr != null) rollups.Add(satr);
            }

            SpendingAccountTypeRollups = rollups;
        }

        private SpendingAccountTypeRollup GetSpendingAccountRollupByType(SpendingAccountType accountType, List<SpendingAccountDetail> spendingAccounts)
        {
            SpendingAccountTypeRollup rollup = new SpendingAccountTypeRollup();
            rollup.AccountType = accountType;

            List<SpendingAccountPeriodRollup> periodRollups = new List<SpendingAccountPeriodRollup>();

            List<SpendingAccountDetail> detailsForGivenType = spendingAccounts.FindAll(sa => sa.ModelID == accountType.ModelID).ToList();
            IEnumerable<int> uniqueYears = spendingAccounts.Select(sa => sa.Year).Distinct();
            foreach (int year in uniqueYears)
            {
                List<SpendingAccountDetail> deets = detailsForGivenType.FindAll(d => d.Year == year).ToList();
                if (deets != null && deets.Count > 0)
                {
                    SpendingAccountPeriodRollup pr = new SpendingAccountPeriodRollup();
                    pr.SpendingAccounts = deets;
                    pr.Year = year;
                    periodRollups.Add(pr);
                }
            }

            rollup.AccountRollups = periodRollups;
            return rollup;
        }

        private List<SpendingAccountDetail> RemoveInvalidSpendingAccountDetails(List<SpendingAccountDetail> spendingAccounts)
        {
            int currentYear = DateTime.UtcNow.Year;
            int previousYear = currentYear - 1;

            List<SpendingAccountDetail> newSads = new List<SpendingAccountDetail>();
            foreach (SpendingAccountDetail sad in spendingAccounts)
            {
                if (sad.Year == currentYear || sad.Year == previousYear)
                {
                    newSads.Add(sad);
                }
            }
            return newSads;
        }

        /// <summary>
        /// Deprecated. Only used for Windows.
        /// </summary>
        private void SetYearTotalRemainCollection()
        {
            ObservableCollection<YearTotalRemain> ytrc = new ObservableCollection<YearTotalRemain>();
            int preYearNumber = DateTime.Now.Year - 1;
            int currentYearNumber = DateTime.Now.Year;

            if (this.SpendingAccountTypeRollups != null && this.SpendingAccountTypeRollups.Count > 0)
            {
                foreach (SpendingAccountTypeRollup satr in SpendingAccountTypeRollups)
                {
                    YearTotalRemain rtr = new YearTotalRemain();
                    string name = string.Empty;
                    SpendingAccountTypeRollup satrEntity = satr as SpendingAccountTypeRollup;
                    if (satr.AccountRollups != null)
                    {
                        rtr.AccountName = satrEntity.AccountType.ModelName;
                        if (satr.AccountRollups.Count > 0)
                        {
                            List<SpendingAccountPeriodRollup> sapr = satrEntity.AccountRollups as List<SpendingAccountPeriodRollup>;
                            if (satr.AccountRollups.Count == 1)
                            {
                                if (preYearNumber == sapr[0].Year)
                                {
                                    rtr.preYear = sapr[0].Year;
                                    rtr.PreYearTotal = sapr[0].TotalRemaining;
                                }
                                else
                                {
                                    if (currentYearNumber == sapr[0].Year)
                                    {
                                        rtr.currentYear = sapr[0].Year;
                                        rtr.currentYearTotal = sapr[0].TotalRemaining;
                                    }
                                }
                            }
                            else
                            {
                                if (preYearNumber == sapr[0].Year)
                                {
                                    rtr.preYear = sapr[0].Year;
                                    rtr.PreYearTotal = sapr[0].TotalRemaining;
                                    rtr.currentYear = sapr[1].Year;
                                    rtr.currentYearTotal = sapr[1].TotalRemaining;
                                }
                                else
                                {
                                    if (preYearNumber == sapr[1].Year)
                                    {
                                        rtr.preYear = sapr[1].Year;
                                        rtr.PreYearTotal = sapr[1].TotalRemaining;
                                        rtr.currentYear = sapr[0].Year;
                                        rtr.currentYearTotal = sapr[0].TotalRemaining;
                                    }
                                }
                            }
                        }
                        bool isExist = false;
                        if (ytrc.Count > 0)
                        {
                            foreach (YearTotalRemain y in ytrc)
                            {
                                if (y.preYear == rtr.preYear && y.PreYearTotal == rtr.PreYearTotal && y.currentYear == rtr.currentYear && y.currentYearTotal == rtr.currentYearTotal)
                                    isExist = true;
                            }
                        }
                        if (!isExist)
                            ytrc.Add(rtr);
                    }
                }
            }

            this.YearTotalRemainCollection = ytrc;
            if(YearTotalRemainCollection .Count >0)
            {
                foreach (YearTotalRemain item in YearTotalRemainCollection )
                {
                    item.SumofTotalRemaining = item.PreYearTotal + item.currentYearTotal;
                }
            }
        }

        private List<SpendingAccountDetail> GenerateFakeData()
        {
            List<SpendingAccountDetail> sads = new List<SpendingAccountDetail>();

            SpendingAccountDetail sadetail = new SpendingAccountDetail();
            sadetail.ModelID = "HCSA";
            //sadetail.ModelName = "HCSA Account";
            sadetail.AccountName = "FLEX";
            sadetail.Year = 2014;
            sadetail.StartDate = new DateTime(sadetail.Year, 1, 1);
            sadetail.EndDate = new DateTime(sadetail.Year, 12, 31);
            sadetail.Deposited = 200.0;
            sadetail.UsedToDate = 150.0;
            sadetail.Remaining = 50.0;
            sadetail.UseByDate = sadetail.EndDate.AddDays(1).AddMonths(1);
            sads.Add(sadetail);

            sadetail = new SpendingAccountDetail();
            sadetail.ModelID = "HCSA";
            //sadetail.ModelName = "HCSA Account";
            sadetail.AccountName = "FLEX";
            sadetail.Year = 2015;
            sadetail.StartDate = new DateTime(sadetail.Year, 1, 1);
            sadetail.EndDate = new DateTime(sadetail.Year, 12, 31);
            sadetail.Deposited = 800.0;
            sadetail.UsedToDate = 600.0;
            sadetail.Remaining = 200.0;
            sadetail.UseByDate = sadetail.EndDate.AddDays(1).AddMonths(1);
            sads.Add(sadetail);

            sadetail = new SpendingAccountDetail();
            sadetail.ModelID = "HCSA";
            //sadetail.ModelName = "HCSA Account";
            sadetail.AccountName = "WELLNESS";
            sadetail.Year = 2014;
            sadetail.StartDate = new DateTime(sadetail.Year, 1, 1);
            sadetail.EndDate = new DateTime(sadetail.Year, 12, 31);
            sadetail.Deposited = 1000.0;
            sadetail.UsedToDate = 400.0;
            sadetail.Remaining = 600.0;
            sadetail.UseByDate = sadetail.EndDate.AddDays(1).AddMonths(1);
            sads.Add(sadetail);

            sadetail = new SpendingAccountDetail();
            sadetail.ModelID = "HCSA";
            //.ModelName = "HCSA Account";
            sadetail.AccountName = "WELLNESS";
            sadetail.Year = 2015;
            sadetail.StartDate = new DateTime(sadetail.Year, 1, 1);
            sadetail.EndDate = new DateTime(sadetail.Year, 12, 31);
            sadetail.Deposited = 1000.0;
            sadetail.UsedToDate = 300.0;
            sadetail.Remaining = 700.0;
            sadetail.UseByDate = sadetail.EndDate.AddDays(1).AddMonths(1);
            sads.Add(sadetail);

            sadetail = new SpendingAccountDetail();
            sadetail.ModelID = "PSA";
            //sadetail.ModelName = "PSA Account";
            sadetail.AccountName = "PSA";
            sadetail.Year = 2014;
            sadetail.StartDate = new DateTime(sadetail.Year, 1, 1);
            sadetail.EndDate = new DateTime(sadetail.Year, 12, 31);
            sadetail.Deposited = 500.0;
            sadetail.UsedToDate = 300.0;
            sadetail.Remaining = 200.0;
            sadetail.UseByDate = sadetail.EndDate.AddDays(1).AddMonths(1);
            sads.Add(sadetail);

            sadetail = new SpendingAccountDetail();
            sadetail.ModelID = "PSA";
            //sadetail.ModelName = "PSA Account";
            sadetail.AccountName = "PSA";
            sadetail.Year = 2015;
            sadetail.StartDate = new DateTime(sadetail.Year, 1, 1);
            sadetail.EndDate = new DateTime(sadetail.Year, 12, 31);
            sadetail.Deposited = 500.0;
            sadetail.UsedToDate = 100.0;
            sadetail.Remaining = 400.0;
            sadetail.UseByDate = sadetail.EndDate.AddDays(1).AddMonths(1);
            sads.Add(sadetail);

            return sads;
        }
    }
}
