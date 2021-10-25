using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTextAlterationPhoneError : MvxMessage
    {
        public string Message { get; set; }
        public GetTextAlterationPhoneError(object sender) : base(sender)
        {

        }
    }
}
