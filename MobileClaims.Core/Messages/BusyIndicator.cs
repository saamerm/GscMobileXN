using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class BusyIndicator : MvxMessage
    {
        public BusyIndicator(object sender)
            : base(sender)
        {
        }
        public bool Busy { get; set; }
    }
}
