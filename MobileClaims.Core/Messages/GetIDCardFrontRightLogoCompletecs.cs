using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardFrontRightLogoComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardFrontRightLogoComplete(object sender) : base(sender)
        {

        }
    }
}
