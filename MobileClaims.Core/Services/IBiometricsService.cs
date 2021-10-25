using System.Threading;
using System.Threading.Tasks;

namespace MobileClaims.Services
{
    public interface IBiometricsService
    {
        Task<bool> SavePasswordToKeychain(string password, CancellationToken token);
        Task<string> GetPasswordFromKeychain(CancellationToken token);
        Task<bool> BiometricsAvailable();
        Task<bool> Authenticate(string reason, CancellationToken token);
        Task<bool> CanLoginWithBiometrics();
        void RemoveStoredCredentials();
        string GetConfirmMessage();
    }

    public enum BiometricsType
    {
        Fingerprint,
        TouchId,
        FaceId
    }
}