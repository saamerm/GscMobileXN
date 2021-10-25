using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetIDCardBackTopLogoComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetIDCardBackTopLogoComplete(object sender) : base(sender)
        {

        }
    }
}
