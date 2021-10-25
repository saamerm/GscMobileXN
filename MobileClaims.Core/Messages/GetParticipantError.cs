using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetParticipantError : MvxMessage
    {
        public string Message { get; set; }
        public GetParticipantError(object sender) : base(sender)
        {

        }
    }
}
