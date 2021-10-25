using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SelectedServiceProviderChanged :MvxMessage
    {
        public SelectedServiceProviderChanged(object sender)
            : base(sender)
        { }

        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public string Address { get; set; }
        public string DoctorName { get; set; }
        public string BusnissName { get; set; }
        public string FormattedAddress { get; set; }
        public string PhoneNo { get; set; }
    }
}
