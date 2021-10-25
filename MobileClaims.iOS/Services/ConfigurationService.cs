using MobileClaims.Core.Services;

namespace MobileClaims.iOS.Services
{
    public class ConfigurationService : IConfigurationService
    {
        //TODO geolocation working temporary on testing key
#if DEBUG
        private const string GoogleMapsApiKey = "AIzaSyCx3rui_I6Fjk2UoCPNwBkAsU529LPhA5I";
        private const string IpStackGeolocationApiKey = "c46b98f4b7da01c13724ed6624c29d2a";
#else
        private const string GoogleMapsApiKey = "AIzaSyDWOg06ZigZg8K0TShiszOBxfLC-8it1Es";
        private const string IpStackGeolocationApiKey = "0e95eae6857f234e5ebfe9f2658cea6b";
#endif

        public string GetMapsApiKey()
        {
            return GoogleMapsApiKey;
        }

        public string GetIpStackApiKey()
        {
            return IpStackGeolocationApiKey;

        }
    }
}