using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class EmailSpecialAuthorizationFormError : MvxMessage
    {
        public string Message { get; set; }
        public EmailSpecialAuthorizationFormError(object sender) : base(sender)
        {

        }
    }
}