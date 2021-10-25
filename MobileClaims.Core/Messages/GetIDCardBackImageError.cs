using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardBackImageError : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardBackImageError(object sender) : base(sender)
        {

        }
    }
}
