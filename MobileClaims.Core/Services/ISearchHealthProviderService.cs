using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Models;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Services
{
    public interface ISearchHealthProviderService
    {
        Task<IList<HealthProviderSummaryModel>> GetHealthProviders(HealthProviderSearchParameters parameters);

        Task ResetProviderTypes();

        List<HealthProviderTypeViewModel> ProviderTypes { get; }
    }
}