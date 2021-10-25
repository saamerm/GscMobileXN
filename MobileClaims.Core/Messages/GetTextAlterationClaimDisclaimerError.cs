using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTextAlterationClaimDisclaimerError : MvxMessage
    {
        public string Message { get; set; }
        public GetTextAlterationClaimDisclaimerError(object sender) : base(sender)
        {

        }
    }
}
