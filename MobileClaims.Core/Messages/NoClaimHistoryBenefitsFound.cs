using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoClaimHistoryBenefitsFound : MvxMessage
    {
        public string Message { get; set; }
        public NoClaimHistoryBenefitsFound(object sender) : base(sender)
        {

        }
    }
}
