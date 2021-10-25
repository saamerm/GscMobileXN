using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardBackImageComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardBackImageComplete(object sender) : base(sender)
        {

        }
    }
}
