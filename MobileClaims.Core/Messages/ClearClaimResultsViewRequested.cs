using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimResultsViewRequested : MvxMessage
    {
        public ClearClaimResultsViewRequested(object sender) : base(sender)
        { }
    }
}
