using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetSpecialAuthorizationFormError : MvxMessage
    {
        public string Message { get; set; }
        public GetSpecialAuthorizationFormError(object sender) : base(sender)
        {

        }
    }
}