using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class EmailSpecialAuthorizationFormComplete : MvxMessage
    {
        public string Message { get; set; }
        public EmailSpecialAuthorizationFormComplete(object sender) : base(sender)
        {

        }
    }
}