using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NotEligibleForVisionClaimsMessage : MvxMessage
    {
        public string Message { get; set; }
        public NotEligibleForVisionClaimsMessage(object sender) : base(sender)
        {

        }
    }
}
