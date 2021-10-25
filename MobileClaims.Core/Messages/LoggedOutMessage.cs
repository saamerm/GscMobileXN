using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class LoggedOutMessage : MvxMessage
    {
        public LoggedOutMessage(object sender):base(sender)
        {
        }
    }
}
