using MobileClaims.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileClaims.Core.Entities.HCSA;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services.HCSA
{
    public class HCSAClaimService :ServiceBase, IHCSAClaimService
    {
        private IMvxMessenger _messenger;
        private IDataService _dataservice;
        private ILoginService _loginservice;
        private ILanguageService _languageservice;
        private ILoggerService _loggerservice;

        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private const string CLAIM_DATE_FORMAT = GSCHelper.GSC_DATE_FORMAT;
        private string CURRENT_LANGUAGE;
        private readonly MvxSubscriptionToken _languageupdated;
        
        private MvxSubscriptionToken _refreshed;
        private MvxSubscriptionToken _refreshFailed;
        private List<ExpenseTypeRequiresReferralHelper> _expensetypesthatrequireareferral;
        private const string ClaimFileName = "HCSAClaim.json";

        public HCSAClaimService(IMvxMessenger messenger, IDataService dataservice, ILoginService loginservice, ILanguageService languageservice, ILoggerService loggerservice)
        {
            _messenger = messenger;
            _dataservice = dataservice;
            _loginservice = loginservice;
            _languageservice = languageservice;
            _loggerservice = loggerservice;

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
                CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;

            _claimsubmissiontypes = new List<ClaimType>();
            TermsAndConditionsHaveBeenShown = false;
            _expensetypesthatrequireareferral = JsonConvert.DeserializeObject<List<ExpenseTypeRequiresReferralHelper>>(Resource.ExpenseTypesWithReferralRequired);
            Claim = _dataservice.GetHCSAClaim();
            if(Claim != null)
            {
                Mvx.IoCProvider.Resolve<IClaimService>().IsHCSAClaim = true;
                Claim.ExpenseType = _dataservice.GetSelectedHCSAExpenseType();
                
                this.SelectedExpenseType = Claim.ExpenseType;
                this.SelectedClaimType = _dataservice.GetSelectedHCSAClaimType();
                //GetClaimTypes(this.Claim.ParticipantNumber, () => SelectedClaimType = ClaimSubmissionTypes.Where(ct => ct.ID == Claim.ClaimTypeID).FirstOrDefault(), (a,b) => { });
            }
            else
            {
                Mvx.IoCProvider.Resolve<IClaimService>().IsHCSAClaim = false;
            }
        }

        public bool HaveClaimDetailsAlreadyBeenInitialized
        { get; set; }
        public bool TermsAndConditionsHaveBeenShown { get; set; }
        public ClaimType SelectedClaimType { get; set; }

        private Claim _claim;

        public Claim Claim
        {
            get { return _claim; }
            set
            {
                _claim = value;
            }
        }

        private void Details_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(Claim.Details.Count==0)
            {
                System.Diagnostics.Debug.WriteLine("WTF");
            }
        }

        private List<ExpenseType> _expensetypes;
        public List<ExpenseType>ExpenseTypes
        {
            get
            {
                return _expensetypes ?? new List<ExpenseType>(); ;
            }
            set
            {
                _expensetypes = value;
            }
        }
        public bool SelectedExpenseTypeRequiresReferral
        {
            get
            {
                if (this.SelectedExpenseType != null)
                {
                    var qry = from ExpenseTypeRequiresReferralHelper etr in this._expensetypesthatrequireareferral
                              where etr.Description.Equals(this.SelectedExpenseType.Name)
                              select etr;

                    return qry.FirstOrDefault() != null;
                }
                else
                {
                    return false;
                }
            }
        }
        public ExpenseType SelectedExpenseType
        {
            get; set;
        }
        private List<ClaimType> _claimsubmissiontypes;
        public List<ClaimType>ClaimSubmissionTypes
        {
            get
            {
                return _claimsubmissiontypes ?? new List<ClaimType>();
            }
            set
            {
                _claimsubmissiontypes = value;
                if(_claimsubmissiontypes[0] != null)
                {
                    _claimsubmissiontypes.Insert(0, null);
                }
            }
        }

        public async Task GetClaimTypes(string planMemberID, Action success, Action<string, int> failure)
        {
            //TODO: Double-check that we should be trimming down to PlanMember ID and not ParticipantID when the web services come online
            string participantID = GSCHelper.TrimParticipantNumberFromPlanMemberID(planMemberID);
            var service = new ApiClient<List<ClaimType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/hcsaclaimtype", participantID));

            try
            {
                List<ClaimType> typesList = await service.ExecuteRequest();
                ClaimSubmissionTypes = typesList.OrderBy(cst => cst.Name).ToList<ClaimType>();
                success();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimTypes(planMemberID, success, failure));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    string message = "No Claim Submission Types Found";
                    failure(message, (int)System.Net.HttpStatusCode.NotFound);
                    _messenger.Publish<NoClaimSubmissionTypesFound>(new NoClaimSubmissionTypesFound(this)); 
                }
                else
                {
                    failure(ex.ReasonPhrase, (int)ex.StatusCode);
                    _messenger.Publish<GetClaimSubmissionTypesError>(new GetClaimSubmissionTypesError(this) { Message = ex.ReasonPhrase });
                }
            }
        }

        public async Task GetClaimExpenseTypes(string planMemberID, long claimTypeID, Action success, Action<string, int> failure)
        {
            //TODO: Double-check that we should be trimming down to PlanMember ID and not ParticipantID when the web services come online
            string participantID = GSCHelper.TrimParticipantNumberFromPlanMemberID(planMemberID);
            var service = new ApiClient<List<ExpenseType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/hcsaexpensetype?claimtype={1}", participantID, claimTypeID.ToString()));

            try
            {
                List<ExpenseType> typesList = await service.ExecuteRequest();
                ExpenseTypes = typesList.OrderBy(tl => tl.Name).ToList<ExpenseType>();
                success();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimExpenseTypes(planMemberID, claimTypeID, success, failure));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    failure(ex.StatusCode.ToString(), (int)ex.StatusCode); 
                }
                else
                {
                    failure(ex.ReasonPhrase, (int)ex.StatusCode);
                }
            }
        }

        public async Task SubmitClaim(string PlanMemberID, long claimTypeID, long expenseTypeID, string medicalProfessionalID, ObservableCollection<ClaimDetail> claimDetails, Action success, Action failure)
        {
            string planMemberID = GSCHelper.TrimParticipantNumberFromPlanMemberID(PlanMemberID);
            string participantNumber = GSCHelper.GetParticipantNumberFromPlanMemberID(PlanMemberID);
            ObservableCollection<object> wireformatteddetails = new ObservableCollection<object>();
            foreach (ClaimDetail cd in claimDetails)
            {
                var edate = cd.ExpenseDate.Value; //DateTime.Parse(cd.ExpenseDate);
                string sdate = edate.ToString("yyyy/MM/dd");
                wireformatteddetails.Add(new
                {
                    claimAmount = cd.ClaimAmount,
                    otherPaidAmount = cd.OtherPaidAmount,
                    expenseDate = sdate
                });
            }
            var c = new
            {
                participantNumber = participantNumber,
                planMemberId = long.Parse(planMemberID),
                claimTypeId = claimTypeID,
                expenseTypeId = expenseTypeID,
                referralRequired = SelectedExpenseTypeRequiresReferral,
                medicalProfessionalId = medicalProfessionalID,
                details = wireformatteddetails
            };
            var service = new ApiClient<Claim>(new Uri(SERVICE_BASE_URL), HttpMethod.Post, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/hcsaclaim", apiBody: c);
            try
            {
                Claim = await service.ExecuteRequest();
                _dataservice.PersistHCSAClaim(null);
                Mvx.IoCProvider.Resolve<IClaimService>().IsHCSAClaim = false;
                success();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await SubmitClaim(PlanMemberID, claimTypeID, expenseTypeID, medicalProfessionalID, claimDetails, success, failure));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    failure(); 
                }
                else
                {
                    failure();
                }
            }
        }

        public void PersistClaim()
        {
            _dataservice.PersistHCSAClaim(Claim);
        }

        public void ClearClaim()
        {
            this.Claim = new Claim();
            HaveClaimDetailsAlreadyBeenInitialized = false;
        }
    }
}
