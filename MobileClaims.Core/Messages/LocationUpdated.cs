using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class LocationUpdated : MvxMessage
    {
        public string Message { get; set; }
        public LocationUpdated(object sender) : base(sender)
        {

        }
    }
}
