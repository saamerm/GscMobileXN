using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearMainNavigationViewRequested : MvxMessage
    {
        public ClearMainNavigationViewRequested(object sender)
            : base(sender)
        { }
    }
}
