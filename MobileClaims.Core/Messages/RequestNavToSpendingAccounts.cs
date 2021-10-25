using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RequestNavToSpendingAccounts : MvxMessage
    {
        public RequestNavToSpendingAccounts(object sender)
            : base(sender)
        {
        }       
    }
}
