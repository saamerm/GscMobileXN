using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class DrugLookupService : ServiceBase, IDrugLookupService
    {
        private readonly IParticipantService _participantservice;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxFileStore _filesystem;
        private readonly ILoginService _loginservice;
        private readonly ILanguageService _languageservice;
        private readonly MvxSubscriptionToken _participantchanged;
        private readonly PlanMember _planmember;
        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private string CURRENT_LANGUAGE;
        private MvxSubscriptionToken _languageupdated;

#if FakingIt
        private int counter = 1;
#endif

        public DrugLookupService(IParticipantService participantservice, IMvxMessenger messenger, IMvxFileStore filesystem, ILoginService loginservice, ILanguageService languageservice)
        {
            _participantservice = participantservice;
            _messenger = messenger;
            _planmember = _participantservice.PlanMember;
            _filesystem = filesystem;
            _loginservice = loginservice;
            _languageservice = languageservice;

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
                CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;

            SearchResult = new List<DrugInfo>();
            _participantchanged = _messenger.Subscribe<RetrievedPlanMemberMessage>((message) =>
                {
                    //respond!
                });
        }

        public DrugInfo EligibilityResult { get; private set; }
        public List<DrugInfo> SearchResult { get; private set; }
        public DrugInfo SearchByDINResult { get; set; }
        public string SpecialAuthorizationFormPath { get; private set; }

        public async Task CheckEligibility(Participant participant, DrugInfo drug)
        {

            this.EligibilityResult = null;

            DrugEligibility de = new DrugEligibility() { DrugID = drug.ID, DIN = drug.DIN };
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("planMemberId", participant.PlanMemberID);

            var client = new ApiClient<DrugEligibility>(new Uri(SERVICE_BASE_URL), HttpMethod.Post, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{planMemberId}/drugeligibility", parameters, de);
            try
            {
                var resDE = await client.ExecuteRequest();
                EligibilityResult = new DrugInfo()
                {
                    DIN = resDE.DIN,
                    Name = resDE.DrugName,
                    CoveredMessage = resDE.CoverageMessage,
                    PlanParticipant = (Participant)participant,
                    Reimbursement = resDE.AccumMessage,
                    SpecialAuthRequired = resDE.RequiresSpecialAuthorization,
                    SpecialAuthFormName = resDE.SpecialAuthorizationFormUri,
                    AuthorizationMessage = resDE.AuthorizationMessage,
                    Notes = resDE.Timestamp.ToLocalTime().ToString(),
                    LowCostReplacementOccurred = resDE.LowCostReplacementOccurred
                };
                _messenger.Publish<CheckDrugEligibilityComplete>(new CheckDrugEligibilityComplete(this));
            }
            catch (ApiException apiEx)
            {
                if (apiEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await CheckEligibility(participant, drug));
                }
                else
                {
                    _messenger.Publish<CheckDrugEligibilityError>(new CheckDrugEligibilityError(this) { Message = apiEx.Message });
                    return;
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<CheckDrugEligibilityError>(new CheckDrugEligibilityError(this) { Message = ex.Message });
                return;
            }
        }

        public async Task GetByName(string DrugName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("drugname", DrugName);
            var client = new ApiClient<List<DrugInfo>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/drug?namestartswith={drugname}", parameters);
            try
            {
                SearchResult = await client.ExecuteRequest();
                _messenger.Publish<SearchDrugByNameComplete>(new SearchDrugByNameComplete(this));
            }
            catch (ApiException apiEx)
            {
                if (apiEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetByName(DrugName));
                }
                else
                {
                    _messenger.Publish<DrugSearchByNameError>(new DrugSearchByNameError(this) { Message = apiEx.Message });
                    return;
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<DrugSearchByNameError>(new DrugSearchByNameError(this) { Message = ex.Message });
                return;
            }
        }

        public async Task GetByDIN(int DIN)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("din", DIN.ToString());
            var client = new ApiClient<List<DrugInfo>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, "/"+ GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/drug?din={din}", parameters);
            try
            {
                SearchResult = await client.ExecuteRequest();
                SearchByDINResult = SearchResult.FirstOrDefault();
                _messenger.Publish<DrugSearchComplete>(new DrugSearchComplete(this));
            }
            catch(ApiException apiEx)
            {
                if(apiEx.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetByDIN(DIN));
                }
                else
                {
                    _messenger.Publish<DrugSearchByDINError>(new DrugSearchByDINError(this) { Message = apiEx.Message });
                    return;
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<DrugSearchByDINError>(new DrugSearchByDINError(this) { Message = ex.Message });
                return;
            }
        }

        public async Task GetSpecialAuthorizationForm(string formPath)
        {
            string encodedPath = Uri.EscapeUriString(formPath);
            var service = new ApiClient<HttpResponseMessage>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/form/{0}", formPath));
            Uri serviceUri = new Uri(string.Format("{0}/api/form/{1}", SERVICE_BASE_URL, formPath));
            try
            {
                var response = await service.ExecuteRequest();
                string path = WriteFormToDevice(await response.Content.ReadAsByteArrayAsync(), formPath);
                SpecialAuthorizationFormPath = path;
                _messenger.Publish<GetSpecialAuthorizationFormComplete>(new GetSpecialAuthorizationFormComplete(this));
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetSpecialAuthorizationForm(formPath));
                }
                else
                {
                    _messenger.Publish<GetSpecialAuthorizationFormError>(new GetSpecialAuthorizationFormError(this) { Message = ex.ReasonPhrase});

                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<GetSpecialAuthorizationFormError>(new GetSpecialAuthorizationFormError(this) { Message = ex.Message});

            }
        }

        public async Task EmailSpecialAuthorizationForm(string formPath, EmailRequest er)
        {
            var service = new ApiClient<HttpResponseMessage>(new Uri(SERVICE_BASE_URL), HttpMethod.Post, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/form/{0}/email", formPath), apiBody:er);
            string encodedPath = Uri.EscapeUriString(formPath);
            try
            {
                var response = await service.ExecuteRequest();
                _messenger.Publish<EmailSpecialAuthorizationFormComplete>(new EmailSpecialAuthorizationFormComplete(this));
            }
            catch(ApiException ex)
            {
                _messenger.Publish<EmailSpecialAuthorizationFormError>(new EmailSpecialAuthorizationFormError(this) { Message = ex.ReasonPhrase });

            }
            catch (Exception ex)
            {
                _messenger.Publish<EmailSpecialAuthorizationFormError>(new EmailSpecialAuthorizationFormError(this) { Message = ex.Message });
            }
        }

        private string WriteFormToDevice(byte[] data, string filename)
        {
            string formPath = string.Empty;

            if (_filesystem.Exists(filename)) _filesystem.DeleteFile(filename);
            _filesystem.WriteFile(filename, data);
            formPath = _filesystem.NativePath(filename);

            return formPath;
        }
    }
}
