using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface IClaimExService
    {
        Task<ClaimResultResponse> GetClaimResult(string claimFormId);

        Task<IList<ClaimAudit>> GetAuditClaimsAsync(string planMemeberId);

        Task<IList<ClaimCOP>> GetCopClaimsAsync(string planMemeberId);

        Task<IList<RecentClaim>> GetRecentClaimsAsync(string planMemeberId);

        Task<ValidateGSCPlanMemberIdResponse> ValidateSecondaryPlanMemeberId(string primaryPlanMemeberId, string primaryParticipantNumber, string secondaryPlanMemberId);
    }
}