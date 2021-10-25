using MobileClaims.Core.Entities;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public interface IPlanMemberService
    {
        Task<Address> GetPlanMemberAddress(string planMemberId);
    }
}