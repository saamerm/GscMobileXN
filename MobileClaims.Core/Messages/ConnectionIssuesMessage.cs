using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ConnectionIssuesMessage : MvxMessage
    {
        public string Message { get; set; }
        public ConnectionIssuesMessage(object sender) : base(sender)
        {

        }
    }
}
