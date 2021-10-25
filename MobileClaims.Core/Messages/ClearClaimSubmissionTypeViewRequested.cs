using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimSubmissionTypeViewRequested : MvxMessage
    {
        public ClearClaimSubmissionTypeViewRequested(object sender)
            : base(sender)
        { }
    }
}
