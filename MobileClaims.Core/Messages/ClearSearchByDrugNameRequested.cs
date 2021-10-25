using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearSearchByDrugNameRequested : MvxMessage
    {
        public ClearSearchByDrugNameRequested(object sender) : base (sender)
        { }
    }
}
