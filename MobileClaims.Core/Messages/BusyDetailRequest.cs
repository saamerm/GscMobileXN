using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
   public class BusyDetailRequest: MvxMessage
    {
        public bool Busy { get; set; }
        public BusyDetailRequest(object sender)
            : base(sender)
        {

        }
    }
}
