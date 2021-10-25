using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoClaimHistoryTypesFound : MvxMessage
    {
        public string Message { get; set; }
        public NoClaimHistoryTypesFound(object sender) : base(sender)
        {

        }
    }
}
