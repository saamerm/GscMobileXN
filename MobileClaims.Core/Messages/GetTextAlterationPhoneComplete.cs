using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTextAlterationPhoneComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetTextAlterationPhoneComplete(object sender) : base(sender)
        {

        }
    }
}
