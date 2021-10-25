using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClaimSubmissionError : MvxMessage
    {
        public string Message { get; set; }

        public ClaimSubmissionError(object sender)
            : base(sender)
        {
        }
    }
}