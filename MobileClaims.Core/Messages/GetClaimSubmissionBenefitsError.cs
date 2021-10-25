using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimSubmissionBenefitsError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimSubmissionBenefitsError(object sender) : base(sender)
        {

        }
    }
}
