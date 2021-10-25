using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimTreatmentDetailsListViewRequested : MvxMessage
    {
        public ClearClaimTreatmentDetailsListViewRequested(object sender)
            : base(sender)
        { }
    }
}
