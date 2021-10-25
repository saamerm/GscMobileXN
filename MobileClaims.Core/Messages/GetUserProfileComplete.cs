using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetUserProfileComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetUserProfileComplete(object sender) : base(sender)
        {

        }
    }
}