namespace MobileClaims.Core.Services
{
    public interface ILocationService
    {
        bool LocationAvailable { get; }
        double Latitude { get; }
        double Longitude { get; }

        void GetCurrentPosition();
    }
}
