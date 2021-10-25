using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchClaimsHistoryComplete : MvxMessage
    {
        public string Message { get; set; }
        public SearchClaimsHistoryComplete(object sender) : base(sender)
        {

        }
    }
}
