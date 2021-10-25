using MobileClaims.Core.Entities;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ClaimParticipantSelected : MvxMessage
    {
        public ClaimParticipantSelected(object sender) : base (sender)
        {

        }
        public ClaimParticipantSelected(object sender, Participant selectedParticipant)
            : base(sender)
        {
            SelectedParticipant = selectedParticipant;
        }
        public Participant SelectedParticipant
        { get; set; }
    }
}
