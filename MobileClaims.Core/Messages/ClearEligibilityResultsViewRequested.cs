using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearEligibilityResultsViewRequested: MvxMessage
    {
        public ClearEligibilityResultsViewRequested(object sender)
            : base(sender)
        { }
    }
}
