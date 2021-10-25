using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class COPDocumentsSubmissionError : MvxMessage
    {
        public string Message { get; set; }

        public COPDocumentsSubmissionError(object sender)
            : base(sender)
        {
        }
    }
}