using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClearEligibilityParticipantsRequest : MvxMessage
    {
        public ClearEligibilityParticipantsRequest(object sender)
            : base(sender)
        { }
    }
}
