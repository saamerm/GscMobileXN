//#define ACFakeData

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services.FacadeEntities;
using MobileClaims.Core.Services.HCSA;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace MobileClaims.Core.Services
{
    public class ClaimService : ServiceBase, IClaimService
    {
        private readonly IMvxLog _log;
        private readonly IMvxMessenger _messenger;
        private readonly IDataService _dataservice;
        private readonly ILoginService _loginservice;
        private readonly ILanguageService _languageservice;

        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private const string CLAIM_DATE_FORMAT = GSCHelper.GSC_DATE_FORMAT;
        private string CURRENT_LANGUAGE;
        private readonly MvxSubscriptionToken _languageupdated;

        public ClaimService(IMvxMessenger messenger,
            IDataService dataservice, 
            ILoginService loginservice, 
            ILanguageService languageservice, 
            IMvxLog log)
        {
            _messenger = messenger;
            _dataservice = dataservice;
            _loginservice = loginservice;
            _languageservice = languageservice;
            _log = log;

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
                CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;

            ClaimSubmissionTypes = new List<ClaimSubmissionType>();

            ClaimSubmissionBenefits = new List<ClaimSubmissionBenefit>();

            Claim = _dataservice.GetClaim();
            if (Claim != null && Claim.Type != null && Claim.ClaimSubmissionTypes != null)
            {
                var qry = from ClaimSubmissionType cst in Claim.ClaimSubmissionTypes
                          where cst.ID.Equals(Claim.Type.ID)
                          select cst;
                this.SelectedClaimSubmissionType = qry.FirstOrDefault();

                if (Claim.ClaimSubmissionTypes.Count > 0) this.ClaimSubmissionTypes = Claim.ClaimSubmissionTypes;
                if (Claim.TypesOfMedicalProfessional != null && Claim.TypesOfMedicalProfessional.Count > 0) this.TypesOfMedicalProfessional = Claim.TypesOfMedicalProfessional;
            }
            this.IsHCSAClaim = false;
        }

        public List<ClaimSubmissionType> ClaimSubmissionTypes { get; private set; }

        private ClaimSubmissionType _selectedClaimSubmissionType;
        public ClaimSubmissionType SelectedClaimSubmissionType
        {
            get
            {
                return _selectedClaimSubmissionType;
            }
            set
            {
                _selectedClaimSubmissionType = value;
                if (_selectedClaimSubmissionType != null && _loginservice.AuthInfo != null)
                {
                    GetClaimSubmissionBenefits(_selectedClaimSubmissionType.ID);

                    GetTypesOfMedicalProfessional();

                    string claimOptionType = GetClaimOptionTypeByClaimSubmissionType(_selectedClaimSubmissionType.ID);
                    if (!string.IsNullOrEmpty(claimOptionType))
                        GetClaimOptions(claimOptionType);

                    if (_selectedClaimSubmissionType.ID == "GLASSES")
                        GetLensTypes(_loginservice.CurrentPlanMemberID);

                    if (_selectedClaimSubmissionType.ID == "CONTACTS" || _selectedClaimSubmissionType.ID == "GLASSES")
                    {
                        GetVisionAxes();
                        GetVisionBifocals();
                        GetVisionCylinders();
                        GetVisionPrisms();
                        GetVisionSpheres();
                    }
                }
            }
        }

        public List<ClaimSubmissionBenefit> ClaimSubmissionBenefits { get; private set; }
        public List<ClaimOption> ClaimOptions { get; private set; }

        private List<ClaimOption> _typesOfMedicalProfessional;
        public List<ClaimOption> TypesOfMedicalProfessional
        {
            get
            {
                return _typesOfMedicalProfessional;
            }
            private set
            {
                _typesOfMedicalProfessional = value;

                // TODO: Varify why this is add, this create an empty item in the table.
                //if (_typesOfMedicalProfessional[0] != null)
                //_typesOfMedicalProfessional.Insert(0, null);
            }
        }

        public List<LensType> LensTypes { get; private set; }

        public List<TextAlteration> ClaimAgreementTextAlterations { get; private set; }
        public List<TextAlteration> ClaimAgreementTextAlterations_1 { get; private set; }

        public string ClaimAgreementTextAlterationsDate { get; private set; }

        public List<TextAlteration> PhoneTextAlterations { get; private set; }
        public TextAlteration PhoneText { get; private set; }
        public bool NoPhoneNumber { get; set; }
        public bool PhoneError { get; set; }
        public bool ClaimError { get; set; }
        public bool ClaimDisclaimerError { get; set; }
        public List<TextAlteration> ClaimDisclaimerTextAlterations { get; private set; }

        private List<ClaimOption> _visionSpheres;
        public List<ClaimOption> VisionSpheres
        {
            get
            {
                return _visionSpheres;
            }
            private set
            {
                _visionSpheres = value;
                if (_visionSpheres != null && _visionSpheres[0] != null)
                    _visionSpheres.Insert(0, new ClaimOption());
            }
        }

        private List<ClaimOption> _visionCylinders;
        public List<ClaimOption> VisionCylinders
        {
            get
            {
                return _visionCylinders;
            }
            private set
            {
                _visionCylinders = value;
                if (_visionCylinders != null && _visionCylinders[0] != null)
                    _visionCylinders.Insert(0, new ClaimOption());
            }
        }

        private List<ClaimOption> _visionAxes;
        public List<ClaimOption> VisionAxes
        {
            get
            {
                return _visionAxes;
            }
            private set
            {
                _visionAxes = value;
                if (_visionAxes != null && _visionAxes[0] != null)
                    _visionAxes.Insert(0, new ClaimOption());
            }
        }

        private List<ClaimOption> _visionPrisms;
        public List<ClaimOption> VisionPrisms
        {
            get
            {
                return _visionPrisms;
            }
            private set
            {
                _visionPrisms = value;
                if (_visionPrisms != null && _visionPrisms[0] != null)
                    _visionPrisms.Insert(0, new ClaimOption());
            }
        }

        private List<ClaimOption> _visionBifocals;
        public List<ClaimOption> VisionBifocals
        {
            get
            {
                return _visionBifocals;
            }
            private set
            {
                _visionBifocals = value;
                if (_visionBifocals != null && _visionBifocals[0] != null)
                    _visionBifocals.Insert(0, new ClaimOption());
            }
        }
        public Claim Claim { get; set; }

        private ClaimGSC _claimResults;
        public ClaimGSC ClaimResults
        {
            get
            {
                return _claimResults;
            }
            private set
            {
                _claimResults = value;
                SetClaimFormIDOnClaimResultDetails();
            }
        }

        public Guid SelectedTreatmentDetailID { get; set; }
        public List<ClaimAudit> ClaimAudits { get; set; }
        public ClaimResultResponse ClaimResult { get; private set; }
        public bool IsTreatmentDetailsListInNavStack { get; set; }
        private bool _ishcsaclaim;
        public bool IsHCSAClaim
        {
            get
            {
                return _ishcsaclaim;
            }
            set
            {
                if (_ishcsaclaim != value)
                {
                    _ishcsaclaim = value;
                    if (value && ClaimSubmissionTypes == null || ClaimSubmissionTypes.Count == 0)
                    {
                        GetClaimSubmissionTypes(_loginservice.CurrentPlanMemberID);
                    }
                }
            }
        }

        public string HCSAName { get; private set; }

        public async Task GetPhoneNumber(string planMemberID)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                PhoneError = true;
                _messenger.Publish<GetTextAlterationPhoneError>(new GetTextAlterationPhoneError(this) { Message = "no connection" });
                return;
            }
            var service = new ApiClient<List<TextAlteration>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/textalteration?tag=CCC_PHONE_NO", planMemberID));
            try
            {
                var tlist = await service.ExecuteRequest();
                PhoneTextAlterations = tlist.OrderBy(s => s.SequenceNo).ToList();
                PhoneText = tlist.FirstOrDefault();
                _messenger.Publish<GetTextAlterationPhoneComplete>(new GetTextAlterationPhoneComplete(this));
            }

            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetPhoneNumber(planMemberID));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NoPhoneNumber = true;
                    _messenger.Publish<NoTextAlterationPhoneFound>(new NoTextAlterationPhoneFound(this));
                }
                else
                {
                    _messenger.Publish<GetTextAlterationPhoneError>(new GetTextAlterationPhoneError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetTextAlterationPhoneError>(new GetTextAlterationPhoneError(this) { Message = ex.Message });
            }
        }

        public async Task GetClaimAgreement(string planMemberID)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                ClaimError = true;
                _messenger.Publish<GetTextAlterationClaimAgreementError>(new GetTextAlterationClaimAgreementError(this) { Message = "no connection" });
                return;
            }
            var service = new ApiClient<List<TextAlteration>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/textalteration?tag=CLAIM_AGREEMENT_MOBILE_1", planMemberID));
            try
            {
                List<TextAlteration> tlist = await service.ExecuteRequest();
                ClaimAgreementTextAlterations = tlist.OrderBy(s => s.SequenceNo).ToList();
                _messenger.Publish<GetTextAlterationClaimAgreementComplete>(new GetTextAlterationClaimAgreementComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimAgreement(planMemberID));
                }
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _messenger.Publish<NoTextAlterationClaimAgreementFound>(new NoTextAlterationClaimAgreementFound(this));
                }
                else
                {
                    _log.Trace(ex.ReasonPhrase);
                    _messenger.Publish<GetTextAlterationClaimAgreementError>(new GetTextAlterationClaimAgreementError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                _messenger.Publish<GetTextAlterationClaimAgreementError>(new GetTextAlterationClaimAgreementError(this) { Message = ex.Message });
            }
        }

        public async Task<TextAlteration_1Response> GetClaimAgreementTextAsync(string planMemberId)
        {
            TextAlteration_1Response response = new TextAlteration_1Response();
            var service = new ApiClient<List<TextAlteration>>(new Uri(SERVICE_BASE_URL),
                HttpMethod.Get,
                string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/textalteration?tag=CLAIM_AGREEMENT_MOBILE_2", planMemberId));
            try
            {
                var taList = await service.ExecuteRequest();
                ClaimAgreementTextAlterations_1 = taList.OrderBy(s => s.SequenceNo).ToList();
                response.Results = ClaimAgreementTextAlterations_1;
                response.ResponseCode = System.Net.HttpStatusCode.OK;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimAgreementTextAsync(planMemberId));
                }
                else
                {
                    response.ResponseCode = ex.StatusCode;
                    response.Message = ex.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<TextAlterationDateResponse> GetClaimAgreementDateAsync(string planMemberId)
        {
            var response = new TextAlterationDateResponse();
            var service = new ApiClient<List<TextAlteration>>(new Uri(SERVICE_BASE_URL),
                HttpMethod.Get,
                string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/textalteration?tag=CLAIM_AGREEMENT_DATE_MOBILE_1", planMemberId));
            try
            {
                var result = await service.ExecuteRequest();
                var resultItem = result.FirstOrDefault();

                ClaimAgreementTextAlterationsDate = resultItem != null ? resultItem.Text : string.Empty;
                response = new TextAlterationDateResponse()
                {
                    Results = resultItem.Text
                };

                response.ResponseCode = System.Net.HttpStatusCode.OK;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimAgreementDateAsync(planMemberId));
                }
                else
                {
                    response.ResponseCode = ex.StatusCode;
                    response.Message = ex.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task GetClaimDisclaimer(string planMemberID)
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                ClaimDisclaimerError = true;
                _messenger.Publish<GetTextAlterationClaimDisclaimerError>(new GetTextAlterationClaimDisclaimerError(this) { Message = "no connection" });
                return;
            }
            var service = new ApiClient<List<TextAlteration>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/textalteration?tag=CLAIM_DISCLAIMER_MOBILE_1", planMemberID), useDefaultHeaders: false);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("Bearer {0}", _loginservice.AuthInfo.AccessToken));
            headers.Add("Accept-Language", _languageservice.CurrentLanguage);
            try
            {
                List<TextAlteration> tlist = await service.ExecuteRequest(headers);
                ClaimDisclaimerTextAlterations = tlist.OrderBy(s => s.SequenceNo).ToList();
                _messenger.Publish<GetTextAlterationClaimDisclaimerComplete>(new GetTextAlterationClaimDisclaimerComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimDisclaimer(planMemberID));
                }
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _messenger.Publish<NoTextAlterationClaimDisclaimerFound>(new NoTextAlterationClaimDisclaimerFound(this));
                }
                else
                {
                    _messenger.Publish<GetTextAlterationClaimDisclaimerError>(new GetTextAlterationClaimDisclaimerError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetTextAlterationClaimDisclaimerError>(new GetTextAlterationClaimDisclaimerError(this) { Message = ex.Message });
            }
        }

        public async Task GetClaimSubmissionTypes(string planMemberID)
        {
            var service = new ApiClient<List<ClaimSubmissionType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/claimsubmissiontype", planMemberID));
            try
            {
                List<ClaimSubmissionType> typesList = await service.ExecuteRequest();
                ClaimSubmissionTypes = typesList;
                SetHCSAName();
                _messenger.Publish<GetClaimSubmissionTypesComplete>(new GetClaimSubmissionTypesComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimSubmissionTypes(planMemberID));
                }
                else
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _messenger.Publish<NoClaimSubmissionTypesFound>(new NoClaimSubmissionTypesFound(this));

                }
                {
                    _messenger.Publish<GetClaimSubmissionTypesError>(new GetClaimSubmissionTypesError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetClaimSubmissionTypesError>(new GetClaimSubmissionTypesError(this) { Message = ex.Message });
            }
        }

        public async Task GetClaimSubmissionBenefits(string claimSubmissionTypeID)
        {
            var service = new ApiClient<List<ClaimSubmissionBenefit>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimsubmissiontype/{0}/claimsubmissionbenefit", claimSubmissionTypeID));

            try
            {
                List<ClaimSubmissionBenefit> benefitsList = await service.ExecuteRequest();
                ClaimSubmissionBenefits = benefitsList;
                _messenger.Publish<GetClaimSubmissionBenefitsComplete>(new GetClaimSubmissionBenefitsComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimSubmissionBenefits(claimSubmissionTypeID));
                }
                else
                {
                    _messenger.Publish<GetClaimSubmissionBenefitsError>(new GetClaimSubmissionBenefitsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetClaimSubmissionBenefitsError>(new GetClaimSubmissionBenefitsError(this) { Message = ex.Message });
            }
        }

        public async Task GetClaimOptions(string optionType)
        {

            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type={0}", optionType));

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                ClaimOptions = optionsList;
                _messenger.Publish<GetClaimOptionsComplete>(new GetClaimOptionsComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimOptions(optionType));
                }
                else
                {
                    _messenger.Publish<GetClaimOptionsError>(new GetClaimOptionsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetClaimOptionsError>(new GetClaimOptionsError(this) { Message = ex.Message });
            }
        }

        public async Task GetTypesOfMedicalProfessional()
        {

            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type=medicalprofessional");

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                TypesOfMedicalProfessional = optionsList;
                _messenger.Publish<GetTypesOfMedicalProfessionalComplete>(new GetTypesOfMedicalProfessionalComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetTypesOfMedicalProfessional());
                }
                else
                {
                    _messenger.Publish<GetTypesOfMedicalProfessionalError>(new GetTypesOfMedicalProfessionalError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetTypesOfMedicalProfessionalError>(new GetTypesOfMedicalProfessionalError(this) { Message = ex.Message });
            }
        }

        public async Task GetLensTypes(string planMemberID)
        {
            var service = new ApiClient<List<LensType>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/lenstype", planMemberID));

            try
            {
                List<LensType> typesList = await service.ExecuteRequest();
                LensTypes = typesList;
                _messenger.Publish<GetLensTypesComplete>(new GetLensTypesComplete(this));

            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetLensTypes(planMemberID));
                }
                else
                {
                    _messenger.Publish<GetLensTypesError>(new GetLensTypesError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetLensTypesError>(new GetLensTypesError(this) { Message = ex.Message });
            }
        }

        public async Task GetVisionSpheres()
        {
            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type=visionsphere");

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                VisionSpheres = optionsList;
                _messenger.Publish<GetVisionSpheresComplete>(new GetVisionSpheresComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetVisionSpheres());
                }
                else
                {
                    _messenger.Publish<GetVisionSpheresError>(new GetVisionSpheresError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetVisionSpheresError>(new GetVisionSpheresError(this) { Message = ex.Message });
            }
        }

        public async Task GetVisionCylinders()
        {
            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type=visioncylinder");

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                VisionCylinders = optionsList;
                _messenger.Publish<GetVisionCylindersComplete>(new GetVisionCylindersComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetVisionCylinders());
                }
                else
                {
                    _messenger.Publish<GetVisionCylindersError>(new GetVisionCylindersError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetVisionCylindersError>(new GetVisionCylindersError(this) { Message = ex.Message });
            }
        }

        public async Task GetVisionAxes()
        {
            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type=visionaxis");

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                VisionAxes = optionsList;
                _messenger.Publish<GetVisionAxesComplete>(new GetVisionAxesComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetVisionAxes());
                }
                else
                {
                    _messenger.Publish<GetVisionAxesError>(new GetVisionAxesError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetVisionAxesError>(new GetVisionAxesError(this) { Message = ex.Message });
            }
        }

        public async Task GetVisionPrisms()
        {

            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type=visionprism");

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                VisionPrisms = optionsList;
                _messenger.Publish<GetVisionPrismsComplete>(new GetVisionPrismsComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetVisionPrisms());
                }
                else
                {
                    _messenger.Publish<GetVisionPrismsError>(new GetVisionPrismsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetVisionPrismsError>(new GetVisionPrismsError(this) { Message = ex.Message });
            }
        }

        public async Task GetVisionBifocals()
        {
            var service = new ApiClient<List<ClaimOption>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimoption?type=visionbifocal");

            try
            {
                List<ClaimOption> optionsList = await service.ExecuteRequest();
                VisionBifocals = optionsList;
                _messenger.Publish<GetVisionBifocalsComplete>(new GetVisionBifocalsComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetVisionBifocals());
                }
                else
                {
                    _messenger.Publish<GetVisionBifocalsError>(new GetVisionBifocalsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetVisionBifocalsError>(new GetVisionBifocalsError(this) { Message = ex.Message });
            }
        }

        public async Task SubmitClaim()
        {
            ClaimGSC gscClaim = CreateSubmitClaimRequestParameter();
            if (gscClaim == null)
            {
                _messenger.Publish<ClaimSubmissionError>(new ClaimSubmissionError(this) { Message = "Invalid Plan Member ID." });
                return;
            }

            var service = new ApiClient<ClaimGSC>(new Uri(SERVICE_BASE_URL), HttpMethod.Post, GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claim", apiBody: gscClaim);

            try
            {
                ClaimGSC returnedClaim = await service.ExecuteRequest();
                //TODO: REMOVE THIS!
#if ACFakeData
                    returnedClaim.Results.Add(GetFakeHCSAClaimResult());
#endif
                ClaimResults = returnedClaim;
                _messenger.Publish<ClaimSubmissionComplete>(new ClaimSubmissionComplete(this));
                return;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await SubmitClaim());
                }
                else
                {
                    _messenger.Publish<ClaimSubmissionError>(new ClaimSubmissionError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<ClaimSubmissionError>(new ClaimSubmissionError(this) { Message = ex.Message });
            }
        }

        public async Task SubmitClaimAsync(string comments, string uploadDocumentProcessType, IEnumerable<DocumentInfo> documents)
        {
            var gscClaim = CreateSubmitClaimRequestParameter();
            gscClaim.Comments = comments;
            gscClaim.Type = uploadDocumentProcessType;

            var apiClient = new ApiClient<HttpResponseMessage>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Post,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/upload");

            try
            {
                var uploadParameterString = JsonConvert.SerializeObject(gscClaim);
                var uploadParameterBytes = Encoding.UTF8.GetBytes(uploadParameterString);
                string base64 = Convert.ToBase64String(uploadParameterBytes);

                await apiClient.UploadDocumentsAsync(documents, base64);                
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await SubmitClaimAsync(comments, uploadDocumentProcessType, documents));                    
                }
                else
                {                    
                    throw;
                }                
            }
            catch
            {
                throw;
            }
        }

        // TODO: Remove, instead we should use IClaimExService
        public async Task<IEnumerable<ClaimCOP>> GetCOPClaimsAsync()
        {
            if (string.IsNullOrEmpty(_loginservice.CurrentPlanMemberID))
            {
                _messenger.Publish(new GetClaimCOPError(this)
                {
                    Message = "Plan Member ID is null."
                });
                return new List<ClaimCOP>();
            }

            var service = new ApiClient<IEnumerable<ClaimCOP>>(new Uri(SERVICE_BASE_URL),
                                                        HttpMethod.Post,
                                                        string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/claimlist", _loginservice.GroupPlanNumber),
                                                        apiBody: new ClaimStatusType() { ClaimType = UploadDocumentProcessType.COP });

            try
            {
                return await service.ExecuteRequest();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetCOPClaimsAsync());
                }
                else
                {
                    _messenger.Publish(new GetClaimCOPError(this)
                    {
                        Message = ex.ReasonPhrase
                    });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish(new GetClaimCOPError(this)
                {
                    Message = ex.Message
                });
            }
            return null;
        }

        public async Task<IEnumerable<ClaimSummary>> GetCOPClaimSummaryAsync(string claimFormId)
        {
            var service = new ApiClient<IEnumerable<ClaimSummary>>(new Uri(SERVICE_BASE_URL),
                                                  HttpMethod.Get,
                                                  string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/claimsummary/{0}", claimFormId));
            try
            {
                var claimSummary = await service.ExecuteRequest();
                return claimSummary;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetCOPClaimsAsync());
                }
                else
                {
                    _messenger.Publish(new GetClaimCOPError(this)
                    {
                        Message = ex.ReasonPhrase
                    });
                }
            }

            return null;
        }

        public async Task<bool> UploadDocumentsAsync(string comments, string claimId, string uploadDocumentProcessType, IEnumerable<DocumentInfo> documents, string participantNumber = null)
        {
            var uploadParameters = new Dictionary<string, string>();
            uploadParameters.Add("type", uploadDocumentProcessType);
            uploadParameters.Add("claimFormId", claimId);
            uploadParameters.Add("participantNumber", string.IsNullOrWhiteSpace(participantNumber) ? _loginservice.ParticipantNumber : participantNumber);
            uploadParameters.Add("comments", comments);

            var service = new ApiClient<HttpResponseMessage>(new Uri(SERVICE_BASE_URL),
                HttpMethod.Post,
                GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/upload");

            try
            {
                await service.UploadDocumentsAsync(documents, uploadParameters);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await UploadDocumentsAsync(comments, claimId, uploadDocumentProcessType, documents, participantNumber));
                }
                else
                {
                    _messenger.Publish(new COPDocumentsSubmissionError(this) { Message = ex.ReasonPhrase });
                    return false;
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish(new COPDocumentsSubmissionError(this) { Message = ex.Message });
                return false;
            }

            return true;
        }

        public async Task GetClaimAudits()
        {
            if (string.IsNullOrEmpty(_loginservice.CurrentPlanMemberID))
            {
                _messenger.Publish<GetClaimAuditsError>(new GetClaimAuditsError(this) { Message = "Plan Member ID is null." });
                return;
            }

            var service = new ApiClient<List<ClaimAudit>>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format(GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/claimaudit", _loginservice.GroupPlanNumber));


            try
            {
                List<ClaimAudit> auditsList = await service.ExecuteRequest();
                ClaimAudits = auditsList;
                _messenger.Publish<GetClaimAuditsComplete>(new GetClaimAuditsComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimAudits());
                }
                else
                {
                    _messenger.Publish<GetClaimAuditsError>(new GetClaimAuditsError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetClaimAuditsError>(new GetClaimAuditsError(this) { Message = ex.Message });
            }
        }

        // TODO: Remove, instead we should use IClaimExService.GetAuditClaimsAsync
        public async Task<IList<ClaimAudit>> GetAuditClaimsAsync()
        {
            var auditsList = new List<ClaimAudit>();

            var service = new ApiClient<List<ClaimAudit>>(new Uri(SERVICE_BASE_URL),
                HttpMethod.Post,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{_loginservice.GroupPlanNumber}/claimlist",
                apiBody: new ClaimStatusType() { ClaimType = UploadDocumentProcessType.Audit });

            try
            {
                auditsList = await service.ExecuteRequest();
                ClaimAudits = auditsList;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetAuditClaimsAsync());
                }
            }

            return auditsList;
        }

        public async Task<ClaimResultResponse> GetClaimResultByID(string claimFormId)
        {
            var service = new ApiClient<ClaimResultResponse>(new Uri(SERVICE_BASE_URL), HttpMethod.Get,
                GSCHelper.GSC_SERVICE_BASE_URL_SUB + $"/api/claimresult/{claimFormId}");
            var result = new ClaimResultResponse();

            try
            {
                result = await service.ExecuteRequest();
                ClaimResult = result;
                _messenger.Publish(new GetClaimResultByIDComplete(this));
                return result;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetClaimAgreement(claimFormId));
                }
                else
                {
                    _messenger.Publish<GetClaimResultByIDError>(new GetClaimResultByIDError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetClaimResultByIDError>(new GetClaimResultByIDError(this) { Message = ex.Message });
            }

            return result;
        }

        public void PersistClaim()
        {
            _dataservice.PersistClaim(this.Claim);
        }

        public void ClearClaimDetails()
        {
            this.Claim = null;
            // fix for 29626 - reset ClaimAudits to null if new user is logged in
            this.ClaimAudits = null;
            this.SelectedClaimSubmissionType = null;
            this.ClaimSubmissionTypes = new List<ClaimSubmissionType>();
            this.ClaimSubmissionBenefits = new List<ClaimSubmissionBenefit>();
            this.ClaimOptions = null;
            this.LensTypes = null;
            this.VisionSpheres = null;
            this.VisionCylinders = null;
            this.VisionAxes = null;
            this.VisionPrisms = null;
            this.VisionBifocals = null;
            this.SelectedTreatmentDetailID = Guid.Empty;
            this.IsHCSAClaim = false;
            var hcsaservice = Mvx.IoCProvider.Resolve<IHCSAClaimService>();
            hcsaservice.Claim = null;
            hcsaservice.SelectedClaimType = null;
            hcsaservice.SelectedExpenseType = null;
            hcsaservice.PersistClaim();
            PersistClaim();
        }

        public void ClearOldClaimData()
        {
            //clean everything except ClaimSubmissionTypes out
            this.Claim = null;
            this.SelectedClaimSubmissionType = null;
            this.ClaimSubmissionBenefits = new List<ClaimSubmissionBenefit>();
            this.ClaimOptions = null;
            this.LensTypes = null;
            this.VisionSpheres = null;
            this.VisionCylinders = null;
            this.VisionAxes = null;
            this.VisionPrisms = null;
            this.VisionBifocals = null;
            this.SelectedTreatmentDetailID = Guid.Empty;

            this.IsHCSAClaim = false;
            var hcsaservice = Mvx.IoCProvider.Resolve<IHCSAClaimService>();
            hcsaservice.Claim = null;
            hcsaservice.PersistClaim();
            PersistClaim();
        }

        private string GetClaimOptionTypeByClaimSubmissionType(string submissionType)
        {
            string optionType = string.Empty;
            switch (submissionType)
            {
                case "MASSAGE":
                    optionType = "massagetime";
                    break;
                case "NATUROPATHY":
                    optionType = "naturopathictime";
                    break;
                case "SPEECH":
                    optionType = "speechtherapytime";
                    break;
                case "PSYCHOLOGY":
                    optionType = "psychologytime";
                    break;
            }
            return optionType;
        }

        private ClaimGSC CreateSubmitClaimRequestParameter()
        {
            ClaimGSC gscClaim = new ClaimGSC();

            string[] planMemberIDAndParticipantNumber = Claim.Participant.PlanMemberID.Split('-');

            // TODO: This is a workaround - there is a problem with selecting the plan participant that adds a set of numbers to the ID.  
            // Find that and then set the valid length to 2.
            // will do this on the weekend of June 9th
            if (planMemberIDAndParticipantNumber.Length != 2)
            {
                return null; //abort - missing Plan Member ID and/or Participant Number
            }

            gscClaim.PlanMemberID = long.Parse(planMemberIDAndParticipantNumber[0]);
            gscClaim.ParticipantNumber = planMemberIDAndParticipantNumber[1];
            gscClaim.ClaimSubmissionTypeID = Claim.Type.ID;
            gscClaim.ServiceProvider = Claim.Provider;

            //Other Benefits
            gscClaim.IsCoveredUnderAnotherPlan = Claim.CoverageUnderAnotherBenefitsPlan;
            if (gscClaim.IsCoveredUnderAnotherPlan)
            {
                gscClaim.IsCoveredUnderAnotherGSCPlan = Claim.IsOtherCoverageWithGSC;
                gscClaim.IsSubmittedToOtherPlan = Claim.HasClaimBeenSubmittedToOtherBenefitPlan;

                if (gscClaim.IsCoveredUnderAnotherGSCPlan && !gscClaim.IsSubmittedToOtherPlan)
                {
                    gscClaim.PayUnpaidAmountThroughOtherGSCPlan = Claim.PayAnyUnpaidBalanceThroughOtherGSCPlan;

                    if (gscClaim.PayUnpaidAmountThroughOtherGSCPlan)
                    {
                        gscClaim.OtherGSCPlanMemberID = Claim.OtherGSCNumber;
                        gscClaim.OtherGSCParticipantNumber = Claim.OtherGSCParticipantNumber;
                    }
                }
            }
            gscClaim.UseSpendingAccountAutoCoordination = Claim.PayUnderHCSA;

            //Motor Vehicle
            gscClaim.IsRequiredDueToMotorVehicleAccident = Claim.IsTreatmentDueToAMotorVehicleAccident;
            if (gscClaim.IsRequiredDueToMotorVehicleAccident)
            {
                gscClaim.MotorVehicleAccidentDate = Claim.DateOfMotorVehicleAccident.ToString(CLAIM_DATE_FORMAT);
            }

            //Work Injury
            gscClaim.IsRequiredDueToWorkInjury = Claim.IsTreatmentDueToAWorkRelatedInjury;
            if (gscClaim.IsRequiredDueToWorkInjury)
            {
                gscClaim.WorkRelatedInjuryDate = Claim.DateOfWorkRelatedInjury.ToString(CLAIM_DATE_FORMAT);
                gscClaim.WorkRelatedInjuryCaseNumber = Claim.WorkRelatedInjuryCaseNumber;
            }


            //Other type of accident
            gscClaim.IsRequiredDueToDentalAccident = Claim.IsOtherTypeOfAccident;

            //Medical Item
            if (gscClaim.ClaimSubmissionTypeID == "MI")
            {
                gscClaim.IsRequiredForSport = Claim.IsMedicalItemForSportsOnly;
            }
            else
            {
                if (Claim.HasReferralBeenPreviouslySubmitted)
                {
                    gscClaim.IsReferralSubmitted = false;
                }
                else
                {
                    gscClaim.IsReferralSubmitted = true;
                }
                //gscClaim.IsReferralSubmitted = Claim.HasReferralBeenPreviouslySubmitted;
                if (!gscClaim.IsReferralSubmitted)
                {
                    if (Claim.DateOfReferral > DateTime.MinValue) gscClaim.ReferralDate = Claim.DateOfReferral.ToString(CLAIM_DATE_FORMAT);
                    if (Claim.TypeOfMedicalProfessional != null) gscClaim.MedicalProfessionalId = Claim.TypeOfMedicalProfessional.ID;
                }
            }

            //Question 16
            gscClaim.IsGSTIncluded = Claim.IsGSTHSTIncluded;

            //Details
            gscClaim.Details = GetGSCClaimDetails(gscClaim.ClaimSubmissionTypeID);

            return gscClaim;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetails(string claimSubmissionTypeID)
        {
            switch (claimSubmissionTypeID)
            {
                case "ACUPUNCTURE":
                case "CHIROPODY":
                case "CHIRO":
                case "PHYSIO":
                case "PODIATRY":
                    return GetGSCClaimDetailsForEntry1();
                case "PSYCHOLOGY":
                case "MASSAGE":
                case "NATUROPATHY":
                case "SPEECH":
                    return GetGSCClaimDetailsForEntry2();
                case "MI":
                    return GetGSCClaimDetailsForMI();
                case "ORTHODONTIC":
                    return GetGSCClaimDetailsForOMF();
                case "CONTACTS":
                    return GetGSCClaimDetailsForPC();
                case "GLASSES":
                    return GetGSCClaimDetailsForPG();
                case "EYEEXAM":
                    return GetGSCClaimDetailsForREE();
                case "DENTAL":
                    return GetGSCClaimDetailsForDental();
                default:
                    return null;
            }
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForDental()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();
            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.ProcedureCode = td.ProcedureCode;
                detail.TreatmentDate = td.TreatmentDate.ToString(CLAIM_DATE_FORMAT);
                detail.ToothCode = td.ToothCode.ToString();
                detail.ToothSurface = td.ToothSurface;
                detail.DentalLabFee = td.LaboratoryCharges ?? 0;
                detail.DentalFee = td.DentistFees ?? 0;
                detail.OtherPaidAmount = td.AlternateCarrierPayment;
                detail.ClaimAmount = (td.LaboratoryCharges ?? 0) + (td.DentistFees ?? 0);
                details.Add(detail);
            }
            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForEntry1()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.ProcedureCode = td.TypeOfTreatment.ProcedureCode;
                detail.TreatmentDate = td.TreatmentDate.ToString(CLAIM_DATE_FORMAT);
                detail.ClaimAmount = td.TreatmentAmount;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;

                details.Add(detail);
            }

            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForEntry2()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.ProcedureCode = td.TypeOfTreatment.ProcedureCode;
                detail.LengthOfTreatment = td.TreatmentDuration.ID;
                detail.TreatmentDate = td.TreatmentDate.ToString(CLAIM_DATE_FORMAT);
                detail.ClaimAmount = td.TreatmentAmount;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;

                details.Add(detail);
            }

            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForMI()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.ProcedureCode = td.ItemDescription.ProcedureCode;
                detail.TreatmentDate = td.PickupDate.ToString(CLAIM_DATE_FORMAT);
                detail.Quantity = td.Quantity;
                detail.ClaimAmount = td.TreatmentAmount;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;
                detail.IsGSTIncluded = td.GSTHSTIncludedInTotal;
                detail.IsPSTIncluded = td.PSTIncludedInTotal;
                detail.IsReferralSubmitted = td.IsReferralNotSubmitted;
                if (!detail.IsReferralSubmitted)
                {
                    if (td.DateOfReferral > DateTime.MinValue) detail.ReferralDate = td.DateOfReferral.ToString(CLAIM_DATE_FORMAT);
                    if (td.TypeOfMedicalProfessional != null) detail.MedicalProfessionalID = td.TypeOfMedicalProfessional.ID;
                }

                details.Add(detail);
            }

            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForOMF()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.ProcedureCode = td.TypeOfTreatment.ProcedureCode;
                detail.TreatmentDate = td.DateOfMonthlyTreatment.ToString(CLAIM_DATE_FORMAT);
                detail.ClaimAmount = td.OrthodonticMonthlyFee;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;

                details.Add(detail);
            }

            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForPC()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.TreatmentDate = td.DateOfPurchase.ToString(CLAIM_DATE_FORMAT);
                detail.ProcedureCode = td.TypeOfEyewear.ProcedureCode;
                if (td.IsAcknowledgeEyewearIsCorrectiveVisible)
                    detail.IsCorrectiveEyewear = td.AcknowledgeEyewearIsCorrective;
                detail.ClaimAmount = td.TreatmentAmount;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;
