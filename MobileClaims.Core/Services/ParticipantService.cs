using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using MobileClaims.Core.Services.FacadeEntities;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class ParticipantService : ServiceBase, IParticipantService
    {
        private readonly ILoginService _loginservice;
        private readonly IMvxMessenger _messenger;
        private readonly IDataService _dataservice;
        private readonly IMvxFileStore _filesystem;
        private readonly ILanguageService _languageservice;
        private object _sync = new object();
        private const string SERVICE_BASE_URL = GSCHelper.GSC_SERVICE_BASE_URL;
        private string CURRENT_LANGUAGE;
        private MvxSubscriptionToken _languageupdated;

        public ParticipantService(ILoginService loginservice, IMvxMessenger messenger, IDataService dataservice, IMvxFileStore filesystem, ILanguageService languageservice)
        {
            _loginservice = loginservice;
            _messenger = messenger;
            _dataservice = dataservice;
            _filesystem = filesystem;
            _languageservice = languageservice;
            ReloadPersistedParticipant();
            this.IDCard = _dataservice.GetIDCard();

            _languageupdated = _messenger.Subscribe<LanguageUpdatedMessage>((message) =>
            {
                CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
            });

            CURRENT_LANGUAGE = _languageservice.CurrentLanguage;
        }

        public PlanMember PlanMember { get; private set; }
        public IDCard IDCard { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public UserAgreement UserAgreement { get; private set; }


        private Participant selectedparticipant;
        public Participant SelectedParticipant
        {
            get
            {
                return selectedparticipant;
            }
            set
            {
                selectedparticipant = value;
            }
        }
        public Participant SelectedDrugParticipant { get; set; }

        public void ReloadPersistedParticipant()
        {
            this.PlanMember = _dataservice.GetCardPlanMember();
            if (this.PlanMember != null)
                _loginservice.CurrentPlanMemberID = this.PlanMember.PlanMemberID;
        }

        public async Task GetParticipant(string planMemberID)
        {
            await ExecuteParticipantCall(planMemberID);
        }

        private async Task ExecuteParticipantCall(string planMemberID)
        {
            var service = new ApiClient<PlanMemberGSC>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}", planMemberID));
            try
            {
                var pmGSC = await service.ExecuteRequest();
                this.PlanMember = GetMobileClaimsPlanMemberFromGSCPlanMember(pmGSC);
                _dataservice.PersistCardPlanMember(this.PlanMember);
                _messenger.Publish<RetrievedPlanMemberMessage>(new RetrievedPlanMemberMessage(this, this.PlanMember));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)

                {
                    RetryWithRefresh(async () => await ExecuteParticipantCall(planMemberID));
                }
                else
                {
                    _messenger.Publish<GetParticipantError>(new GetParticipantError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception exc)
            {
                _messenger.Publish<GetParticipantError>(new GetParticipantError(this) { Message = exc.Message });
            }

        }

        public async Task<UserAgreement> GetUserAgreement()
        {
            return await ExecuteUserAgreementCall(PlanMember.PlanMemberID);
        }

        private async Task<UserAgreement> ExecuteUserAgreementCall(string planMemberID)
        {
            string trimmedPlanMemberID = planMemberID;
            if (trimmedPlanMemberID.IndexOf('-') > -1)
                trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
            var service = new ApiClient<UserAgreement>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/useragreement/C4L", trimmedPlanMemberID));
            try
            {
                UserAgreement = await service.ExecuteRequest();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await ExecuteUserAgreementCall(planMemberID));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    UserAgreement ua = new UserAgreement(false);
                    this.UserAgreement = ua;
                }
                else
                {
                    _messenger.Publish<GetUserAgreementError>(new GetUserAgreementError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetUserAgreementError>(new GetUserAgreementError(this) { Message = ex.Message });
            }
            return UserAgreement;
        }

        public async Task PutUserAgreement(UserAgreement ua)
        {
            string trimmedPlanMemberID = PlanMember.PlanMemberID;
            if (trimmedPlanMemberID.IndexOf('-') > -1)
                trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
            var service = new ApiClient<UserAgreement>(new Uri(SERVICE_BASE_URL), HttpMethod.Put, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/useragreement/C4L", trimmedPlanMemberID), apiBody: ua);
            try
            {
                UserAgreement = await service.ExecuteRequest();
                _messenger.Publish<PutUserAgreementSuccessful>(new PutUserAgreementSuccessful(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await PutUserAgreement(ua));
                }
                else
                {
                    _messenger.Publish<PutUserAgreementError>(new PutUserAgreementError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<PutUserAgreementError>(new PutUserAgreementError(this) { Message = ex.Message });
            }
        }

        public async Task GetUserAgreementWCS()
        {
            await ExecuteUserAgreementCallWCS(PlanMember.PlanMemberID);
        }

        private async Task ExecuteUserAgreementCallWCS(string planMemberID)
        {
            string trimmedPlanMemberID = planMemberID;
            if (trimmedPlanMemberID.IndexOf('-') > -1)
                trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
            var service = new ApiClient<UserAgreement>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/useragreement/WCS", trimmedPlanMemberID));
            try
            {
                UserAgreement = await service.ExecuteRequest();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await ExecuteUserAgreementCallWCS(planMemberID));
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    UserAgreement ua = new UserAgreement("WCS", false);
                    this.UserAgreement = ua;
                }
                else
                {
                    _messenger.Publish<GetUserAgreementError>(new GetUserAgreementError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetUserAgreementError>(new GetUserAgreementError(this) { Message = ex.Message });
            }
        }

        public async Task<PutAgreementWCSResponse> PutUserAgreementWCSAsync(UserAgreement userAgreement)
        {
            var trimmedPlanMemberID = PlanMember.PlanMemberID;
            if (trimmedPlanMemberID.IndexOf('-') > -1)
            {
                trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
            }

            var service = new ApiClient<UserAgreement>(new Uri(SERVICE_BASE_URL), HttpMethod.Put, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/useragreement/WCS", trimmedPlanMemberID), apiBody: userAgreement);
            var response = new PutAgreementWCSResponse();
            try
            {
                UserAgreement = await service.ExecuteRequest();
                response.Results = UserAgreement;
                response.ResponseCode = System.Net.HttpStatusCode.OK;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await PutUserAgreementWCSAsync(userAgreement));
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

        public async Task GetUserProfile(string planMemberID)
        {
            string trimmedPlanMemberID = planMemberID;
            if (trimmedPlanMemberID.IndexOf('-') > -1)
                trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
            var service = new ApiClient<UserProfile>(new Uri(SERVICE_BASE_URL), HttpMethod.Get, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/userprofile", trimmedPlanMemberID));
            try
            {
                UserProfile = await service.ExecuteRequest();
                _messenger.Publish<GetUserProfileComplete>(new GetUserProfileComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await GetUserProfile(planMemberID));
                }
                else
                {
                    _messenger.Publish<GetUserProfileError>(new GetUserProfileError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<GetUserProfileError>(new GetUserProfileError(this) { Message = ex.Message });
            }
        }

        public async Task PutUserProfile(UserProfile up)
        {
            var service = new ApiClient<UserProfile>(new Uri(SERVICE_BASE_URL), HttpMethod.Put, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/userprofile", up.PlanMemberIDWithoutParticipantCode), apiBody: up);
            try
            {
                var upResponse = await service.ExecuteRequest();
                _messenger.Publish<PutUserProfileComplete>(new PutUserProfileComplete(this));
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    RetryWithRefresh(async () => await PutUserProfile(up));
                }
                else
                {
                    _messenger.Publish<PutUserProfileError>(new PutUserProfileError(this) { Message = ex.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                _messenger.Publish<PutUserProfileError>(new PutUserProfileError(this) { Message = ex.Message });
            }
        }

        private PlanMember GetMobileClaimsPlanMemberFromGSCPlanMember(PlanMemberGSC pmGSC)
        {
            PlanMember pm = new PlanMember()
            {
                FirstName = pmGSC.FirstName,
                LastName = pmGSC.LastName,
                LastThenFirst = false,
                Email = pmGSC.Email,
                Status = pmGSC.Status,
                PlanMemberID = string.Format("{0}-{1}", pmGSC.PlanMemberID.ToString(), pmGSC.ParticipantNumber),
                ProvinceCode = pmGSC.ProviceCode,
                DateOfBirth = pmGSC.DateOfBirth
            };

            List<Participant> dependents = new List<Participant>();
            if (pmGSC.Participants != null)
            {
                foreach (ParticipantGSC pGSC in pmGSC.Participants)
                {
                    Participant p = new Participant()
                    {
                        FirstName = pGSC.FirstName,
                        LastName = pGSC.LastName,
                        Status = pGSC.Status,
                        LastThenFirst = false,
                        PlanMemberID = string.Format("{0}-{1}", pmGSC.PlanMemberID.ToString(), pGSC.ParticipantNumber),
                        ProvinceCode = pGSC.ProviceCode,
                        DateOfBirth = pGSC.BirthDate
                    };
                    dependents.Add(p);
                }
            }
            pm.Dependents = dependents;

            pm.PlanConditions = pmGSC.PlanConditions;

            return pm;
        }

        public void ClearOldClaimData()
        {
            this.SelectedParticipant = null;
        }
    }
}
