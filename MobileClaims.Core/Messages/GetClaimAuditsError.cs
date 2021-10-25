using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimAuditsError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimAuditsError(object sender) : base(sender)
        {

        }
    }
}
