using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class EligibilityService : ServiceBase, IEligibilityService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IDataService _dataservice;
        private readonly ILoginService _loginservice;
        private readonly ILanguageService _languageservice;
        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private string CURRENT_LANGUAGE;
        private MvxSubscriptionToken _languageupdated;

        public EligibilityService(IMvxMessenger messenger, IDataService dataservice, ILoginService loginservice, ILanguageService languageservice)
        {
            _messenger = messenger;
            _dataservice = dataservice;
            _loginservice = loginservice;
            _languageservice = languageservice;

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;

            EligibilityCheckTypes = new List<EligibilityCheckType>();
        }

        public EligibilityCheck EligibilityCheck { get; set; }

        private EligibilityCheckType _selectedEligibilityCheckType;
        public EligibilityCheckType SelectedEligibilityCheckType
        {
            get
            {
                return _selectedEligibilityCheckType;
            }
            set 
            {
                _selectedEligibilityCheckType = value;

                if (_selectedEligibilityCheckType == null)
                    return;

                /* Benefits */
                GetEligibilityBenefits(_selectedEligibilityCheckType.ID);

                /* Province */
                GetEligibilityProvinces(_selectedEligibilityCheckType.ID);

                /* Options */
                if (_selectedEligibilityCheckType.ID == "GLASSES")
                    GetEligibilityOptions("lenstype");
                else if (_selectedEligibilityCheckType.ID == "MASSAGE")
                    GetEligibilityOptions("massagetime");
            }
        }

        public List<EligibilityCheckType> EligibilityCheckTypes { get; private set; }
        public List<EligibilityProvince> EligibilityProvinces { get; private set; }
        public List<EligibilityBenefit> EligibilityBenefits { get; private set; }
        public List<EligibilityOption> EligibilityOptions { get; private set; }
        public EligibilityCheck EligibilityCheckResults { get; private set; }
        public EligibilityInquiry EligibilityInquiryResults { get; private set; }
        public Participant EligibilitySelectedParticipant { get; set; }
        public List<ParticipantEligibilityResult> SelectedParticipantsForBenefitInquiry { get; set; }

        public async Task GetEligibilityCheckTypes(string planMemberID)
        {

            if (planMemberID.IndexOf('-') > -1)
                planMemberID = planMemberID.Split('-')[0];

            var service = new ApiClient<List<EligibilityCheckType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/eligibilitychecktype", planMemberID));
            try
            {
                EligibilityCheckTypes = await service.ExecuteRequest();
                _messenger.Publish<GetEligibilityCheckTypesComplete>(new GetEligibilityCheckTypesComplete(this));
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetEligibilityCheckTypes(planMemberID));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _messenger.Publish<NoEligibilityCheckTypesFound>(new NoEligibilityCheckTypesFound(this)); 
                }
                else
                {
                    _messenger.Publish<GetEligibilityCheckTypesError>(new GetEligibilityCheckTypesError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<GetEligibilityCheckTypesError>(new GetEligibilityCheckTypesError(this) { Message = ex.Message });
            }
        }

        public async Task GetEligibilityProvinces(string eligibilityCheckTypeID)
        {

            var service = new ApiClient<List<EligibilityProvince>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/eligibilitychecktype/{0}/eligibilityprovince", eligibilityCheckTypeID));
            try
            {
                EligibilityProvinces = await service.ExecuteRequest();
                _messenger.Publish<GetEligibilityProvincesComplete>(new GetEligibilityProvincesComplete(this));
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetEligibilityProvinces(eligibilityCheckTypeID));
                }
                else
                {
                    _messenger.Publish<GetEligibilityProvincesError>(new GetEligibilityProvincesError(this) { Message = ex.ReasonPhrase });
                }
            }

            catch(Exception ex)
            {
                _messenger.Publish<GetEligibilityProvincesError>(new GetEligibilityProvincesError(this) { Message = ex.Message });
            }
        }

        public async Task GetEligibilityBenefits(string eligibilityCheckTypeID)
        {
            var service = new ApiClient<List<EligibilityBenefit>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/eligibilitychecktype/{0}/eligibilitybenefit", eligibilityCheckTypeID));
            try
            {
                EligibilityBenefits = await service.ExecuteRequest();
                _messenger.Publish<GetEligibilityBenefitsComplete>(new GetEligibilityBenefitsComplete(this)); 
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetEligibilityBenefits(eligibilityCheckTypeID));
                }
                else
                {
                    _messenger.Publish<GetEligibilityBenefitsError>(new GetEligibilityBenefitsError(this) { Message = ex.ReasonPhrase });
                }

            }
            catch(Exception ex)
            {
                _messenger.Publish<GetEligibilityBenefitsError>(new GetEligibilityBenefitsError(this) { Message = ex.Message });
            }
        }

        public async Task GetEligibilityOptions(string optionType)
        {
            var service = new ApiClient<List<EligibilityOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/eligibilityoption?type={0}", optionType));
            try
            {
                EligibilityOptions = await service.ExecuteRequest();
                _messenger.Publish<GetEligibilityOptionsComplete>(new GetEligibilityOptionsComplete(this));
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetEligibilityOptions(optionType));
                }
                else
                {
                    _messenger.Publish<GetEligibilityOptionsError>(new GetEligibilityOptionsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<GetEligibilityOptionsError>(new GetEligibilityOptionsError(this) { Message = ex.Message });
            }
        }

        public async Task CheckEligibility()
        {
            EligibilityCheck ec = EligibilityCheck;
            var service = new ApiClient<EligibilityCheck>(new Uri(SERVICE_BASE_URL), HttpMethod.Post, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/eligibilitycheck", apiBody:ec);

            try
            {
                var returnedEC = await service.ExecuteRequest();
                EligibilityCheckResults = returnedEC;
                EligibilityCheckResults.EligibilityCheckType = EligibilityCheck.EligibilityCheckType;
                EligibilityCheckResults.TypeOfTreatment = EligibilityCheck.TypeOfTreatment;
                EligibilityCheckResults.Province = EligibilityCheck.Province;
                EligibilityCheckResults.LengthOfTreatmentFull = EligibilityCheck.LengthOfTreatmentFull;
                EligibilityCheckResults.LensType = EligibilityCheck.LensType;
                _messenger.Publish<EligibilityCheckSubmissionComplete>(new EligibilityCheckSubmissionComplete(this));
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await CheckEligibility());
                }
                else
                {
                    _messenger.Publish<EligibilityCheckSubmissionError>(new EligibilityCheckSubmissionError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<EligibilityCheckSubmissionError>(new EligibilityCheckSubmissionError(this) { Message = ex.Message });
            }
        }

        public async Task EligibilityInquiry(EligibilityInquiry ei)
        {
            var service = new ApiClient<HttpResponseMessage>(new Uri(SERVICE_BASE_URL), HttpMethod.Post, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/eligibilityinquiry", apiBody: ei);

            try
            {

            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await service.ExecuteRequest();
                    _messenger.Publish<EligibilityInquirySubmissionComplete>(new EligibilityInquirySubmissionComplete(this));
                }
                else
                {
                    _messenger.Publish<EligibilityInquirySubmissionError>(new EligibilityInquirySubmissionError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                _messenger.Publish<EligibilityInquirySubmissionError>(new EligibilityInquirySubmissionError(this) { Message = ex.Message });
            }
        }
    }
}