#if CCQ || FPPM
            
#else
                if ((string.Equals(SelectedClaimSubmissionType.ID, "GLASSES", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(SelectedClaimSubmissionType.ID, "CONTACTS", StringComparison.OrdinalIgnoreCase))
                    && Claim.Participant.IsOrUnderAgeOf18()
                    && Claim.Participant.IsResidentOfQuebecProvince())
                {
                    detail.PublicOrGvtAmount = td.AmountPaidByPPorGP;
                }
#endif
                //Prescription Details
                if (td.IsPrescriptionDetailsVisible)
                {
                    if (detail.ProcedureCode == "61500") // Prescription Contacts, Pair(s)
                    {
                        detail.RightSphere = td.RightSphere.ID;
                        detail.RightCylinder = td.RightCylinder.ID;
                        detail.RightAxis = td.RightAxis.ID;
                        detail.RightPrism = td.RightPrism.ID;
                        detail.LeftSphere = td.LeftSphere.ID;
                        detail.LeftCylinder = td.LeftCylinder.ID;
                        detail.LeftAxis = td.LeftAxis.ID;
                        detail.LeftPrism = td.LeftPrism.ID;
                    }
                    else if (detail.ProcedureCode == "62400") // Prescription Contact(s), Left Only
                    {
                        detail.LeftSphere = td.LeftSphere.ID;
                        detail.LeftCylinder = td.LeftCylinder.ID;
                        detail.LeftAxis = td.LeftAxis.ID;
                        detail.LeftPrism = td.LeftPrism.ID;
                    }
                    else if (detail.ProcedureCode == "62500") // Prescription Contact(s), Right Only
                    {
                        detail.RightSphere = td.RightSphere.ID;
                        detail.RightCylinder = td.RightCylinder.ID;
                        detail.RightAxis = td.RightAxis.ID;
                        detail.RightPrism = td.RightPrism.ID;
                    }

                    if (td.RightBifocal != null)
                        detail.RightBifocal = td.RightBifocal.ID;
                    if (td.LeftBifocal != null)
                        detail.LeftBifocal = td.LeftBifocal.ID;
                }

                details.Add(detail);
            }

            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForPG()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.TreatmentDate = td.DateOfPurchase.ToString(CLAIM_DATE_FORMAT);
                detail.ProcedureCode = td.TypeOfEyewear.ProcedureCode;
                detail.LensTypeCode = td.TypeOfLens.ID;
                if (td.IsTotalAmountChargedVisible)
                    detail.ClaimAmount = td.TotalAmountCharged;
                else
                    detail.ClaimAmount = td.FrameAmount + td.EyeglassLensesAmount + td.FeeAmount;
                detail.FrameAmount = td.FrameAmount.ToString();
                detail.LensAmount = td.EyeglassLensesAmount.ToString();
                detail.FeeAmount = td.FeeAmount.ToString();
                if (td.IsAcknowledgeEyewearIsCorrectiveVisible)
                    detail.IsCorrectiveEyewear = td.AcknowledgeEyewearIsCorrective;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;

