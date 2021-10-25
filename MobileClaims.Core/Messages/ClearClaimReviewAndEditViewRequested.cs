using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimReviewAndEditViewRequested : MvxMessage
    {
        public ClearClaimReviewAndEditViewRequested(object sender)
            : base(sender)
        { }
    }
}
