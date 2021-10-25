using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimDetailsViewRequested : MvxMessage
    {
        public ClearClaimDetailsViewRequested(object sender)
            : base(sender)
        { }
    }
}
