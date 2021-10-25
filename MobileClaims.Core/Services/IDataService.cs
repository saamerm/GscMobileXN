using System;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface IDataService
    {
        string AH { get; }
        string AHX { get; }
        void PersistLastLogin(DateTime datetime);
        DateTime GetLastLogin();
        void PersistUserName(string username);
        string GetUserName();
        void PersistCardPlanMember(PlanMember planMember);
        PlanMember GetCardPlanMember();
        void PersistClaim(Claim claim);
        Claim GetClaim();
        Entities.HCSA.Claim GetHCSAClaim();
        void PersistAcceptedTC(bool acceptedTC);
        bool GetAcceptedTC();
        void PersistIDCard(IDCard card);
        IDCard GetIDCard();
        void PersistLoggedInState(bool loggedIn);
        bool GetLoggedInState();
        void PersistAuthInfo(AuthInfo authInfo);
        AuthInfo GetAuthInfo();
        bool ClearPersistedData();
        void PersistLastUserToLogin(string lastUserToLogin);
        void PersistHCSAClaim(Entities.HCSA.Claim claim);
        string GetLastUserToLogin();
        void PersistSelectedHCSAClaimType(Entities.HCSA.ClaimType claimType);
        void PersistSelectedHCSAExpenseType(Entities.HCSA.ExpenseType expenseType);
        Entities.HCSA.ExpenseType GetSelectedHCSAExpenseType();
        Entities.HCSA.ClaimType GetSelectedHCSAClaimType();
        void PersistUseBiometricsSetting(bool activate);
        bool? GetUseBiometricsSetting();

    }
}