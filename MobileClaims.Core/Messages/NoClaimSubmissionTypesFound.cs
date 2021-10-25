using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoClaimSubmissionTypesFound : MvxMessage
    {
        public string Message { get; set; }
        public NoClaimSubmissionTypesFound(object sender) : base(sender)
        {

        }
    }
}
