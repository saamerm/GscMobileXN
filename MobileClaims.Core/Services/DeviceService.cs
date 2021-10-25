namespace MobileClaims.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private GSCHelper.OS _os;
        public GSCHelper.OS CurrentDevice
        {
            get
            {
                return _os;
            }
        }

        public void SetCurrentDevice(GSCHelper.OS os)
        {
            _os = os;
        }
        public bool IsTablet
        {
            get;
            set;
        }
    }
}
