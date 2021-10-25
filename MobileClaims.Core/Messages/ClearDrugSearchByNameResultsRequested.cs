using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearDrugSearchByNameResultsRequested : MvxMessage
    {
        public ClearDrugSearchByNameResultsRequested(object sender) :base(sender)
        { }
    }
}
