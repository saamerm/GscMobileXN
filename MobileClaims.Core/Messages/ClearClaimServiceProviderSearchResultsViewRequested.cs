using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimServiceProviderSearchResultsViewRequested : MvxMessage
    {
        public ClearClaimServiceProviderSearchResultsViewRequested(object sender)
            : base(sender)
        { }
    }
}
