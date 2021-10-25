using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RetrievedUpdatedSpendingAccountDetails : MvxMessage
    {
        public RetrievedUpdatedSpendingAccountDetails(object sender)
            : base(sender)
        {
        }
    }
}
