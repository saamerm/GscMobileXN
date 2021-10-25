using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchDrugByDINComplete : MvxMessage
    {
        public SearchDrugByDINComplete(object sender)
            : base(sender)
        {
        }
    }
}
