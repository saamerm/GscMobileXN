using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardFrontLeftLogoError : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardFrontLeftLogoError(object sender) : base(sender)
        {

        }
    }
}
