using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearDrugSearchResultsRequested : MvxMessage
    {
        public ClearDrugSearchResultsRequested(object sender) : base(sender)
        { }
    }
}
