using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class LoggedInMessage : MvxMessage
    {
        public LoggedInMessage(object sender)
            : base(sender)
        {
        }
    }
}
