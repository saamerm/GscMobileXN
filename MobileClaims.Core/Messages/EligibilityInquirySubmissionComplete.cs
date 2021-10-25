using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class EligibilityInquirySubmissionComplete : MvxMessage
    {
        public string Message { get; set; }
        public EligibilityInquirySubmissionComplete(object sender) : base(sender)
        {

        }
    }
}
