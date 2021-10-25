using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardFrontImageComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardFrontImageComplete(object sender) : base(sender)
        {

        }
    }
}
