using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileClaims.Core.Models;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Services
{
    public class SearchHealthProviderService : ISearchHealthProviderService
    {
        private readonly IHealthProviderService _healthProviderService;
        private const int NumberOfItems = 100;

        public SearchHealthProviderService(IHealthProviderService healthProviderService)
        {
            _healthProviderService = healthProviderService;
        }

        public List<HealthProviderTypeViewModel> ProviderTypes { get; private set; }

        public async Task<IList<HealthProviderSummaryModel>> GetHealthProviders(HealthProviderSearchParameters parameters)
        {
            if (ProviderTypes == null || ProviderTypes.Count == 0)
                throw new NotSupportedException("Provider types have to be initiated before calling provider search");

            int startRowNumber = parameters.PageIndex * NumberOfItems;

            var request = new HealthProviderSearchCriteriaRequest()
            {
                Distance = parameters.Distance,
                Latitude = parameters.Latitude,
                Longitude = parameters.Longitude,
                ProviderSearchQuery = parameters.SearchQuery,
                StartRowNumber = startRowNumber,
                EndRowNumber = startRowNumber + NumberOfItems,
                ProviderTypeCodes = parameters.ProviderTypeCodes,
                ProviderRating = parameters.ProviderRating,
                SearchTypeChoice = parameters.SearchTypeChoice,
                SortByChoice = parameters.SortByChoice,
                RecentlyVisited = parameters.IsRecentlyVisited
            };

            var providers = await _healthProviderService.SearchServiceProviders(request);

            if (providers == null)
            {
                return new List<HealthProviderSummaryModel>();
            }

            if (parameters.IsDirectBill)
            {
                return providers.Select(provider => new HealthProviderSummaryModel(provider, ProviderTypes))
                    .Where(p => p.IsDirectBill).ToList();
            }

            return providers.Select(provider => new HealthProviderSummaryModel(provider, ProviderTypes)).ToList();
        }

        public async Task ResetProviderTypes()
        {
            var providerTypes = await _healthProviderService.GetProviderTypes();

            if (providerTypes == null)
            {
                throw new NullResponseException();
            }

            ProviderTypes = providerTypes.Where(p => p.ParentId == null)
                .Select(p => new HealthProviderTypeViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Title = p.Category,
                    LineOfBusinessCode = p.LineOfBusinessCode,
                    ProviderTypeCodes = p.ProviderTypeCodes,
                    SortOrder = p.SortOrder,
                    ParentId = p.ParentId,
                    DisplayRating = p.DisplayRating == "Y",
                    IsSearchable = p.IsSearchable
                }).ToList();
            var childrenLookup = providerTypes.Where(p => p.ParentId != null).ToLookup(p => p.ParentId);

            foreach (var vm in ProviderTypes)
            {
                vm.ChildItems = childrenLookup[vm.Id].Select(p => new HealthProviderTypeViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Title = p.Category,
                    LineOfBusinessCode = p.LineOfBusinessCode,
                    ProviderTypeCodes = p.ProviderTypeCodes,
                    SortOrder = p.SortOrder,
                    ParentId = p.ParentId
                }).ToList();
            }

        }
    }
}
