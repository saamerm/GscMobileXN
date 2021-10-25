using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetUserProfileError : MvxMessage
    {
        public string Message { get; set; }
        public GetUserProfileError(object sender) : base(sender)
        {

        }
    }
}