#if CCQ || FPPM
            
#else
                if ((string.Equals(SelectedClaimSubmissionType.ID, "GLASSES", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(SelectedClaimSubmissionType.ID, "CONTACTS", StringComparison.OrdinalIgnoreCase))
                    && Claim.Participant.IsOrUnderAgeOf18()
                    && Claim.Participant.IsResidentOfQuebecProvince())
                {
                    detail.PublicOrGvtAmount = td.AmountPaidByPPorGP;
                }
#endif
                //Prescription Details
                if (td.IsPrescriptionDetailsVisible)
                {
                    if (detail.ProcedureCode == "60000" || detail.ProcedureCode == "60500" || detail.ProcedureCode == "62000") // Prescription Glasses, Frame and Lenses; Prescription Glasses, Frame Only; Prescription Lenses, Pair
                    {
                        detail.RightSphere = td.RightSphere.ID;
                        detail.RightCylinder = td.RightCylinder.ID;
                        detail.RightAxis = td.RightAxis.ID;
                        detail.RightPrism = td.RightPrism.ID;
                        detail.LeftSphere = td.LeftSphere.ID;
                        detail.LeftCylinder = td.LeftCylinder.ID;
                        detail.LeftAxis = td.LeftAxis.ID;
                        detail.LeftPrism = td.LeftPrism.ID;
                    }
                    else if (detail.ProcedureCode == "62200") // Prescription Lens, Left Only
                    {
                        detail.LeftSphere = td.LeftSphere.ID;
                        detail.LeftCylinder = td.LeftCylinder.ID;
                        detail.LeftAxis = td.LeftAxis.ID;
                        detail.LeftPrism = td.LeftPrism.ID;
                    }
                    else if (detail.ProcedureCode == "62300") // Prescription Lens, Right Only
                    {
                        detail.RightSphere = td.RightSphere.ID;
                        detail.RightCylinder = td.RightCylinder.ID;
                        detail.RightAxis = td.RightAxis.ID;
                        detail.RightPrism = td.RightPrism.ID;
                    }

                    if (td.RightBifocal != null)
                        detail.RightBifocal = td.RightBifocal.ID;
                    if (td.LeftBifocal != null)
                        detail.LeftBifocal = td.LeftBifocal.ID;
                }

                details.Add(detail);
            }

            return details;
        }

        private List<ClaimDetailGSC> GetGSCClaimDetailsForREE()
        {
            List<ClaimDetailGSC> details = new List<ClaimDetailGSC>();

            foreach (TreatmentDetail td in Claim.TreatmentDetails)
            {
                ClaimDetailGSC detail = new ClaimDetailGSC();
                detail.ProcedureCode = td.TypeOfTreatment.ProcedureCode;
                detail.TreatmentDate = td.DateOfExamination.ToString(CLAIM_DATE_FORMAT);
                detail.ClaimAmount = td.TreatmentAmount;
                if (Claim.HasClaimBeenSubmittedToOtherBenefitPlan)
                    detail.OtherPaidAmount = td.AlternateCarrierPayment;

                details.Add(detail);
            }

            return details;
        }

        private void SetClaimFormIDOnClaimResultDetails()
        {
            if (ClaimResults != null)
            {
                if (ClaimResults.Results != null)
                {
                    foreach (ClaimResultGSC result in ClaimResults.Results)
                    {
                        foreach (ClaimResultDetailGSC detail in result.ClaimResultDetails)
                        {
                            detail.ClaimFormID = result.ClaimFormID;
                            detail.HasHCSADetails = (result.ResultTypeID == 2);
                        }
                    }
                }
            }
        }

        private void SetHCSAName()
        {
            HCSAName = Resource.HealthCareSpendingAccount; //default
            if (ClaimSubmissionTypes != null && ClaimSubmissionTypes.Count > 0)
            {
                ClaimSubmissionType hc = ClaimSubmissionTypes.FirstOrDefault(cst => cst.ID == "HC");
                if (hc != null)
                    HCSAName = hc.Name;
            }
        }

        private ClaimResultGSC GetFakeHCSAClaimResult()
        {
            List<ClaimEOBMessageGSC> fakeEOBs = new List<ClaimEOBMessageGSC>();
            ClaimEOBMessageGSC fakeEOB = new ClaimEOBMessageGSC
            {
                Message = "Claim coordinated with your other payor."
            };

            List<ClaimResultDetailGSC> fakeDetails = new List<ClaimResultDetailGSC>();
            ClaimResultDetailGSC fakeResultDetail = new ClaimResultDetailGSC
            {
                ClaimID = 267068364,
                ServiceDate = DateTime.Parse("2015/12/09"),
                ServiceDescription = "Chiropractor, initial",
                ClaimedAmount = 15.0,
                OtherPaidAmount = 5.0,
                DeductibleAmount = 0.0,
                CopayAmount = 0.0,
                PaidAmount = 0.0,
                ClaimStatus = "Awaiting payment",
                EOBMessages = fakeEOBs
            };
            fakeDetails.Add(fakeResultDetail);

            ClaimResultGSC fake = new ClaimResultGSC
            {
                ResultTypeID = 2,
                ClaimFormID = 222464890,
                PlanMemberDisplayID = "5938984 - 00",
                ParticipantFullName = "AMY, APPLE",
                SubmissionDate = DateTime.Parse("2015-12-18T13:41:38"),
                ClaimResultDetails = fakeDetails,
                SpendingAccountModelName = "Health Care Spending Account"
            };

            return fake;
        }
    }
}