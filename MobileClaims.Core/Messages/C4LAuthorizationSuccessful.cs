using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class C4LAuthorizationSuccessful : MvxMessage
    {
        public string Message { get; set; }
        public C4LAuthorizationSuccessful(object sender) : base(sender)
        {
        }
    }
}
