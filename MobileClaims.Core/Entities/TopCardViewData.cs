namespace MobileClaims.Core.Entities
{
    public class TopCardViewData
    {
        public string UserName { get; set; }
        public string ServiceDate { get; set; }
        public string ClaimForm { get; set; }
        public string ServiceDescription { get; set; }
        public string ClaimedAmount { get; set; }
        public string ParticipantNumber { get; set; }
        public string EobMessages { get; set; }
        public ClaimActionState ClaimActionState { get; set; }
    }
}