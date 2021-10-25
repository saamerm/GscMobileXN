using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class UpdateMapMessage : MvxMessage
    {
        public UpdateMapMessage(object sender)
            : base(sender)
        {
        }
    }
}