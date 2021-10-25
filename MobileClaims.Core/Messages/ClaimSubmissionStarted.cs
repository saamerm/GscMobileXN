using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClaimSubmissionStarted : MvxMessage
    {
        public ClaimSubmissionStarted(object sender)
            : base(sender)
        { }
    }
}

