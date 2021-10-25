using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimTreatmentDetailsViewRequested : MvxMessage
    {
        public ClearClaimTreatmentDetailsViewRequested(object sender)
            : base(sender)
        { }
    }
}
