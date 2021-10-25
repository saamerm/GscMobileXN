using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RecentClaimsError : MvxMessage
    {
        public string Message { get; set; }

        public RecentClaimsError(object sender)
            : base(sender)
        {
        }
    }
}