using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class AuditListItemHelightRequest: MvxMessage
    {
        public long GSCClaimFormID { get; set; }
        public AuditListItemHelightRequest(object sender)
            : base(sender)
        {
            
        }
    }
}
