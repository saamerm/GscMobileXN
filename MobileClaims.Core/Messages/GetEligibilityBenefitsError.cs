using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityBenefitsError : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityBenefitsError(object sender) : base(sender)
        {

        }
    }
}
