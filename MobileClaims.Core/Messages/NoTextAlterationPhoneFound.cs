using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoTextAlterationPhoneFound : MvxMessage
    {
        public string Message { get; set; }
        public NoTextAlterationPhoneFound(object sender) : base(sender)
        {

        }
    }
}
