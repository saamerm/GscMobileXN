using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoClaimHistoryPayeesFound : MvxMessage
    {
        public string Message { get; set; }
        public NoClaimHistoryPayeesFound(object sender) : base(sender)
        {

        }
    }
}
