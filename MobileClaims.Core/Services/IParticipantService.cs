using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.FacadeEntities;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IParticipantService
    {
        IDCard IDCard { get; }
        UserProfile UserProfile { get; }
        UserAgreement UserAgreement { get; }

        //void GetParticipant();
        Task GetParticipant(string planMemberID);
        void ReloadPersistedParticipant();
        
        PlanMember PlanMember { get;  }
        Participant SelectedParticipant { get; set; }
        Participant SelectedDrugParticipant { get; set; }

        Task PutUserProfile(UserProfile up);
        Task<UserAgreement> GetUserAgreement();
        Task PutUserAgreement(UserAgreement ua);
        Task GetUserAgreementWCS();

        Task<PutAgreementWCSResponse> PutUserAgreementWCSAsync(UserAgreement userAgreement);
   
        void ClearOldClaimData();
    }
}