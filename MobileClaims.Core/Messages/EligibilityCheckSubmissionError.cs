using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class EligibilityCheckSubmissionError : MvxMessage
    {
        public string Message { get; set; }
        public EligibilityCheckSubmissionError(object sender) : base(sender)
        {

        }
    }
}
