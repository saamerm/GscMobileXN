using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class IsRightSideGreyedOutUpdated : MvxMessage
    {
        public string Message { get; set; }
        public IsRightSideGreyedOutUpdated(object sender) : base(sender)
        {
        }
    }
}
