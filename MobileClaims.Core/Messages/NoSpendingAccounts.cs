using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoSpendingAccounts : MvxMessage
    {
        public string Message { get; set; }
        public NoSpendingAccounts(object sender) : base(sender)
        {

        }
    }
}
