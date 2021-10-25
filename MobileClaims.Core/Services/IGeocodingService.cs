using System.Threading.Tasks;
using MobileClaims.Core.Services.Responses;

namespace MobileClaims.Core.Services
{
    public interface IGeocodingService
    {
        Task<Location> GetLocations(string address);

        Task<Prediction[]> GetSuggestions(string address);
    }
}
