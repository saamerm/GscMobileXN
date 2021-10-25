using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardError : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardError(object sender) : base(sender)
        {

        }
    }
}
