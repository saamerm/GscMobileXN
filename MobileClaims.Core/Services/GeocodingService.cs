using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Services.Responses;
using MobileClaims.Core.Util;
using MvvmCross;
using MvvmCross.Logging;

namespace MobileClaims.Core.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly IMvxLog _log;
        private readonly ILoginService _loginService;

        public GeocodingService(ILoginService loginService,
            IMvxLog log)
        {
            _loginService = loginService;
            _log = log;
        }

        public async Task<Location> GetLocations(string address)
        {
            var service = new ApiClient<GoogleGeocodingResponse>(
                new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                UrlFactory.GetSubUrl(UrlFactory.UriPaths.PlaceGeocoding), 
                new Dictionary<string, string> { {"address", address } });
            try
            {
                var response = await ApiHelper.TryWithRetry(() => service.ExecuteRequest(), _loginService);
                if (response.Status == "OK")
                {
                    return response.Results?.FirstOrDefault()?.Geometry?.Location;
                }
                return null;
            }
            catch (ApiException ex)
            {
                _log.Error(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }
        }

        public async Task<Prediction[]> GetSuggestions(string address)
        {
            var service = new ApiClient<GoogleAutocompleteResponse>(
                new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                UrlFactory.GetSubUrl(UrlFactory.UriPaths.PlaceAutocomplete),
                new Dictionary<string, string> {{"address", address}});
            try
            {
                var response = await ApiHelper.TryWithRetry(() => service.ExecuteRequest(), _loginService);
                if (response.Status == "OK")
                {
                    return response.Predictions;
                }
                return null;
            }
            catch (ApiException ex)
            {
                _log.Error(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }
        }
    }
}
