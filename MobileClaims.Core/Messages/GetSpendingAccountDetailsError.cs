using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetSpendingAccountDetailsError : MvxMessage
    {
        public string Message { get; set; }
        public GetSpendingAccountDetailsError(object sender) : base(sender)
        {

        }
    }
}
