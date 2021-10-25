using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimTermsAndConditionsViewRequested : MvxMessage
    {
        public ClearClaimTermsAndConditionsViewRequested(object sender)
            : base(sender)
        { }
    }
}
