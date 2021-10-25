using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RetrievedSpendingAccountDetails : MvxMessage
    {
        public RetrievedSpendingAccountDetails(object sender)
            : base(sender)
        {
        }
    }
}
