using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RetrievedAllSpendingAccountDetails : MvxMessage
    {
        public RetrievedAllSpendingAccountDetails(object sender)
            : base(sender)
        {
        }
    }
}
