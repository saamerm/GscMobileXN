using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;
using MobileClaims.Core.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public class HealthProviderService : IHealthProviderService
    {
        private readonly ILoginService _loginService;

        public HealthProviderService(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<HealthProviderTypesResponse> GetProviderTypes()
        {
            var client = new ApiClient<HealthProviderTypesResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                                                                    HttpMethod.Get,
                                                                    UrlFactory.GetSubUrl(UrlFactory.UriPaths.ProviderTypeOption),
                                                                    new Dictionary<string, string>() { { "planMemberId", _loginService.CurrentPlanMemberID } });

            return await ApiHelper.TryWithRetry(() => client.ExecuteRequest(), _loginService);
        }

        public async Task<HealthProviderSearchResponse> SearchServiceProviders(HealthProviderSearchCriteriaRequest request)
        {
            var client = new ApiClient<HealthProviderSearchResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                                                                    HttpMethod.Post,
                                                                    UrlFactory.GetSubUrl(UrlFactory.UriPaths.ServiceProvider),
                                                                    null,
                                                                    request);

            return await ApiHelper.TryWithRetry(() => client.ExecuteRequest(), _loginService);
        }

        public async Task<HealthProviderSummary> GetProviderDetails(int providerId)
        {
            var client = new ApiClient<HealthProviderSummary>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                                                            HttpMethod.Get,
                                                            UrlFactory.GetSubUrl(UrlFactory.UriPaths.ServiceProviderDetails),
                                                            new Dictionary<string, string>() { { "providerId", providerId.ToString() } });

            return await ApiHelper.TryWithRetry(() => client.ExecuteRequest(), _loginService);
        }

        public async Task<HttpResponseMessage> ToggleFavouriteProvider(int providerId)
        {
            var client = new ApiClient<HttpResponseMessage>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Post,
                UrlFactory.GetSubUrl(UrlFactory.UriPaths.ProviderFavourite),
                new Dictionary<string, string>() { { "providerId", providerId.ToString() } });

            return await ApiHelper.TryWithRetry(() => client.ExecuteRequest(), _loginService);
        }
    }
}
