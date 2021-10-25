using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchClaimsHistoryError : MvxMessage
    {
        public string Message { get; set; }
        public SearchClaimsHistoryError(object sender) : base(sender)
        {

        }
    }
}
