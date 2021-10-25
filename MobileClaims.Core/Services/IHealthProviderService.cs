using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.Services.Responses;

namespace MobileClaims.Core.Services
{
    public interface IHealthProviderService
    {
        Task<HealthProviderTypesResponse> GetProviderTypes();
        Task<HealthProviderSummary> GetProviderDetails(int providerId);
        Task<HealthProviderSearchResponse> SearchServiceProviders(HealthProviderSearchCriteriaRequest request);
        Task<HttpResponseMessage> ToggleFavouriteProvider(int providerId);
    }
}