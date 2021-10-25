using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimAuditsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimAuditsComplete(object sender) : base(sender)
        {

        }
    }
}
