using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services.Requests;

namespace MobileClaims.Core.Services
{
    public interface IDirectDepositService
    {
        Task<DirectDepositInfo> GetDirectDepositInfoAsync(string planMemberId);
        Task<DirectDepositBankDetails> ValidateUserBankDetails(string planMemberId, BankValidationRequest bankValidationRequest);
        Task<bool> GetHasEFTInfoAsync();
        Task<DirectDepositInfo> SubmitDirectDepositDetails(string planMemberId, DirectDepositInfo directDepositInfo);
    }
}