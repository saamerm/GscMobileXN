using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class EligibilityCheckSubmissionComplete : MvxMessage
    {
        public string Message { get; set; }
        public EligibilityCheckSubmissionComplete(object sender) : base(sender)
        {

        }
    }
}
