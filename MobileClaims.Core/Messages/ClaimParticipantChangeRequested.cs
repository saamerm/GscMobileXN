using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClaimParticipantChangeRequested : MvxMessage
    {
        public ClaimParticipantChangeRequested(object sender)
            : base(sender)
        { }
    }
}
