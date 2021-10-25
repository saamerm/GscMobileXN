using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RefreshSelectedAuditRequest : MvxMessage
    {
        public string SelectedClaimAuditID { get; set; }
        public RefreshSelectedAuditRequest(object sender)
            : base(sender)
        {
        }
    }
}
