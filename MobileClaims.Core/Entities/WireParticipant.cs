namespace MobileClaims.Core.Entities
{
    public class WireParticipant
    {
        public int ID { get; set; }
        public string PlanMemberID { get; set; }
        public string Name { get; set; }
        public bool HasTravelCoverage { get; set; }

        public int UserID { get; set; }
        public int? EmployerID { get; set; }

        public WireEmployer Employer { get; set; }
    }
}
