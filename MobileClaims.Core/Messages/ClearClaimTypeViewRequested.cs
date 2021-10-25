using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimTypeViewRequested : MvxMessage
    {
        public ClearClaimTypeViewRequested(object sender)
            : base(sender)
        { }
    }
}
