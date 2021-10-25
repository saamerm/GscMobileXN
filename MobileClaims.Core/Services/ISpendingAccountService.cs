using MobileClaims.Core.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface ISpendingAccountService
    {
        List<SpendingAccountType> AccountTypes { get; }
        List<SpendingAccountDetail> SpendingAccounts { get; }
        List<SpendingAccountTypeRollup> SpendingAccountTypeRollups { get; }
        ObservableCollection<YearTotalRemain> YearTotalRemainCollection { get; }
        SpendingAccountType SelectedSpendingAccountType { get; set; }
        bool IsGetSpendingAccountTypesBusy { get; }
        bool IsGetSpendingAccountTypesDone { get; }
        bool IsGetSpendingAccountsBusy { get; }
        bool IsGetSpendingAccountsDone { get; }

        Task GetAllSpendingAccountsInfo(string planMemberID, bool forceRefresh);
        Task GetSpendingAccountTypes(string planMemberID);
        Task GetSpendingAccounts(string planMemberID);
        SpendingAccountTypeRollup GetSpendingAccountDetailsByType(SpendingAccountType accountType);
    }
}
