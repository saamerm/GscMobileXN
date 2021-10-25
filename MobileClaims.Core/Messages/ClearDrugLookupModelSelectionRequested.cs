using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearDrugLookupModelSelectionRequested : MvxMessage
    {
        public ClearDrugLookupModelSelectionRequested(object sender)
            : base(sender)
        { }
    }
}
