namespace MobileClaims.Core.Services
{
    public interface IDeviceService
    {
        GSCHelper.OS CurrentDevice { get; }
        bool IsTablet{ get; set; }
        void SetCurrentDevice(GSCHelper.OS os);
    }
}
