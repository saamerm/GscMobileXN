using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RequestCloseClaimsHistoryResultsListViewModel : MvxMessage
    {
        public string Message { get; set; }
        public RequestCloseClaimsHistoryResultsListViewModel(object sender) : base(sender)
        {

        }
    }
}
