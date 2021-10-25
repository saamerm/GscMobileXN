using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoSpendingAccountTypes : MvxMessage
    {
        public string Message { get; set; }
        public NoSpendingAccountTypes(object sender) : base(sender)
        {

        }
    }
}
