using System;
using System.Threading;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface ILoginService
    {
        string CurrentUser { get; }
        string CurrentPlanMemberID { get; set; }
        string GroupPlanNumber { get; }
        string ParticipantNumber { get; }

        bool IsLoggedIn { get; set; }
        bool ShouldExit { get; set; }

        DateTime LastLogin { get; set; }

        C4LOAuth C4LOAuth { get; }
        AuthInfo AuthInfo { get; }

        Task AuthorizeCredentialsAsync(string userName, string password);
        Task<bool> AuthorizeC4LCredentialsAsync();        
        Task<bool> RefreshAccessTokenAsync();
        void Logout();

        CancellationTokenSource CancellationTokenSource { get; }
    }
}