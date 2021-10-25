using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimServiceProviderViewRequested : MvxMessage
    {
        public ClearClaimServiceProviderViewRequested(object sender)
            : base(sender)
        { }
    }
}
