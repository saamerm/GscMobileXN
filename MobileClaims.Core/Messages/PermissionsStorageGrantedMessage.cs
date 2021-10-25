using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class PermissionsStorageGrantedMessage : MvxMessage
    {
        public string Message { get; set; }
        public PermissionsStorageGrantedMessage(object sender) : base(sender)
        {

        }
    }
}
