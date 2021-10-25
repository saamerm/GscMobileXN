using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardFrontRightLogoError : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardFrontRightLogoError(object sender) : base(sender)
        {

        }
    }
}
