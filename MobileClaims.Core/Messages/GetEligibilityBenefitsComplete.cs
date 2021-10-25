using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityBenefitsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityBenefitsComplete(object sender) : base(sender)
        {

        }
    }
}
