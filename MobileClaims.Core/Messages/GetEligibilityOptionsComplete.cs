using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityOptionsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityOptionsComplete(object sender) : base(sender)
        {

        }
    }
}
