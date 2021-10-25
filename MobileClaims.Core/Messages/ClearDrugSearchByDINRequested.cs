using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearDrugSearchByDINRequested : MvxMessage
    {
        public ClearDrugSearchByDINRequested(object sender):base(sender)
        { }
    }
}
