using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimOptionsError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimOptionsError(object sender) : base(sender)
        {

        }
    }
}
