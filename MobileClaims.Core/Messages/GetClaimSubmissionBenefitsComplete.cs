using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimSubmissionBenefitsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimSubmissionBenefitsComplete(object sender) : base(sender)
        {

        }
    }
}
