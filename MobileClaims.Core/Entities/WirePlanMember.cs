using System.Collections.Generic;

namespace MobileClaims.Core.Entities
{
    public class WirePlanMember : WireParticipant
    {
        public List<WireParticipant> Dependents { get; set; }
    }
}
