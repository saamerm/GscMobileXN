using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetSpecialAuthorizationFormComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetSpecialAuthorizationFormComplete(object sender) : base(sender)
        {

        }
    }
}