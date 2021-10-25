using MobileClaims.Core.Entities;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ParticipantSelected : MvxMessage
    {
        public ParticipantSelected(object sender) : base (sender)
        {

        }
        public ParticipantSelected(object sender, Participant selectedParticipant) : base(sender)
        {
            SelectedParticipant = selectedParticipant;
        }
        public Participant SelectedParticipant
        { get; set; }
    }
}
