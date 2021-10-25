using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardBackTopLogoError : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardBackTopLogoError(object sender) : base(sender)
        {

        }
    }
}
