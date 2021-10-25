using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
	public class EligibilityParticipantChangeRequested : MvxMessage
    {
		public string Message { get; set; }
		public EligibilityParticipantChangeRequested(object sender)
            : base(sender)
        { }
    }
}
