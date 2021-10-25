using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchGoToCurrentLocationMessage : MvxMessage
    {
        public SearchGoToCurrentLocationMessage(object sender)
            : base(sender)
        {
        }
    }
}