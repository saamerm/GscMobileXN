using System;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;

namespace MobileClaims.Core.ViewModelParameters
{
    public class AuditClaimSummaryViewModelParameters
    {
        public DashboardRecentClaim RecentClaim { get; set; }
        public IClaimSummaryProperties ClaimSummary { get; set; }


        public AuditClaimSummaryViewModelParameters(DashboardRecentClaim recentClaim, IClaimSummaryProperties claimSummary)
        {
            RecentClaim = recentClaim;
            ClaimSummary = claimSummary;
        }
    }
}
