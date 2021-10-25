using System.Threading;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface ICardService
    {
        TaskCompletionSource<IDCard> TaskCompletionSource { get; }
        Task<IDCard> GetIdCardAsync(string planMemberId, CancellationToken cancellationToken);
        IDCard UpdateIdCardImage(IDCard idCard);
    }
}