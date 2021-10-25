using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services
{
    public class ClaimExService : ApiClientHelper, IClaimExService
    {
        public async Task<ClaimResultResponse> GetClaimResult(string claimFormId)
        {
            if (string.IsNullOrWhiteSpace(claimFormId))
            {
                throw new ArgumentNullException(nameof(claimFormId));
            }

            var apiClient = new ApiClient<ClaimResultResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/claimresult/{claimFormId}");

            return await ExecuteRequestWithRetry(apiClient);
        }

        public async Task<IList<ClaimAudit>> GetAuditClaimsAsync(string planMemeberId)
        {
            return await GetClaimsAsync<ClaimAudit>(planMemeberId, UploadDocumentProcessType.Audit);
        }

        public async Task<IList<ClaimCOP>> GetCopClaimsAsync(string planMemeberId)
        {
            return await GetClaimsAsync<ClaimCOP>(planMemeberId, UploadDocumentProcessType.COP);
        }

        public async Task<IList<RecentClaim>> GetRecentClaimsAsync(string planMemeberId)
        {
            return await GetClaimsAsync<RecentClaim>(planMemeberId, UploadDocumentProcessType.RecentClaims);
        }

        private async Task<IList<T>> GetClaimsAsync<T>(string planMemeberId, string uploadDocumentProcessType)
        {
            if (string.IsNullOrWhiteSpace(planMemeberId))
            {
                throw new ArgumentNullException(nameof(planMemeberId));
            }

            var apiClient = new ApiClient<IList<T>>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Post,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{planMemeberId}/claimlist",
                apiBody: new ClaimStatusType() { ClaimType = uploadDocumentProcessType });

            return await ExecuteRequestWithRetry(apiClient);
        }

        public async Task<ValidateGSCPlanMemberIdResponse> ValidateSecondaryPlanMemeberId(string primaryPlanMemeberId, string primaryParticipantNumber, string secondaryPlanMemberId)
        {
            if (string.IsNullOrWhiteSpace(primaryPlanMemeberId))
            {
                throw new ArgumentNullException(nameof(primaryPlanMemeberId));
            }

            if (string.IsNullOrWhiteSpace(primaryParticipantNumber))
            {
                throw new ArgumentNullException(nameof(primaryParticipantNumber));
            }

            if (string.IsNullOrWhiteSpace(secondaryPlanMemberId))
            {
                throw new ArgumentNullException(nameof(secondaryPlanMemberId));
            }

            var apiClient = new ApiClient<ValidateGSCPlanMemberIdResponse>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
               HttpMethod.Post,
               $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/SecondaryPlanValidation",
               apiBody: new ValidateGSCPlanMemberIdRequest()
               {
                   PrimaryPlanMemberID = primaryPlanMemeberId,
                   primaryParticipantNumber = primaryParticipantNumber,
                   secondaryPlanMemberID = secondaryPlanMemberId
               });

            return await ExecuteRequestWithRetry(apiClient);
        }
    }

    public class ValidateGSCPlanMemberIdRequest
    {
        [JsonProperty(PropertyName = "primaryPlanMemberID")]
        public string PrimaryPlanMemberID { get; set; }

        [JsonProperty(PropertyName = "primaryParticipantNumber")]
        public string primaryParticipantNumber { get; set; }

        [JsonProperty(PropertyName = "secondaryPlanMemberID")]
        public string secondaryPlanMemberID { get; set; }
    }

    public class ValidateGSCPlanMemberIdResponse
    {
        [JsonProperty(PropertyName = "validationstatusCode")]
        public int ValidationstatusCode { get; set; }
    }
}