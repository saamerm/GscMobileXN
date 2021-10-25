using System.Threading.Tasks;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface IAppUpgradeService
    {
        Task<ClientValidationStatus> CheckIfUpdateRequiredAsync();
    }
}