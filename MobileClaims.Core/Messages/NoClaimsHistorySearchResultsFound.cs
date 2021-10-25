using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoClaimsHistorySearchResultsFound : MvxMessage
    {
        public string Message { get; set; }
        public NoClaimsHistorySearchResultsFound(object sender) : base(sender)
        {

        }
    }
}
