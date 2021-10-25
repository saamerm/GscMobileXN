using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionBifocalsError : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionBifocalsError(object sender) : base(sender)
        {

        }
    }
}
