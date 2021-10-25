using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class C4LAuthorizationFailure : MvxMessage
    {
        public string Message { get; set; }
        public C4LAuthorizationFailure(object sender) : base(sender)
        {
        }
    }
}
