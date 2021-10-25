using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityProvincesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityProvincesComplete(object sender) : base(sender)
        {

        }
    }
}
