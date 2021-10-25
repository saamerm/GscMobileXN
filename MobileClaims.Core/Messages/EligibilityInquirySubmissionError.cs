using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class EligibilityInquirySubmissionError : MvxMessage
    {
        public string Message { get; set; }
        public EligibilityInquirySubmissionError(object sender) : base(sender)
        {

        }
    }
}
