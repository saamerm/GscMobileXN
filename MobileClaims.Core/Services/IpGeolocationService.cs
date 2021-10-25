using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Helpers;
using MobileClaims.Core.Services.Requests;
using MvvmCross;
using MvvmCross.Logging;
using Plugin.Geolocator.Abstractions;

namespace MobileClaims.Core.Services
{

    public class IpGeolocationService : IIpGeolocationService
    {
        private readonly IMvxLog _log;
        private readonly IConfigurationService _configurationService;

        public IpGeolocationService(IConfigurationService configurationService,
            IMvxLog log)
        {
            _configurationService = configurationService;
            _log = log;
        }

        public async Task<GeolocationResult> GetIpLocation()
        {
            var key = _configurationService.GetIpStackApiKey();
            var request = new GoogleGeolocationRequest();
            var service = new ApiClient<IpLocationData>(
                new Uri(GSCHelper.IPSTACK_GEOCODING_BASE_URL),
                HttpMethod.Get, $"/check?access_key={{Key}}&output=json",
                new Dictionary<string, string> {{"Key", key}}, request)
            {
                UseDefaultHeaders = false
            };

            try
            {
                var response = await service.ExecuteRequest();
                if (response == null || Math.Abs(response.Latitude) < double.Epsilon || Math.Abs(response.Longitude) < double.Epsilon)
                {
                    return GeolocationResult.Failure(GeolocationStatus.UnableToGetLocation);
                }

                return GeolocationResult.SuccessIp(new Position(response.Latitude, response.Longitude));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return GeolocationResult.Failure(GeolocationStatus.UnableToGetLocation);
            }
        }
    }
}
