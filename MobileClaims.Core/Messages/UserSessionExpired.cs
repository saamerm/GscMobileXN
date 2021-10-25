using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class UserSessionExpired : MvxMessage
    {
        public UserSessionExpired(object sender) : base (sender)
        {
        }
    }
}

