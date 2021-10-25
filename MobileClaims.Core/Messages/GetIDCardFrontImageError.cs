using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardFrontImageError : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardFrontImageError(object sender) : base(sender)
        {

        }
    }
}
