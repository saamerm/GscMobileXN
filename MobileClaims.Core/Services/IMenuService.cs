using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface IMenuService
    {
        IList<MenuItem> MenuItems { get; }

        Task<IEnumerable<MenuItem>> GetMenuAsync(string planMemberId);

        bool IsAnyClaimForAudit();
    }
}