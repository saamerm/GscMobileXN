using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchFilterDataChangedMessage : MvxMessage
    {
        public SearchFilterData SearchFilterData { get; }

        public SearchFilterDataChangedMessage(object sender, SearchFilterData searchFilterData)
            : base(sender)
        {
            SearchFilterData = searchFilterData;
        }
    }
}