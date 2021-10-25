using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimSubmissionConfirmationViewRequested : MvxMessage
    {
        public ClearClaimSubmissionConfirmationViewRequested(object sender)
            : base(sender)
        { }
    }
}
