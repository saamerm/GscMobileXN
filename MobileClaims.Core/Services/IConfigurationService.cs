namespace MobileClaims.Core.Services
{
    public interface IConfigurationService
    {
        string GetMapsApiKey();
        
        string GetIpStackApiKey();
    }
}
