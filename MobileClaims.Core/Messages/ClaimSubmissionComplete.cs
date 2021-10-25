using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClaimSubmissionComplete : MvxMessage
    {
        public string Message { get; set; }
        public ClaimSubmissionComplete(object sender) : base(sender)
        {

        }
    }
}
