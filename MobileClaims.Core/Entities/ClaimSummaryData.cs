namespace MobileClaims.Core.Entities
{
    public class ClaimSummaryData
    {
        public string UserName { get; set; }
        public string ServiceDate { get; set; }
        public string ClaimForm { get; set; }
        public string ServiceDescription { get; set; }
        public string ClaimedAmount { get; set; }
        public string OtherPaidAmount { get; set; }
        public string PaidAmount { get; set; }
        public string Copay { get; set; }
        public string PaymentDate { get; set; }
        public string PayTo { get; set; }
        public string ParticipantNumber { get; set; }
        public string EobMessages { get; set; }
        public bool IsCOP { get; set; }
        public bool IsSelectedForAudit { get; set; }
    }
}