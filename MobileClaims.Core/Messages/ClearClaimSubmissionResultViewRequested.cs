using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimSubmissionResultViewRequested : MvxMessage
    {
        public ClearClaimSubmissionResultViewRequested(object sender)
            : base(sender)
        { }
    }
}
