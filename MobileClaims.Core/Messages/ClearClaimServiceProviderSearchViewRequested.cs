using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimServiceProviderSearchViewRequested : MvxMessage
    {
        public ClearClaimServiceProviderSearchViewRequested(object sender)
            : base(sender)
        { }
    }
}
