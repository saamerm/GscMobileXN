using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class PutUserProfileComplete : MvxMessage
    {
        public string Message { get; set; }
        public PutUserProfileComplete(object sender) : base(sender)
        {

        }
    }
}