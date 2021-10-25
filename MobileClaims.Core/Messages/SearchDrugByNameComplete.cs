using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchDrugByNameComplete : MvxMessage
    {
        public SearchDrugByNameComplete(object sender)
            : base(sender)
        { }
    }
}
