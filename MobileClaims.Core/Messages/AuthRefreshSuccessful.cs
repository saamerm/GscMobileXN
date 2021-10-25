using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class AuthRefreshSuccessful : MvxMessage
    {
        public string Message { get; set; }
        public AuthRefreshSuccessful(object sender) : base(sender)
        {

        }
    }
}
