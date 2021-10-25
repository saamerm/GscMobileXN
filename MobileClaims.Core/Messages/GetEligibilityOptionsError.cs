using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityOptionsError : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityOptionsError(object sender) : base(sender)
        {

        }
    }
}
