using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services
{
    public class AppUpgradeService : ApiClientHelper, IAppUpgradeService
    {
        private string _authHeader = "Basic ";
        private string _currentLanguage = null;

        private IDataService _dataservice;
        private IMvxMessenger _messangerService;
        private ILanguageService _languageservice;
        private ILoggerService _loggerservice;
        private MvxSubscriptionToken _languageUpdatedMessagetoken;

        public AppUpgradeService(IMvxMessenger mvxMessenger, ILanguageService languageService, IDataService dataService, ILoggerService loggerservice)
        {
            _messangerService = mvxMessenger;
            _dataservice = dataService;
            _languageservice = languageService;
            _loggerservice = loggerservice;

            _languageUpdatedMessagetoken = _messangerService.Subscribe<LanguageUpdatedMessage>(LanguageUpdatedMessageHandler);
            _currentLanguage = _languageservice.CurrentLanguage;

            InitializeAuthHeader();
        }

        public async Task<ClientValidationStatus> CheckIfUpdateRequiredAsync()
        {
            ClientValidationStatus updateRequired = ClientValidationStatus.UpdateNotRequired;
            var apiClient = new ApiClient<HttpResponseMessage>(new Uri(GSCHelper.GSC_APP_UPGRADE_BASE_URL), 
                HttpMethod.Post, 
                apiPath: GSCHelper.GSC_APP_UPGRADE_BASE_URL_SUB, 
                apiBody: null, 
                useDefaultHeaders: false);

            var customHeaders = new Dictionary<string, string>();
            customHeaders.Add("Authorization", _authHeader);
            customHeaders.Add("Accept-Language", _currentLanguage);

            try
            {
                var result = await ExecuteRequestWithCustomHeaderParameters(apiClient, customHeaders);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Validation completed, don't need to update.
                    updateRequired = ClientValidationStatus.UpdateNotRequired;
                }
                else
                {
                    switch (result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.BadRequest:
                            var responseBodyTask = Task.Run<string>(async () => await result.Content.ReadAsStringAsync());
                            var str = responseBodyTask.Result;
                            var errorResponse = JsonConvert.DeserializeObject<ClientValidationResponse>(str);
                            if (errorResponse.ErrorMessage.Equals("deprecated_client", StringComparison.OrdinalIgnoreCase))
                            {
                                updateRequired = ClientValidationStatus.UpdateRequired;
                            }
                            else if (errorResponse.ErrorMessage.Equals("invalid_client", StringComparison.OrdinalIgnoreCase))
                            {
                                updateRequired = ClientValidationStatus.InvalidClientError;
                            }
                            break;
                        case System.Net.HttpStatusCode.BadGateway:
                        case System.Net.HttpStatusCode.GatewayTimeout:
                        case System.Net.HttpStatusCode.InternalServerError:
                        default:
                            updateRequired = ClientValidationStatus.NetworkError;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                updateRequired = ClientValidationStatus.Error;
            }
            return updateRequired;
        }

        private void LanguageUpdatedMessageHandler(LanguageUpdatedMessage obj)
        {
            _currentLanguage = _languageservice.CurrentLanguage;
        }

        private void InitializeAuthHeader()
        {
            _authHeader = "Basic ";
			_authHeader += string.Concat(_loggerservice.AH, _languageservice.AH, _dataservice.AH);
		}
    }
}