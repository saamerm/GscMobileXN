using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimCOPError : MvxMessage
    {
        public string Message { get; set; }

        public GetClaimCOPError(object sender)
            : base(sender)
        {

        }
    }
}