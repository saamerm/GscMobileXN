using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardComplete(object sender) : base(sender)
        {

        }
    }
}
