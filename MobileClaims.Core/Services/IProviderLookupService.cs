using MobileClaims.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IProviderLookupService
    {
        List<ServiceProvider> ClaimSearchResults { get; set; }
        List<ServiceProvider> PreviousServiceProviders { get; }
        List<ServiceProvider> PreviousClaimServiceProviders { get; set; }
        Task GetPreviousServiceProviders(string planMemberID);
        Task GetClaimPreviousServiceProviders(string planMemberID, string claimSubmissionTypeID, int lastUsed = 5);
        Task GetServiceProviderByName(string claimSubmissionTypeID, string lastName, string firstInitial);
        Task GetServiceProviderByPhoneNumber(string claimSubmissionTypeID, string phoneNumber);

        void ClearOldClaimData();

        List<ServiceProviderProvince> ServiceProviderProvinces { get; }
        Task GetServiceProviderProvinces(Action success, Action<string, int> failure);
    }
}
