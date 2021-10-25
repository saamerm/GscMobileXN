using MobileClaims.Core.Entities;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClaimTreatmentDetailsListUpdated : MvxMessage
    {
        public ClaimTreatmentDetailsListUpdated(object sender)
            : base(sender)
        { }
        public TreatmentDetail TreatmentDetail { get; set; }
    }
}
