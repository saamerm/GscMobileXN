using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardFrontLeftLogoComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardFrontLeftLogoComplete(object sender) : base(sender)
        {

        }
    }
}
