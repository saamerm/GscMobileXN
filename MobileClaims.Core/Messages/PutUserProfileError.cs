using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class PutUserProfileError : MvxMessage
    {
        public string Message { get; set; }
        public PutUserProfileError(object sender) : base(sender)
        {

        }
    }
}