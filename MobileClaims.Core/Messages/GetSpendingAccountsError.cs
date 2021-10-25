using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetSpendingAccountsError : MvxMessage
    {
        public string Message { get; set; }
        public GetSpendingAccountsError(object sender) : base(sender)
        {

        }
    }
}
