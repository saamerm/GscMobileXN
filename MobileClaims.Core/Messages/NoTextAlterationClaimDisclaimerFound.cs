using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoTextAlterationClaimDisclaimerFound : MvxMessage
    {
        public string Message { get; set; }
        public NoTextAlterationClaimDisclaimerFound(object sender) : base(sender)
        {

        }
    }
}
