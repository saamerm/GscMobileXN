//#define FakingIt
//#define C4LFakingIt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.Services.FacadeEntities;
using MvvmCross;
using MvvmCross.Logging;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services
{
    public class LoginService : ApiClientHelper, ILoginService
    {
        private readonly IMvxLog _log;
        private readonly IMenuService _menuService;
        private readonly IDataService _dataService;
        private readonly ILoggerService _loggerService;
        private readonly ILanguageService _languageService;
        private readonly IRehydrationService _rehydrationService;

        private IClaimService _claimService;
        private IClaimsHistoryService _claimsHistoryService;
        private IParticipantService _participantService;

        private DateTime _lastLogin;
        private bool _isloggedin;
        private bool _isXeroxSupported;
        private string _authHeader = "Basic ";
        private string _authApiUrl = GSCHelper.GSC_AUTH_BASE_URL;
        private string _authApiPath = GSCHelper.GSC_AUTH_BASE_URL_SUB;
        private string _c4LAuthApiUrl = GSCHelper.C4L_AUTH_BASE_URL;

        public bool ShouldExit { get; set; }

        public bool IsLoggedIn
        {
            get => _isloggedin;
            set
            {
                _isloggedin = value;
                _dataService.PersistLoggedInState(value);
            }
        }

        public string CurrentPlanMemberID { get; set; }

        public string GroupPlanNumber
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CurrentPlanMemberID))
                {
                    var groupNumberWithParticipant = CurrentPlanMemberID.Split('-');
                    if (groupNumberWithParticipant.Any())
                    {
                        return groupNumberWithParticipant[0];
                    }
                }

                return CurrentPlanMemberID;
            }
        }

        public string ParticipantNumber
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CurrentPlanMemberID))
                {
                    var groupNumberWithParticipant = CurrentPlanMemberID.Split('-');
                    if (groupNumberWithParticipant.Count() > 1)
                    {
                        return groupNumberWithParticipant[1];
                    }
                }

                return CurrentPlanMemberID;
            }
        }

        public string CurrentUser { get; private set; }

        public AuthInfo AuthInfo { get; private set; }

        public C4LOAuth C4LOAuth { get; private set; }

        public DateTime LastLogin
        {
            get
            {
                if (_lastLogin == DateTime.MinValue)
                {
                    _lastLogin = _dataService.GetLastLogin();
                }

                return _lastLogin;
            }
            set
            {
                _lastLogin = value;
                _dataService.PersistLastLogin(_lastLogin);
            }
        }

        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public LoginService(IMenuService menuService,
            IDataService dataService, ILanguageService languageService, ILoggerService loggerService,
            IRehydrationService rehydrationService, IMvxLog log)
        {
            _menuService = menuService;
            _dataService = dataService;
            _languageService = languageService;
            _loggerService = loggerService;
            _rehydrationService = rehydrationService;
            _log = log;

            _isXeroxSupported = GSCHelper.IsXeroxSupported(_loggerService.AHX, _languageService.AHX, _dataService.AHX);

            IsLoggedIn = _dataService.GetLoggedInState();
            if (IsLoggedIn)
            {
                AuthInfo = _dataService.GetAuthInfo();
                if (AuthInfo == null)
                {
                    IsLoggedIn = false;
                }
                else
                {
                    CurrentPlanMemberID = _dataService.GetCardPlanMember().PlanMemberID;
                }
            }
        }

        public async Task AuthorizeCredentialsAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var shouldUseXeroxApi = GSCHelper.IsXeroxUser(userName) && _isXeroxSupported;
            InitializeAuthHeader(shouldUseXeroxApi);
            if (shouldUseXeroxApi)
            {
                _authApiUrl = GSCHelper.XEROX_AUTH_BASE_URL;
                _authApiPath = GSCHelper.XEROX_AUTH_BASE_URL_SUB;
            }

            var userRequest = new User
            {
                username = userName,
                password = WebUtility.UrlEncode(password)
            };

            var apiClient = new ApiClient<HttpResponseMessage>(new Uri(_authApiUrl),
                HttpMethod.Post,
                apiPath: _authApiPath,
                apiBody: userRequest,
                useDefaultHeaders: false);

            var requestHeader = new Dictionary<string, string>();
            requestHeader.Add("Authorization", _authHeader);
            requestHeader.Add("Accept-Language", _languageService.CurrentLanguage);

            try
            {
                var loginResponse = await apiClient.ExecuteRequestWithUrlEncodedBody(requestHeader);

                if (loginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ClearUserData(userName);
                    CurrentPlanMemberID = GetPlanMember(loginResponse);
                    AuthInfo = await GetAuthInfoTask(loginResponse);


                    _participantService = Mvx.IoCProvider.Resolve<IParticipantService>();
                    await _participantService.GetParticipant(CurrentPlanMemberID);
                    await _menuService.GetMenuAsync(CurrentPlanMemberID);

                    // This is a fire and forget call
                    // We need this call here because we want to store and show it login view, and there
                    // might be case when user never opens cardview.
                    Task.Run(() => GetIdCardInfoAsync(CurrentPlanMemberID));

                    IsLoggedIn = true;
                    CurrentUser = userName;
                    LastLogin = DateTime.Now;
                }
                else if (loginResponse.StatusCode == 0
                    || loginResponse.StatusCode == HttpStatusCode.BadGateway
                    || loginResponse.StatusCode == HttpStatusCode.GatewayTimeout
                    || loginResponse.StatusCode == HttpStatusCode.InternalServerError
                    || (loginResponse.StatusCode == HttpStatusCode.NotFound && loginResponse.ReasonPhrase == null))
                {
                    throw new WebException(Resource.ConnectionError, WebExceptionStatus.ConnectFailure);
                }
                else
                {
#if CCQ
                    var content = await loginResponse.Content.ReadAsStringAsync();
                    if (content.ToLowerInvariant().Contains("message_en")
                        || content.ToLowerInvariant().Contains("message_fr"))
                    {
                        var errorResponse = JsonConvert.DeserializeObject<CCQUnauthorizedUserResponse>(content);
                        if (errorResponse != null)
                        {
                            var errorMessage = _languageService.IsEnglishLanguage
                                ? errorResponse.MessageEn
                                : errorResponse.MessageFr;
                            throw new UnauthorizedAccessException(errorMessage);
                        }
                    }
                    throw new UnauthorizedAccessException(string.Empty);
#else
                    throw new UnauthorizedAccessException(string.Empty);
#endif
                }
            }
            catch (Exception ex)
            {
                IsLoggedIn = false;
                throw;
            }
        }

        public async Task<bool> AuthorizeC4LCredentialsAsync()
        {
            var tokenRequest = new C4LRequest
            {
                token = AuthInfo.AccessToken
            };

            var apiClient = new ApiClient<HttpResponseMessage>(new Uri(_c4LAuthApiUrl),
                HttpMethod.Post,
                GSCHelper.C4L_AUTH_BASE_URL_SUB,
                useDefaultHeaders: false,
                apiBody: tokenRequest);

            var requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("Authorization", _authHeader);
            requestHeaders.Add("Accept-Language", _languageService.CurrentLanguage);

            try
            {
                var result = await apiClient.ExecuteRequestWithUrlEncodedBody(requestHeaders);
                C4LOAuth = JsonConvert.DeserializeObject<C4LOAuth>(await result.Content.ReadAsStringAsync());
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                switch (result.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        if (await RefreshAccessTokenAsync())
                        {
                            return await AuthorizeC4LCredentialsAsync();
                        }
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RefreshAccessTokenAsync()
        {
            var refreshRequest = new RefreshRequest(AuthInfo.RefreshToken);

            var requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("Authorization", _authHeader);
            requestHeaders.Add("Accept-Language", _languageService.CurrentLanguage);

            var apiClient = new ApiClient<HttpResponseMessage>(new Uri(_authApiUrl),
                HttpMethod.Post,
                apiPath: $"/{_authApiPath}",
                apiBody: refreshRequest,
                useDefaultHeaders: false);

            try
            {
                var response = await apiClient.ExecuteRequestWithUrlEncodedBody(requestHeaders);

                if (response.IsSuccessStatusCode)
                {
                    AuthInfo = JsonConvert.DeserializeObject<AuthInfo>(await response.Content.ReadAsStringAsync());

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Logout()
        {
            _dataService.PersistLastLogin(DateTime.Now.AddYears(-5));
            IsLoggedIn = false;
            _dataService.PersistLoggedInState(false);
        }

        private string GetPlanMember(HttpResponseMessage loginResponse)
        {
            var planMemberKVP = loginResponse.Headers.FirstOrDefault(x => string.Equals(x.Key, "planmemberid", StringComparison.OrdinalIgnoreCase));
            if (planMemberKVP.Value != null)
            {
                return $"{planMemberKVP.Value.First()}-00";
            }

            throw new InvalidOperationException("An error occured while logging in. Unable to retrieve PlanMemberID.");
        }

        private void ClearUserData(string newUserName)
        {
            var isNewUser = !string.Equals(_dataService.GetLastUserToLogin(), newUserName);
            if (isNewUser)
            {
                _claimService = Mvx.IoCProvider.Resolve<IClaimService>();
                _claimsHistoryService = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();

                _claimService.ClearClaimDetails();
                _claimsHistoryService.ClearAllHistory();
                _dataService.ClearPersistedData();
                _rehydrationService.ClearData();
            }
            _dataService.PersistLastUserToLogin(newUserName);
        }

        private async Task<AuthInfo> GetAuthInfoTask(HttpResponseMessage loginResponse)
        {
            return JsonConvert.DeserializeObject<AuthInfo>(await loginResponse.Content.ReadAsStringAsync());
        }

        private async Task GetIdCardInfoAsync(string planMember)
        {
            var _idCardService = Mvx.IoCProvider.Resolve<ICardService>();

            _idCardService.GetIdCardAsync(_participantService.PlanMember.PlanMemberID, CancellationTokenSource.Token);
            _log.Trace("Id card request started from Login Service");
        }

        private void InitializeAuthHeader(bool shouldUseXeroxApi)
        {
            _authHeader = "Basic ";
            if (shouldUseXeroxApi)
            {
                _authHeader += string.Concat(_loggerService.AHX, _languageService.AHX, _dataService.AHX);
            }
            else
            {
                _authHeader += string.Concat(_loggerService.AH, _languageService.AH, _dataService.AH);
            }
        }
    }
}