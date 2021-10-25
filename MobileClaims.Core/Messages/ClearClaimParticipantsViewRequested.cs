using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearClaimParticipantsViewRequested : MvxMessage
    {
        public ClearClaimParticipantsViewRequested(object sender)
            : base(sender)
        { }
    }
}
