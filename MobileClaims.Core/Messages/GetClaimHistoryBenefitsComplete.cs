using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimHistoryBenefitsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimHistoryBenefitsComplete(object sender) : base(sender)
        {

        }
    }
}
