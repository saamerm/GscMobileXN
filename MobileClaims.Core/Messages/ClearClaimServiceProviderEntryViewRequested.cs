using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimServiceProviderEntryViewRequested : MvxMessage
    {
        public ClearClaimServiceProviderEntryViewRequested(object sender)
            : base(sender)
        { }
    }
}
