using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Messages
{
    public class DrugParticipantSelected : ParticipantSelected
    {
        public DrugParticipantSelected(object sender, Participant p) : base(sender, p) { }
    }
}
