using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearTermsAndConditionsRequest:MvxMessage
    {
        public ClearTermsAndConditionsRequest(object sender)
            : base(sender)
        { }
    }
}
