using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimHistoryBenefitsError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimHistoryBenefitsError(object sender) : base(sender)
        {

        }
    }
}
