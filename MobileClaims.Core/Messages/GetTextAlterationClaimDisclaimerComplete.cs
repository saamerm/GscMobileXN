using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTextAlterationClaimDisclaimerComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetTextAlterationClaimDisclaimerComplete(object sender) : base(sender)
        {

        }
    }
}
