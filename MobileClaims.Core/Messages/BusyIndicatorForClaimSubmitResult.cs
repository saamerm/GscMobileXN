using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class BusyIndicatorForClaimSubmitResult: MvxMessage
    {
        public BusyIndicatorForClaimSubmitResult(object sender)
            : base(sender)
        {
        }
        public bool Busy { get; set; }
    }
}
