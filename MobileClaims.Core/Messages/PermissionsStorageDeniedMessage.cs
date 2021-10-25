using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class PermissionsStorageDeniedMessage : MvxMessage
    {
        public string Message { get; set; }
        public PermissionsStorageDeniedMessage(object sender) : base(sender)
        {

        }
    }
}
