using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    // TODO: sPublished but not subscribed. It seems like this is not used. Therefore, may be we can delete itss        
    public class OnTreatmentDetailsListViewModelMessage : MvxMessage
    {
        public string Message { get; set; }
        public OnTreatmentDetailsListViewModelMessage(object sender) : base(sender)
        {

        }
    }
}
