using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class AuthRefreshError : MvxMessage
    {
        public string Message { get; set; }
        public AuthRefreshError(object sender) : base(sender)
        {

        }
    }
}
