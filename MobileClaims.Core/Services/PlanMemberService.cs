using System;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.Responses;

namespace MobileClaims.Core.Services
{
    public class PlanMemberService : ApiClientHelper, IPlanMemberService
    {
        private Address _planMemberAddress;

        public PlanMemberService()
        {
        }

        public async Task<Address> GetPlanMemberAddress(string planMemberId)
        {
            if (string.IsNullOrWhiteSpace(planMemberId))
            {
                throw new ArgumentNullException(nameof(planMemberId));
            }

            if (_planMemberAddress != null
                && string.Equals(_planMemberAddress.PlanMemberId, planMemberId, StringComparison.OrdinalIgnoreCase))
            {
                return _planMemberAddress;
            }

            var apiClient = new ApiClient<PlanMemberAddressResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{planMemberId}/address");

            var response = await ExecuteRequestWithRetry(apiClient);
            _planMemberAddress = new Address()
            {
                PlanMemberId = planMemberId,
                AddressLine1 = response.Line1,
                AddressLine2 = response.Line2,
                AddressLine3 = response.Line3,
                City = response.City,
                Country = response.Country,
                ProvinceCode = response.ProvinceCode,
                PostalCode = response.PostalCode,
                Latitude = response.Latitude,
                Longitude = response.Longitude
            };
            return _planMemberAddress;
        }
    }
}
