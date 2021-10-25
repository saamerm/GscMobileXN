using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionBifocalsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionBifocalsComplete(object sender) : base(sender)
        {

        }
    }
}
