using MobileClaims.Core.Models;

namespace MobileClaims.Core.Services
{
    public interface IDirectionsService
    {
        bool ShowDirectionsInMaps(string address, Coordinates coordinatesTo);
    }
}
