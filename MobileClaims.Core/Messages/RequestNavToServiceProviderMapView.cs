using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RequestNavToServiceProviderMapView : MvxMessage
    {
        public RequestNavToServiceProviderMapView(object sender) : base(sender)
        {

        }

        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int Radius { get; set; }
        public string Phone { get; set; }
        public string BusinessName { get; set; }
        public string LastName { get; set; }
        public string ProviderType { get; set; }
        public string LocationType { get; set; }
        public string SearchType { get; set; }
        public bool UsedDeviceLocation { get; set; }
        public int serviceProviderID { get; set; }
    }
}

